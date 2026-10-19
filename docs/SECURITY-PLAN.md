# SECURITY PLAN — MERQO ShikkhaDesk

PART 26. Security properties, controls, and what is explicitly **not** claimed.
Controls are designed; the implemented code is authored but **not yet built or
tested** (see `PROJECT-STATE.md`).

---

## 1. Authentication & credentials
- Secure password hashing with a per-user random salt using a maintained KDF
  (PBKDF2-HMAC-SHA256, ≥ 210,000 iterations, or Argon2id where available). Never
  store plaintext or reversible passwords.
- Session management: server(hash)-issued session tokens with expiry and revocation;
  logout invalidates the session.
- Failed-login handling: count failures, temporary lockout/backoff, audit the
  events. No user enumeration in error messages.
- Password change requires the current password (or admin reset that forces a
  change on next login). **No public/default administrator password** — the
  first-run wizard forces the owner to set one.

## 2. Authorization
- Role-based + granular permissions (View/Create/Edit/Approve/Reverse/Print/
  Export/Restore/Manage users/Manage accounts/Manage devices).
- **Enforced in the Application/domain service layer** on every operation — not by
  hiding UI controls. UI hiding is a convenience, never the control.
- Sensitive data (salary fields) restricted to authorized roles at the query layer.

## 3. Data protection
- **Parameterized queries only** (ORM or prepared statements). No SQL string
  concatenation with user input.
- Input validation and length/type constraints at the domain boundary.
- Safe file handling: validate paths for backup/restore/import/export; restrict
  restore to authorized users with confirmation and a pre-restore backup.
- The authoritative SQLite file lives on the institution's own disk; **no customer
  data is transmitted to MERQO**. No analytics/phone-home of customer content.

## 4. LAN transport
- Clients authenticate to the host; requests authorized per permission.
- Timeouts, safe retry, idempotency keys for payments/receipts.
- The host binds to the LAN and is protected by **Windows Firewall** rules
  (documented in `LAN-SETUP.md`); the service is not exposed to the internet.
- Transport integrity/confidentiality appropriate to a trusted LAN; secrets never
  logged.

## 5. Licensing security
- Asymmetric signatures (ECDSA P-256 or RSA) via maintained .NET crypto. **No
  custom cryptography.**
- Customer app verifies with the **public key** only. **The production private
  signing key is never embedded in the customer app or the public repo.**
- Institution binding via immutable InstitutionUid; name changes don't transfer a
  license. See `LICENSE-DESIGN.md`.
- Test keys are clearly labeled and distinct from any production key; **no
  production key is created without owner authorization (DEC-3).**

## 6. Audit & logging
- Append-only audit log for financial and administrative actions (who/what/when/
  before/after). Ordinary users cannot delete posted records or audit history.
- Logs are safe: **never log passwords, session tokens, or signing keys**; minimize
  customer personal data in logs; configurable levels; local storage only.

## 7. Dependency & secret hygiene
- No embedded secrets in source or config. Keys/certs excluded via `.gitignore`
  (`*.pfx`, `*.key`, `secrets/`). Dependency review before release; pin versions.
- No production private key in the client; signing happens in a seller-controlled
  environment (see `LICENSE-MANAGER-GUIDE.md`).

## 8. Explicit non-claims
- The product is **not** claimed to be digitally signed by a CA until a real valid
  signature exists (see `RELEASE-CHECKLIST.md`). SmartScreen warnings may appear on
  unsigned builds; this is disclosed, not hidden.
- No claim of compliance with any specific data-protection statute; the owner must
  approve any such statement.
- Offline license revocation is inherently limited (documented limitation).

## 9. Threat model summary (high level)
- Local DB theft → mitigate with file permissions + optional at-rest encryption
  (documented as optional; keys managed by institution).
- LAN eavesdropping on trusted network → authenticated API; document firewall.
- Malicious client → server-side authorization + invariants; host is authoritative.
- License forgery → asymmetric signature + institution binding + device limits.
- Insider financial fraud → audit log, immutable posted entries, reversal-only
  corrections, approval workflow.
