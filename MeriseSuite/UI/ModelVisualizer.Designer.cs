using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MeriseSuite
{
    partial class ModelVisualizer
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            #region Menu contextuel

            // Nouvelle entité
            Default_AddEntityButton = new ToolStripMenuItem("Nouvelle entité");
            Default_AddEntityButton.Image = Resources.NewEntity;
            Default_AddEntityButton.Click += Default_AddEntityButton_Click;

            // Nouvelle relation
            Default_AddRelationButton = new ToolStripMenuItem("Nouvelle relation");
            Default_AddRelationButton.Image = Resources.NewRelation;
            Default_AddRelationButton.Click += Default_AddRelationButton_Click;

            // Copier l'image
            Default_CopyImageButton = new ToolStripMenuItem("Copier l'image");
            Default_CopyImageButton.Click += Default_CopyImageButton_Click;

            // Exporter l'image
            Default_ExportImageButton = new ToolStripMenuItem("Exporter l'image");
            Default_ExportImageButton.Click += Default_ExportImageButton_Click;

            #endregion

            #region Entités

            Entity_Label = new ToolStripMenuLabel()
            {
                Image = Resources.Entity,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
            };

            // Lier
            Entity_LinkButton = new ToolStripMenuItem("Lier");
            Entity_LinkButton.Click += Entity_LinkButton_Click;

            // Créer l'héritage
            Entity_AddInheritanceButton = new ToolStripMenuItem("Créer l'héritage");
            Entity_AddInheritanceButton.Click += Entity_AddInheritanceButton_Click;

            // Renommer
            Entity_RenameButton = new ToolStripMenuItem("Renommer");
            Entity_RenameButton.Click += Entity_RenameButton_Click;

            // Supprimer
            Entity_RemoveButton = new ToolStripMenuItem("Supprimer");
            Entity_RemoveButton.Click += Entity_RemoveButton_Click;

            #endregion
            #region Relations

            Relation_Label = new ToolStripMenuLabel()
            {
                Image = Resources.Relation,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
            };

            // Lier
            Relation_LinkButton = new ToolStripMenuItem("Lier");
            Relation_LinkButton.Click += Relation_LinkButton_Click;

            // Agréger
            Relation_AggregateButton = new ToolStripMenuItem("Agréger");
            Relation_AggregateButton.Click += Relation_AggregateButton_Click;

            // Renommer
            Relation_RenameButton = new ToolStripMenuItem("Renommer");
            Relation_RenameButton.Click += Relation_RenameButton_Click;

            // Supprimer
            Relation_RemoveButton = new ToolStripMenuItem("Supprimer");
            Relation_RemoveButton.Click += Relation_RemoveButton_Click;

            #endregion
            #region Liens

            Link_Label = new ToolStripMenuLabel()
            {
                Image = Resources.Link,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
            };

            // Cardinalité
            Link_CardinalityMenu = new ToolStripMenuItem("Cardinalité");

            // - 0, 1
            Link_ZeroOrOneButton = new ToolStripMenuItem("0, 1");
            Link_ZeroOrOneButton.Click += Link_ZeroOrOneButton_Click;
            Link_CardinalityMenu.DropDownItems.Add(Link_ZeroOrOneButton);

            // - 0, n
            Link_ZeroOrMoreButton = new ToolStripMenuItem("0, n");
            Link_ZeroOrMoreButton.Click += Link_ZeroOrMoreButton_Click;
            Link_CardinalityMenu.DropDownItems.Add(Link_ZeroOrMoreButton);

            // - 1, 1
            Link_OneButton = new ToolStripMenuItem("1, 1");
            Link_OneButton.Click += Link_OneButton_Click;
            Link_CardinalityMenu.DropDownItems.Add(Link_OneButton);

            // - 1, n
            Link_OneOrMoreButton = new ToolStripMenuItem("1, n");
            Link_OneOrMoreButton.Click += Link_OneOrMoreButton_Click;
            Link_CardinalityMenu.DropDownItems.Add(Link_OneOrMoreButton);

            // Identification relative
            Link_RelativeButton = new ToolStripMenuItem("Identification relative");
            Link_RelativeButton.Click += Link_RelativeButton_Click;
            
            // Supprimer
            Link_RemoveButton = new ToolStripMenuItem("Supprimer");
            Link_RemoveButton.Click += Link_RemoveButton_Click;

            // Annuler
            Link_CancelButton = new ToolStripMenuItem("Annuler");
            Link_CancelButton.Click += Link_CancelButton_Click;

            #endregion
            #region Propriétés

            Property_Label = new ToolStripMenuLabel()
            {
                Image = Resources.Property,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
            };

            // Clé primaire
            Property_PrimaryKeyButton = new ToolStripMenuItem("Clé primaire");
            Property_PrimaryKeyButton.Click += Property_PrimaryKeyButton_Click;

            // Monter
            Property_MoveUpButton = new ToolStripMenuItem("Monter");
            Property_MoveUpButton.Click += Property_MoveUpButton_Click;

            // Descendre
            Property_MoveDownButton = new ToolStripMenuItem("Descendre");
            Property_MoveDownButton.Click += Property_MoveDownButton_Click;
            
            // Renommer
            Property_RenameButton = new ToolStripMenuItem("Renommer");
            Property_RenameButton.Click += Property_RenameButton_Click;

            // Supprimer
            Property_RemoveButton = new ToolStripMenuItem("Supprimer");
            Property_RemoveButton.Click += Property_RemoveButton_Click;

            #endregion
            #region Héritages

            Inheritance_Label = new ToolStripMenuLabel()
            {
                Image = Resources.Inheritance,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
            };

            // Partition
            Inheritance_PartitionButton = new ToolStripMenuItem("Partition (XT)");
            Inheritance_PartitionButton.Click += Inheritance_PartitionButton_Click;

            // Exclusion
            Inheritance_ExclusionButton = new ToolStripMenuItem("Exclusion (X)");
            Inheritance_ExclusionButton.Click += Inheritance_ExclusionButton_Click;

            // Totalité
            Inheritance_TotalButton = new ToolStripMenuItem("Totalité (T)");
            Inheritance_TotalButton.Click += Inheritance_TotalButton_Click;

            // Lier
            Inheritance_LinkButton = new ToolStripMenuItem("Lier");
            Inheritance_LinkButton.Click += Inheritance_LinkButton_Click;

            // Supprimer
            Inheritance_RemoveButton = new ToolStripMenuItem("Supprimer");
            Inheritance_RemoveButton.Click += Inheritance_RemoveButton_Click;

            #endregion

            DynamicContextMenu = new ContextMenuStrip(components);
            ContextMenuStrip = DynamicContextMenu;

            ResumeLayout(false);
        }

        private ContextMenuStrip DynamicContextMenu;

        private ToolStripMenuItem Default_AddEntityButton;
        private ToolStripMenuItem Default_AddRelationButton;
        private ToolStripMenuItem Default_CopyImageButton;
        private ToolStripMenuItem Default_ExportImageButton;

        private ToolStripMenuItem Entity_Label;
        private ToolStripMenuItem Entity_LinkButton;
        private ToolStripMenuItem Entity_AddInheritanceButton;
        private ToolStripMenuItem Entity_RenameButton;
        private ToolStripMenuItem Entity_RemoveButton;

        private ToolStripMenuItem Relation_Label;
        private ToolStripMenuItem Relation_LinkButton;
        private ToolStripMenuItem Relation_AggregateButton;
        private ToolStripMenuItem Relation_RenameButton;
        private ToolStripMenuItem Relation_RemoveButton;

        private ToolStripMenuItem Link_Label;
        private ToolStripMenuItem Link_CardinalityMenu;
        private ToolStripMenuItem Link_ZeroOrOneButton;
        private ToolStripMenuItem Link_ZeroOrMoreButton;
        private ToolStripMenuItem Link_OneButton;
        private ToolStripMenuItem Link_OneOrMoreButton;
        private ToolStripMenuItem Link_RelativeButton;
        private ToolStripMenuItem Link_RemoveButton;
        private ToolStripMenuItem Link_CancelButton;

        private ToolStripMenuItem Property_Label;
        private ToolStripMenuItem Property_PrimaryKeyButton;
        private ToolStripMenuItem Property_MoveUpButton;
        private ToolStripMenuItem Property_MoveDownButton;
        private ToolStripMenuItem Property_RenameButton;
        private ToolStripMenuItem Property_RemoveButton;

        private ToolStripMenuItem Inheritance_Label;
        private ToolStripMenuItem Inheritance_PartitionButton;
        private ToolStripMenuItem Inheritance_ExclusionButton;
        private ToolStripMenuItem Inheritance_TotalButton;
        private ToolStripMenuItem Inheritance_LinkButton;
        private ToolStripMenuItem Inheritance_RemoveButton;
    }
}