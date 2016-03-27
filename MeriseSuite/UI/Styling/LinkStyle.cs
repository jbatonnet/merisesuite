using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;

using MeriseSuite.Modelling;

namespace MeriseSuite.Styling
{
    public class LinkStyle : Style
    {
        private const int PinSize = 6;
        private const int TriangleSize = 4;

        public Style End1Style { get; private set; }
        public Direction End1Orientation { get; set; }
        public PointF End1Location { get; private set; }

        public Style End2Style { get; set; }
        public Direction End2Orientation { get; private set; }
        public PointF End2Location { get; private set; }

        public object Tag { get; set; }

        public LinkStyle(Style end1Style, Style end2Style, object element, Graphics graphics, ModelVisualizer visualizer) : base(element, visualizer)
        {
            End1Style = end1Style;
            End2Style = end2Style;

            Update(graphics);
        }

        public override void Update(Graphics graphics)
        {
            int hDelta = (End2Style.Left + End2Style.Width / 2) - (End1Style.Left + End1Style.Width / 2);
            int vDelta = (End2Style.Top + End2Style.Height / 2) - (End1Style.Top + End1Style.Height / 2);

            if (Math.Abs(hDelta) > Math.Abs(vDelta))
            {
                End1Orientation = hDelta > 0 ? Direction.Right : Direction.Left;
                End2Orientation = hDelta > 0 ? Direction.Left : Direction.Right;
            }
            else
            {
                End1Orientation = vDelta > 0 ? Direction.Bottom : Direction.Top;
                End2Orientation = vDelta > 0 ? Direction.Top : Direction.Bottom;
            }

            if (End1Style is InheritanceStyle)
            {
                if (End2Style is EntityStyle && (End2Style as EntityStyle).Entity == (End1Style as InheritanceStyle).Inheritance.Parent)
                    End1Orientation = Direction.Top;
                else
                    End1Orientation = Direction.Bottom;
            }
            else if (End2Style is InheritanceStyle)
            {
                if ((End1Style as EntityStyle).Entity == (End2Style as InheritanceStyle).Inheritance.Parent)
                    End2Orientation = Direction.Top;
                else
                    End2Orientation = Direction.Bottom;
            }

            List<LinkStyle> end1Links = Visualizer.Styles.Values.OfType<LinkStyle>().Union(Visualizer.NewLinkStyle == null ? new LinkStyle[0] : new[] { Visualizer.NewLinkStyle }).Where(s => (s.End1Style == End1Style && s.End1Orientation == End1Orientation) || (s.End2Style == End1Style && s.End2Orientation == End1Orientation)).ToList();
            List<LinkStyle> end2Links = Visualizer.Styles.Values.OfType<LinkStyle>().Union(Visualizer.NewLinkStyle == null ? new LinkStyle[0] : new[] { Visualizer.NewLinkStyle }).Where(s => (s.End1Style == End2Style && s.End1Orientation == End2Orientation) || (s.End2Style == End2Style && s.End2Orientation == End2Orientation)).ToList();

            switch (End1Orientation)
            {
                case Direction.Top: End1Location = new PointF(End1Style.Left + End1Style.Width / (end1Links.Count + 1) * (end1Links.IndexOf(this) + 1), End1Style.Top - PinSize); break;
                case Direction.Right: End1Location = new PointF(End1Style.Right + PinSize, End1Style.Top + End1Style.Height / (end1Links.Count + 1) * (end1Links.IndexOf(this) + 1)); break;
                case Direction.Bottom: End1Location = new PointF(End1Style.Left + End1Style.Width / (end1Links.Count + 1) * (end1Links.IndexOf(this) + 1), End1Style.Bottom + PinSize); break;
                case Direction.Left: End1Location = new PointF(End1Style.Left - PinSize, End1Style.Top + End1Style.Height / (end1Links.Count + 1) * (end1Links.IndexOf(this) + 1)); break;
            }

            PointF end2 = PointF.Empty;
            switch (End2Orientation)
            {
                case Direction.Top: End2Location = new PointF(End2Style.Left + End2Style.Width / (end2Links.Count + 1) * (end2Links.IndexOf(this) + 1), End2Style.Top - PinSize); break;
                case Direction.Right: End2Location = new PointF(End2Style.Right + PinSize, End2Style.Top + End2Style.Height / (end2Links.Count + 1) * (end2Links.IndexOf(this) + 1)); break;
                case Direction.Bottom: End2Location = new PointF(End2Style.Left + End2Style.Width / (end2Links.Count + 1) * (end2Links.IndexOf(this) + 1), End2Style.Bottom + PinSize); break;
                case Direction.Left: End2Location = new PointF(End2Style.Left - PinSize, End2Style.Top + End2Style.Height / (end2Links.Count + 1) * (end2Links.IndexOf(this) + 1)); break;
            }

            Left = (int)Math.Min(End1Location.X, End2Location.X);
            Top = (int)Math.Min(End1Location.Y, End2Location.Y);
            Width = (int)Math.Max(End1Location.X, End2Location.X) - Left;
            Height = (int)Math.Max(End1Location.Y, End2Location.Y) - Top;
        }
        public override void Draw(Graphics graphics, bool highlighted)
        {
            bool inheritanceTriangle = End1Style is EntityStyle && End2Style is InheritanceStyle && (End2Style as InheritanceStyle).Inheritance.Parent == End1Style.Element;
            PointF a, b, c;
            a = b = c = PointF.Empty;

            if (inheritanceTriangle)
                switch (End1Orientation)
                {
                    case Direction.Top: a = new PointF(End1Location.X - TriangleSize, End1Location.Y); b = new PointF(End1Location.X + TriangleSize, End1Location.Y); c = new PointF(End1Location.X, End1Style.Top); break;
                    case Direction.Right: a = new PointF(End1Location.X, End1Location.Y - TriangleSize); b = new PointF(End1Location.X, End1Location.Y + TriangleSize); c = new PointF(End1Style.Right, End1Location.Y); break;
                    case Direction.Bottom: a = new PointF(End1Location.X - TriangleSize, End1Location.Y); b = new PointF(End1Location.X + TriangleSize, End1Location.Y); c = new PointF(End1Location.X, End1Style.Bottom); break;
                    case Direction.Left: a = new PointF(End1Location.X, End1Location.Y - TriangleSize); b = new PointF(End1Location.X, End1Location.Y + TriangleSize); c = new PointF(End1Style.Left, End1Location.Y); break;
                }

            for (int i = 2; i <= 6 && highlighted; i += 2)
            {
                if (inheritanceTriangle)
                {
                    graphics.DrawLine(new Pen(Color.FromArgb(255 - (i - 2) * 48, Visualizer.SelectionColor), i), a, b);
                    graphics.DrawLine(new Pen(Color.FromArgb(255 - (i - 2) * 48, Visualizer.SelectionColor), i), b, c);
                    graphics.DrawLine(new Pen(Color.FromArgb(255 - (i - 2) * 48, Visualizer.SelectionColor), i), c, a);
                }
                else
                    graphics.DrawLine(new Pen(Color.FromArgb(255 - (i - 2) * 48, Visualizer.SelectionColor), i), new Point(End1Style.Left + End1Style.Width / 2, End1Style.Top + End1Style.Height / 2), End1Location);
                graphics.DrawLine(new Pen(Color.FromArgb(255 - (i - 2) * 48, Visualizer.SelectionColor), i), End1Location, End2Location);
                graphics.DrawLine(new Pen(Color.FromArgb(255 - (i - 2) * 48, Visualizer.SelectionColor), i), new Point(End2Style.Left + End2Style.Width / 2, End2Style.Top + End2Style.Height / 2), End2Location);
            }

            if (inheritanceTriangle)
            {
                graphics.DrawLine(Pens.Black, a, b);
                graphics.DrawLine(Pens.Black, b, c);
                graphics.DrawLine(Pens.Black, c, a);
            }
            else
                graphics.DrawLine(Pens.Black, new Point(End1Style.Left + End1Style.Width / 2, End1Style.Top + End1Style.Height / 2), End1Location);
            graphics.DrawLine(Pens.Black, End1Location, End2Location);
            graphics.DrawLine(Pens.Black, new Point(End2Style.Left + End2Style.Width / 2, End2Style.Top + End2Style.Height / 2), End2Location);

            string text = "";
            if (End2Style is RelationStyle && this != Visualizer.NewLinkStyle)
            {
                switch ((Tag as EntityLink).Cardinality)
                {
                    case Cardinality.One: text = "1, 1"; break;
                    case Cardinality.OneOrMore: text = "1, n"; break;
                    case Cardinality.ZeroOrMore: text = "0, n"; break;
                    case Cardinality.ZeroOrOne: text = "0, 1"; break;
                }

                if ((Tag as EntityLink).Relative)
                    text = '(' + text + ')';
            }

            float textWidth = graphics.MeasureString(text, Visualizer.Font).Width;
            float angle = (float)Math.Atan2(End2Location.Y - End1Location.Y, End2Location.X - End1Location.X);
            PointF textPoint = new PointF(End1Location.X + (End2Location.X - End1Location.X) / 2, End1Location.Y + (End2Location.Y - End1Location.Y) / 2);

            if (Math.Abs(angle) > Math.PI / 2)
                angle -= (float)Math.PI;

            GraphicsState gs = graphics.Save();
            graphics.TranslateTransform(textPoint.X, textPoint.Y);
            graphics.RotateTransform(angle / (float)Math.PI * 180);
            graphics.TranslateTransform(-textWidth / 2, 2);

            graphics.DrawString(text, Visualizer.Font, Brushes.Black, Point.Empty);

            graphics.Restore(gs);
        }
    }
}