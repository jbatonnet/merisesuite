using System.Collections.Generic;
using System.Linq;

using MeriseSuite.Definitions;
using MeriseSuite.Modelling;

namespace MeriseSuite.History
{
    public class EntityDeletion : Action
    {
        public Entity Entity { get; private set; }
        public Dictionary<Relation, List<EntityLink>> Relations { get; private set; }

        public EntityDeletion(ModelVisualizer visualizer, Entity entity) : base(visualizer)
        {
            Entity = entity;

            Relations = new Dictionary<Relation, List<EntityLink>>();
            foreach (Relation relation in visualizer.Model.Relations)
            {
                List<EntityLink> entities = relation.Entities.Where(e => e.Entity == entity).ToList();
                if (entities.Count > 0)
                    Relations.Add(relation, entities);
            }
        }

        public override void Execute()
        {
            Visualizer.Model.Entities.Remove(Entity);
            foreach (var relation in Relations)
                foreach (var entityLink in relation.Value)
                    relation.Key.Entities.Remove(entityLink);

            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Visualizer.Model.Entities.Add(Entity);
            foreach (var relation in Relations)
                foreach (var entityLink in relation.Value)
                    relation.Key.Entities.Add(entityLink);

            Visualizer.Redraw();
        }
    }
    public class RelationDeletion : Action
    {
        public Relation Relation { get; private set; }

        public RelationDeletion(ModelVisualizer visualizer, Relation relation) : base(visualizer)
        {
            Relation = relation;
        }

        public override void Execute()
        {
            Visualizer.Model.Relations.Remove(Relation);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Visualizer.Model.Relations.Add(Relation);
            Visualizer.Redraw();
        }
    }
    public class InheritanceDeletion : Action
    {
        public Inheritance Inheritance { get; private set; }

        public InheritanceDeletion(ModelVisualizer visualizer, Inheritance inheritance) : base(visualizer)
        {
            Inheritance = inheritance;
        }

        public override void Execute()
        {
            // FIXME: Supprimer les parents ?

            Visualizer.Model.Inheritances.Remove(Inheritance);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Visualizer.Model.Inheritances.Add(Inheritance);
            Visualizer.Redraw();
        }
    }
    public class PropertyDeletion : Action
    {
        public Element Parent { get; private set; }
        public Property Property { get; private set; }

        public PropertyDeletion(ModelVisualizer visualizer, Element parent, Property property) : base(visualizer)
        {
            Parent = parent;
            Property = property;
        }

        public override void Execute()
        {
            Parent.Properties.Remove(Property);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Parent.Properties.Add(Property);
            Visualizer.Redraw();
        }
    }

    public class RelationLinkDeletion : Action
    {
        public RelationLink RelationLink { get; private set; }

        public RelationLinkDeletion(ModelVisualizer visualizer, RelationLink relationLink) : base(visualizer)
        {
            RelationLink = relationLink;
        }

        public override void Execute()
        {
            RelationLink.Relation.Entities.Remove(RelationLink.EntityLink);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            RelationLink.Relation.Entities.Add(RelationLink.EntityLink);
            Visualizer.Redraw();
        }
    }
    public class InheritanceLinkDeletion : Action
    {
        public InheritanceLink InheritanceLink { get; private set; }

        public InheritanceLinkDeletion(ModelVisualizer visualizer, InheritanceLink inheritanceLink) : base(visualizer)
        {
            InheritanceLink = inheritanceLink;
        }

        public override void Execute()
        {
            InheritanceLink.Inheritance.Children.Remove(InheritanceLink.Entity);
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            InheritanceLink.Inheritance.Children.Add(InheritanceLink.Entity);
            Visualizer.Redraw();
        }
    }
}