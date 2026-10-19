namespace ShikkhaDesk.Persistence;

/// <summary>
/// Authoritative database schema version. Bumped by every migration. The app
/// records/applies forward-only migrations at startup, after an automatic
/// pre-migration backup (ARCHITECTURE.md section 8).
/// </summary>
public static class SchemaVersion
{
    public const int Current = 1;
    public const string Product = "MERQO ShikkhaDesk";
}
