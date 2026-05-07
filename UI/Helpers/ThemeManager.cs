using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace FASTSocietiesSystem.UI.Helpers
{
    /// <summary>
    /// Central design system manager for the "Midnight Editorial" theme.
    /// Provides consistent colors, fonts, and styling logic.
    /// </summary>
    public static class ThemeManager
    {
        // Brand Palette - "Midnight Editorial"
        public static readonly Color Background = Color.FromArgb(15, 17, 26);    // Deep Midnight
        public static readonly Color Surface = Color.FromArgb(26, 29, 46);       // Deep Slate
        public static readonly Color SurfaceLight = Color.FromArgb(40, 44, 68);  // Muted Slate
        public static readonly Color Accent = Color.FromArgb(0, 212, 255);       // Electric Cyan
        public static readonly Color AccentHover = Color.FromArgb(0, 180, 220);  
        public static readonly Color TextPrimary = Color.White;
        public static readonly Color TextSecondary = Color.FromArgb(143, 155, 179); // Muted Grey
        public static readonly Color Border = Color.FromArgb(45, 50, 80);

        // Typography
        public static readonly Font TitleFont = new Font("Trebuchet MS", 24, FontStyle.Bold);
        public static readonly Font HeaderFont = new Font("Trebuchet MS", 16, FontStyle.Bold);
        public static readonly Font SubHeaderFont = new Font("Trebuchet MS", 12, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 10);
        public static readonly Font SmallFont = new Font("Segoe UI", 9);

        /// <summary>
        /// Applies the global theme to a form and all its children.
        /// </summary>
        public static void ApplyTheme(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = TextPrimary;
            form.Font = BodyFont;

            foreach (Control control in form.Controls)
            {
                ApplyToControl(control);
            }
        }

        private static void ApplyToControl(Control control)
        {
            if (control is Button btn)
            {
                StyleButton(btn);
            }
            else if (control is TextBox tb)
            {
                StyleTextBox(tb);
            }
            else if (control is Label lbl)
            {
                if (lbl.Font.Size > 18) lbl.Font = TitleFont;
                else if (lbl.Font.Size > 12) lbl.Font = HeaderFont;
                else lbl.Font = BodyFont;

                if (lbl.ForeColor == Color.DarkBlue || lbl.ForeColor == Color.Black)
                    lbl.ForeColor = TextPrimary;
            }
            else if (control is Panel pnl)
            {
                // Recursive apply
                foreach (Control child in pnl.Controls)
                {
                    ApplyToControl(child);
                }
            }
        }

        public static void StyleButton(Button btn, bool isPrimary = true)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = isPrimary ? Accent : Surface;
            btn.ForeColor = isPrimary ? Background : TextPrimary;
            btn.Cursor = Cursors.Hand;
            btn.Font = SubHeaderFont;

            // Simple hover effect
            btn.MouseEnter += (s, e) => {
                btn.BackColor = isPrimary ? AccentHover : SurfaceLight;
            };
            btn.MouseLeave += (s, e) => {
                btn.BackColor = isPrimary ? Accent : Surface;
            };
        }

        public static void StyleTextBox(TextBox tb)
        {
            tb.BackColor = Surface;
            tb.ForeColor = TextPrimary;
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.Font = BodyFont;
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Background;
            grid.GridColor = Border;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Surface;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Accent;
            grid.ColumnHeadersDefaultCellStyle.Font = SubHeaderFont;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersHeight = 55;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.EnableHeadersVisualStyles = false;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Accent;
            grid.DefaultCellStyle.SelectionForeColor = Background;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowTemplate.Height = 35;
        }

        public static void StyleSidebarButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            btn.ForeColor = TextSecondary;
            btn.Cursor = Cursors.Hand;
            btn.Font = SubHeaderFont;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(30, 0, 0, 0);

            // Hover effect: Change text to Accent and background to SurfaceLight
            btn.MouseEnter += (s, e) => {
                btn.ForeColor = Accent;
                btn.BackColor = SurfaceLight;
            };
            btn.MouseLeave += (s, e) => {
                btn.ForeColor = TextSecondary;
                btn.BackColor = Color.Transparent;
            };
        }

        /// <summary>
        /// Creates a gradient panel for branding.
        /// </summary>
        public static void MakeGradientPanel(Panel pnl)
        {
            pnl.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    pnl.ClientRectangle,
                    Background,
                    Surface,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, pnl.ClientRectangle);
                }
            };
        }
    }
}
