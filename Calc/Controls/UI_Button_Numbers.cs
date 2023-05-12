using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    public class UI_Button_Numbers : Control
    {
        private StringFormat SF = new StringFormat();

        private bool MouseEntered = false;

        public UI_Button_Numbers()
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

            BackColor = Color.FromArgb(59, 59, 59);
            ForeColor = Color.White;

            Font = new Font("Yu Gothic UI", 20F, FontStyle.Bold);

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

            // Resize += new EventHandler(FormResize);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            Color BoxColor = BackColor;
            Color FontColor = ForeColor;

            if (!Enabled)
            {
                BoxColor = Color.FromArgb(40, 40, 40);
                FontColor = Color.FromArgb(98, 98, 98);
            }

            if (MouseEntered)
                BoxColor = Color.FromArgb(50, 50, 50);

            graph.DrawRectangle(new Pen(BoxColor), rect);
            graph.FillRectangle(new SolidBrush(BoxColor), rect);

            graph.DrawString(Text, Font, new SolidBrush(FontColor), rect, SF);
        }

        private void FormResize(object sender, EventArgs e)
        {
            Location = new Point(Width / 2, Height / 2);
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
