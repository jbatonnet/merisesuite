using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using MeriseSuite.Definitions;
using MeriseSuite.Modelling;
using MeriseSuite.Styling;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using MeriseSuite.History;

namespace MeriseSuite
{
    public partial class ModelVisualizer : ScrollableControl
    {
        private const int SelectionMarkerSize = 4;
        private const int LinkSelectionMargin = 6;

        public ActionStack Actions { get; private set; }

        public Color BorderColor { get; set; }
        public Color EntityColor { get; set; }
        public Color RelationColor { get; set; }
        public Color InheritanceColor { get; set; }
        public Color SelectionColor { get; set; }

        public Model Model { get; set; }
        public Dictionary<object, Style> Styles { get; set; }

        public bool Quality { get; set; }
        public bool Linking { get; set; }

        private ToolTip toolTip = new ToolTip();
        private TextBox editor = new TextBox();

        private object highlightedElement = null;
        private object selectedElement = null;
        private Dictionary<object, Point> selectedElements = new Dictionary<object, Point>();

        private MouseStyle mouseStyle;
        public LinkStyle NewLinkStyle = null;

        private Point clickLocation;
        private Point elementLocation;
        private Point mouseLocation;

        private Point selectionLocation;
        private Rectangle selectionRectangle;
        private bool selecting = false;

        private Queue<double> times = new Queue<double>();

        public ModelVisualizer()
        {
            DoubleBuffered = true;

            Actions = new ActionStack();

            EntityColor = Color.LightSteelBlue;
            RelationColor = Color.LightPink;
            SelectionColor = Color.CadetBlue;
            mouseStyle = new MouseStyle(this);

            Quality = true;

            InitializeComponent();

            Model = new Model();
            Styles = new Dictionary<object, Style>();

            toolTip.AutoPopDelay = 30000;

            editor.BorderStyle = BorderStyle.None;
            editor.Visible = false;
            editor.VisibleChanged += Editor_VisibleChanged;
            editor.KeyPress += Editor_KeyPress;
            Controls.Add(editor);
        }

        public void Select(object element)
        {
            if (!Styles.ContainsKey(element))
                return;

            UpdateStyles(CreateGraphics());

            selectedElement = element;
            elementLocation = Styles[selectedElement].Location;

            if (!selectedElements.ContainsKey(selectedElement))
            {
                if (!selecting)
                    selectedElements.Clear();
                else
                    for (int i = 0; i < selectedElements.Count; i++)
                    {
                        object e = selectedElements.Keys.ElementAt(i);
                        selectedElements[e] = Styles[e].Location;
                    }

                selectedElements.Add(selectedElement, elementLocation);
            }
            else
                selectedElements[selectedElement] = elementLocation;
        }
        public void Unselect()
        {
            selectedElement = null;
            selectedElements.Clear();

            selectionLocation = Point.Empty;
            selectionRectangle = Rectangle.Empty;
        }
        public void Edit(object element)
        {
            if (element is Entity)
                Editor_Edit(element as Entity);
            else if (element is Relation)
                Editor_Edit(element as Relation);
            else if (element is Property)
                Editor_Edit(element as Property);
        }
        public void Redraw()
        {
            Invalidate(false);
        }
        public void UpdateStyles()
        {
            UpdateStyles(CreateGraphics());
        }
        public void UpdateStyles(Graphics graphics)
        {
            if (Model == null)
                return;

            #region Unused styles

            for (int i = 0; i < Styles.Count; i++)
            {
                object element = Styles.Keys.ElementAt(i);
                bool remove = false;

                if (element is Entity)
                {
                    if (!Model.Entities.Contains(element as Entity))
                        remove = true;
                }
                else if (element is Relation)
                {
                    if (!Model.Relations.Contains(element as Relation))
                        remove = true;
                }
                else if (element is RelationLink)
                {
                    if (!Model.Entities.Contains((element as RelationLink).EntityLink.Entity) || !Model.Relations.Contains((element as RelationLink).Relation) || !(element as RelationLink).Relation.Entities.Contains((element as RelationLink).EntityLink))
                        remove = true;
                }
                else if (element is Inheritance)
                {
                    if (!Model.Inheritances.Contains(element as Inheritance))
                        remove = true;
                }
                else if (element is InheritanceLink)
                {
                    if (!Model.Entities.Contains((element as InheritanceLink).Entity) || !Model.Inheritances.Contains((element as InheritanceLink).Inheritance))
                        remove = true;
                    if (((element as InheritanceLink).Inheritance.Parent != (element as InheritanceLink).Entity) && (!(element as InheritanceLink).Inheritance.Children.Contains((element as InheritanceLink).Entity)))
                        remove = true;
                }

                if (remove)
                {
                    Styles.Remove(element);
                    i--;
                }
            }

            #endregion
            #region Entities

            foreach (Entity entity in Model.Entities)
                if (!Styles.ContainsKey(entity))
                    Styles.Add(entity, new EntityStyle(entity, graphics, this));

            #endregion
            #region Relations

            foreach (Relation relation in Model.Relations)
            {
                if (!Styles.ContainsKey(relation))
                    Styles.Add(relation, new RelationStyle(relation, graphics, this));

                foreach (EntityLink entity in relation.Entities)
                {
                    RelationLink relationLink = new RelationLink(entity, relation);
                    if (Styles.Count(kvp => kvp.Key.Equals(relationLink)) == 0)
                        Styles.Add(relationLink, new LinkStyle(Styles[entity.Entity], Styles[relation], relationLink, graphics, this) { Tag = entity });
                }
            }

            #endregion
            #region Inheritances

            foreach (Inheritance inheritance in Model.Inheritances)
            {
                if (!Styles.ContainsKey(inheritance))
                    Styles.Add(inheritance, new InheritanceStyle(inheritance, graphics, this));

                InheritanceLink parentLink = new InheritanceLink(inheritance.Parent, inheritance);
                if (Styles.Count(kvp => kvp.Key.Equals(parentLink)) == 0)
                    Styles.Add(parentLink, new LinkStyle(Styles[inheritance.Parent], Styles[inheritance], parentLink, graphics, this));

                foreach (Entity child in inheritance.Children)
                {
                    InheritanceLink inheritanceLink = new InheritanceLink(child, inheritance);
                    if (Styles.Count(kvp => kvp.Key.Equals(inheritanceLink)) == 0)
                        Styles.Add(inheritanceLink, new LinkStyle(Styles[child], Styles[inheritance], inheritanceLink, graphics, this));
                }
            }

            #endregion

            foreach (Style style in Styles.Values)
                style.Update(graphics);

            mouseStyle.Update(mouseLocation.X, mouseLocation.Y);
            if (NewLinkStyle != null)
                NewLinkStyle.Update(graphics);

            if (Styles.Count > 0)
            {
                int maxX = Math.Max(Styles.Values.Max(s => s.Right) + 16, MinimumSize.Width);
                int maxY = Math.Max(Styles.Values.Max(s => s.Bottom) + 16, MinimumSize.Height);

                Size = new Size(maxX, maxY);
            }
            else
                Size = MinimumSize;

            (Parent as Panel).AutoScroll = Size.Width > Parent.Width || Size.Height > Parent.Height;
        }

