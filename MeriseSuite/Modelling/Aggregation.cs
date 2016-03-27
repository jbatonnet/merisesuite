namespace MeriseSuite.Modelling
{
    public class Aggregation : Entity
    {
        public Relation Relation;

        public Aggregation(Relation relation) : base("@" + (relation == null ? "" : relation.Name))
        {
            Relation = relation;
        }
    }
}