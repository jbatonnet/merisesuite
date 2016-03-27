using System;
using System.Drawing;
using System.Linq;

using MeriseSuite.Modelling;
using MeriseSuite.Shapes;
using MeriseSuite.Properties;

namespace MeriseSuite.Styling
{
    public class RelationStyle : Style
    {
        public Relation Relation
        {
            get
            {
                return Element as Relation;
            }
        }
        public override Color Color
        {
            get
            {
                return Visualizer.RelationColor;
            }
            set
            {
            }
        }

        public RelationStyle(Relation relation, Graphics graphics, ModelVisualizer visualizer) : base(relation, visualizer)
        {
            if (visualizer != null)
                Color = visualizer.RelationColor;
            else
                Color = Settings.Default.RelationColor;

            // Calcul de la position
            Location = new Point(8, 8);
            if (visualizer != null)
            {
                int index = visualizer.Model.Relations.IndexOf(Relation);
                for (int i = 0; i < index; i++)
                    Location += new Size(16, 16);

                Update(graphics);
            }
        }

        public override void Update(Graphics graphics)
        {
            // Calcul de la largeur
            float maxWidth = graphics.MeasureString(Relation.Name, Visualizer.Font).Width;
            foreach (Property p in Relation.Properties)
                maxWidth = Math.Max(maxWidth, graphics.MeasureString(p.Name, Visualizer.Font).Width);
            Width = (int)maxWidth + 24;

            Aggregation aggregation = Visualizer.Model.Entities.SingleOrDefault(e => e is Aggregation && (e as Aggregation).Relation == Element) as Aggregation;
            if (aggregation != null)
            {
                float aggregationMaxWidth = graphics.MeasureString(aggregation.Name, Visualizer.Font).Width;
                foreach (Property p in aggregation.Properties)
                    aggregationMaxWidth = Math.Max(aggregationMaxWidth, graphics.MeasureString(p.Name, Visualizer.Font).Width);
                //aggregationMaxWidth += 24;
                Width = Math.Max(Width, (int)aggregationMaxWidth);
            }

            // Calcul de la hauteur
            Height = 26 + Relation.Properties.Count * 16;
            if (Height < 40)
                Height = 40;
        }
        public override void Draw(Graphics graphics, bool highlighted)
        {
            // Affichage du décor
            WeirdRectangle back = new WeirdRectangle(Left, Top, Width, Height, new Size(CornerWidth, CornerHeight));
            WeirdRectangle head = new WeirdRectangle(Left, Top, Width, 20, new Size(CornerWidth, CornerHeight), new Size(CornerWidth, CornerHeight), Size.Empty, Size.Empty);
            graphics.FillPath(Brushes.White, back);
            graphics.DrawPath(new Pen(Visualizer.BorderColor), back);
            graphics.FillPath(new SolidBrush(Color), head);
            graphics.DrawPath(new Pen(Visualizer.BorderColor), head);

            if (highlighted)
                for (int i = 0; i < 4; i++)
                    graphics.DrawPath(new Pen(Color.FromArgb(255 - i * 64, Color.R / HighlightColorFactor, Color.G / HighlightColorFactor, Color.B / HighlightColorFactor)), new WeirdRectangle(Left - i, Top - i, Width + i * 2, Height + i * 2, new Size(CornerWidth, CornerHeight) + new Size(i * 2, i * 2)));

            // Affichage du texte
            int nameWidth = (int)graphics.MeasureString(Relation.Name, Visualizer.Font).Width;
            graphics.DrawString(Relation.Name, Visualizer.Font, new SolidBrush(Visualizer.ForeColor), Left + (Width - nameWidth) / 2, Top + 4);

            for (int i = 0; i < Relation.Properties.Count && 24 + i * 16 < Height; i++)
                graphics.DrawString(Relation.Properties[i].Name, Visualizer.Font, new SolidBrush(Visualizer.ForeColor), Left + 18, Top + 24 + i * 16);
        }
    }
}