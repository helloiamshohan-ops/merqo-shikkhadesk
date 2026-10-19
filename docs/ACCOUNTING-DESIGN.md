# ACCOUNTING DESIGN — MERQO ShikkhaDesk

The double-entry engine is a **core product module** (PART 17). Money is integer
poisha. Every posted journal balances. This document defines the model, the
posting rules for each business event, the statements, and the invariants that
must be tested. **The engine is designed and scaffolded in
`ShikkhaDesk.Accounting`; it is not yet compiled or tested (see
`PROJECT-STATE.md`) — no financial correctness is claimed until tests run.**

---

## 1. Chart of accounts (PART 17.1)

Categories: **Assets, Liabilities, Equity/Capital, Income, Expenses.** Configurable
code/name/category/active. Sensible starter accounts include:

Assets: Cash on Hand (per till), Bank accounts, Student Fee Receivable (অনুপস্থিত
বকেয়া), Advance Credit is a **liability** (see below), Other Receivable.
Liabilities: Student Advance Credit (অগ্রিম জমা — money held for future fees, an
obligation), Supplier Payable (দেয়ানি), Salary Payable.
Equity: Owner's Capital, Retained Earnings.
Income: Tuition/Fee Income, Admission Fee Income, Other Income.
Expenses: Salary Expense, Rent, Utilities, Stationery, Bank Charges, Other Expense.

> **Advance credit is a liability**, not income — it is money received but not yet
> earned. It becomes income only when allocated to a charge.

---

## 2. Journal engine (PART 17.2)

- A `JournalEntry` has a unique `JournalNo`, date, description, source reference,
  status (Draft/Posted/Reversed), and ≥2 `JournalLine`s.
- **Invariant: total debits = total credits.** Enforced before posting, inside one
  DB transaction. Unbalanced entries cannot be posted.
- Posted entries are **immutable**; corrections are **reversal** entries that
  reference the original (`ReversesJournalId`). No silent edits.
- Optional approval workflow per entry where configured.
- Sign convention: each line is either a debit or a credit (never both); amounts ≥ 0.

---

## 3. Posting rules per business event

Each event creates one balanced journal. Source references link back to the
originating record for audit and reconciliation.

### 3.1 Fee assessment (charge raised) — not yet cash
- Dr **Student Fee Receivable** (asset ↑)
- Cr **Fee/Tuition Income** (income ↑)
- *Only when the assessment is posted.* Draft assessments post nothing.

### 3.2 Fee payment (cash received, allocated to charges)
- Dr **Cash/Bank** (asset ↑)
- Cr **Student Fee Receivable** (asset ↓)
- This settles the receivable; it is **not** new income (income was recognized at
  assessment). Prevents counting fees twice.

### 3.3 Advance payment (cash received, not yet allocated)
- Dr **Cash/Bank** (asset ↑)
- Cr **Student Advance Credit** (liability ↑)
- When later allocated to a charge: Dr **Advance Credit**, Cr **Fee Receivable**
  (moves the obligation into settling the receivable). Income was already booked
  at assessment; allocation never re-books income.

### 3.4 Refund / reversal
- Reverse the specific source journal (reference it). A refund of an allocated
  payment debits the receivable (or advance credit) and credits cash/bank — never
  exceeding the refundable amount, never double-refunding.

### 3.5 Income (non-fee)
- Dr **Cash/Bank**
- Cr **Income category**

### 3.6 Expense
- Dr **Expense category**
- Cr **Cash/Bank** (paid now) **or** Cr **Supplier Payable** (on credit)

### 3.7 Transfer between own cash/bank accounts (PART 17.4)
- Dr **Destination Cash/Bank**
- Cr **Source Cash/Bank**
- **Never touches Income/Expense.** Transfers cannot inflate revenue.

### 3.8 Payable settlement (PART 17.5)
- The expense was booked when the bill was recorded (3.6 on credit). Settlement:
- Dr **Supplier Payable**
- Cr **Cash/Bank**
- **No second expense.** Prevents double-counting.

### 3.9 Payroll (PART 17.6)
- Accrual: Dr **Salary Expense**, Cr **Salary Payable**.
- Payment: Dr **Salary Payable**, Cr **Cash/Bank**.
- Unique per (staff, period) absent an authorized adjustment; no duplicate posting.

---

## 4. Cash & bank (PART 17.4)

Multiple `FinancialAccount`s (cash tills, bank accounts) each linked to a CoA
account, with opening balances. Running balance = opening + Σ(receipts in) −
Σ(payments out) + Σ(transfers in) − Σ(transfers out). Cashbook/Bankbook are the
per-account ledgers. Reconciliation compares the book balance to a statement
balance and records exceptions; bank charges are an expense (Dr Bank Charges, Cr
Bank).

---

## 5. Receivables & payables (PART 17.5)

- **Student fee receivables** = Σ(posted charges) − Σ(allocations) − Σ(refunds),
  per student. This must reconcile to the Student Fee Receivable control account.
- **Supplier payables** = Σ(bills) − Σ(settlements), per supplier, reconciling to
  the Supplier Payable control account. Aging by due date.

---

## 6. Financial statements (PART 17.7)

- **Trial balance:** every account with Σdebits, Σcredits, net; total debits =
  total credits.
- **Profit & Loss:** Income − Expenses over a period.
- **Balance sheet:** Assets = Liabilities + Equity at a date. Advance credit sits
  in liabilities.
- **Cash-flow:** movement in cash/bank split operating/transfer, derived from the
  ledger (no fabricated figures).
- **General ledger / account ledger:** posted lines per account/period.
- **Receivables / advance-credit liabilities / payables / payroll summary /
  reconciliation exceptions** as listed in REQUIREMENTS.
- **No legal/VAT/statutory compliance is claimed** (DEC / KNOWN-ISSUES). Periods
  and assumptions are explicit.

---

## 7. Financial invariants to TEST (PART 17.8 / PART 27)

Each is a named test in `ShikkhaDesk.Accounting.Tests` (scaffolded; must pass
before release):

1. Every posted journal balances (Σ debit = Σ credit).
2. Payments cannot be over-allocated (Σ allocations ≤ available).
3. Credit cannot be used twice (advance available never double-spent).
4. Refunds cannot duplicate allocations.
5. Transfers do not inflate revenue (Income/Expense untouched).
6. Payable settlements do not duplicate expenses.
7. Receipts cannot create duplicate payments (idempotency).
8. Reversals reference their source and restore balances.
9. Student balances reconcile to the ledger control account.
10. Cash and bank balances reconcile to their ledger accounts.
11. Payroll reconciles (expense = payable movement = payments).
12. Dashboard totals match reports.
13. Historical reports remain reproducible (posted data immutable).

**Status: designed and scaffolded; 0 executed.** These gates are OPEN in
`RELEASE-CHECKLIST.md` until `dotnet test` passes on a real .NET runtime.
