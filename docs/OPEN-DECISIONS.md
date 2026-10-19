# OPEN DECISIONS — MERQO ShikkhaDesk

Decisions that require the **owner's** input before the product can be finalized.
Each entry states the question, why it matters, the options, and a recommendation.
Nothing here is decided unilaterally where the prompt requires owner approval.

---

## DEC-1 — Product name / trademark review (owner decision required)

- **Question:** Public research indicates at least one similarly named product in
  the Bangladesh education-software market (evidence in
  `COMPETITOR-RESEARCH.md`). Should MERQO proceed with the name **ShikkhaDesk**,
  adjust it, or commission a formal trademark search?
- **Why it matters:** The prompt explicitly forbids silently renaming the product
  and requires a documented naming decision if a material conflict exists.
- **Options:**
  - A. Proceed with **ShikkhaDesk** (accept the risk; proceed to market).
  - B. Run a formal trademark search before launch (recommended for commercial
    distribution).
  - C. Adopt a distinguishing modifier, e.g. **"MERQO ShikkhaDesk"** used
    consistently as the full brand (already the plan) to reduce direct confusion.
- **Recommendation:** C now (consistent full-brand usage) + B before paid
  distribution. **Do not rename without owner approval.**
- **Status:** OPEN — awaiting owner.

## DEC-2 — Final commercial / license terms (owner decision required)

- **Question:** The prompt distinguishes perpetual use, maintenance fixes,
  included updates, future major versions, and optional support, but says the
  owner must approve the final commercial policy and that legal terms must not be
  invented.
- **Options:** Owner supplies the actual terms text (duration of free updates,
  paid-upgrade policy, support scope, refund/transfer policy).
- **Status:** OPEN — the License Manager and About screen will display only
  owner-approved terms. Placeholders are clearly marked, not invented.

## DEC-3 — Production license signing key handling (owner authorization required)

- **Question:** The prompt forbids embedding the production private signing key
  in the customer app and requires a secure seller-controlled signing environment,
  encrypted backup, recovery and rotation — **and owner authorization before a
  production key is created.**
- **Options:**
  - A. Use **test keys only** during development (current stance); production key
    is generated later in an HSM/offline environment under owner authorization.
  - B. Owner designates a specific signing workstation / key custodian now.
- **Recommendation:** A until commercial release; then B under a documented key
  management plan.
- **Status:** OPEN — **no production signing key exists or will be created
  without owner authorization.** Development uses clearly-labeled test keys.

## DEC-4 — Currency symbol / format policy

- **Question:** Default is Bangladeshi Taka (৳ / BDT). Confirm the exact display
  format and whether the customer can change it.
- **Recommendation:** default `৳ #,##0.00`, Gregorian calendar, configurable per
  institution; money stored as integer minor units (see `ACCOUNTING-DESIGN.md`).
- **Status:** OPEN (low risk) — sensible default applied pending confirmation.

## DEC-5 — Scope of v1.0 "Tier C" modules

- **Question:** PART 4 allows staged delivery of advanced modules (multi-branch,
  question banks, certificates/ID cards, library, inventory, portal) if their
  omission is documented and approved.
- **Options:** Owner confirms which Tier-C modules, if any, are in-scope for the
  first commercial release.
- **Status:** OPEN — default plan ships Tier A + full Tier B (core coaching +
  complete double-entry accounting) in v1.0; Tier C staged unless owner says
  otherwise.

---

_Decisions move to a "Resolved" section (with date and rationale) once the owner
answers; they are never removed._