        private void UpdateHighlightedElement(Point mouseLocation)
        {
            object newHighlightedElement = null;
            if (selectionLocation == Point.Empty)
            {
                foreach (var style in Styles.Where(kvp => kvp.Key is Entity))
                    if (mouseLocation.X > style.Value.Left && mouseLocation.X < style.Value.Right && mouseLocation.Y > style.Value.Top && mouseLocation.Y < style.Value.Bottom)
                        newHighlightedElement = style.Key;
                foreach (var style in Styles.Where(kvp => kvp.Key is Relation))
                    if (mouseLocation.X > style.Value.Left && mouseLocation.X < style.Value.Right && mouseLocation.Y > style.Value.Top && mouseLocation.Y < style.Value.Bottom)
                        newHighlightedElement = style.Key;
                foreach (var style in Styles.Where(kvp => kvp.Key is Inheritance))
                    if (mouseLocation.X > style.Value.Left && mouseLocation.X < style.Value.Right && mouseLocation.Y > style.Value.Top && mouseLocation.Y < style.Value.Bottom)
                        newHighlightedElement = style.Key;

                foreach (var style in Styles.Where(kvp => kvp.Value is LinkStyle))
                {
                    if (Linking || NewLinkStyle != null)
                        continue;

                    LinkStyle linkStyle = style.Value as LinkStyle;
                    PointF a = linkStyle.End1Location;
                    PointF b = linkStyle.End2Location;
                    Point c = mouseLocation;

                    double l = Math.Sqrt((a.Y - b.Y) * (a.Y - b.Y) + (a.X - b.X) * (a.X - b.X));
                    double r = ((a.Y - c.Y) * (a.Y - b.Y) - (a.X - c.X) * (b.X - a.X)) / (l * l);

                    if (r < 0 || r > 1)
                        continue;

                    PointF p = new PointF((float)(a.X + r * (b.X - a.X)), (float)(a.Y + r * (b.Y - a.Y)));
                    double d = Math.Sqrt((p.Y - c.Y) * (p.Y - c.Y) + (p.X - c.X) * (p.X - c.X));

                    if (d <= LinkSelectionMargin)
                        newHighlightedElement = style.Key;
                }
            }

            if (newHighlightedElement != highlightedElement)
            {
                highlightedElement = newHighlightedElement;
                Redraw();

                toolTip.Active = highlightedElement != null && !(highlightedElement is Entity);
                if (toolTip.Active)
                    toolTip.SetToolTip(this, highlightedElement.ToString());
            }
        }
        private void Draw(Graphics g)
        {
            if (Quality)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }

            if (NewLinkStyle != null)
                NewLinkStyle.Draw(g, false);
            foreach (LinkStyle linkStyle in Styles.Values.OfType<LinkStyle>().Where(l => l.End1Style.Element is Aggregation || l.End2Style.Element is Aggregation))
                linkStyle.Draw(g, (linkStyle.Element == highlightedElement || linkStyle.End1Style.Element == highlightedElement || linkStyle.End2Style.Element == highlightedElement) && (NewLinkStyle == null || NewLinkStyle.End2Style == linkStyle));
            foreach (EntityStyle aggregationStyle in Styles.Values.OfType<EntityStyle>().Where(s => s.Entity is Aggregation))
                aggregationStyle.Draw(g, aggregationStyle.Element == highlightedElement && (NewLinkStyle == null || NewLinkStyle.End2Style == aggregationStyle) || (highlightedElement != null && Styles.ContainsKey(highlightedElement) && Styles[highlightedElement] is LinkStyle && ((Styles[highlightedElement] as LinkStyle).End1Style.Element == aggregationStyle.Element || (Styles[highlightedElement] as LinkStyle).End2Style.Element == aggregationStyle.Element)));
            foreach (LinkStyle linkStyle in Styles.Values.OfType<LinkStyle>().Where(l => !(l.End1Style.Element is Aggregation) && !(l.End2Style.Element is Aggregation)))
                linkStyle.Draw(g, (linkStyle.Element == highlightedElement || linkStyle.End1Style.Element == highlightedElement || linkStyle.End2Style.Element == highlightedElement) && (NewLinkStyle == null || NewLinkStyle.End2Style == linkStyle));
            foreach (InheritanceStyle inheritanceStyle in Styles.Values.OfType<InheritanceStyle>())
                inheritanceStyle.Draw(g, inheritanceStyle.Element == highlightedElement && (NewLinkStyle == null || NewLinkStyle.End2Style == inheritanceStyle) || (highlightedElement != null && Styles.ContainsKey(highlightedElement) && Styles[highlightedElement] is LinkStyle && ((Styles[highlightedElement] as LinkStyle).End1Style.Element == inheritanceStyle.Element || (Styles[highlightedElement] as LinkStyle).End2Style.Element == inheritanceStyle.Element)));
            foreach (Style style in Styles.Values.Where(s => s is RelationStyle || (s is EntityStyle && !(s.Element is Aggregation))))
                style.Draw(g, style.Element == highlightedElement && (NewLinkStyle == null || NewLinkStyle.End2Style == style) || (highlightedElement != null && Styles.ContainsKey(highlightedElement) && Styles[highlightedElement] is LinkStyle && ((Styles[highlightedElement] as LinkStyle).End1Style.Element == style.Element || (Styles[highlightedElement] as LinkStyle).End2Style.Element == style.Element)));

