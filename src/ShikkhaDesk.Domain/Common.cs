namespace ShikkhaDesk.Domain;

/// <summary>Base entity with a surrogate key.</summary>
public abstract class Entity
{
    public long Id { get; set; }
}

/// <summary>Marker for aggregate roots (transaction/consistency boundaries).</summary>
public interface IAggregateRoot;

/// <summary>Domain event marker (raised inside aggregates, dispatched by Application).</summary>
public interface IDomainEvent;

/// <summary>
/// The institution identity. <see cref="InstitutionUid"/> is IMMUTABLE and binds
/// the license (see LICENSE-DESIGN.md). The visible name may be corrected via an
/// authorized, audited workflow without changing the uid.
/// </summary>
public sealed class Institution : Entity, IAggregateRoot
{
    public Guid InstitutionUid { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? LogoPath { get; private set; }
    public string AcademicSession { get; private set; } = string.Empty;
    public string CurrencyCode { get; private set; } = "BDT";
    public DigitPreference Digits { get; private set; } = DigitPreference.Bengali;

    private Institution() { }

    public static Institution Create(string name, string academicSession,
        string currencyCode = "BDT", DigitPreference digits = DigitPreference.Bengali)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Institution name is required.", nameof(name));
        return new Institution { Name = name.Trim(), AcademicSession = academicSession, CurrencyCode = currencyCode, Digits = digits };
    }

    /// <summary>Authorized name correction — preserves the immutable uid.</summary>
    public void CorrectName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.", nameof(newName));
        Name = newName.Trim();
    }

    public void UpdateContact(string? address, string? phone, string? email)
    {
        Address = address; Phone = phone; Email = email;
    }

    public void SetLogo(string? path) => LogoPath = path;
    public void SetDigits(DigitPreference digits) => Digits = digits;
}

/// <summary>Global digit display preference (storage always uses English digits).</summary>
public enum DigitPreference { Bengali, English }
