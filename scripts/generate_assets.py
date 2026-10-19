#!/usr/bin/env python3
"""
MERQO ShikkhaDesk — brand asset generator.

Generates an ORIGINAL logo mark (open book + verification check), multi-resolution
PNG rasters, light/dark lockups, and a GENUINE multi-resolution Windows .ico
(assembled from real embedded PNG images at each size — NOT a renamed PNG).

Design direction: open book (education) + check mark (verified records),
geometric silhouette, professional business-software look, clear at small sizes.

Run:  python3 scripts/generate_assets.py
Requires: Pillow (already a dev-only dependency; the product itself does not).
"""
import os
from PIL import Image, ImageDraw, ImageFont

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ICONS = os.path.join(ROOT, "assets", "icons")
LOGO = os.path.join(ROOT, "assets", "logo")

# Brand palette (PART 1.3)
NAVY = (16, 42, 67, 255)        # #102A43 primary navy
NAVY2 = (36, 59, 83, 255)       # #243B53 secondary navy
TEAL = (37, 194, 160, 255)      # #25C2A0 primary teal
DARKTEAL = (19, 138, 114, 255)  # #138A72 dark teal
BG = (245, 247, 250, 255)       # #F5F7FA
WHITE = (255, 255, 255, 255)
LIGHT_INK = (233, 238, 245, 255)

DEJAVU_BOLD = "/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf"
DEJAVU = "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf"

SS = 4  # supersample factor for anti-aliasing


def draw_mark(size, book_color=NAVY, page2_color=NAVY2, check_color=TEAL,
              accent_color=TEAL):
    """Draw the ShikkhaDesk mark (open book + check) on a transparent RGBA image.

    For small sizes the fine page-edge highlights are dropped and strokes are
    thickened so the mark stays crisp and legible (e.g. 16px taskbar icon)."""
    S = size * SS
    simple = size <= 40  # small-size simplification
    img = Image.new("RGBA", (S, S), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)

    def P(nx, ny):
        return (nx * S, ny * S)

    # --- Open book silhouette (symmetric, center dip at top) ---
    spine = (0.50, 0.84)
    topc = (0.50, 0.60)
    # Left page
    left = [P(*topc), P(0.09, 0.44), P(0.09, 0.68), P(*spine)]
    # Right page
    right = [P(*topc), P(0.91, 0.44), P(0.91, 0.68), P(*spine)]
    d.polygon(left, fill=page2_color)
    d.polygon(right, fill=book_color)

    if not simple:
        # Inner page-edge highlights (thin lighter lines) for a printed-page feel
        edge = (233, 238, 245, 90)
        lw = max(1, int(S * 0.012))
        d.line([P(0.115, 0.475), P(0.475, 0.615)], fill=edge, width=lw)
        d.line([P(0.885, 0.475), P(0.525, 0.615)], fill=edge, width=lw)

    # --- Verification check mark (bold, on the right page) ---
    cw = max(2, int(S * (0.095 if simple else 0.075)))  # check stroke width
    d.line([P(0.585, 0.610), P(0.650, 0.685)], fill=check_color, width=cw)
    d.line([P(0.650, 0.685), P(0.805, 0.500)], fill=check_color, width=cw)
    # rounded joins
    r = cw // 2
    for pt in [(0.585, 0.610), (0.650, 0.685), (0.805, 0.500)]:
        d.ellipse([P(pt[0], pt[1])[0] - r, P(pt[0], pt[1])[1] - r,
                   P(pt[0], pt[1])[0] + r, P(pt[0], pt[1])[1] + r], fill=check_color)

    # --- Teal accent bar (verified foundation) beneath the book ---
    bar_h = S * (0.05 if simple else 0.045)
    bar_y = S * 0.90
    m = S * 0.14
    d.rounded_rectangle([m, bar_y, S - m, bar_y + bar_h],
                        radius=bar_h / 2, fill=accent_color)

    return img.resize((size, size), Image.LANCZOS)


def make_lockup(width, dark=False):
    """Horizontal lockup: mark + 'ShikkhaDesk' wordmark + 'A MERQO Product'."""
    mark = draw_mark(256, book_color=(WHITE if dark else NAVY),
                     page2_color=(LIGHT_INK if dark else NAVY2),
                     check_color=TEAL, accent_color=TEAL)
    pad = 24
    mark_size = 256
    title_font = ImageFont.truetype(DEJAVU_BOLD, 108)
    sub_font = ImageFont.truetype(DEJAVU, 40)
    title = "ShikkhaDesk"
    sub = "A MERQO Product"
    tb = title_font.getbbox(title)
    sb = sub_font.getbbox(sub)
    text_w = max(tb[2] - tb[0], sb[2] - sb[0])
    text_h = (tb[3] - tb[1]) + (sb[3] - sb[1]) + 28
    W = pad + mark_size + 34 + text_w + pad
    H = max(mark_size, text_h) + pad * 2
    bgc = NAVY if dark else (0, 0, 0, 0)
    img = Image.new("RGBA", (W, H), bgc)
    img.paste(mark, (pad, (H - mark_size) // 2), mark)
    d = ImageDraw.Draw(img)
    tx = pad + mark_size + 34
    title_color = WHITE if dark else NAVY
    sub_color = TEAL
    total_text_h = (tb[3] - tb[1]) + 28 + (sb[3] - sb[1])
    ty = (H - total_text_h) // 2
    d.text((tx, ty - tb[1]), title, font=title_font, fill=title_color)
    d.text((tx, ty + (tb[3] - tb[1]) + 28 - sb[1]), sub, font=sub_font, fill=sub_color)
    return img


def main():
    os.makedirs(ICONS, exist_ok=True)
    os.makedirs(LOGO, exist_ok=True)

    # 1) Multi-resolution PNG rasters of the mark (light background use)
    sizes = [16, 24, 32, 48, 64, 128, 256]
    for s in sizes:
        draw_mark(s).save(os.path.join(ICONS, f"mark-{s}.png"))

    # 2) Primary mark + dark-background variant
    draw_mark(512).save(os.path.join(LOGO, "logo-mark.png"))
    draw_mark(512, book_color=WHITE, page2_color=LIGHT_INK).save(
        os.path.join(LOGO, "logo-mark-dark.png"))

    # 3) Horizontal lockups (light + dark)
    make_lockup(1024, dark=False).save(os.path.join(LOGO, "logo-light.png"))
    make_lockup(1024, dark=True).save(os.path.join(LOGO, "logo-dark.png"))

    # 4) GENUINE multi-resolution Windows .ico (embedded PNG per size)
    base = draw_mark(256)
    ico_path = os.path.join(ICONS, "shikkhadesk.ico")
    base.save(ico_path, format="ICO",
              sizes=[(16, 16), (24, 24), (32, 32), (48, 48), (64, 64),
                     (128, 128), (256, 256)])

    print("Generated:")
    for f in sorted(os.listdir(ICONS)):
        print("  assets/icons/" + f)
    for f in sorted(os.listdir(LOGO)):
        print("  assets/logo/" + f)


if __name__ == "__main__":
    main()
