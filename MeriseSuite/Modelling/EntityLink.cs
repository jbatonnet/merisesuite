using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeriseSuite.Modelling
{
    public class EntityLink
    {
        public Entity Entity { get; set; }

        public Cardinality Cardinality { get; set; }
        public bool Relative { get; set; }

        public EntityLink(Entity entity, Cardinality cardinality)
        {
            Entity = entity;
            Cardinality = cardinality;
            Relative = false;
        }
    }
}