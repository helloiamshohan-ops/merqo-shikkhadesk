# TEST RESULTS — MERQO ShikkhaDesk

**Honest status: 0 tests executed.** This environment (Debian Linux, no .NET SDK,
cannot build/run WPF) cannot compile or run the .NET test suites
(`KNOWN-ISSUES.md` BLOCKER-1). Nothing below is marked passed. This file is the
template that will hold real results once `dotnet test` runs on a .NET runtime.

Per the directive: *"Do not mark tests as passed unless they actually ran."*

---

## Run log

| Date | Suite | Environment | Result | Evidence |
|------|-------|-------------|--------|----------|
| — | (none yet) | — | **Not run** | No .NET runtime available |

---

## Financial invariants (B) — status: NOT RUN
| # | Invariant | Result |
|---|-----------|--------|
| 1 | Journal entries balance | ⬜ not run |
| 2 | No over-allocation | ⬜ not run |
| 3 | No double credit use | ⬜ not run |
| 4 | Refunds don't duplicate | ⬜ not run |
| 5 | Transfers don't inflate income | ⬜ not run |
| 6 | Payable settlement no dup expense | ⬜ not run |
| 7 | Receipts no dup payment | ⬜ not run |
| 8 | Reversals reference source | ⬜ not run |
| 9 | Student balances reconcile | ⬜ not run |
| 10 | Cash/bank reconcile | ⬜ not run |
| 11 | Payroll reconciles | ⬜ not run |
| 12 | Dashboard = reports | ⬜ not run |
| 13 | Historical reports reproducible | ⬜ not run |

## License (C), LAN (D), install (G) — status: NOT RUN
All ⬜. Blocked on Windows/.NET.

---

## What must happen before any gate can pass
1. Provision Windows + .NET 8 SDK.
2. `dotnet build` the solution; fix compile errors.
3. `dotnet test` — record actual pass/fail counts here with the run id/date.
4. Run LAN + installation matrices on Windows.

**Until then, `RELEASE-CHECKLIST.md` gates 5–16 remain OPEN and the product is
not commercially ready.**
