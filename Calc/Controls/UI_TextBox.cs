﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    public class UI_TextBox : Control
    {
        #region Переменные

        private StringFormat SF = new StringFormat();

        public static int textBoxWidth = 626;
        public static int textBoxHeight = 125;

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

            Size = new Size(textBoxWidth, textBoxHeight);

            BackColor = Color.FromArgb(32, 32, 32);
            ForeColor = Color.White;

            Font = new Font("Yu Gothic UI", 40F, FontStyle.Bold);

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

            //Resize += new EventHandler(FormResize);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            graph.DrawRectangle(new Pen(BackColor), rect);
            graph.FillRectangle(new SolidBrush(BackColor), rect);

            graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }

        #endregion

        #region События

        protected override void InitLayout()
        {
            base.InitLayout();

            originalLocation = Location;
        }

        #endregion
    }
}
