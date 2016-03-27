using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using MeriseSuite.Styling;
using MeriseSuite.Modelling;
using MeriseSuite.Definitions;

namespace MeriseSuite.History
{
    public abstract class Modification<T> : Action
    {
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public Modification(ModelVisualizer visualizer, T oldValue, T newValue) : base(visualizer)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class LocationModification : Modification<Point>
    {
        public Style Style { get; private set; }

        public LocationModification(ModelVisualizer visualizer, Style style, Point position) : base(visualizer, style.Location, position)
        {
            Style = style;
        }

        public override void Execute()
        {
            Style.Location = NewValue;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Style.Location = OldValue;
            Visualizer.Redraw();
        }
    }
    public class ElementNameModification : Modification<string>
    {
        public Element Element { get; private set; }

        public ElementNameModification(ModelVisualizer visualizer, Element element, string name) : base(visualizer, element.Name, name)
        {
            Element = element;
        }

        public override void Execute()
        {
            Element.Name = NewValue;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Element.Name = OldValue;
            Visualizer.Redraw();
        }
    }
    public class PropertyNameModification : Modification<string>
    {
        public Property Property { get; private set; }

        public PropertyNameModification(ModelVisualizer visualizer, Property property, string name) : base(visualizer, property.Name, name)
        {
            Property = property;
        }

        public override void Execute()
        {
            Property.Name = NewValue;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Property.Name = OldValue;
            Visualizer.Redraw();
        }
    }
    public class PropertyPositionUpModification : Action
    {
        public Element Element { get; private set; }
        public Property Property { get; private set; }

        public PropertyPositionUpModification(ModelVisualizer visualizer, Element element, Property property) : base(visualizer)
        {
            Element = element;
            Property = property;
        }

        public override void Execute()
        {
            List<Property> properties = new List<Property>();
            int position = Element.Properties.IndexOf(Property);

            if (position > 1)
                properties.AddRange(Element.Properties.Take(position - 1));
            properties.Add(Element.Properties[position]);
            properties.Add(Element.Properties[position - 1]);
            if (position < Element.Properties.Count - 1)
                properties.AddRange(Element.Properties.Skip(position + 1));

            Element.Properties = properties;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            List<Property> properties = new List<Property>();
            int position = Element.Properties.IndexOf(Property);

            if (position > 0)
                properties.AddRange(Element.Properties.Take(position));
            properties.Add(Element.Properties[position + 1]);
            properties.Add(Element.Properties[position]);
            if (position < Element.Properties.Count - 2)
                properties.AddRange(Element.Properties.Skip(position + 2));

            Element.Properties = properties;
            Visualizer.Redraw();
        }
    }
    public class PropertyPositionDownModification : Action
    {
        public Element Element { get; private set; }
        public Property Property { get; private set; }

        public PropertyPositionDownModification(ModelVisualizer visualizer, Element element, Property property) : base(visualizer)
        {
            Element = element;
            Property = property;
        }

        public override void Execute()
        {
            List<Property> properties = new List<Property>();
            int position = Element.Properties.IndexOf(Property);

            if (position > 0)
                properties.AddRange(Element.Properties.Take(position));
            properties.Add(Element.Properties[position + 1]);
            properties.Add(Element.Properties[position]);
            if (position < Element.Properties.Count - 2)
                properties.AddRange(Element.Properties.Skip(position + 2));

            Element.Properties = properties;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            List<Property> properties = new List<Property>();
            int position = Element.Properties.IndexOf(Property);

            if (position > 1)
                properties.AddRange(Element.Properties.Take(position - 1));
            properties.Add(Element.Properties[position]);
            properties.Add(Element.Properties[position - 1]);
            if (position < Element.Properties.Count - 1)
                properties.AddRange(Element.Properties.Skip(position + 1));

            Element.Properties = properties;
            Visualizer.Redraw();
        }
    }
    public class PropertyPrimaryModification : Modification<bool>
    {
        public Property Property { get; private set; }

        public PropertyPrimaryModification(ModelVisualizer visualizer, Property property, bool primary) : base(visualizer, property.Primary, primary)
        {
            Property = property;
        }

        public override void Execute()
        {
            Property.Primary = NewValue;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Property.Primary = OldValue;
            Visualizer.Redraw();
        }
    }
    public class InheritanceTypeModification : Modification<InheritanceType>
    {
        public Inheritance Inheritance { get; private set; }

        public InheritanceTypeModification(ModelVisualizer visualizer, Inheritance inheritance, InheritanceType type) : base(visualizer, inheritance.Type, type)
        {
            Inheritance = inheritance;
        }

        public override void Execute()
        {
            Inheritance.Type = NewValue;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            Inheritance.Type = OldValue;
            Visualizer.Redraw();
        }
    }
    public class RelationLinkRelativeModification : Modification<bool>
    {
        public RelationLink RelationLink { get; private set; }

        public RelationLinkRelativeModification(ModelVisualizer visualizer, RelationLink relationLink, bool relative) : base(visualizer, relationLink.EntityLink.Relative, relative)
        {
            RelationLink = relationLink;
        }

        public override void Execute()
        {
            RelationLink.EntityLink.Relative = NewValue;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            RelationLink.EntityLink.Relative = OldValue;
            Visualizer.Redraw();
        }
    }
    public class RelationLinkCardinalityModification : Modification<Cardinality>
    {
        public RelationLink RelationLink { get; private set; }

        public RelationLinkCardinalityModification(ModelVisualizer visualizer, RelationLink relationLink, Cardinality cardinality) : base(visualizer, relationLink.EntityLink.Cardinality, cardinality)
        {
            RelationLink = relationLink;
        }

        public override void Execute()
        {
            RelationLink.EntityLink.Cardinality = NewValue;
            Visualizer.Redraw();
        }
        public override void Rollback()
        {
            RelationLink.EntityLink.Cardinality = OldValue;
            Visualizer.Redraw();
        }
    }
}

// TODO: StyleColor