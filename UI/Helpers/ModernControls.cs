using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace FASTSocietiesSystem.UI.Helpers
{
    public static class ModernControls
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        /// <summary>
        /// Rounds the corners of a control.
        /// </summary>
        public static void SetRoundedCorners(Control control, int radius)
        {
            control.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius));
        }

        /// <summary>
        /// Applies a "Card" style to a panel.
        /// </summary>
        public static void ApplyCardStyle(Panel pnl)
        {
            pnl.BackColor = ThemeManager.Surface;
            SetRoundedCorners(pnl, 15);
            
            pnl.Paint += (s, e) => {
                using (Pen pen = new Pen(pnl.Tag?.ToString() == "Hover" ? ThemeManager.Accent : ThemeManager.Border, 1))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
                }
            };

            pnl.MouseEnter += (s, e) => {
                pnl.Tag = "Hover";
                pnl.BackColor = ThemeManager.SurfaceLight;
                pnl.Invalidate();
            };
            pnl.MouseLeave += (s, e) => {
                pnl.Tag = null;
                pnl.BackColor = ThemeManager.Surface;
                pnl.Invalidate();
            };
        }
    }
}