            foreach (object se in selectedElements.Keys)
            {
                if (Styles.ContainsKey(se))
                {
                    Style style = Styles[se];

                    if (se is Entity || se is Relation || se is Inheritance)
                    {
                        g.FillRectangle(Brushes.White, new Rectangle(style.Left - SelectionMarkerSize / 2, style.Top - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.DrawRectangle(Pens.Black, new Rectangle(style.Left - SelectionMarkerSize / 2, style.Top - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.FillRectangle(Brushes.White, new Rectangle(style.Right - SelectionMarkerSize / 2, style.Top - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.DrawRectangle(Pens.Black, new Rectangle(style.Right - SelectionMarkerSize / 2, style.Top - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.FillRectangle(Brushes.White, new Rectangle(style.Right - SelectionMarkerSize / 2, style.Bottom - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.DrawRectangle(Pens.Black, new Rectangle(style.Right - SelectionMarkerSize / 2, style.Bottom - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.FillRectangle(Brushes.White, new Rectangle(style.Left - SelectionMarkerSize / 2, style.Bottom - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.DrawRectangle(Pens.Black, new Rectangle(style.Left - SelectionMarkerSize / 2, style.Bottom - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                    }
                    else if (style is LinkStyle)
                    {
                        LinkStyle linkStyle = style as LinkStyle;

                        g.FillRectangle(Brushes.White, new Rectangle((int)linkStyle.End1Location.X - SelectionMarkerSize / 2, (int)linkStyle.End1Location.Y - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.DrawRectangle(Pens.Black, new Rectangle((int)linkStyle.End1Location.X - SelectionMarkerSize / 2, (int)linkStyle.End1Location.Y - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.FillRectangle(Brushes.White, new Rectangle((int)linkStyle.End2Location.X - SelectionMarkerSize / 2, (int)linkStyle.End2Location.Y - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                        g.DrawRectangle(Pens.Black, new Rectangle((int)linkStyle.End2Location.X - SelectionMarkerSize / 2, (int)linkStyle.End2Location.Y - SelectionMarkerSize / 2, SelectionMarkerSize, SelectionMarkerSize));
                    }
                }
            }

            if (selectionLocation != Point.Empty)
            {
                if (Quality)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(96, SelectionColor)), selectionRectangle);
                g.DrawRectangle(new Pen(SelectionColor), selectionRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Model == null)
                return;

            Graphics g = e.Graphics;
            UpdateStyles(g);

            Draw(g);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (editor.Visible && editor.Tag != null)
                editor.Hide();

            clickLocation = e.Location;
            UpdateHighlightedElement(e.Location);

            #region Clic gauche

            if (e.Button == MouseButtons.Left)
            {
                if (Linking)
                {
                    if (highlightedElement != null)
                        NewLinkStyle = new LinkStyle(Styles[highlightedElement], mouseStyle, null, null, this);
                    clickLocation = Point.Empty;
                    Linking = false;
                }

                #region Sélection d'un élément

                else if (highlightedElement != null && NewLinkStyle == null)
                {
                    Select(highlightedElement);

                    // Mise en avant de l'élément sélectionné
                    Style style = Styles[selectedElement];

                    if (style is LinkStyle && Styles.Values.Count(s => s is LinkStyle && (s as LinkStyle).End1Style == (style as LinkStyle).End1Style && (s as LinkStyle).End2Style == (style as LinkStyle).End2Style) == 1)
                    {
                        Styles.Remove(selectedElement);
                        Styles = Styles.Union(new List<KeyValuePair<object, Style>>() { new KeyValuePair<object, Style>(selectedElement, style) }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    }
                }

                #endregion
                #region Liaison d'un élément

                else if (highlightedElement != null && NewLinkStyle != null && NewLinkStyle.End2Style.Element == highlightedElement)
                {
                    if (NewLinkStyle.End1Style.Element is Entity && highlightedElement is Inheritance)
                    {
                        Actions.Execute(new InheritanceLinkCreation(this, new InheritanceLink(NewLinkStyle.End1Style.Element as Entity, highlightedElement as Inheritance)));
                        NewLinkStyle = null;
                    }
                    else if (NewLinkStyle.End1Style.Element is Inheritance && highlightedElement is Entity)
                    {
                        Actions.Execute(new InheritanceLinkCreation(this, new InheritanceLink(highlightedElement as Entity, NewLinkStyle.End1Style.Element as Inheritance)));
                        NewLinkStyle = null;
                    }

                    else if (NewLinkStyle.End1Style.Element is Entity && highlightedElement is Relation)
                        Link_Label.Text = (NewLinkStyle.End1Style.Element as Entity).Name + " - " + (highlightedElement as Relation).Name + " (Nouveau lien)";
                    else if (NewLinkStyle.End1Style.Element is Relation && highlightedElement is Entity)
                        Link_Label.Text = (highlightedElement as Entity).Name + " - " + (NewLinkStyle.End1Style.Element as Relation).Name + " (Nouveau lien)";

                    if (NewLinkStyle != null && ((NewLinkStyle.End1Style.Element is Entity && highlightedElement is Relation) || (NewLinkStyle.End1Style.Element is Relation && highlightedElement is Entity)))
                    {
                        DynamicContextMenu.Items.Clear();

                        Link_ZeroOrOneButton.Checked = Link_ZeroOrMoreButton.Checked = Link_OneButton.Checked = Link_OneOrMoreButton.Checked = false;

                        DynamicContextMenu.Items.Add(Link_Label);
                        DynamicContextMenu.Items.Add(Link_ZeroOrOneButton);
                        DynamicContextMenu.Items.Add(Link_ZeroOrMoreButton);
                        DynamicContextMenu.Items.Add(Link_OneButton);
                        DynamicContextMenu.Items.Add(Link_OneOrMoreButton);
                        DynamicContextMenu.Items.Add(new ToolStripSeparator());
                        DynamicContextMenu.Items.Add(Link_CancelButton);

                        DynamicContextMenu.Tag = NewLinkStyle;
                        DynamicContextMenu.Show(PointToScreen(e.Location));
                    }
                }

                #endregion
                #region Clic dans le vide

                else if (highlightedElement == null)
                {
                    if (NewLinkStyle != null)
                    {
                        NewLinkStyle = null;
                        clickLocation = Point.Empty;
                    }
                    else if (selectedElement is Entity)
                        (selectedElement as Entity).Properties.RemoveAll(p => p.Name.Trim() == "");
                    else if (selectedElement is Relation)
                        (selectedElement as Relation).Properties.RemoveAll(p => p.Name.Trim() == "");

                    Unselect();
                }

                #endregion
            }

            #endregion
            #region Clic droit

            if (e.Button == MouseButtons.Right)
            {
                if (NewLinkStyle != null)
                {
                    NewLinkStyle = null;
                    Redraw();
                }

                #region Sur un élément

                if (highlightedElement != null && NewLinkStyle == null)
                {
                    Select(highlightedElement);

                    DynamicContextMenu.Tag = selectedElement;
                    DynamicContextMenu.Items.Clear();

                    if (selectedElement is Entity)
                    {
                        Entity_Label.Text = (selectedElement as Entity).Name + " (" + (selectedElement is Aggregation ? "Agrégation" : "Entité") + ")";

                        DynamicContextMenu.Items.Add(Entity_Label);
                        DynamicContextMenu.Items.Add(Entity_LinkButton);
                        DynamicContextMenu.Items.Add(Entity_AddInheritanceButton);
                        DynamicContextMenu.Items.Add(Entity_RenameButton);
                        DynamicContextMenu.Items.Add(Entity_RemoveButton);
                    }
                    else if (selectedElement is Relation)
                    {
                        Relation_Label.Text = (selectedElement as Relation).Name + " (Relation)";

                        DynamicContextMenu.Items.Add(Relation_Label);
                        DynamicContextMenu.Items.Add(Relation_LinkButton);
                        if (Model.Entities.Count(a => a is Aggregation && (a as Aggregation).Relation == selectedElement) == 0)
                            DynamicContextMenu.Items.Add(Relation_AggregateButton);
                        DynamicContextMenu.Items.Add(Relation_RenameButton);
                        DynamicContextMenu.Items.Add(Relation_RemoveButton);
                    }
                    else if (selectedElement is Inheritance)
                    {
                        Inheritance inheritance = selectedElement as Inheritance;

                        Inheritance_Label.Text = (selectedElement as Inheritance).Parent.Name + " (Héritage)";
                        Inheritance_PartitionButton.Checked = inheritance.Type == InheritanceType.Partition;
                        Inheritance_ExclusionButton.Checked = inheritance.Type == InheritanceType.Exclusion;
                        Inheritance_TotalButton.Checked = inheritance.Type == InheritanceType.Total;

                        DynamicContextMenu.Items.Add(Inheritance_Label);
                        DynamicContextMenu.Items.Add(Inheritance_PartitionButton);
                        DynamicContextMenu.Items.Add(Inheritance_ExclusionButton);
                        DynamicContextMenu.Items.Add(Inheritance_TotalButton);
                        DynamicContextMenu.Items.Add(new ToolStripSeparator());
                        DynamicContextMenu.Items.Add(Inheritance_LinkButton);
                        DynamicContextMenu.Items.Add(Inheritance_RemoveButton);
                    }

                    if (selectedElement is RelationLink)
                    {
                        RelationLink link = selectedElement as RelationLink;

                        Link_Label.Text = link.EntityLink.Entity.Name + " - " + link.Relation.Name + " (Lien)";
                        Link_ZeroOrOneButton.Checked = link.EntityLink.Cardinality == Cardinality.ZeroOrOne;
                        Link_ZeroOrMoreButton.Checked = link.EntityLink.Cardinality == Cardinality.ZeroOrMore;
                        Link_OneButton.Checked = link.EntityLink.Cardinality == Cardinality.One;
                        Link_OneOrMoreButton.Checked = link.EntityLink.Cardinality == Cardinality.OneOrMore;
                        Link_RelativeButton.Checked = link.EntityLink.Relative;

                        DynamicContextMenu.Items.Add(Link_Label);
                        DynamicContextMenu.Items.Add(Link_ZeroOrOneButton);
                        DynamicContextMenu.Items.Add(Link_ZeroOrMoreButton);
                        DynamicContextMenu.Items.Add(Link_OneButton);
                        DynamicContextMenu.Items.Add(Link_OneOrMoreButton);
                        DynamicContextMenu.Items.Add(Link_RelativeButton);
                        DynamicContextMenu.Items.Add(new ToolStripSeparator());
                        DynamicContextMenu.Items.Add(Link_RemoveButton);
                    }

                    if (selectedElement is Entity || selectedElement is Relation)
                    {
                        Property property = null;
                        int propertyIndex = clickLocation.Y - 24 - Styles[selectedElement].Top;
                        int propertyCount = 0;
                        if (propertyIndex > 0)
                            propertyIndex /= 16;

                        if (selectedElement is Entity && propertyIndex >= 0 && propertyIndex < (selectedElement as Entity).Properties.Count)
                        {
                            property = (selectedElement as Entity).Properties[propertyIndex];
                            propertyCount = (selectedElement as Entity).Properties.Count;
                        }
                        else if (selectedElement is Relation && propertyIndex >= 0 && propertyIndex < (selectedElement as Relation).Properties.Count)
                        {
                            property = (selectedElement as Relation).Properties[propertyIndex];
                            propertyCount = (selectedElement as Relation).Properties.Count;
                        }

                        if (property != null)
                        {
                            if (DynamicContextMenu.Items.Count > 0)
                                DynamicContextMenu.Items.Add(new ToolStripSeparator());

                            Property_Label.Tag = property;
                            Property_Label.Text = property.Name + " (Propriété)";
                            Property_PrimaryKeyButton.Checked = property.Primary;

                            DynamicContextMenu.Items.Add(Property_Label);
                            if (selectedElement is Entity)
                                DynamicContextMenu.Items.Add(Property_PrimaryKeyButton);
                            if (propertyIndex > 0)
                                DynamicContextMenu.Items.Add(Property_MoveUpButton);
                            if (propertyIndex < propertyCount - 1)
                                DynamicContextMenu.Items.Add(Property_MoveDownButton);
                            DynamicContextMenu.Items.Add(Property_RenameButton);
                            DynamicContextMenu.Items.Add(Property_RemoveButton);
                        }
                    }

                    ContextMenuStrip = DynamicContextMenu;
                }

                #endregion
                #region Dans le vide

                else if (highlightedElement == null)
                {
                    Unselect();

                    DynamicContextMenu.Items.Clear();
                    DynamicContextMenu.Items.Add(Default_AddEntityButton);
                    DynamicContextMenu.Items.Add(Default_AddRelationButton);

                    if (Styles.Count > 0)
                    {
                        DynamicContextMenu.Items.Add(new ToolStripSeparator());
                        DynamicContextMenu.Items.Add(Default_CopyImageButton);
                        DynamicContextMenu.Items.Add(Default_ExportImageButton);
                    }
                }

                #endregion

                else
                    ContextMenuStrip = null;
            }

            #endregion

            Redraw();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            mouseLocation = e.Location;

            // Cas d'un linkage
            if (NewLinkStyle != null)
            {
                if (NewLinkStyle.End1Style.Element is Entity)
                {
                    Entity entity = NewLinkStyle.End1Style.Element as Entity;
                    if (highlightedElement == null ||
                        highlightedElement is Entity ||
                        Styles[highlightedElement] is LinkStyle ||
                        Styles.Values.OfType<LinkStyle>().Count(l => l.End1Style.Element == entity && l.End2Style.Element == highlightedElement && highlightedElement is Inheritance) > 0 ||
                        (entity is Aggregation && (entity as Aggregation).Relation == highlightedElement))
                        NewLinkStyle.End2Style = mouseStyle;
                    else
                        NewLinkStyle.End2Style = Styles[highlightedElement];
                }
                else if (NewLinkStyle.End1Style.Element is Relation)
                {
                    if (!(highlightedElement is Entity) ||
                        (highlightedElement is Aggregation && (highlightedElement as Aggregation).Relation == NewLinkStyle.End1Style.Element))
                        NewLinkStyle.End2Style = mouseStyle;
                    else
                        NewLinkStyle.End2Style = Styles[highlightedElement];
                }
                else if (NewLinkStyle.End1Style.Element is Inheritance)
                {
                    if (highlightedElement is Entity && (NewLinkStyle.End1Style.Element as Inheritance).Parent != highlightedElement && !(NewLinkStyle.End1Style.Element as Inheritance).Children.Contains(highlightedElement as Entity))
                        NewLinkStyle.End2Style = Styles[highlightedElement];
                    else
                        NewLinkStyle.End2Style = mouseStyle;
                }

                Redraw();
            }

            if (selectedElements.Count > 0 && e.Button == MouseButtons.Left && e.Location != clickLocation && selectionLocation == Point.Empty && clickLocation != Point.Empty)
            {
                bool redraw = false;

                ActionGroup actions = new ActionGroup(this);
                foreach (var se in selectedElements)
                {
                    Point oldLocation = Styles[se.Key].Location;
                    Point newLocation = se.Value + new Size(e.Location) - new Size(clickLocation);

                    newLocation.X -= newLocation.X % 10;
                    if (newLocation.X < 0) newLocation.X = 0;
                    newLocation.Y -= newLocation.Y % 10;
                    if (newLocation.Y < 0) newLocation.Y = 0;

                    if (oldLocation != newLocation)
                    {
                        // TODO: Actions.Add(new Modification);
                        if (se.Key is Aggregation)
                        {
                            Relation selectedRelation = (se.Key as Aggregation).Relation;
                            Styles[selectedRelation].Location = newLocation + new Size(4, 24);
                        }
                        else
                            Styles[se.Key].Location = newLocation;

                        redraw = true;
                    }
                }

                if (redraw)
                    Redraw();
            }
            else if (e.Button == MouseButtons.Left && e.Location != clickLocation && clickLocation != Point.Empty)
            {
                selectionLocation = e.Location;

                int x = Math.Min(clickLocation.X, selectionLocation.X);
                int y = Math.Min(clickLocation.Y, selectionLocation.Y);
                int width = Math.Max(clickLocation.X, selectionLocation.X) - x;
                int height = Math.Max(clickLocation.Y, selectionLocation.Y) - y;
                selectionRectangle = new Rectangle(x, y, width, height);

                selectedElements.Clear();
                foreach (Style style in Styles.Values)
                    if (style.Left > selectionRectangle.Left && style.Right < selectionRectangle.Right && style.Top > selectionRectangle.Top && style.Bottom < selectionRectangle.Bottom)
                        selectedElements.Add(style.Element, style.Location);
                foreach (LinkStyle style in Styles.Values.OfType<LinkStyle>())
                    if (style.End1Location.X > selectionRectangle.Left && style.End1Location.X < selectionRectangle.Right && style.End1Location.Y > selectionRectangle.Top && style.End1Location.Y < selectionRectangle.Bottom && style.End2Location.X > selectionRectangle.Left && style.End2Location.X < selectionRectangle.Right && style.End2Location.Y > selectionRectangle.Top && style.End2Location.Y < selectionRectangle.Bottom)
                        if (!selectedElements.ContainsKey(style.Element))
                            selectedElements.Add(style.Element, style.Location);

                Redraw();
            }

            UpdateHighlightedElement(e.Location);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            selectionLocation = Point.Empty;

            if (NewLinkStyle != null && highlightedElement != null)
                OnMouseDown(e);

            Redraw();
        }
        protected override void OnDoubleClick(EventArgs e)
        {
            if (selectedElement == null || !(selectedElement is Element))
                return;
            Style style = Styles[selectedElement];

            // Edition du nom
            if (clickLocation.X > style.Left && clickLocation.X < style.Right && clickLocation.Y > style.Top && clickLocation.Y < style.Top + 20)
                Editor_Edit(selectedElement as Element);

            // Edition des propriétés
            else if (clickLocation.X > style.Left && clickLocation.X < style.Right && clickLocation.Y > style.Top + 20 && clickLocation.Y < style.Bottom && !(selectedElement is Aggregation))
            {
                Property newProperty;
                int propertyIndex = (clickLocation.Y - 24 - style.Top) / 16;

                if (propertyIndex < (selectedElement as Element).Properties.Count)
                    Editor_Edit((selectedElement as Element).Properties[propertyIndex]);
                else
                {
                    Actions.Execute(new PropertyCreation(this, selectedElement as Element, newProperty = new Property("", PropertyType.Text)));
                    Editor_Edit(newProperty);
                }

                Redraw();
            }
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            selecting = false;

            #region Ctrl + A

            if (e.Control && e.KeyCode == Keys.A)
            {
                selectedElements.Clear();
                foreach (var element in Styles)
                    selectedElements.Add(element.Key, element.Value.Location);
                Redraw();
            }

            #endregion
            #region Ctrl + Z

            if (e.Control && e.KeyCode == Keys.Z)
                Actions.Rollback();

            #endregion
            #region Ctrl + Y

            if (e.Control && e.KeyCode == Keys.Y)
                Actions.Execute();

            #endregion

            #region Supprimer

            if (e.KeyCode == Keys.Delete)
            {
                ActionGroup group = new ActionGroup(this);

                foreach (object element in selectedElements.Keys)
                {
                    if (element is RelationLink)
                        group.Add(new RelationLinkDeletion(this, element as RelationLink));
                    else if (element is InheritanceLink)
                        group.Add(new InheritanceLinkDeletion(this, element as InheritanceLink));
                }
                foreach (object element in selectedElements.Keys)
                {
                    if (element is Entity)
                        group.Add(new EntityDeletion(this, element as Entity));
                    else if (element is Relation)
                        group.Add(new RelationDeletion(this, element as Relation));
                    else if (element is Inheritance)
                        group.Add(new InheritanceDeletion(this, element as Inheritance));
                }

                Actions.Execute(group);

                Unselect();
                Redraw();
            }

            #endregion
            #region Echap

            if (e.KeyCode == Keys.Escape)
            {
                NewLinkStyle = null;
                Redraw();
            }

            #endregion
            #region Touches directionnelles

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
            {
                if (selectedElement != null)
                {
                    Style style;

                    if (selectedElement is Aggregation)
                        style = Styles[(selectedElement as Aggregation).Relation];
                    else
                        style = Styles[selectedElement];

                    if (e.KeyCode == Keys.Left)
                        style.Location = new Point(style.Location.X - 10, style.Location.Y);
                    if (e.KeyCode == Keys.Up)
                        style.Location = new Point(style.Location.X, style.Location.Y - 10);
                    if (e.KeyCode == Keys.Right)
                        style.Location = new Point(style.Location.X + 10, style.Location.Y);
                    if (e.KeyCode == Keys.Down)
                        style.Location = new Point(style.Location.X, style.Location.Y + 10);

                    UpdateStyles(CreateGraphics());
                    Redraw();
                }
            }

            #endregion
            #region Tabulation

            if (e.KeyCode == Keys.Tab && selectedElements.Count == 1)
            {
                int i = Styles.Keys.ToList().IndexOf(selectedElement);
                if (e.Shift)
                    i--;
                else
                    i++;
                if (i < 0) i = Styles.Count - 1;
                if (i >= Styles.Count) i = 0;

                Select(Styles.Keys.ElementAt(i));
                Redraw();
            }

            #endregion
            #region Maj ou Ctrl

            if (e.Shift || e.Control)
                selecting = true;

            #endregion
            #region F2

            if (e.KeyCode == Keys.F2 && selectedElements.Count == 1)
                Edit(selectedElement);

            #endregion
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            #region Maj ou Ctrl

            if (e.Shift || e.Control)
                selecting = false;

            #endregion
        }

        private void Default_AddEntityButton_Click(object sender, EventArgs e)
        {
            Entity entity = new Entity();
            Actions.Execute(new EntityCreation(this, entity));

            Styles[entity].Location = clickLocation;
            Select(entity);
            Redraw();

            Edit(entity);
        }
        private void Default_AddRelationButton_Click(object sender, EventArgs e)
        {
            Relation relation = new Relation();
            Actions.Execute(new RelationCreation(this, relation));

            Styles[relation].Location = clickLocation;
            Select(relation);
            Redraw();

            Edit(relation);
        }
        private void Default_CopyImageButton_Click(object sender, EventArgs e)
        {
            UpdateStyles(CreateGraphics());

            int minLeft = Styles.Values.Min(s => s.Left);
            int minTop = Styles.Values.Min(s => s.Top);
            int maxRight = Styles.Values.Max(s => s.Right);
            int maxBottom = Styles.Values.Max(s => s.Bottom);

            Bitmap image1 = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(image1))
            {
                g.Clear(Color.Transparent);
                Draw(g);

                image1 = image1.Clone(new Rectangle(minLeft, minTop, maxRight - minLeft + 1, maxBottom - minTop + 1), PixelFormat.Format32bppArgb);
            }

            Bitmap image2 = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(image2))
            {
                g.Clear(Color.White);
                Draw(g);

                image2 = image2.Clone(new Rectangle(minLeft, minTop, maxRight - minLeft + 1, maxBottom - minTop + 1), PixelFormat.Format24bppRgb);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                image1.Save(stream, ImageFormat.Png);

                DataObject dataObject = new DataObject();
                dataObject.SetImage(image2);
                dataObject.SetData("PNG", stream);

                Clipboard.SetDataObject(dataObject, true);
            }
        }
        private void Default_ExportImageButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.DefaultExt = "png";
            fileDialog.Filter = "Images PNG|*.png";
            fileDialog.Title = "Enregistrer l'image du modèle";

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            UpdateStyles(CreateGraphics());

            int minLeft = Styles.Values.Min(s => s.Left);
            int minTop = Styles.Values.Min(s => s.Top);
            int maxRight = Styles.Values.Max(s => s.Right);
            int maxBottom = Styles.Values.Max(s => s.Bottom);

            Bitmap image = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.Transparent);
                Draw(g);

                image = image.Clone(new Rectangle(minLeft, minTop, maxRight - minLeft + 1, maxBottom - minTop + 1), PixelFormat.Format32bppArgb);
            }
            
            try
            {
                image.Save(fileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de l'enregistrement de l'image");
            }
        }

        private void Entity_LinkButton_Click(object sender, EventArgs e)
        {
            NewLinkStyle = new LinkStyle(Styles[selectedElement], mouseStyle, null, null, this);
        }
        private void Entity_AddInheritanceButton_Click(object sender, EventArgs e)
        {
            Inheritance inheritance = new Inheritance(selectedElement as Entity, InheritanceType.None);
            Actions.Execute(new InheritanceCreation(this, inheritance));

            Styles[inheritance].Location = new Point(Styles[selectedElement].Left + (Styles[selectedElement].Width - Styles[inheritance].Width) / 2, Styles[selectedElement].Bottom + 24);
            Select(inheritance);
            Redraw();
        }
        private void Entity_RenameButton_Click(object sender, EventArgs e)
        {
            Edit(selectedElement);
        }
        private void Entity_RemoveButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new EntityDeletion(this, selectedElement as Entity));
        }

        private void Relation_LinkButton_Click(object sender, EventArgs e)
        {
            NewLinkStyle = new LinkStyle(Styles[selectedElement], mouseStyle, null, null, this);
        }
        private void Relation_AggregateButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new EntityCreation(this, new Aggregation(selectedElement as Relation)));
        }
        private void Relation_RenameButton_Click(object sender, EventArgs e)
        {
            Edit(selectedElement);
        }
        private void Relation_RemoveButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new RelationDeletion(this, selectedElement as Relation));
        }

        private void Link_ZeroOrOneButton_Click(object sender, EventArgs e)
        {
            if (DynamicContextMenu.Tag == NewLinkStyle)
            {

                if (highlightedElement is Relation)
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(NewLinkStyle.End1Style.Element as Entity, Cardinality.ZeroOrOne), highlightedElement as Relation)));
                else
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(highlightedElement as Entity, Cardinality.ZeroOrOne), NewLinkStyle.End1Style.Element as Relation)));
                NewLinkStyle = null;
            }
            else
                Actions.Execute(new RelationLinkCardinalityModification(this, DynamicContextMenu.Tag as RelationLink, Cardinality.ZeroOrOne));
        }
        private void Link_ZeroOrMoreButton_Click(object sender, EventArgs e)
        {
            if (DynamicContextMenu.Tag == NewLinkStyle)
            {

                if (highlightedElement is Relation)
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(NewLinkStyle.End1Style.Element as Entity, Cardinality.ZeroOrMore), highlightedElement as Relation)));
                else
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(highlightedElement as Entity, Cardinality.ZeroOrMore), NewLinkStyle.End1Style.Element as Relation)));
                NewLinkStyle = null;
            }
            else
                Actions.Execute(new RelationLinkCardinalityModification(this, DynamicContextMenu.Tag as RelationLink, Cardinality.ZeroOrMore));
        }
        private void Link_OneButton_Click(object sender, EventArgs e)
        {
            if (DynamicContextMenu.Tag == NewLinkStyle)
            {

                if (highlightedElement is Relation)
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(NewLinkStyle.End1Style.Element as Entity, Cardinality.One), highlightedElement as Relation)));
                else
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(highlightedElement as Entity, Cardinality.One), NewLinkStyle.End1Style.Element as Relation)));
                NewLinkStyle = null;
            }
            else
                Actions.Execute(new RelationLinkCardinalityModification(this, DynamicContextMenu.Tag as RelationLink, Cardinality.One));
        }
        private void Link_OneOrMoreButton_Click(object sender, EventArgs e)
        {
            if (DynamicContextMenu.Tag == NewLinkStyle)
            {

                if (highlightedElement is Relation)
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(NewLinkStyle.End1Style.Element as Entity, Cardinality.OneOrMore), highlightedElement as Relation)));
                else
                    Actions.Execute(new RelationLinkCreation(this, new RelationLink(new EntityLink(highlightedElement as Entity, Cardinality.OneOrMore), NewLinkStyle.End1Style.Element as Relation)));
                NewLinkStyle = null;
            }
            else
                Actions.Execute(new RelationLinkCardinalityModification(this, DynamicContextMenu.Tag as RelationLink, Cardinality.OneOrMore));
        }
        private void Link_RelativeButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new RelationLinkRelativeModification(this, DynamicContextMenu.Tag as RelationLink, !(DynamicContextMenu.Tag as RelationLink).EntityLink.Relative));
        }
        private void Link_RemoveButton_Click(object sender, EventArgs e)
        {
            if (DynamicContextMenu.Tag is RelationLink)
                Actions.Execute(new RelationLinkDeletion(this, DynamicContextMenu.Tag as RelationLink));
            else if (DynamicContextMenu.Tag is InheritanceLink)
                Actions.Execute(new InheritanceLinkDeletion(this, DynamicContextMenu.Tag as InheritanceLink));
        }
        private void Link_CancelButton_Click(object sender, EventArgs e)
        {
            NewLinkStyle = null;
        }

        private void Property_PrimaryKeyButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new PropertyPrimaryModification(this, Property_Label.Tag as Property, !(Property_Label.Tag as Property).Primary));
        }
        private void Property_MoveUpButton_Click(object sender, EventArgs e)
        {
            if (DynamicContextMenu.Tag is Element)
            {
                Element element = DynamicContextMenu.Tag as Element;
                Property property = Property_Label.Tag as Property;
                if (element.Properties.First() != property)
                    Actions.Execute(new PropertyPositionUpModification(this, element, property));
            }
        }
        private void Property_MoveDownButton_Click(object sender, EventArgs e)
        {
            if (DynamicContextMenu.Tag is Element)
            {
                Element element = DynamicContextMenu.Tag as Element;
                Property property = Property_Label.Tag as Property;
                if (element.Properties.Last() != property)
                    Actions.Execute(new PropertyPositionDownModification(this, element, property));
            }
        }
        private void Property_RenameButton_Click(object sender, EventArgs e)
        {
            Edit(Property_Label.Tag);
        }
        private void Property_RemoveButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new PropertyDeletion(this, DynamicContextMenu.Tag as Element, Property_Label.Tag as Property));
        }

        private void Inheritance_PartitionButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new InheritanceTypeModification(this, selectedElement as Inheritance, InheritanceType.Partition));
        }
        private void Inheritance_ExclusionButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new InheritanceTypeModification(this, selectedElement as Inheritance, InheritanceType.Exclusion));
        }
        private void Inheritance_TotalButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new InheritanceTypeModification(this, selectedElement as Inheritance, InheritanceType.Total));
        }
        private void Inheritance_LinkButton_Click(object sender, EventArgs e)
        {
            NewLinkStyle = new LinkStyle(Styles[selectedElement], mouseStyle, null, null, this);
        }
        private void Inheritance_RemoveButton_Click(object sender, EventArgs e)
        {
            Actions.Execute(new InheritanceDeletion(this, selectedElement as Inheritance));
        }

        private void Editor_Edit(Element element)
        {
            Style style = Styles[element];

            editor.Tag = selectedElement;
            editor.Location = new Point(style.X + 6, style.Y + 4);
            editor.Size = new Size(style.Width - 12, 16);
            editor.Font = Font;
            editor.BackColor = style.Color;
            editor.TextAlign = HorizontalAlignment.Center;
            editor.Text = element.Name;
            editor.Show();
            editor.Focus();
            editor.SelectAll();
        }
        private void Editor_Edit(Property property)
        {
            Element element = Model.GetElementByProperty(property);
            Style style;
            int index;

            style = Styles[element];
            index = element.Properties.IndexOf(property);

            editor.Tag = property;
            editor.Location = new Point(style.X + 20, style.Y + 24 + index * 16);
            editor.Size = new Size(style.Width - 24, 16);
            editor.Font = Font;
            editor.BackColor = Color.White;
            editor.TextAlign = HorizontalAlignment.Left;
            editor.Text = (editor.Tag as Property).Name;
            editor.Show();
            editor.Focus();
            editor.SelectAll();
        }
        private void Editor_VisibleChanged(object sender, EventArgs e)
        {
            if (editor.Visible)
                return;

            string name = editor.Text.Trim();

            #region Entity

            if (editor.Tag is Entity)
            {
                if ((editor.Tag as Entity).Name != name)
                    Actions.Execute(new ElementNameModification(this, editor.Tag as Entity, name));
            }

            #endregion
            #region Relation

            else if (editor.Tag is Relation)
            {
                Relation relation = editor.Tag as Relation;
                Aggregation aggregation = Model.Entities.SingleOrDefault(el => el is Aggregation && (el as Aggregation).Relation == relation) as Aggregation;

                ElementNameModification nameChange = new ElementNameModification(this, relation, name);

                if (aggregation != null && aggregation.Name == "@" + relation.Name)
                {
                    ActionGroup group = new ActionGroup(this);
                    group.Add(nameChange);
                    group.Add(new ElementNameModification(this, aggregation, "@" + name));
                    Actions.Execute(group);
                }
                else
                    Actions.Execute(nameChange);
            }

            #endregion
            #region Property

            else if (editor.Tag is Property)
            {
                if ((editor.Tag as Property).Name != name)
                    Actions.Execute(new PropertyNameModification(this, editor.Tag as Property, name));
            }

            #endregion
        }
        private void Editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            #region Entrée

            if ((e.KeyChar == '\r' || e.KeyChar == '\n') && editor.Text != "")
            {
                e.Handled = true;
                editor.Hide();

                if (editor.Tag is Property)
                {
                    Element element = Model.GetElementByProperty(editor.Tag as Property);

                    Property property = new Property("", PropertyType.Text);
                    Actions.Execute(new PropertyCreation(this, element, property));

                    Editor_Edit(property);
                }
            }

            #endregion
            #region Backspace

            if (e.KeyChar == '\b' && editor.Text == "")
            {
                e.Handled = true;
                editor.Hide();

                if (editor.Tag is Property)
                {
                    Element element = Model.GetElementByProperty(editor.Tag as Property);

                    int index = element.Properties.IndexOf(editor.Tag as Property) - 1;
                    if (index < 0)
                        index = 0;

                    Actions.Execute(new PropertyDeletion(this, element, editor.Tag as Property));
                    if (element.Properties.Count > 0)
                        Editor_Edit(element.Properties[index]);
                }
            }

            #endregion
        }
    }
}