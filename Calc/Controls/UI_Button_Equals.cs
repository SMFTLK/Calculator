using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Calc
{
    public class UI_Button_Equals : Control
    {
        private StringFormat SF = new StringFormat();

        private bool MouseEntered = false;

        public UI_Button_Equals()
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

            BackColor = Color.FromArgb(89, 89, 89);
            ForeColor = Color.Black;

            Font = new Font("Yu Gothic", 20F, FontStyle.Regular);

            Anchor = AnchorStyles.Left;

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

            GraphicsPath path = MakeCornersRounded(rect, rect.Height - 80);

            Color BoxColor = BackColor;
            Color FontColor = ForeColor;

            graph.DrawPath(new Pen(BoxColor), path);
            graph.FillPath(new SolidBrush(BoxColor), path);

            if (MouseEntered)
            {
                graph.DrawRectangle(new Pen(Color.FromArgb(30, Color.Black)), rect);
                graph.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Black)), rect);
            }

            graph.DrawString(Text, Font, new SolidBrush(FontColor), rect, SF);
        }

        private GraphicsPath MakeCornersRounded(Rectangle figure, int cornerSize)
        {
            GraphicsPath path = new GraphicsPath();

            // Левая верхняя арка
            path.AddArc(figure.X, figure.Y, cornerSize, cornerSize, 180, 90);

            // Правая верхняя арка
            path.AddArc(figure.X + figure.Width - cornerSize, figure.Y, cornerSize, cornerSize, 270, 90);

            // Левая нижняя арка
            path.AddArc(figure.X + figure.Width - cornerSize, figure.Y + figure.Height - cornerSize, cornerSize, cornerSize, 0, 90);

            // Правая нижняя арка
            path.AddArc(figure.X, figure.Y + figure.Height - cornerSize, cornerSize, cornerSize, 90, 90);

            path.CloseFigure();

            return path;
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
