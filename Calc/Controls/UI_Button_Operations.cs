using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    public class UI_Button_Operations : Control
    {
        private StringFormat SF = new StringFormat();

        private bool MouseEntered = false;

        public UI_Button_Operations()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true
            );
            DoubleBuffered = true;

            Size = new Size(155, 90);

            BackColor = Color.FromArgb(50, 50, 50);
            ForeColor = Color.White;

            Font = new Font("Yu Gothic", 20F, FontStyle.Regular);

            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;

            graph.Clear(Parent.BackColor);
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            Color BoxColor = BackColor;
            Color FontColor = ForeColor;

            if (!Enabled)
            {
                BoxColor = Color.FromArgb(40, 40, 40);
                FontColor = Color.FromArgb(98, 98, 98);
            }

            if (MouseEntered)
                BoxColor = Color.FromArgb(59, 59, 59);

            graph.DrawRectangle(new Pen(BoxColor), rect);
            graph.FillRectangle(new SolidBrush(BoxColor), rect);

            graph.DrawString(Text, Font, new SolidBrush(FontColor), rect, SF);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            MouseEntered = true;

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            MouseEntered = false;

            Invalidate();
        }
    }
}
