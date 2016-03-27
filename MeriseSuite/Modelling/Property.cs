using System;

namespace MeriseSuite.Modelling
{
    public enum PropertyType : byte
    {
        Text,
        Integer,
        Decimal,
        Date,
    }

    public class Property
    {
        public string Name { get; set; }
        public PropertyType Type { get; set; }
        public bool Primary { get; set; }

        public Property() : this("", PropertyType.Text, false) { }
        public Property(string name) : this(name, PropertyType.Text, false) { }
        public Property(string name, PropertyType type) : this(name, type, false) { }
        public Property(string name, PropertyType type, bool primary)
        {
            Name = name;
            Type = type;
            Primary = primary;
        }

        public override string ToString()
        {
            return Name + " : " + Type;
        }
    }
}