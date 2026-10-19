# ShikkhaDesk.Persistence

SQLite persistence + forward-only migrations (PART 23). The authoritative database
is a single SQLite file on the host's local disk; LAN clients never open it over
SMB (they call the host service).

**Status: scaffold. Not built or tested (needs Windows/.NET; NuGet restore needs
nuget.org access).**

Intended contents (see `docs/DATA-MODEL.md` for the full schema):
- `SchemaVersion` — current schema version constant.
- EF Core `DbContext` mapping the entities in `docs/DATA-MODEL.md`.
- Migrations folder: `Migrations/0001_Initial.cs`, … each with `Up`/`Down`.
- Repository implementations of `ShikkhaDesk.Application` abstractions
  (`IRepository<T>`, `IUnitOfWork`, `IIdempotencyStore`, `INumberAllocator`).
- A SQLite connection factory and an integrity-check helper used by Backup.

Key rules enforced at this layer and in the domain:
- Money is INTEGER poisha; foreign keys, unique constraints and indexes per the
  data model; financial writes inside one transaction.
