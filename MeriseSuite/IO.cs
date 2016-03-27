using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using MeriseSuite.Modelling;
using MeriseSuite.Styling;
using System.Windows.Forms;

namespace MeriseSuite
{
    public static class IO
    {
        public static Version Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        private static XElement FromProperty(Property property)
        {
            XElement element = new XElement("Property");
            element.SetAttributeValue("Name", property.Name);
            element.SetAttributeValue("Type", property.Type);
            element.SetAttributeValue("Primary", property.Primary);
            return element;
        }
        private static XElement FromEntityLink(EntityLink entityLink)
        {
            XElement element = new XElement("Link");
            element.SetAttributeValue("Id", entityLink.Entity.Id);
            element.SetAttributeValue("Cardinality", entityLink.Cardinality);
            element.SetAttributeValue("Relative", entityLink.Relative);
            return element;
        }
        private static XElement FromStyle(Style style)
        {
            XElement element = new XElement("Style");
            element.SetAttributeValue("X", style.X);
            element.SetAttributeValue("Y", style.Y);
            element.SetAttributeValue("Width", style.Width);
            element.SetAttributeValue("Height", style.Height);
            element.SetAttributeValue("Color", style.Color.ToArgb());
            return element;
        }
        private static XElement FromEntity(Entity entity, Style style)
        {
            XElement element = new XElement("Entity");
            element.SetAttributeValue("Name", entity.Name);
            element.SetAttributeValue("Id", entity.Id);

            foreach (Property p in entity.Properties)
                element.Add(FromProperty(p));
            element.Add(FromStyle(style));

            return element;
        }
        private static XElement FromRelation(Relation relation, Style style)
        {
            XElement element = new XElement("Relation");
            element.SetAttributeValue("Name", relation.Name);
            element.SetAttributeValue("Id", relation.Id);

            foreach (Property p in relation.Properties)
                element.Add(FromProperty(p));
            foreach (EntityLink e in relation.Entities)
                element.Add(FromEntityLink(e));

            element.Add(FromStyle(style));

            return element;
        }
        private static XElement FromAggregation(Aggregation aggregation, Style style)
        {
            XElement relation, element = new XElement("Aggregation");
            element.SetAttributeValue("Name", aggregation.Name);
            element.SetAttributeValue("Id", aggregation.Id);

            element.Add(relation = new XElement("Relation"));
            relation.SetAttributeValue("Id", aggregation.Relation.Id);
            foreach (Property p in aggregation.Properties)
                element.Add(FromProperty(p));

            element.Add(FromStyle(style));

            return element;
        }
        private static XElement FromInheritance(Inheritance inheritance, Style style)
        {
            XElement link, parent, element = new XElement("Inheritance");
            element.SetAttributeValue("Type", inheritance.Type);
            element.SetAttributeValue("Id", inheritance.Id);

            element.Add(parent = new XElement("Parent"));
            parent.SetAttributeValue("Id", inheritance.Parent.Id);
            foreach (Entity e in inheritance.Children)
            {
                element.Add(link = new XElement("Link"));
                link.SetAttributeValue("Id", e.Id);
            }

            element.Add(FromStyle(style));

            return element;
        }

        private static Property ToProperty(XElement element)
        {
            Property property = new Property();
            property.Name = element.Attribute("Name").Value;
            property.Primary = bool.Parse(element.Attribute("Primary").Value);
            property.Type = (PropertyType)Enum.Parse(typeof(PropertyType), element.Attribute("Type").Value);
            return property;
        }
        private static EntityLink ToEntityLink(XElement element, Model model)
        {
            EntityLink entityLink = new EntityLink(null, Cardinality.One);
            entityLink.Entity = model[int.Parse(element.Attribute("Id").Value)] as Entity;
            entityLink.Cardinality = (Cardinality)Enum.Parse(typeof(Cardinality), element.Attribute("Cardinality").Value);
            entityLink.Relative = bool.Parse(element.Attribute("Relative").Value);
            return entityLink;
        }
        private static void ToStyle(XElement element, Style style)
        {
            style.X = int.Parse(element.Attribute("X").Value);
            style.Y = int.Parse(element.Attribute("Y").Value);
            style.Width = int.Parse(element.Attribute("Width").Value);
            style.Height = int.Parse(element.Attribute("Height").Value);
            style.Color = Color.FromArgb(int.Parse(element.Attribute("Color").Value));
        }
        private static EntityStyle ToEntity(XElement element)
        {
            Entity entity = new Entity();
            EntityStyle style = new EntityStyle(entity, null, null);

            entity.Id = int.Parse(element.Attribute("Id").Value);
            entity.Name = element.Attribute("Name").Value;

            foreach (XElement e in element.Elements())
                switch (e.Name.LocalName)
                {
                    case "Property": entity.Properties.Add(ToProperty(e)); break;
                    case "Style": ToStyle(e, style); break;
                }

            return style;
        }
        private static EntityStyle ToAggregation(XElement element)
        {
            Aggregation aggregation = new Aggregation(null);
            EntityStyle style = new EntityStyle(aggregation, null, null);

            aggregation.Id = int.Parse(element.Attribute("Id").Value);
            aggregation.Name = element.Attribute("Name").Value;

            foreach (XElement e in element.Elements())
                switch (e.Name.LocalName)
                {
                    case "Property": aggregation.Properties.Add(ToProperty(e)); break;
                    case "Style": ToStyle(e, style); break;
                }

            return style;
        }
        private static RelationStyle ToRelation(XElement element)
        {
            Relation relation = new Relation();
            RelationStyle style = new RelationStyle(relation, null, null);

            relation.Id = int.Parse(element.Attribute("Id").Value);
            relation.Name = element.Attribute("Name").Value;

            foreach (XElement e in element.Elements())
                switch (e.Name.LocalName)
                {
                    case "Property": relation.Properties.Add(ToProperty(e)); break;
                    case "Style": ToStyle(e, style); break;
                }

            return style;
        }
        private static InheritanceStyle ToInheritance(XElement element)
        {
            Inheritance inheritance = new Inheritance(null, InheritanceType.None);
            InheritanceStyle style = new InheritanceStyle(inheritance, null, null);

            inheritance.Id = int.Parse(element.Attribute("Id").Value);
            inheritance.Type = (InheritanceType)Enum.Parse(typeof(InheritanceType), element.Attribute("Type").Value);

            foreach (XElement e in element.Elements())
                switch (e.Name.LocalName)
                {
                    case "Style": ToStyle(e, style); break;
                }

            return style;
        }

