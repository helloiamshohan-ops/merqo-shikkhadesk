# BRAND GUIDELINES — MERQO ShikkhaDesk

**Owner:** MERQO · **Product:** ShikkhaDesk · **Contact:** merqoonline@gmail.com
These guidelines define the original visual identity created for ShikkhaDesk.
The logo is an **original design** for this project; it is **not** a registered
trademark (see naming decision DEC-1).

---

## 1. Identity

- **Parent brand:** MERQO
- **Product:** ShikkhaDesk
- **Full name (always use in full where space allows):** MERQO ShikkhaDesk
- **Category:** Coaching Center Management & Accounts
- **Tagline (Bengali):** কোচিং সেন্টারের সব কাজ, এক সফটওয়্যার।
- **English gloss (for reference, not a slogan replacement):** "All coaching-center work, in one software."

Never invent a company address, phone, website, registration number or social
account. The only official contact is **merqoonline@gmail.com**. Do not claim a
legal corporate status that has not been verified.

---

## 2. Logo

**Concept:** an **open book** (education) carrying a bold **check mark** (verified
records), resting on a teal **accent bar** (a verified foundation). Geometric,
single-glance, professional business-software appearance; legible from 16px to
print.

**Construction rules**
- Keep clear space around the mark of at least the height of the accent bar.
- Do not stretch, rotate, add effects (shadows/bevels/gradients beyond the two
  flat page tones), recolor outside the palette, or place on low-contrast backgrounds.
- Minimum sizes: mark 16px (taskbar), lockup height 24px.

**Variants (in `assets/`)**
| File | Use |
|------|-----|
| `logo/mark.svg`, `logo/mark.svg` (square) | vector source, app icon / splash |
| `logo/logo.svg` | vector horizontal lockup (mark + wordmark + tagline) |
| `logo/logo-light.png` | lockup for light backgrounds |
| `logo/logo-dark.png` | lockup for dark/navy backgrounds |
| `logo/logo-mark.png` / `logo-mark-dark.png` | raster mark (light / dark) |
| `icons/mark-16…256.png` | raster mark at standard sizes |
| `icons/shikkhadesk.ico` | **genuine multi-resolution Windows icon** (16–256) |
| `icons/icon-contact-sheet.png` | all sizes on one sheet (reference) |

The `.ico` is a **real multi-resolution Windows icon** assembled from embedded
PNG images at 16/24/32/48/64/128/256px (verified: ICONDIR type=1, 7 entries). It
is **not** a renamed PNG.

---

## 3. Color palette

| Role | Name | Hex |
|------|------|-----|
| Primary | Primary navy | `#102A43` |
| Secondary | Secondary navy | `#243B53` |
| Primary | Primary teal | `#25C2A0` |
| Dark | Dark teal | `#138A72` |
| Background | Background | `#F5F7FA` |
| Surface | Surface | `#FFFFFF` |
| Text | Main text | `#172B4D` |
| Text | Secondary text | `#627D98` |
| Border | Border | `#D9E2EC` |
| Status | Success | `#16803C` |
| Status | Warning | `#B7791F` |
| Status | Error | `#C53030` |
| Accent | Accent | `#FFB547` |

**Usage:** navy for structure/headers/text; teal for primary actions and the
brand accent; background `#F5F7FA` behind white surfaces; status colors only for
status. Do not introduce off-palette colors in the product UI.

---

## 4. Typography

- **Bengali (UI + documents):** **Noto Sans Bengali** (SIL Open Font License),
  bundled at `assets/fonts/NotoSansBengali.ttf` and `NotoSansBengaliUI.ttf`.
  Proper Unicode shaping is provided by the WPF/DirectWrite text stack at runtime.
- **Latin/UI fallback:** Segoe UI (Windows default) / Noto Sans.
- Financial figures must use tabular/monospaced-aligned digits where available so
  columns line up; respect the global Bengali/English digit preference.

---

## 5. Voice & language

Simple, natural Bengali (সহজ বাংলা). Consistent terms from
`BANGLA-TERMINOLOGY.md`. No unnecessary technical English. Do not claim to be
"the best" — describe measurable features only.

---

## 6. Application & document usage

- **App icon:** `icons/shikkhadesk.ico` (taskbar, window, installer).
- **Splash / About:** mark + "ShikkhaDesk" + "A MERQO Product" + contact.
- **Receipts / statements / reports header:** mark + institution name + license
  id; footer shows "MERQO ShikkhaDesk · merqoonline@gmail.com".
- **Installer:** MERQO brand, ShikkhaDesk product name, genuine icon, version.
