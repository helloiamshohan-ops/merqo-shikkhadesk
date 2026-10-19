# LICENSE MANAGER GUIDE — MERQO ShikkhaDesk

The **MERQO ShikkhaDesk License Manager** is a **separate, private, seller-side
Windows application** (PART 21). It is **not** distributed to customers. It
creates, signs, verifies, exports and registers institution-bound perpetual-use
licenses, and maintains the private license register.

**Status: designed; scaffolded as `src/ShikkhaDesk.LicenseManager`. Not built
(needs Windows/.NET). No production signing key exists or will be created without
owner authorization (DEC-3).**

---

## 1. Identity (must match the customer app & docs)
- App name: **MERQO ShikkhaDesk License Manager**
- Brand: MERQO · Product: ShikkhaDesk · Contact: merqoonline@gmail.com
- Same About identity as the customer app (PART 1.2).

## 2. Core functions (PART 21.1)
Create institution license · generate immutable institution ID · generate unique
license ID · enter institution name · optional contact · set device limit · set
lifetime-use entitlement · set product-version entitlement · sign license · export
license file · print license certificate · verify license · maintain private
license register · record sales reference · record issue/export history · back up
& restore the register · view license details · manage device-replacement records ·
view audit history.

## 3. License creation workflow (PART 21.2)
1. Enter institution name.
2. Generate or select the immutable institution ID (GUID).
3. Enter optional institution details.
4. Set product edition.
5. Set perpetual-use entitlement.
6. Set version entitlement.
7. Set maximum registered computers (default 5).
8. Review the license summary.
9. Generate and sign the license (ECDSA P-256/SHA-256; canonical payload — see
   `LICENSE-DESIGN.md`).
10. Verify the signature independently (re-verify with the public key before
    export).
11. Export the license file (customer deliverable).
12. Generate a customer instruction sheet (how to activate, in Bangla).
13. Save the license record in the register.

> Creating a license is **not** the same as activating it. Activation happens on
> the customer side after verification + institution binding + device registration.

## 4. Private-key protection (PART 21.4)
- The production private signing key is kept **outside** the customer app and
  **outside** the public repository, in a seller-controlled signing environment
  (dedicated offline machine or HSM), access-restricted.
- **Encrypted backup** of the key with documented **recovery** and **rotation**
  procedures. **Never log the private key. Never distribute it with customer
  files.**
- Development/tests use **clearly-labeled test keys** only. **No production key is
  created without the owner's authorization (DEC-3).**

## 5. Register (private)
- Stores: license id, institution uid & name, entitlements, device limit, sales
  reference, issue/export history, device-replacement records, audit history.
- Register can be **backed up and restored** (encrypted). The register is the
  seller's record of what was issued; it is not required for the customer app to
  verify a license (verification is offline via the public key).

## 6. Customer deliverables
- The signed **license file** (payload + signature).
- A Bangla **instruction sheet** (select file → verify → activate; device
  registration; support contact merqoonline@gmail.com).

## 7. Build/distribution
- Separate installer: `ShikkhaDesk-LicenseManager-Setup-vX.Y.Z-x64.exe` (naming
  convention; must be actually built on Windows before release).
- **Never bundled with the customer installer.** Distributed only to the seller.
