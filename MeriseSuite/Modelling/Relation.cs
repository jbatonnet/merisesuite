using System;
using System.Collections.Generic;
using System.Linq;

namespace MeriseSuite.Modelling
{
    public class Relation : Element
    {
        public List<EntityLink> Entities;

        public Relation() : this("NouvelleRelation") { }
        public Relation(string name)
        {
            Name = name;
            Entities = new List<EntityLink>();
            Properties = new List<Property>();
        }

        public override string ToString()
        {
            List<string> parts = new List<string>();

            foreach (var entry in Entities)
            {
                string cardinalityText = "";
                switch (entry.Cardinality)
                {
                    case Cardinality.ZeroOrOne: cardinalityText = "zéro ou un(e)"; break;
                    case Cardinality.ZeroOrMore: cardinalityText = "zéro ou plusieurs"; break;
                    case Cardinality.One: cardinalityText = "un(e) seul(e)"; break;
                    case Cardinality.OneOrMore: cardinalityText = "un(e) ou plusieurs"; break;
                }

                parts.Add("Pour un(e) " + entry.Entity + ", il y a " + string.Join(" et ", Entities.Where(e => e.Entity != entry.Entity).Select(e => cardinalityText + " " + e.Entity).ToArray()));
            }

            return string.Join(Environment.NewLine, parts.ToArray());
        }
    }
}