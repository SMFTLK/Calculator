﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Calc
{
    public class UI_Button_Equals : Control
    {
        #region Переменные

        private StringFormat SF;

        public static int buttonWidth;
        public static int buttonHeight;

        private const int cornerSize = 10;
        private const float fontSize = 20F;

        private bool MouseEntered;

        public Point originalLocation;

        public static Color BoxColor;
        public static Color FontColor;
        public static Color BorderColor;
        public static Color Dimming;

        #endregion

        public UI_Button_Equals()
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

            BackColor = Color.FromArgb(89, 89, 89);
            ForeColor = Color.Black;

            // Я сделал это, чтобы было понятнее, какой цвет за что отвечает
            BoxColor = BackColor;
            FontColor = ForeColor;
            BorderColor = Color.FromArgb(15, Color.White);

            Font = new Font("Yu Gothic", fontSize, FontStyle.Regular);

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        #region Отрисовка

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.Clear(Parent.BackColor);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            GraphicsPath path = MakeCornersRounded(rect, cornerSize);

            graph.DrawPath(new Pen(BoxColor), path);
            graph.FillPath(new SolidBrush(BoxColor), path);

            if (MouseEntered)
            {
                graph.DrawPath(new Pen(Dimming), path);
                graph.FillPath(new SolidBrush(Dimming), path);
            }

            graph.DrawPath(new Pen(BorderColor), path);

            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
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

            buttonWidth = Width;
            buttonHeight = Height;

            // Я сделал это, чтобы было понятнее, какой цвет за что отвечает
            BoxColor = BackColor;
            FontColor = ForeColor;
            BorderColor = Color.FromArgb(15, Color.White);

            Dimming = Color.FromArgb(30, Color.Black);
        }

        #endregion
    }
}
