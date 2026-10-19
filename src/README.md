# MERQO ShikkhaDesk — source

Layered .NET solution (see `../docs/ARCHITECTURE.md`). **The source is authored but
NOT compiled or tested** — the authoring environment is Debian Linux with no .NET
SDK and cannot build WPF (`../docs/KNOWN-ISSUES.md` BLOCKER-1). Nothing here is
claimed to build or pass tests until it has, on Windows + .NET 8.

## Projects

| Project | Layer | Builds on |
|---------|-------|-----------|
| `ShikkhaDesk.Domain` | entities, `Money` (integer poisha), enums | any OS |
| `ShikkhaDesk.Accounting` | double-entry engine, posting rules, ledger | any OS |
| `ShikkhaDesk.Licensing` | canonical license, ECDSA sign/verify, institution binding | any OS |
| `ShikkhaDesk.Application` | use-case services, idempotency, transaction boundary, abstractions | any OS |
| `ShikkhaDesk.Persistence` | SQLite schema + migrations + repositories | any OS |
| `ShikkhaDesk.LanTransport` | host/client contracts | any OS |
| `ShikkhaDesk.Reporting` | reports, CSV/Excel export | any OS |
| `ShikkhaDesk.Backup` | backup/restore, integrity | any OS |
| `ShikkhaDesk.Diagnostics` | logging | any OS |
| `ShikkhaDesk.Presentation` | WPF app (composition root, 48 screens) | **Windows only** |
| `ShikkhaDesk.LicenseManager` | private seller-side WPF app | **Windows only** |
| `tests/*` | xUnit suites (Domain, Accounting, Licensing) | any OS (Accounting/Domain/Licensing) |

## Build & test (on Windows with .NET 8 SDK; needs nuget.org for restore)

```bat
dotnet restore src\ShikkhaDesk.sln
dotnet build   src\ShikkhaDesk.sln -c Release
dotnet test    src\ShikkhaDesk.sln
```

Run only the cross-platform core (works on any OS with a .NET SDK):

```bash
dotnet test tests/ShikkhaDesk.Accounting.Tests
dotnet test tests/ShikkhaDesk.Licensing.Tests
dotnet test tests/ShikkhaDesk.Domain.Tests
```

## Regenerating the solution (optional)

`scripts/create-solution.sh` recreates `ShikkhaDesk.sln` and project references with
the .NET CLI (useful if the hand-written `.sln` needs to be regenerated).

## What is real vs. scaffold

- **Real, reviewable logic:** `Money`, `JournalEntry` (balance + immutability +
  reversal), `Ledger` (trial balance, profit), `PostingRules` (all standard
  double-entry mappings), `LicensePayload`/`SignedLicense`/`LicenseSigner`/
  `LicenseVerifier` (canonical, signed, institution-bound, offline), and the
  financial-invariant + license tests.
- **Scaffold (interfaces/shells to be filled):** Persistence repositories, LAN
  host/client implementation, Reporting, Backup, and the full WPF screen set.

## Keys

Development/tests use a clearly-labeled **test** ECDSA key generated at runtime.
**No production private signing key exists in this repository**, and none may be
created without owner authorization (`../docs/OPEN-DECISIONS.md` DEC-3).
