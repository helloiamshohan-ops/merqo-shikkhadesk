namespace ShikkhaDesk.Application;

/// <summary>
/// Unit of work: one authoritative database transaction per business operation.
/// Nothing is reported successful until <see cref="CommitAsync"/> succeeds
/// (LAN-SETUP / ACCOUNTING-DESIGN invariants).
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}

/// <summary>Minimal repository contract (implemented in ShikkhaDesk.Persistence).</summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(long id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
}

/// <summary>
/// Idempotency store: maps a client operation id to the already-produced result so
/// a retry after a timeout never double-posts a payment/receipt (LAN invariant).
/// </summary>
public interface IIdempotencyStore
{
    Task<T?> GetAsync<T>(string operationId, CancellationToken ct = default);
    Task StoreAsync<T>(string operationId, T result, CancellationToken ct = default);
}

/// <summary>
/// Journal/receipt number allocator. The HOST allocates numbers inside the
/// transaction so concurrent clients never collide (concurrent receipt numbering).
/// </summary>
public interface INumberAllocator
{
    Task<string> NextAsync(string series, CancellationToken ct = default);
}
