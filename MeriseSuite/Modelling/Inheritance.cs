using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeriseSuite.Modelling
{
    public enum InheritanceType
    {
        None,
        Partition,
        Exclusion,
        Total
    }

    public class Inheritance
    {
        public int Id { get; set; }
        public Entity Parent { get; set; }
        public List<Entity> Children { get; private set; }
        public InheritanceType Type { get; set; }

        public Inheritance(Entity parent, InheritanceType type)
        {
            Id = Program.Id++;
            Parent = parent;
            Children = new List<Entity>();
            Type = type;
        }

        public override string ToString()
        {
            if (Children.Count == 0)
                return "";

            string children = Children[0].ToString();
            for (int i = 1; i < Children.Count - 1; i++)
                children += ", " + Children[i];
            if (Children.Count > 1)
                children += " et " + Children[Children.Count - 1];

            return children + " hérite" + (Children.Count > 1 ? "nt" : "") + " de " + Parent;
        }
    }
}