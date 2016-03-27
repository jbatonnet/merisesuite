using System;
using System.Drawing;

using MeriseSuite.Modelling;
using MeriseSuite.Shapes;
using MeriseSuite.Properties;

namespace MeriseSuite.Styling
{
    public class EntityStyle : Style
    {
        public Entity Entity
        {
            get
            {
                return Element as Entity;
            }
        }
        public override Color Color
        {
            get
            {
                return Visualizer.EntityColor;
            }
            set
            {
            }
        }

        public EntityStyle(Entity entity, Graphics graphics, ModelVisualizer visualizer) : base(entity, visualizer)
        {
            if (visualizer != null)
                Color = visualizer.EntityColor;
            else
                Color = Settings.Default.EntityColor;

            // Calcul de la position
            Location = new Point(8, 8);
            if (visualizer != null)
            {
                int index = visualizer.Model.Entities.IndexOf(entity);
                for (int i = 0; i < index; i++)
                    Location += new Size(16, 16);

                Update(graphics);
            }
        }

        public override void Update(Graphics graphics)
        {
            // Calcul de la largeur
            float maxWidth = graphics.MeasureString(Entity.Name, Visualizer.Font).Width;
            foreach (Property p in Entity.Properties)
                maxWidth = Math.Max(maxWidth, graphics.MeasureString(p.Name, Visualizer.Font).Width);
            Width = (int)maxWidth + 24;

            // Calcul de la hauteur
            Height = 26 + Entity.Properties.Count * 16;
            if (Height < 40)
                Height = 40;

            // Calcul de la position si c'est une agrégation
            if (Entity is Aggregation && Visualizer.Styles != null && (Entity as Aggregation).Relation != null && Visualizer.Styles.ContainsKey((Entity as Aggregation).Relation))
            {
                Style relationStyle = Visualizer.Styles[(Entity as Aggregation).Relation];
                Location = relationStyle.Location - new Size(4, 24);
                Size = relationStyle.Size + new Size(8, 28);
            }
        }
        public override void Draw(Graphics graphics, bool highlighted)
        {
            // Affichage du décor
            RoundRectangle back = new RoundRectangle(Left, Top, Width, Height, CornerRadius);
            RoundRectangle head = new RoundRectangle(Left, Top, Width, 20, CornerRadius, CornerRadius, 0, 0);
            graphics.FillRoundRectangle(Brushes.White, back);
            graphics.DrawRoundRectangle(new Pen(Visualizer.BorderColor), back);
            graphics.FillRoundRectangle(new SolidBrush(Color), head);
            graphics.DrawRoundRectangle(new Pen(Visualizer.BorderColor), head);

            if (highlighted)
                for (int i = 0; i < 4; i++)
                    graphics.DrawRoundRectangle(new Pen(Color.FromArgb(255 - i * 64, Color.R / HighlightColorFactor, Color.G / HighlightColorFactor, Color.B / HighlightColorFactor)), new RoundRectangle(Left - i, Top - i, Width + i * 2, Height + i * 2, CornerRadius + i * 2));

            // Affichage du texte
            int nameWidth = (int)graphics.MeasureString(Entity.Name, Visualizer.Font).Width;
            graphics.DrawString(Entity.Name, Visualizer.Font, new SolidBrush(Visualizer.ForeColor), Left + (Width - nameWidth) / 2, Top + 4);

            for (int i = 0; i < Entity.Properties.Count; i++)
            {
                if (Entity.Properties[i].Primary)
                    graphics.DrawImage(Resources.KeyOnly, Left + 4, Top + 22 + i * 16);
                graphics.DrawString(Entity.Properties[i].Name, Entity.Properties[i].Primary ? new Font(Visualizer.Font, FontStyle.Underline) : Visualizer.Font, new SolidBrush(Visualizer.ForeColor), Left + 18, Top + 24 + i * 16);
            }
        }
    }
}