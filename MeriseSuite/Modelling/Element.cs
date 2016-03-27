using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeriseSuite.Modelling
{
    [Serializable]
    public class Element
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Property> Properties { get; set; }

        public Element()
        {
            Id = Program.Id++;
        }
    }
}
