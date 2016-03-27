using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeriseSuite.Modelling;
using System.Drawing;
using MeriseSuite.Shapes;

namespace MeriseSuite.Styling
{
    public class InheritanceStyle : Style
    {
        public Inheritance Inheritance
        {
            get
            {
                return Element as Inheritance;
            }
        }
        public override Color Color
        {
            get
            {
                return Visualizer.InheritanceColor;
            }
            set
            {
            }
        }

        public Direction ParentOrientation { get; private set; }
        public float ParentPosition { get; private set; }

        public InheritanceStyle(Inheritance inheritance, Graphics graphics, ModelVisualizer visualizer) : base(inheritance, visualizer)
        {
            Width = 36;
            Height = 18;

            Color = Color.LightGreen;
        }

        public override void Draw(Graphics graphics, bool highlighted)
        {
            WeirdRectangle shape = new WeirdRectangle(Left, Top, Width, Height, new Size(Width, Height * 2), new Size(Width, Height * 2), Size.Empty, Size.Empty);
            graphics.FillPath(new SolidBrush(Color), shape);
            graphics.DrawPath(new Pen(Visualizer.BorderColor), shape);

            if (highlighted)
                for (int i = 0; i < 4; i++)
                    graphics.DrawPath(new Pen(Color.FromArgb(255 - i * 64, Color.R / HighlightColorFactor, Color.G / HighlightColorFactor, Color.B / HighlightColorFactor)), new WeirdRectangle(Left - i, Top - i, Width + i * 2, Height + i * 2, new Size(Width + i * 2, Height * 2 + i * 2), new Size(Width + i * 2, Height * 2 + i * 2), Size.Empty, Size.Empty));

            string inheritance = "";
            switch (Inheritance.Type)
            {
                case InheritanceType.Partition: inheritance = "XT"; break;
                case InheritanceType.Exclusion: inheritance = "X"; break;
                case InheritanceType.Total: inheritance = "T"; break;
            }

            int inheritanceWidth = (int)graphics.MeasureString(inheritance, Visualizer.Font).Width;
            graphics.DrawString(inheritance, Visualizer.Font, new SolidBrush(Visualizer.ForeColor), Left + (Width - inheritanceWidth) / 2, Top + 4);
        }
    }
}