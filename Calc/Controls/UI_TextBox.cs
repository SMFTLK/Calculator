using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    public class UI_TextBox : Control
    {
        private StringFormat SF = new StringFormat();

        public UI_TextBox()
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
            ForeColor = Color.White;

            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            Font = new Font("Yu Gothic UI", 40F, FontStyle.Bold);

            SF.Alignment = StringAlignment.Far;
            SF.LineAlignment = StringAlignment.Center;
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

        //private void FormResize(object sender, EventArgs e)
        //{
        //    Location = new Point(Width / 2, Height / 2);
        //}
    }
}
