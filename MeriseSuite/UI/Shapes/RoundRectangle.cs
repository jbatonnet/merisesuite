using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MeriseSuite.Shapes
{
    public static partial class GraphicsExtensions
    {
        public static void DrawRoundRectangle(this Graphics me, Pen pen, RoundRectangle shape)
        {
            me.DrawPath(pen, shape);
        }
        public static void FillRoundRectangle(this Graphics me, Brush brush, RoundRectangle shape)
        {
            me.FillPath(brush, shape);
        }
    }

    public class RoundRectangle
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

        public int TopLeftRadius
        {
            get
            {
                return topLeftRadius;
            }
            set
            {
                topLeftRadius = Math.Max(0, value);
            }
        }
        public int TopRightRadius
        {
            get
            {
                return topRightRadius;
            }
            set
            {
                topRightRadius = Math.Max(0, value);
            }
        }
        public int BottomRightRadius
        {
            get
            {
                return bottomRightRadius;
            }
            set
            {
                bottomRightRadius = Math.Max(0, value);
            }
        }
        public int BottomLeftRadius
        {
            get
            {
                return bottomLeftRadius;
            }
            set
            {
                bottomLeftRadius = Math.Max(0, value);
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
                if (topLeftRadius > 0)
                    path.AddArc(X, Y, topLeftRadius, topLeftRadius, 180, 90);
                path.AddLine(X + topLeftRadius, Y, X + width - topRightRadius, Y);
                if (topRightRadius > 0)
                    path.AddArc(X + width - topRightRadius, Y, topRightRadius, topRightRadius, 270, 90);
                path.AddLine(X + width, Y + topRightRadius, X + width, Y + height - bottomRightRadius);
                if (bottomRightRadius > 0)
                    path.AddArc(X + width - bottomRightRadius, Y + height - bottomRightRadius, bottomRightRadius, bottomRightRadius, 0, 90);
                path.AddLine(X + width - bottomRightRadius, Y + height, X + bottomLeftRadius, Y + height);
                if (bottomLeftRadius > 0)
                    path.AddArc(X, Y + height - bottomLeftRadius, bottomLeftRadius, bottomLeftRadius, 90, 90);
                path.AddLine(X, Y + height - bottomLeftRadius, X, Y + topLeftRadius);
                path.CloseFigure();
                return path;
            }
        }

        private int width, height, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius;

        public RoundRectangle(int x, int y, int width, int height, int radius)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            TopLeftRadius = TopRightRadius = BottomRightRadius = BottomLeftRadius = radius;
        }
        public RoundRectangle(int x, int y, int width, int height, int topLeftRadius, int topRightRadius, int bottomRightRadius, int bottomLeftRadius)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            TopLeftRadius = topLeftRadius;
            TopRightRadius = topRightRadius;
            BottomRightRadius = bottomRightRadius;
            BottomLeftRadius = bottomLeftRadius;
        }

        public static implicit operator GraphicsPath(RoundRectangle shape)
        {
            return shape.Path;
        }
    }
}
