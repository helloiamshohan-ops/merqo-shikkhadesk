# LAN SETUP — MERQO ShikkhaDesk

Host/client operation for one institution across up to five registered computers
(PART 6). Offline-first: all of this works on the local network with **no
internet**.

**Status: design + scaffold (`ShikkhaDesk.LanTransport`). Not built or tested
(needs Windows). LAN concurrency tests are OPEN in `RELEASE-CHECKLIST.md`.**

---

## 1. Modes
- **Single-computer:** the app hosts locally; SQLite on this PC's disk. No other
  computer needed.
- **LAN host:** one computer is the authoritative host. It owns the SQLite file
  and runs the host service. Other computers are clients.
- **LAN client:** connects to the host over the local network.

**Rule:** clients **never** open the SQLite file directly over an SMB share. All
access goes through the authenticated host service.

## 2. Choosing the host
- Pick the computer that stays on during working hours and has reliable disk.
- The host stores the database at a known local path (default under the user's
  data folder; configurable). Back up the host.

## 3. Configuring the host
1. Run ShikkhaDesk → Settings → Host/Client → choose **Host**.
2. Note the host's **LAN IP address** and **port** (default shown in-app).
3. Allow the app through **Windows Firewall** (see §6) — the app can add the rule
   or the wizard shows the exact rule.
4. Start the host service; confirm "Host running" status.

## 4. Configuring a client
1. Install ShikkhaDesk on the client PC.
2. Settings → Host/Client → choose **Client**.
3. Enter the **host address** (e.g. `http://192.168.0.10:port`).
4. **Test connection** — the app reports success/failure with a clear Bangla
   message.
5. Log in with a user account; the host authorizes by role/permission.

## 5. Connection behavior
- **Connection status** is always visible (top bar): connected / reconnecting /
  offline.
- **Timeouts & safe retry:** if a request times out, the client retries with the
  same **idempotency key**; the host deduplicates, so a payment is never
  double-posted. Nothing is reported successful before the host commits.
- **Reconnection & host-restart recovery:** the client reconnects automatically;
  after a host restart it resumes without data loss (the host is authoritative).
- **Concurrent payments/receipts:** the host allocates receipt/payment numbers and
  validates invariants inside a transaction, so two clients never collide.
- **Host migration:** move the SQLite file (via backup/restore) to a new host,
  point clients at the new address. Documented in `BACKUP-RESTORE.md`.

## 6. Windows Firewall (documented requirement)
The host service needs an **inbound** rule on the host's **Private** network:
- App: ShikkhaDesk host executable (or `dotnet`), Protocol TCP, the chosen port,
  Profile: Private. The setup wizard can create this rule (requires one-time
  admin consent) or prints the exact rule for an administrator to add.
- The service is **not** exposed to Public networks / the internet.
- If clients can't connect: verify same subnet, firewall rule, host running, and
  correct address/port.

## 7. Device limit
- Up to **five** registered computers per institution (license `maxDevices`).
- Registration is recorded per device; a 6th is refused with guidance to
  release/replace. **No customer data is deleted** at the limit.

## 8. Troubleshooting (Bangla, in-app)
- Can't find host → check address/port, host running, same network.
- Connection drops → host asleep/off; client reconnects automatically.
- Payment shows pending after timeout → the client resolves it by operation id;
  do not re-enter blindly (the app prevents duplicates).