        public static void Save(string path, Model model, Dictionary<object, Style> styles)
        {
            using (FileStream fileStream = File.Create(path))
            using (GZipStream gzStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
                gzStream.WriteByte((byte)'M');
                gzStream.WriteByte((byte)'S');
                gzStream.WriteByte((byte)Version.Major);
                gzStream.WriteByte((byte)Version.Minor);

                using (DeflateStream deflateStream = new DeflateStream(gzStream, CompressionMode.Compress))
                using (XmlWriter xmlWriter = XmlWriter.Create(deflateStream))
                {
                    XDocument document = new XDocument();
                    document.Add(new XElement("Model"));

                    foreach (Entity e in model.Entities)
                        document.Root.Add(e is Aggregation ? FromAggregation(e as Aggregation, styles[e]) : FromEntity(e, styles[e]));
                    foreach (Relation r in model.Relations)
                        document.Root.Add(FromRelation(r, styles[r]));
                    foreach (Inheritance i in model.Inheritances)
                        document.Root.Add(FromInheritance(i, styles[i]));

                    document.WriteTo(xmlWriter);
                }
            }
        }
        public static KeyValuePair<Model, Dictionary<object, Style>> Load(string path)
        {
            Model model = new Model();
            Dictionary<object, Style> styles = new Dictionary<object, Style>();

            using (FileStream fileStream = File.OpenRead(path))
            using (GZipStream gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[4];
                gzStream.Read(buffer, 0, 4);

                if (buffer[0] != (byte)'M' || buffer[1] != (byte)'S')
                    throw new NotSupportedException("Le fichier sélectionné ne contient pas de modèle MeriseSuite ou est corrompu");
                Version version = new Version(buffer[2], buffer[3]);
                if (version > Version)
                    MessageBox.Show("Le fichier sélectionné a été généré à partir d'une version plus récente de MeriseSuite. Toutes les fonctionnalités ne seront peut-être pas présentes.");

                using (DeflateStream deflateStream = new DeflateStream(gzStream, CompressionMode.Decompress))
                using (XmlReader xmlReader = XmlReader.Create(deflateStream))
                {
                    XDocument document = XDocument.Load(xmlReader);

                    // Décodage des objets
                    foreach (XElement element in document.Root.Elements())
                    {
                        try
                        {
                            switch (element.Name.LocalName)
                            {
                                case "Entity":
                                    EntityStyle entityStyle = ToEntity(element);
                                    model.Entities.Add(entityStyle.Entity);
                                    styles.Add(entityStyle.Entity, entityStyle);
                                    break;

                                case "Aggregation":
                                    EntityStyle aggregationStyle = ToAggregation(element);
                                    model.Entities.Add(aggregationStyle.Entity);
                                    styles.Add(aggregationStyle.Entity, aggregationStyle);
                                    break;

                                case "Relation":
                                    RelationStyle relationStyle = ToRelation(element);
                                    model.Relations.Add(relationStyle.Relation);
                                    styles.Add(relationStyle.Relation, relationStyle);
                                    break;

                                case "Inheritance":
                                    InheritanceStyle inheritanceStyle = ToInheritance(element);
                                    model.Inheritances.Add(inheritanceStyle.Inheritance);
                                    styles.Add(inheritanceStyle.Inheritance, inheritanceStyle);
                                    break;
                            }
                        }
                        catch
                        { }
                    }

                    // Liaison des objets
                    foreach (XElement element in document.Root.Elements())
                    {
                        try
                        {
                            int id = int.Parse(element.Attribute("Id").Value);

                            switch (element.Name.LocalName)
                            {
                                case "Aggregation":
                                    foreach (XElement e in element.Elements())
                                        if (e.Name.LocalName == "Relation")
                                            (model[id] as Aggregation).Relation = model[int.Parse(e.Attribute("Id").Value)] as Relation;
                                    break;

                                case "Relation":
                                    foreach (XElement e in element.Elements())
                                        if (e.Name.LocalName == "Link")
                                            (model[id] as Relation).Entities.Add(ToEntityLink(e, model));
                                    break;

                                case "Inheritance":
                                    foreach (XElement e in element.Elements())
                                        if (e.Name.LocalName == "Parent")
                                            model.Inheritances.Single(i => i.Id == id).Parent = model[int.Parse(e.Attribute("Id").Value)] as Entity;
                                        else if (e.Name.LocalName == "Link")
                                            model.Inheritances.Single(i => i.Id == id).Children.Add(model[int.Parse(e.Attribute("Id").Value)] as Entity);
                                    break;
                            }
                        }
                        catch
                        { }
                    }
                }

                return new KeyValuePair<Model, Dictionary<object, Style>>(model, styles);
            }
        }

