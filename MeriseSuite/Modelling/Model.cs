using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeriseSuite.Modelling
{
    public class Model
    {
        public List<Entity> Entities { get; set; }
        public List<Relation> Relations { get; set; }
        public List<Inheritance> Inheritances { get; set; }

        public Model()
        {
            Entities = new List<Entity>();
            Relations = new List<Relation>();
            Inheritances = new List<Inheritance>();
        }

        public Element this[int id]
        {
            get
            {
                return Entities.SingleOrDefault(e => e.Id == id) ?? Relations.SingleOrDefault(r => r.Id == id) as Element;
            }
        }

        public Element GetElementByProperty(Property property)
        {
            return Entities.SingleOrDefault(e => e.Properties.Contains(property)) ?? Relations.SingleOrDefault(r => r.Properties.Contains(property)) as Element;
        }
    }
}
