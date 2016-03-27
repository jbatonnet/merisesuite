using MeriseSuite.Modelling;

namespace MeriseSuite.Definitions
{
    public class RelationLink
    {
        public EntityLink EntityLink { get; set; }
        public Relation Relation { get; set; }

        public RelationLink(EntityLink entityLink, Relation relation)
        {
            EntityLink = entityLink;
            Relation = relation;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RelationLink))
                return false;

            RelationLink relationLink = obj as RelationLink;
            return EntityLink == relationLink.EntityLink && Relation == relationLink.Relation;
        }
        public override string ToString()
        {
            return "";
        }
    }
}