        public static KeyValuePair<Model, Dictionary<object, Style>> ImportAsi(string path)
        {
            Model model = new Model();
            Dictionary<object, Style> styles = new Dictionary<object, Style>();

            FileStream fileStream = File.OpenRead(path);
            GZipStream gzStream = new GZipStream(fileStream, CompressionMode.Decompress);
            XmlReader xmlReader = XmlReader.Create(gzStream);
            XDocument document = XDocument.Load(xmlReader);

            XElement analyse = document.Element("analyse");
            XElement merise = analyse.Element("module");
            XElement dico = merise.Element("dictionnaire");
            XElement mcd = merise.Element("mcd");

            IEnumerable<XElement> props = dico.Elements("information");

            // Chargement des entités
            IEnumerable<XElement> xEntities = mcd.Elements("entite");
            foreach (XElement xEntity in xEntities)
            {
                Entity entity = new Entity(xEntity.Attribute("nom").Value);
                IEnumerable<XElement> xProperties = xEntity.Elements("information");
                foreach (XElement xProperty in xProperties)
                {
                    string name = xProperty.Attribute("code").Value;
                    string type = props.Single(p => p.Attribute("code").Value == name).Attribute("type").Value;

                    entity.Properties.Add(new Property(name, PropertyType.Text, type == "AUTO_INCREMENT"));
                }

                EntityStyle style = new EntityStyle(entity, null, null);
                style.X = int.Parse(xEntity.Attribute("x").Value);
                style.Y = int.Parse(xEntity.Attribute("y").Value);

                model.Entities.Add(entity);
                styles.Add(entity, style);
            }

            // Chargement des relations
            IEnumerable<XElement> xRelations = mcd.Elements("association");
            foreach (XElement xRelation in xRelations)
            {
                Relation relation = new Relation(xRelation.Attribute("nom").Value);
                IEnumerable<XElement> xProperties = xRelation.Elements("information");
                foreach (XElement xProperty in xProperties)
                    relation.Properties.Add(new Property(xProperty.Attribute("code").Value));

                RelationStyle style = new RelationStyle(relation, null, null);
                style.X = int.Parse(xRelation.Attribute("x").Value);
                style.Y = int.Parse(xRelation.Attribute("y").Value);

                model.Relations.Add(relation);
                styles.Add(relation, style);
            }

            // Chargement des liens entité ~ relation
            IEnumerable<XElement> xLinks = mcd.Elements("lien");
            foreach (XElement xLink in xLinks)
            {
                string min = xLink.Attribute("cardmin").Value;
                string max = xLink.Attribute("cardmax").Value;
                string relationName = xLink.Attribute("elem1").Value;
                string entityName = xLink.Attribute("elem2").Value;

                Entity entity = styles.Keys.Single(e => e is Entity && (e as Entity).Name == entityName) as Entity;
                Relation relation = styles.Keys.Single(r => r is Relation && (r as Relation).Name == relationName) as Relation;
                Cardinality cardinality = Cardinality.ZeroOrMore;

                if (min == "0" && max == "1") cardinality = Cardinality.ZeroOrOne;
                else if (min == "0" && max == "N") cardinality = Cardinality.ZeroOrMore;
                else if (min == "1" && max == "1") cardinality = Cardinality.One;
                else if (min == "1" && max == "N") cardinality = Cardinality.OneOrMore;

                relation.Entities.Add(new EntityLink(entity, cardinality));
            }

            // Normalisation des positions
            int minX = styles.Values.Min(s => s.X);
            int minY = styles.Values.Min(s => s.Y);
            foreach (Style style in styles.Values)
            {
                style.X -= minX - 16;
                style.Y -= minY - 16;
            }

            return new KeyValuePair<Model, Dictionary<object, Style>>(model, styles);
        }
    }
}