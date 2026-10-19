# TEST PLAN — MERQO ShikkhaDesk

PART 27. Automated unit, integration and regression tests. **A test is "passed"
only if it actually ran** (see `TEST-RESULTS.md`). The suites below are specified
and scaffolded; none have executed in the current environment (no .NET runtime).

Test stack (intended): xUnit (or NUnit) + FluentAssertions; in-memory or temp-file
SQLite for persistence; fakes for transport. Cross-platform suites run anywhere;
WPF/LAN/installer suites run on Windows.

---

## A. Core workflow tests (`ShikkhaDesk.Tests`)
- Student registration, stable ID, duplicate detection, archive/reactivate.
- Admission, enrollment, batch/course transfer (original preserved), withdrawal.
- Attendance: no duplicate per (student,batch,session); enrollment-date validation;
  correction audit.
- Exams: marks entry, absent, grading/GPA, publish gating.
- **Fee assessment:** single + bulk; **repeated bulk creates no duplicate charge**;
  plan edit does not change historical charges.
- **Payments:** full/partial; allocation bounds; **over-allocation rejected**;
  idempotent duplicate request.
- **Advance credit:** allocate/refund cannot double-use; available ≥ 0.
- **Receipt numbering:** unique, host-allocated, concurrent-safe.
- Refunds/reversals: bounded, reference source, restore balances.
- Permissions: each granular permission enforced at the service layer; salary
  fields restricted.

## B. Financial invariants (`ShikkhaDesk.Accounting.Tests`) — PART 17.8
1. Every posted journal balances (Σ debit = Σ credit); unbalanced rejected.
2. Payments cannot be over-allocated.
3. Credit cannot be used twice.
4. Refunds cannot duplicate allocations.
5. Transfers do not inflate revenue.
6. Payable settlements do not duplicate expenses.
7. Receipts cannot create duplicate payments.
8. Reversals reference their source.
9. Student balances reconcile to the ledger control account.
10. Cash/bank balances reconcile to their ledger accounts.
11. Payroll reconciles (expense = payable movement = payments).
12. Dashboard totals match reports.
13. Historical reports reproducible (posted data immutable).

## C. License tests (`ShikkhaDesk.Licensing.Tests`) — PART 27
- Valid test license verifies offline.
- Invalid signature rejected.
- Altered payload rejected.
- Wrong product id rejected.
- Wrong institution uid rejected.
- Institution-name mismatch (uid still binds) handled.
- Device limit enforced (6th refused); no data deletion at limit.
- Offline activation works.
- License reissue; device replacement; register recovery.
- **Production private key not present** in customer assembly (assert no key
  material embedded).

## D. LAN tests (`ShikkhaDesk.LanTransport.Tests`, Windows) — PART 27
- Concurrent payments (no double-post, idempotency).
- Concurrent receipts (distinct numbers).
- Duplicate request deduplication.
- Network interruption + timeout → safe resolution by operation id.
- Host restart → reconnection, no data loss.
- Five-device limit.

## E. Persistence / migration tests
- Migrations apply forward cleanly; schema version recorded.
- Pre-migration backup taken; failed migration aborts safely.

## F. Backup/restore tests (Windows)
- Snapshot integrity; restore replaces data; pre-restore backup exists; failed
  restore recovers to prior state; retention never removes the only good backup.

## G. Installation tests (Windows) — PART 27
- Clean install; first launch; license activation; upgrade; failed-upgrade
  recovery; backup/restore; uninstall preserves data; reinstall + restore.

## H. Regression
- A golden reconciliation dataset: post a known set of events and assert trial
  balance, student balances, cash/bank, and dashboard totals match expected values.

---

## Coverage targets (to be finalized)
Prioritize the financial invariants (B) and license (C) for high coverage; core
workflows (A) for breadth. No coverage number is claimed until measured.
