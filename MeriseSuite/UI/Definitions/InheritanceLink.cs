using MeriseSuite.Modelling;

namespace MeriseSuite.Definitions
{
    public class InheritanceLink
    {
        public Entity Entity { get; set; }
        public Inheritance Inheritance { get; set; }

        public InheritanceLink(Entity entity, Inheritance inheritance)
        {
            Entity = entity;
            Inheritance = inheritance;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InheritanceLink))
                return false;

            InheritanceLink inheritanceLink = obj as InheritanceLink;
            return Entity == inheritanceLink.Entity && Inheritance == inheritanceLink.Inheritance;
        }
        public override string ToString()
        {
            return "";
        }
    }
}
