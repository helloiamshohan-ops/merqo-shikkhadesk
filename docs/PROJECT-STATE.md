# PROJECT STATE — MERQO ShikkhaDesk

> **Authoritative, evidence-based status.** This file is the single source of
> truth for "what is actually done" vs. "what is planned" vs. "what is blocked".
> It is updated by every work session. Nothing here is aspirational; claims are
> backed by the evidence listed.

**Last updated:** 2026-10-19
**Session branch:** `arena/f4a56233-merqo-shikkhadesk`
**Base commit:** `5bb9eef` (Initial commit) on `main`

---

## 1. PART 33 — First-execution audit (evidence)

### 1.1 Repository status
- Working tree was **clean** at session start, on branch `arena/f4a56233-merqo-shikkhadesk`.
- History: a single commit `5bb9eef Initial commit`.
- Tracked files at start: **exactly one** — `README.md`, whose entire content was
  the single line `# merqo-shikkhadesk`.
- **Conclusion:** there was **no prior implementation** to preserve. The project
  starts from a blank slate; "do not start over unnecessarily / preserve existing
  work" does not apply because there is no existing work.

### 1.2 Existing implementation status
- **None.** No source code, no database, no tests, no build scripts, no assets.

### 1.3 Build-environment status (measured, not assumed)
| Item | Result |
|------|--------|
| OS | Debian GNU/Linux 12 (bookworm), x86_64 — **not Windows** |
| `.NET` / `dotnet` | **Not installed** |
| `dotnet` via apt | **Unavailable** (not in repos) |
| `dot.net` (install script host) | **Blocked** (network) |
| `builds.dotnet.microsoft.com` | **Blocked** (network) |
| `deb.debian.org` (apt mirror) | **Blocked** (network) |
| GitHub release assets (`objects.githubusercontent.com`) | **Blocked** (redirect target) |
| `codeload.github.com` (repo source tarballs) | **Reachable** (verified, 200) |
| `github.com` / `api.github.com` | **Reachable** (200) |
| `pypi.org` / `files.pythonhosted.org` | **Reachable** (used to install Pillow 13.0.0) |
| `registry.npmjs.org` | Reachable |
| Python | 3.11.2 |
| Node | 22.22.3 |
| Mono | Not installed |
| CPU / RAM / Disk | 2 cores / 3.8 GiB / 20 GiB free |

### 1.4 The decisive blocker
The product's target platform is **Windows + WPF + .NET**, and the authoritative
money logic is meant to be verified by **automated .NET tests**. In this
environment:

1. **WPF cannot be compiled or run on Linux** (Windows-only UI stack), so no UI
   build or UI test is possible here even with a .NET SDK.
2. **No .NET SDK is obtainable** (every distribution channel is network-blocked,
   and GitHub release binaries redirect to a blocked host).
3. Consequently **no executable, no Windows installer, and no executed unit/
   integration test can be produced in this environment.**

Per the master directive — *"If an essential external dependency prevents a
particular operation, record the blocker and continue all independent work that
can safely proceed"* — the session proceeds with everything that does **not**
require a Windows/.NET toolchain, and records this blocker precisely.

### 1.5 What "done" therefore means in this environment
**Done and verifiable here:**
- Documentation (design, requirements, research, plans, guides).
- Competitor research from public sources.
- Original brand assets, including a **genuine multi-resolution Windows `.ico`**
  (built from real raster data with Pillow — not a renamed PNG).
- The .NET **source scaffold**: layered projects, domain model, accounting
  engine, licensing, persistence contracts, WPF shell, License Manager.

**NOT done / not verifiable here (blocked on Windows + .NET SDK):**
- Any compiled binary, test run, Windows installer, or License Manager build.
- Runtime/LAN/concurrency tests.
- Real installation, upgrade, backup/restore on Windows.

Every one of these is listed in [`KNOWN-ISSUES.md`](KNOWN-ISSUES.md) and the
[`RELEASE-CHECKLIST.md`](RELEASE-CHECKLIST.md) gates remain explicitly **open**.

---

## 2. Module status board (honest)

Legend: ✅ verified · 🟡 authored-not-built · 🔴 not started · ⛔ blocked (needs Windows/.NET)

| Module | Status | Evidence / note |
|--------|--------|-----------------|
| Repo & environment audit | ✅ | this file §1 |
| Competitor research | 🟡 | `docs/COMPETITOR-RESEARCH.md` (public sources; facts vs analysis separated) |
| Feature matrix / positioning | 🟡 | `docs/FEATURE-MATRIX.md`, `docs/PRODUCT-POSITIONING.md` |
| Brand identity (SVG + genuine ICO + PNG) | ✅ | `assets/` — ICO built from raster data, verified with Pillow |
| Bangla terminology glossary | 🟡 | `docs/BANGLA-TERMINOLOGY.md` |
| Requirements | 🟡 | `docs/REQUIREMENTS.md` |
| Architecture | 🟡 | `docs/ARCHITECTURE.md` |
| Data model | 🟡 | `docs/DATA-MODEL.md` |
| Accounting design | 🟡 | `docs/ACCOUNTING-DESIGN.md` |
| License design + License Manager design | 🟡 | `docs/LICENSE-DESIGN.md`, `docs/LICENSE-MANAGER-GUIDE.md` |
| Domain / Accounting / Licensing source | 🟡 | `src/…` authored; ⛔ not compiled/tested |
| Persistence (SQLite + migrations) source | 🟡 | ⛔ not compiled/tested |
| WPF Presentation source | 🟡 | ⛔ cannot build on Linux |
| LAN host/client source | 🟡 | ⛔ not compiled/tested |
| Automated tests (financial invariants, license, LAN) | 🔴 | ⛔ require .NET runtime to run |
| Windows customer installer | ⛔ | needs Windows + installer toolchain |
| License Manager installer | ⛔ | needs Windows + installer toolchain |
| Documentation suite | 🟡 | `docs/` (near-complete; keep current) |

"🟡 authored" means real, reviewable artifacts exist but have **not been
compiled or executed**. They are not represented as working software.

---

## 3. Commercial acceptance gates (PART 32) — summary

All gates are currently **OPEN**. See [`RELEASE-CHECKLIST.md`](RELEASE-CHECKLIST.md)
for the item-by-item gate list and what each requires. The product is **not**
ready for commercial release and must not be described as such.

---

## 4. Immediate next safe tasks (dependency-ready)

1. Windows + .NET 8 SDK build machine (or CI) — clears the central blocker.
2. `dotnet build` the cross-platform core (`ShikkhaDesk.Domain`,
   `ShikkhaDesk.Accounting`, `ShikkhaDesk.Licensing`) and fix any compile errors.
3. `dotnet test` the financial-invariant, licensing and persistence suites.
4. Build & run WPF shell on Windows; connect to LAN host/client.
5. Produce the customer and License Manager installers on Windows; run the
   installation/upgrade/backup-restore matrix.

Until a Windows/.NET environment exists, engineering continues on documentation,
design review, and source quality.
