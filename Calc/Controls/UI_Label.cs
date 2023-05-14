using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    internal class UI_Label : Control
    {
        #region Переменные

        private StringFormat SF = new StringFormat();

        public static int labelWidth;
        public static int labelHeight;

        private const float fontSize = 10F;

        public Point originalLocation;

        public static Color BoxColor;
        public static Color FontColor;

        #endregion

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

            BackColor = Color.FromArgb(32, 32, 32);
            ForeColor = Color.FromArgb(166, 166, 166);

            Font = new Font("Yu Gothic UI", fontSize, FontStyle.Bold);

            SF.Alignment = StringAlignment.Far;
            SF.LineAlignment = StringAlignment.Far;
        }

        #region Отрисовка

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;

            graph.Clear(Parent.BackColor);

            //Resize += new EventHandler(FormResize);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            graph.DrawRectangle(new Pen(BoxColor), rect);
            graph.FillRectangle(new SolidBrush(BoxColor), rect);

            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graph.DrawString(Text, Font, new SolidBrush(FontColor), rect, SF);
        }

        #endregion

        #region События

        protected override void InitLayout()
        {
            base.InitLayout();

            originalLocation = Location;

            labelHeight = Height;
            labelWidth = Width;

            // Я сделал это, чтобы было понятнее, какой цвет за что отвечает
            BoxColor = BackColor;
            FontColor = ForeColor;
        }

        #endregion
    }
}
