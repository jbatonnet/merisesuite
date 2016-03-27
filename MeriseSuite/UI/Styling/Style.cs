using System.Drawing;

namespace MeriseSuite.Styling
{
    public abstract class Style
    {
        protected const int CornerRadius = 6;
        protected const int HighlightColorFactor = 3;
        protected const int CornerWidth = 24;
        protected const int CornerHeight = 40;

        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual Color Color { get; set; }

        public object Element;
        public ModelVisualizer Visualizer;

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
                return new Size(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public int Right
        {
            get
            {
                return X + Width;
            }
        }
        public int Bottom
        {
            get
            {
                return Y + Height;
            }
        }
        public int Left
        {
            get
            {
                return X;
            }
            set
            {
                X = value;
            }
        }
        public int Top
        {
            get
            {
                return Y;
            }
            set
            {
                Y = value;
            }
        }

        // TODO: Centraliser le comportement des formes ici
        private Region Shape;

        public Style(object element, ModelVisualizer visualizer)
        {
            Element = element;
            this.Visualizer = visualizer;
        }

        public virtual void Update(Graphics graphics)
        {
        }
        public virtual void Draw(Graphics graphics, bool highlighted)
        {
        }

        public static implicit operator Rectangle(Style style)
        {
            return new Rectangle(style.X, style.Y, style.Width, style.Height);
        }
    }
}