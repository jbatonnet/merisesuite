using System;
using System.Collections.Generic;
using System.Linq;

namespace MeriseSuite.Modelling
{
    public class Entity : Element
    {
        public Entity() : this("NouvelleEntité") { }
        public Entity(string name)
        {
            Name = name;
            Properties = new List<Property>();
        }
        public Property this[string name]
        {
            get
            {
                return Properties.SingleOrDefault(p => p.Name == name);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
