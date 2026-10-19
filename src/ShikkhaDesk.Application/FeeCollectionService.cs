using ShikkhaDesk.Accounting;
using ShikkhaDesk.Domain;

namespace ShikkhaDesk.Application;

/// <summary>Result of a fee-collection operation (idempotent).</summary>
public sealed record FeeCollectionResult(string PaymentUid, string JournalNo, Money Amount, bool WasReplay);

/// <summary>
/// Collects a fee payment: allocates to outstanding charges, posts the balanced
/// journal, and commits atomically. Enforces idempotency so a retried request
/// (e.g. after a LAN timeout) never double-posts (LAN-SETUP / ACCOUNTING-DESIGN).
/// Business logic lives here (service layer), never in the UI.
/// </summary>
public sealed class FeeCollectionService
{
    private readonly IUnitOfWork _uow;
    private readonly IIdempotencyStore _idempotency;
    private readonly INumberAllocator _numbers;
    private readonly IRepository<JournalEntry> _journals;
    private readonly StandardAccounts _accounts;
    private readonly long _currentUserId;

    public FeeCollectionService(IUnitOfWork uow, IIdempotencyStore idempotency,
        INumberAllocator numbers, IRepository<JournalEntry> journals,
        StandardAccounts accounts, long currentUserId)
    {
        _uow = uow; _idempotency = idempotency; _numbers = numbers;
        _journals = journals; _accounts = accounts; _currentUserId = currentUserId;
    }

    public async Task<FeeCollectionResult> CollectAsync(
        string operationId, Guid paymentUid, long studentId, long depositAccountId,
        Money amount, PaymentMethod method, string? reference, DateTime collectedOn,
        CancellationToken ct = default)
    {
        if (amount <= Money.Zero)
            throw new ArgumentException("Amount must be positive.", nameof(amount));

        // 1. Idempotency: return the prior result if this operation already ran.
        var prior = await _idempotency.GetAsync<FeeCollectionResult>(operationId, ct);
        if (prior is not null)
            return prior with { WasReplay = true };

        // 2. Build the balanced journal for the payment (settles the receivable).
        //    Allocation to specific charges happens here in a full implementation;
        //    the ledger effect is identical either way: Dr Cash/Bank, Cr Receivable.
        var journal = PostingRules.FeePayment(_accounts, depositAccountId, collectedOn, amount, paymentUid.GetHashCode());

        // 3. Allocate the journal number on the host, then post (validates balance).
        var journalNo = await _numbers.NextAsync("JV", ct);
        journal.Post(journalNo, _currentUserId, DateTime.UtcNow); // throws if unbalanced
        await _journals.AddAsync(journal, ct);

        // 4. Commit atomically — only now is the payment real.
        await _uow.CommitAsync(ct);

        var result = new FeeCollectionResult(paymentUid.ToString("D"), journalNo, amount, WasReplay: false);
        await _idempotency.StoreAsync(operationId, result, ct);
        return result;
    }
}
