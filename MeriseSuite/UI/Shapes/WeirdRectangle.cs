using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MeriseSuite.Shapes
{
    public class WeirdRectangle
    {
        private const int MinWidth = 8;
        private const int MinHeight = 8;

        public int X { get; set; }
        public int Y { get; set; }
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = Math.Max(MinWidth, value);
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = Math.Max(MinHeight, value);
            }
        }

        public Size TopLeftCorner
        {
            get
            {
                return topLeftCorner;
            }
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                topLeftCorner = value;
            }
        }
        public Size TopRightCorner
        {
            get
            {
                return topRightCorner;
            }
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                topRightCorner = value;
            }
        }
        public Size BottomRightCorner
        {
            get
            {
                return bottomRightCorner;
            }
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                bottomRightCorner = value;
            }
        }
        public Size BottomLeftCorner
        {
            get
            {
                return bottomLeftCorner;
            }
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                bottomLeftCorner = value;
            }
        }

        public Point Location
        {
            get
            {
                return new Point(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Size Size
        {
            get
            {
                return new Size(width, height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }
        public GraphicsPath Path
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                if (topLeftCorner != Size.Empty)
                    path.AddArc(X, Y, topLeftCorner.Width, topLeftCorner.Height, 180, 90);
                path.AddLine(X + topLeftCorner.Width / 2, Y, X + width - topRightCorner.Width / 2, Y);
                if (topRightCorner != Size.Empty)
                    path.AddArc(X + width - topRightCorner.Width, Y, topRightCorner.Width, topRightCorner.Height, 270, 90);
                path.AddLine(X + width, Y + topRightCorner.Height / 2, X + width, Y + height - bottomRightCorner.Height / 2);
                if (bottomRightCorner != Size.Empty)
                    path.AddArc(X + width - bottomRightCorner.Width, Y + height - bottomRightCorner.Height, bottomRightCorner.Width, bottomRightCorner.Height, 0, 90);
                path.AddLine(X + width - bottomRightCorner.Width / 2, Y + height, X + bottomLeftCorner.Width / 2, Y + height);
                if (bottomLeftCorner != Size.Empty)
                    path.AddArc(X, Y + height - bottomLeftCorner.Height, bottomLeftCorner.Width, bottomLeftCorner.Height, 90, 90);
                path.AddLine(X, Y + height - bottomLeftCorner.Height / 2, X, Y + topLeftCorner.Height / 2);
                path.CloseFigure();
                return path;
            }
        }

        private int width, height;
        private Size topLeftCorner, topRightCorner, bottomRightCorner, bottomLeftCorner;

        public WeirdRectangle(int x, int y, int width, int height, Size corner)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            TopLeftCorner = TopRightCorner = BottomRightCorner = BottomLeftCorner = corner;
        }
        public WeirdRectangle(int x, int y, int width, int height, Size topLeftCorner, Size topRightCorner, Size bottomRightCorner, Size bottomLeftCorner)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            TopLeftCorner = topLeftCorner;
            TopRightCorner = topRightCorner;
            BottomRightCorner = bottomRightCorner;
            BottomLeftCorner = bottomLeftCorner;
        }

        public static implicit operator GraphicsPath(WeirdRectangle shape)
        {
            return shape.Path;
        }
    }
}
