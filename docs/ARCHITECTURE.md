# ARCHITECTURE — MERQO ShikkhaDesk

Layered, offline-first Windows desktop architecture (PART 5 / PART 6). Business
logic never lives in UI event handlers. This document is the design; the source
scaffold under `src/` mirrors it and is **authored, not yet compiled/tested**
(see `PROJECT-STATE.md`).

---

## 1. Technology choices (subject to evidence)

| Concern | Choice | Rationale |
|---------|--------|-----------|
| Language/runtime | C# on a supported .NET (target **.NET 8 LTS**) | cross-platform core libs build/test on any OS; WPF needs Windows |
| UI | **WPF** (.NET) | native Windows business app; the product target is Windows desktop |
| DB (authoritative) | **SQLite** on the host's local disk | offline-first, zero-server, institution owns the file |
| LAN transport | Authenticated host service/API (HTTP over LAN) + client | clients never open the SQLite file over SMB |
| Money | **integer minor units (poisha)**, never float/double | exact financial precision |
| Crypto | .NET built-in asymmetric signing (ECDSA/RSA) via maintained libs | no custom cryptography |
| Password hashing | PBKDF2 (Rfc2898DeriveBytes) or a maintained KDF | secure, standard |
| Logging | A maintained logger (e.g. Microsoft.Extensions.Logging) | diagnostics |
| Migrations | Versioned schema migrations applied at startup | safe upgrades |
| Installer | Windows installer producing `ShikkhaDesk-Setup-vX.Y.Z-x64.exe` | commercial distribution (built on Windows) |

> **Environment note:** this Linux sandbox has no .NET SDK and cannot build WPF;
> the cross-platform core (Domain, Accounting, Licensing, Persistence) is
> designed to build & test anywhere, only Presentation is Windows-only. See
> `KNOWN-ISSUES.md` BLOCKER-1.

---

## 2. Layers and projects

```
Presentation (WPF)  ──uses──▶  Application  ──uses──▶  Domain
      │                            │  │                   ▲
      │                            │  └──uses──▶ Accounting (engine)
      │                            │                    Licensing
      └──(LAN client)──▶ LanTransport.Client ──HTTP──▶ LanTransport.Host ──▶ Application
                                                                                 │
Persistence (SQLite + migrations) ◀──implements repositories used by── Application
Reporting / Backup / Diagnostics are cross-cutting services used by Application
LicenseManager (separate seller app) uses Licensing (signing) + its own register store
```

**Projects under `src/`:**
- `ShikkhaDesk.Domain` — entities, value objects, enums, domain events, invariants.
  No dependencies on UI/DB/transport.
- `ShikkhaDesk.Accounting` — double-entry engine: journal, posting, ledger,
  statements, reconciliation. Depends on Domain only.
- `ShikkhaDesk.Licensing` — license payload model, canonical serialization,
  sign/verify, institution binding, device rules. Depends on Domain only.
- `ShikkhaDesk.Application` — use-case services (students, fees, payments,
  accounting orchestration, users/permissions, backup), DTOs, validation,
  transaction boundaries, idempotency. Depends on Domain/Accounting/Licensing and
  repository **interfaces** (defined here, implemented in Persistence).
- `ShikkhaDesk.Persistence` — SQLite schema, migrations, repository
  implementations, unit-of-work. Implements Application interfaces.
- `ShikkhaDesk.LanTransport` — host service (API endpoints, auth) and client
  transport (HTTP, retry, idempotency keys, reconnection). Host calls Application.
- `ShikkhaDesk.Reporting` — report models, CSV/Excel export, print documents.
- `ShikkhaDesk.Backup` — snapshot/backup/restore, integrity, retention.
- `ShikkhaDesk.Diagnostics` — logging abstractions/config, safe diagnostic logs.
- `ShikkhaDesk.Presentation` — WPF app: shell, navigation, ViewModels (MVVM),
  Bangla resources, formatting (digits/dates/currency), the 48 screens.
- `ShikkhaDesk.LicenseManager` — separate private seller-side WPF app + register.

**Dependency rule:** dependencies point inward (Presentation → Application →
Domain). Domain/Accounting/Licensing have no outward dependencies. This keeps the
financial and licensing logic unit-testable without UI or DB.

---

## 3. Offline-first runtime

- **Single-computer mode:** Presentation + Application + Persistence + in-process
  host all run locally; SQLite on the local disk. Works with no network.
- **LAN mode:** one computer runs the **Host** (Application + Persistence +
  LanTransport.Host). Up to 5 **Clients** run Presentation + LanTransport.Client
  and call the host over authenticated HTTP. The host is the single writer to
  SQLite; clients never touch the file.
- The app must start and remain usable offline for all core work; no online
  login/activation-refresh/cloud dependency.

## 4. Money, digits, dates, currency

- All money is `long` **poisha** (1 taka = 100 poisha). No `float`/`double` for
  money anywhere (storage, calc, transport, reports).
- Display formatting is centralized: Bengali/English digit preference, Gregorian
  dates, `৳` currency, applied only at the presentation/reporting boundary.
- Database and calculations always use canonical English-digit values.

## 5. Concurrency, idempotency, transactions (LAN)

- Every state-changing host operation runs in a single SQLite transaction; nothing
  is reported successful before commit.
- Clients send an **idempotency key** (operation id) with payment/receipt requests;
  the host deduplicates so a retry after a timeout never double-posts.
- Receipt/payment numbers are allocated by the host inside the transaction
  (gap-safe, unique), so concurrent clients get distinct numbers.

## 6. Security boundaries

- Authentication: per-user credentials against the local user store; LAN calls
  authenticated (token/session) and authorized by the permission layer.
- Authorization enforced in **Application services**, not the UI.
- LAN transport over the local network; Windows Firewall requirements documented
  in `LAN-SETUP.md`. Parameterized queries only; no SQL string concatenation.
- No secrets/keys embedded; no production signing key in the customer app.

## 7. Logging & diagnostics

- Structured logging via `Diagnostics`; levels configurable; **never log
  passwords, tokens, or signing keys**; customer data minimized in logs.

## 8. Migrations & upgrades

- Schema version stored in DB; migrations run forward-only at startup with a
  pre-migration automatic backup; failed migration aborts before data loss.

## 9. Build & test topology

- Cross-platform test projects target `net8.0` and run with `dotnet test` on any
  OS (Domain, Accounting, Licensing, Persistence, Application invariants).
- WPF/LAN end-to-end and installer tests run on Windows. See `TEST-PLAN.md`.
