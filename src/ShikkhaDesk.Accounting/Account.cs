using ShikkhaDesk.Domain;

namespace ShikkhaDesk.Accounting;

/// <summary>
/// A chart-of-accounts entry. System accounts (e.g. Student Fee Receivable,
/// Student Advance Credit, control accounts) are seeded and protected.
/// </summary>
public sealed class Account : Entity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public AccountCategory Category { get; private set; }
    public long? ParentId { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsSystemAccount { get; private set; }

    private Account() { }

    public Account(string code, string name, AccountCategory category,
        long? parentId = null, bool isSystemAccount = false)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required.", nameof(name));
        Code = code.Trim();
        Name = name.Trim();
        Category = category;
        ParentId = parentId;
        IsSystemAccount = isSystemAccount;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required.", nameof(name));
        Name = name.Trim();
    }

    public void Deactivate()
    {
        if (IsSystemAccount) throw new InvalidOperationException("System accounts cannot be deactivated.");
        IsActive = false;
    }

    /// <summary>Normal balance side for the category (debit or credit).</summary>
    public bool IsDebitNormal => Category is AccountCategory.Asset or AccountCategory.Expense;
}
