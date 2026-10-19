# LICENSE DESIGN — MERQO ShikkhaDesk

Signed, institution-bound, offline-activatable, perpetual-use licenses (PART 21.3–
21.7, 22). **No custom cryptography.** The customer app verifies with a public
key; the private signing key lives only in the seller-side License Manager
environment and is **never** embedded in the customer app or repo.

**Status: designed and scaffolded in `ShikkhaDesk.Licensing`; not yet compiled or
tested.** No license behavior is claimed to work until tests run.

---

## 1. Algorithm & keys
- **Signature:** ECDSA with **P-256 + SHA-256** (ES256) via .NET's maintained
  `System.Security.Cryptography` (RSA-PSS-SHA256 is an acceptable alternative).
  Chosen for small signatures and wide, well-maintained support.
- **Key pairs:** a **production** key pair (custodian-controlled, offline) and a
  clearly-labeled **test** key pair used only in development/tests. Different
  `SigningKeyId` values distinguish them.
- **Public key** ships in the customer app for verification. **Private key never
  ships** with customer files, is never logged, and is created only under owner
  authorization (DEC-3).

## 2. Canonical license payload (versioned)

A license is a canonical JSON payload + a detached signature. Canonicalization =
UTF-8, sorted keys, no insignificant whitespace, so signatures are stable.

```json
{
  "format": "MERQO-ShikkhaDesk-License",
  "version": 1,
  "licenseId": "LIC-XXXXXXXX",
  "productId": "MERQO.ShikkhaDesk",
  "institutionUid": "immutable-guid",
  "institutionName": "Licensed institution name",
  "licenseType": "Perpetual",
  "maxDevices": 5,
  "versionEntitlement": "1.x",
  "issuedAt": "2026-10-19T00:00:00Z",
  "signingKeyId": "test-key-2026"
}
```

Signature = `Base64Url(ECDSA-SHA256(canonicalPayloadBytes))`. The exported license
file wraps `{ "payload": <canonical string>, "signature": "...", "signingKeyId":
"..." }`.

## 3. Customer-side verification (PART 22)
On activation and at startup, the app:
1. Parses the file; rejects malformed JSON.
2. Re-canonicalizes the payload and verifies the signature against the **public
   key** matching `signingKeyId`.
3. Checks `productId == MERQO.ShikkhaDesk`.
4. Checks `institutionUid == Institution.InstitutionUid` (institution binding).
5. Checks entitlements (version, device count) and license status.
6. Records the activated license locally; works **fully offline**.

**Rejects:** altered payloads, invalid signatures, wrong product id, wrong
institution uid, institution-identity mismatch, unsupported entitlements, invalid
device registration, malformed files. Clear Bangla error for each.

## 4. Institution binding (PART 21.5)
- The institution has an **immutable `InstitutionUid`** created at first run. A
  license is valid only for that uid.
- **Changing the visible institution name does not transfer the license** — the
  uid is what binds.
- Genuine name correction: an authorized workflow that preserves the uid and audit
  history (admin-only, audited). A *different* institution follows the owner's
  license-transfer policy (DEC-2); the app does not silently re-bind.

## 5. Device registration & limit (PART 21.6)
- Default **max 5 registered computers** per institution (from `maxDevices`).
- Each computer gets a `DeviceUid`; registration records name/status/history.
- Limit enforcement: a 6th registration is refused with a clear Bangla message and
  instructions to release/replace a device via the owner — **customer data is never
  deleted** when the limit is reached.
- Device **replacement/recovery**: mark old device Replaced/Revoked, register the
  new one within the limit. Offline revocation is inherently limited (a device that
  never contacts the host can't be force-stopped) — documented, not hidden.

## 6. Perpetual-use terms (PART 21.7)
Distinguish and let the owner define (DEC-2): perpetual use of the licensed
version; maintenance fixes; included updates; future major versions; optional
support. **A valid perpetual license must not require periodic online checks to
remain usable.** The app does not phone home to stay functional.

## 7. What is NOT claimed
- No claim the license is "activated" merely because it was created (creation ≠
  activation; activation = verified + bound + device-registered on the customer
  side).
- No CA/authenticode code-signing claim here (separate; see SECURITY-PLAN §8).
- No invented legal terms — final commercial/license wording is owner-approved.
