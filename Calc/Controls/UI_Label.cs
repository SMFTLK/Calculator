using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    internal class UI_Label : Control
    {
        private StringFormat SF = new StringFormat();

        public UI_Label()
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

            BackColor = Color.FromArgb(32, 32, 32);
            ForeColor = Color.FromArgb(166, 166, 166);

            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right; 

            Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);

            SF.Alignment = StringAlignment.Far;
            SF.LineAlignment = StringAlignment.Far;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;

            graph.Clear(Parent.BackColor);

            //Resize += new EventHandler(FormResize);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            graph.DrawRectangle(new Pen(BackColor), rect);
            graph.FillRectangle(new SolidBrush(BackColor), rect);

            graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }


    }
}
