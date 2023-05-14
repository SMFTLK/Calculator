using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    public class UI_TextBox : Control
    {
        #region Переменные

        private StringFormat SF = new StringFormat();

        public static int textBoxWidth;
        public static int textBoxHeight;

        public const float fontSize = 40F;

        public static Color BoxColor;

        public Point originalLocation;

        #endregion

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

            BackColor = Color.FromArgb(32, 32, 32);
            ForeColor = Color.White;

            Font = new Font("Yu Gothic UI", fontSize, FontStyle.Bold);

            SF.Alignment = StringAlignment.Far;
            SF.LineAlignment = StringAlignment.Center;
        }

        #region Отрисовка

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;

            graph.Clear(Parent.BackColor);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            graph.DrawRectangle(new Pen(BoxColor), rect);
            graph.FillRectangle(new SolidBrush(BoxColor), rect);

            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }

        #endregion

        #region События

        protected override void InitLayout()
        {
            base.InitLayout();

            originalLocation = Location;

            // Я сделал это, чтобы было понятнее, какой цвет за что отвечает
            BoxColor = BackColor;

            textBoxHeight = Height;
            textBoxWidth = Width;
        }

        #endregion
    }
}
