using MeriseSuite.Definitions;
using MeriseSuite.Modelling;
using MeriseSuite.Styling;

namespace MeriseSuite.History
{
    public class EntityCreation : Action
    {
        public Entity Entity { get; private set; }
        public EntityStyle Style { get; private set; }

        public EntityCreation(ModelVisualizer visualizer, Entity entity) : base(visualizer)
        {
            Entity = entity;
        }

        public override void Execute()
        {
            Visualizer.Model.Entities.Add(Entity);

            if (Style != null)
                Visualizer.Styles.Add(Entity, Style);
            Visualizer.UpdateStyles();
            if (Style == null)
                Style = Visualizer.Styles[Entity] as EntityStyle;

            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Visualizer.Model.Entities.Remove(Entity);
            Visualizer.Redraw();
        }
    }
    public class RelationCreation : Action
    {
        public Relation Relation { get; private set; }
        public RelationStyle Style { get; private set; }

        public RelationCreation(ModelVisualizer visualizer, Relation relation) : base(visualizer)
        {
            Relation = relation;
        }

        public override void Execute()
        {
            Visualizer.Model.Relations.Add(Relation);

            if (Style != null)
                Visualizer.Styles.Add(Relation, Style);
            Visualizer.UpdateStyles();
            if (Style == null)
                Style = Visualizer.Styles[Relation] as RelationStyle;

            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Visualizer.Model.Relations.Remove(Relation);
            Visualizer.Redraw();
        }
    }
    public class InheritanceCreation : Action
    {
        public Inheritance Inheritance { get; private set; }
        public InheritanceStyle Style { get; private set; }

        public InheritanceCreation(ModelVisualizer visualizer, Inheritance inheritance) : base(visualizer)
        {
            Inheritance = inheritance;
        }

        public override void Execute()
        {
            Visualizer.Model.Inheritances.Add(Inheritance);

            if (Style != null)
                Visualizer.Styles.Add(Inheritance, Style);
            Visualizer.UpdateStyles();
            if (Style == null)
                Style = Visualizer.Styles[Inheritance] as InheritanceStyle;

            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Visualizer.Model.Inheritances.Remove(Inheritance);
            Visualizer.Redraw();
        }
    }
    public class PropertyCreation : Action
    {
        public Element Parent { get; private set; }
        public Property Property { get; private set; }

        public PropertyCreation(ModelVisualizer visualizer, Element parent, Property property) : base(visualizer)
        {
            Parent = parent;
            Property = property;
        }

        public override void Execute()
        {
            Parent.Properties.Add(Property);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Parent.Properties.Remove(Property);
            Visualizer.Redraw();
        }
    }

    public class RelationLinkCreation : Action
    {
        public RelationLink RelationLink { get; private set; }

        public RelationLinkCreation(ModelVisualizer visualizer, RelationLink relationLink) : base(visualizer)
        {
            RelationLink = relationLink;
        }

        public override void Execute()
        {
            RelationLink.Relation.Entities.Add(RelationLink.EntityLink);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            RelationLink.Relation.Entities.Remove(RelationLink.EntityLink);
            Visualizer.Redraw();
        }
    }
    public class InheritanceLinkCreation : Action
    {
        public InheritanceLink InheritanceLink { get; private set; }

        public InheritanceLinkCreation(ModelVisualizer visualizer, InheritanceLink inheritanceLink) : base(visualizer)
        {
            InheritanceLink = inheritanceLink;
        }

        public override void Execute()
        {
            InheritanceLink.Inheritance.Children.Add(InheritanceLink.Entity);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            InheritanceLink.Inheritance.Children.Remove(InheritanceLink.Entity);
            Visualizer.Redraw();
        }
    }
}