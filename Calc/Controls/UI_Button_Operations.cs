﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    public class UI_Button_Operations : Control
    {
        #region Переменные

        private StringFormat SF;

        public static int buttonWidth = 155;
        public static int buttonHeight = 90;

        private int cornerSize = 10;

        private bool MouseEntered;

        private Rectangle rect;

        public Point originalLocation;

        private Color BoxColor;
        private Color FontColor;
        private Color BorderColor;

        #endregion

        public UI_Button_Operations()
        {
            SF = new StringFormat();
            MouseEntered = false;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true
            );
            DoubleBuffered = true;

            AutoSize = true;

            Size = new Size(buttonWidth, buttonHeight);

            BackColor = Color.FromArgb(50, 50, 50);
            ForeColor = Color.White;

            Font = new Font("Yu Gothic", 20F, FontStyle.Regular);

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        #region Отрисовка

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.Clear(Parent.BackColor);

            rect = new Rectangle(0, 0, Width - 1, Height - 1);

            GraphicsPath path = MakeCornersRounded(rect, cornerSize);

            // Я сделал это, чтобы было понятнее, какой цвет за что отвечает
            BoxColor = BackColor;
            FontColor = ForeColor;
            BorderColor = Color.FromArgb(15, Color.White);

            if (!Enabled)
            {
                BoxColor = Color.FromArgb(40, 40, 40);
                FontColor = Color.FromArgb(98, 98, 98);
            }

            if (MouseEntered)
                BoxColor = Color.FromArgb(59, 59, 59);

            graph.DrawPath(new Pen(BoxColor), path);
            graph.FillPath(new SolidBrush(BoxColor), path);
            graph.DrawPath(new Pen(BorderColor), path);

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

        #endregion

        #region События

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

        protected override void InitLayout()
        {
            base.InitLayout();

            originalLocation = Location;
        }

        #endregion
    }
}
