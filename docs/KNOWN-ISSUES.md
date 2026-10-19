# KNOWN ISSUES & BLOCKERS — MERQO ShikkhaDesk

Honest, itemized record of blockers, limitations and open risks. Every entry
states the exact cause and what is required to clear it. This file never claims
a blocker is resolved until the evidence exists.

---

## BLOCKER-1 — No Windows / .NET build environment (central)

- **Symptom:** No compiled executable, no Windows installer, no executed
  automated test exists for this product.
- **Cause:** The engineering environment is Debian Linux. WPF is a Windows-only
  UI stack and cannot be compiled or run on Linux. Additionally, **no .NET SDK is
  reachable**: `dot.net`, `builds.dotnet.microsoft.com`, `deb.debian.org` and the
  GitHub release-asset host are all network-blocked; only GitHub *source*
  tarballs (`codeload`) and package registries (PyPI/npm) are reachable.
- **Impact:** Every gate that requires building, running, or installing on
  Windows is blocked (see `RELEASE-CHECKLIST.md`).
- **To clear:** Provide a Windows 10/11 machine (or Windows CI runner) with the
  .NET 8 SDK, the WPF/Windows Desktop targeting pack, and the chosen installer
  toolchain. Then run `dotnet build` + `dotnet test` and the installer matrix.
- **Interim:** Continue all non-build work (documentation, design, research,
  source authoring, brand assets) — this is what the current session does.

## BLOCKER-2 — Product-name market/trademark check not yet completed

- **Symptom:** The master prompt notes "a similar name already exists in the
  education-software market." A formal trademark / market-confusion review has
  not been performed.
- **Cause:** Requires legal review beyond public research; public research alone
  can identify *similarities* but not *legal* conflicts.
- **To clear:** Owner decision. See `OPEN-DECISIONS.md` (DEC-1). Public
  similarity evidence is captured in `COMPETITOR-RESEARCH.md`. **The product is
  not silently renamed.**

## LIMITATION-1 — Offline license revocation is inherently limited

- Any fully offline, perpetual-use license cannot be forcibly revoked on a device
  that never contacts a server. Device limits and institution binding are
  enforced at activation and registration time, not by live phone-home. This is a
  deliberate design property (no mandatory online checks), documented in
  `LICENSE-DESIGN.md`, and disclosed to the customer.

## LIMITATION-2 — No live SMS / bKash / Nagad / gateway integration in v1

- bKash/Nagad are recorded as **payment methods** (manual, with a transaction
  reference), not as live API integrations. No SMS/WhatsApp/gateway/biometric
  integration is claimed. These are optional, cost-bearing integrations outside
  the offline-first core. See `FEATURE-MATRIX.md`.

## LIMITATION-3 — Brand assets are original vector + raster, not a commissioned trademark

- The logo is an original design created for this project from the specified
  palette. It is not a registered trademark and carries no legal guarantee. A
  professional trademark search/registration is an owner decision (DEC-1).

## RISK-1 — Uncompiled source may contain errors

- Source under `src/` is authored but **not compiled or tested** in this
  environment. It must be built and tested on Windows/.NET before any reliance.
  Status is marked 🟡 everywhere; it is never represented as working.

## RISK-2 — Financial correctness is designed, not yet proven

- The double-entry invariants (PART 17.8 / PART 27) are specified in
  `ACCOUNTING-DESIGN.md` and `TEST-PLAN.md`, and reference tests are scaffolded,
  but **no test has executed**. Financial correctness is unproven until the .NET
  test suite runs green.

---

_New issues are appended with evidence; resolved issues are moved to a "Resolved"
section with the proof (build log id, test run id, installer checksum) — never
deleted silently._
