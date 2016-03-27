namespace MeriseSuite
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StandardBar = new System.Windows.Forms.ToolStrip();
            this.Standard_NewButton = new System.Windows.Forms.ToolStripButton();
            this.Standard_OpenButton = new System.Windows.Forms.ToolStripButton();
            this.Standard_SaveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.Standard_AddNewEntityButton = new System.Windows.Forms.ToolStripButton();
            this.Standard_NamingStyle_ApplyButton = new System.Windows.Forms.ToolStripSplitButton();
            this.Standard_NamingStyle_PascalCaseButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Standard_NamingStyle_CamelCaseButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Standard_NamingStyle_LowerCaseButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Standard_NamingStyle_UpperCaseButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Standard_NewRelationButton = new System.Windows.Forms.ToolStripButton();
            this.Standard_PropertyStyle_ApplyButton = new System.Windows.Forms.ToolStripSplitButton();
            this.Standard_PropertyStyle_KeyOnlyButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Standard_PropertyStyle_PrefixButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Standard_PropertyStyle_SuffixButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Standard_AddNewLinkButton = new System.Windows.Forms.ToolStripButton();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.Status_Spacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_CreditsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.File_NewButton = new System.Windows.Forms.ToolStripMenuItem();
            this.File_OpenButton = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.File_SaveAsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.File_QuitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ModelMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_AddNewEntityButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Model_AddNewRelatioButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Model_AddNewLinkButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_ColorsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_SetEntityColorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_SetRelationColorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_SetInheritanceColorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_QualityMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_LowQualityButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_HighQualityButton = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Help_AboutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.MainContainer = new System.Windows.Forms.Panel();
            this.ScrollPanel = new System.Windows.Forms.Panel();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.StandardBar.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.MenuBar.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // StandardBar
            // 
            this.StandardBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Standard_NewButton,
            this.Standard_OpenButton,
            this.Standard_SaveButton,
            this.toolStripSeparator,
            this.Standard_AddNewEntityButton,
            this.Standard_NamingStyle_ApplyButton,
            this.Standard_NewRelationButton,
            this.Standard_PropertyStyle_ApplyButton,
            this.Standard_AddNewLinkButton});
            this.StandardBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            resources.ApplyResources(this.StandardBar, "StandardBar");
            this.StandardBar.Name = "StandardBar";
            // 
            // Standard_NewButton
            // 
            this.Standard_NewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Standard_NewButton.Image = global::MeriseSuite.Resources.New;
            resources.ApplyResources(this.Standard_NewButton, "Standard_NewButton");
            this.Standard_NewButton.Name = "Standard_NewButton";
            this.Standard_NewButton.Click += new System.EventHandler(this.Standard_NewButton_Click);
            // 
            // Standard_OpenButton
            // 
            this.Standard_OpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.Standard_OpenButton, "Standard_OpenButton");
            this.Standard_OpenButton.Image = global::MeriseSuite.Resources.Open;
            this.Standard_OpenButton.Name = "Standard_OpenButton";
            this.Standard_OpenButton.Click += new System.EventHandler(this.Standard_OpenButton_Click);
            // 
            // Standard_SaveButton
            // 
            this.Standard_SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.Standard_SaveButton, "Standard_SaveButton");
            this.Standard_SaveButton.Image = global::MeriseSuite.Resources.Save;
            this.Standard_SaveButton.Name = "Standard_SaveButton";
            this.Standard_SaveButton.Click += new System.EventHandler(this.Standard_SaveButton_Click);
            this.Standard_SaveButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Standard_SaveButton_MouseMove);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            // 
            // Standard_AddNewEntityButton
            // 
            this.Standard_AddNewEntityButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Standard_AddNewEntityButton.Image = global::MeriseSuite.Resources.NewEntity;
            resources.ApplyResources(this.Standard_AddNewEntityButton, "Standard_AddNewEntityButton");
            this.Standard_AddNewEntityButton.Name = "Standard_AddNewEntityButton";
            this.Standard_AddNewEntityButton.Click += new System.EventHandler(this.Standard_AddNewEntityButton_Click);
            // 
            // Standard_NamingStyle_ApplyButton
            // 
            this.Standard_NamingStyle_ApplyButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Standard_NamingStyle_ApplyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Standard_NamingStyle_ApplyButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Standard_NamingStyle_PascalCaseButton,
            this.Standard_NamingStyle_CamelCaseButton,
            this.Standard_NamingStyle_LowerCaseButton,
            this.Standard_NamingStyle_UpperCaseButton});
            resources.ApplyResources(this.Standard_NamingStyle_ApplyButton, "Standard_NamingStyle_ApplyButton");
            this.Standard_NamingStyle_ApplyButton.Name = "Standard_NamingStyle_ApplyButton";
            this.Standard_NamingStyle_ApplyButton.ButtonClick += new System.EventHandler(this.Standard_NamingStyle_ApplyButton_ButtonClick);
            // 
            // Standard_NamingStyle_PascalCaseButton
            // 
            this.Standard_NamingStyle_PascalCaseButton.Checked = true;
            this.Standard_NamingStyle_PascalCaseButton.CheckOnClick = true;
            this.Standard_NamingStyle_PascalCaseButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Standard_NamingStyle_PascalCaseButton.Name = "Standard_NamingStyle_PascalCaseButton";
            resources.ApplyResources(this.Standard_NamingStyle_PascalCaseButton, "Standard_NamingStyle_PascalCaseButton");
            this.Standard_NamingStyle_PascalCaseButton.Click += new System.EventHandler(this.Standard_NamingStyle_PascalCaseButton_Click);
            // 
            // Standard_NamingStyle_CamelCaseButton
            // 
            this.Standard_NamingStyle_CamelCaseButton.CheckOnClick = true;
            this.Standard_NamingStyle_CamelCaseButton.Name = "Standard_NamingStyle_CamelCaseButton";
            resources.ApplyResources(this.Standard_NamingStyle_CamelCaseButton, "Standard_NamingStyle_CamelCaseButton");
            this.Standard_NamingStyle_CamelCaseButton.Click += new System.EventHandler(this.Standard_NamingStyle_CamelCaseButton_Click);
            // 
            // Standard_NamingStyle_LowerCaseButton
            // 
            this.Standard_NamingStyle_LowerCaseButton.CheckOnClick = true;
            this.Standard_NamingStyle_LowerCaseButton.Name = "Standard_NamingStyle_LowerCaseButton";
            resources.ApplyResources(this.Standard_NamingStyle_LowerCaseButton, "Standard_NamingStyle_LowerCaseButton");
            this.Standard_NamingStyle_LowerCaseButton.Click += new System.EventHandler(this.Standard_NamingStyle_LowerCaseButton_Click);
            // 
            // Standard_NamingStyle_UpperCaseButton
            // 
            this.Standard_NamingStyle_UpperCaseButton.CheckOnClick = true;
            this.Standard_NamingStyle_UpperCaseButton.Name = "Standard_NamingStyle_UpperCaseButton";
            resources.ApplyResources(this.Standard_NamingStyle_UpperCaseButton, "Standard_NamingStyle_UpperCaseButton");
            this.Standard_NamingStyle_UpperCaseButton.Click += new System.EventHandler(this.Standard_NamingStyle_UpperCaseButton_Click);
            // 
            // Standard_NewRelationButton
            // 
            this.Standard_NewRelationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Standard_NewRelationButton.Image = global::MeriseSuite.Resources.NewRelation;
            resources.ApplyResources(this.Standard_NewRelationButton, "Standard_NewRelationButton");
            this.Standard_NewRelationButton.Name = "Standard_NewRelationButton";
            this.Standard_NewRelationButton.Click += new System.EventHandler(this.Standard_AddNewRelation_Click);
            // 
            // Standard_PropertyStyle_ApplyButton
            // 
            this.Standard_PropertyStyle_ApplyButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Standard_PropertyStyle_ApplyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Standard_PropertyStyle_ApplyButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Standard_PropertyStyle_KeyOnlyButton,
            this.Standard_PropertyStyle_PrefixButton,
            this.Standard_PropertyStyle_SuffixButton});
            resources.ApplyResources(this.Standard_PropertyStyle_ApplyButton, "Standard_PropertyStyle_ApplyButton");
            this.Standard_PropertyStyle_ApplyButton.Name = "Standard_PropertyStyle_ApplyButton";
            this.Standard_PropertyStyle_ApplyButton.ButtonClick += new System.EventHandler(this.Standard_PropertyStyle_ApplyButton_ButtonClick);
            // 
            // Standard_PropertyStyle_KeyOnlyButton
            // 
            this.Standard_PropertyStyle_KeyOnlyButton.Checked = true;
            this.Standard_PropertyStyle_KeyOnlyButton.CheckOnClick = true;
            this.Standard_PropertyStyle_KeyOnlyButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Standard_PropertyStyle_KeyOnlyButton.Name = "Standard_PropertyStyle_KeyOnlyButton";
            resources.ApplyResources(this.Standard_PropertyStyle_KeyOnlyButton, "Standard_PropertyStyle_KeyOnlyButton");
            this.Standard_PropertyStyle_KeyOnlyButton.Click += new System.EventHandler(this.Standard_PropertyStyle_KeyOnlyButton_Click);
            // 
            // Standard_PropertyStyle_PrefixButton
            // 
            this.Standard_PropertyStyle_PrefixButton.CheckOnClick = true;
            this.Standard_PropertyStyle_PrefixButton.Name = "Standard_PropertyStyle_PrefixButton";
            resources.ApplyResources(this.Standard_PropertyStyle_PrefixButton, "Standard_PropertyStyle_PrefixButton");
            this.Standard_PropertyStyle_PrefixButton.Click += new System.EventHandler(this.Standard_PropertyStyle_PrefixButton_Click);
            // 
            // Standard_PropertyStyle_SuffixButton
            // 
            this.Standard_PropertyStyle_SuffixButton.CheckOnClick = true;
            this.Standard_PropertyStyle_SuffixButton.Name = "Standard_PropertyStyle_SuffixButton";
            resources.ApplyResources(this.Standard_PropertyStyle_SuffixButton, "Standard_PropertyStyle_SuffixButton");
            this.Standard_PropertyStyle_SuffixButton.Click += new System.EventHandler(this.Standard_PropertyStyle_SuffixButton_Click);
            // 
            // Standard_AddNewLinkButton
            // 
            this.Standard_AddNewLinkButton.CheckOnClick = true;
            this.Standard_AddNewLinkButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Standard_AddNewLinkButton.Image = global::MeriseSuite.Resources.NewLink;
            resources.ApplyResources(this.Standard_AddNewLinkButton, "Standard_AddNewLinkButton");
            this.Standard_AddNewLinkButton.Name = "Standard_AddNewLinkButton";
            this.Standard_AddNewLinkButton.Click += new System.EventHandler(this.Standard_AddNewLinkButton_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_Spacer,
            this.Status_CreditsLabel});
            resources.ApplyResources(this.StatusBar, "StatusBar");
            this.StatusBar.Name = "StatusBar";
            // 
            // Status_Spacer
            // 
            this.Status_Spacer.Name = "Status_Spacer";
            resources.ApplyResources(this.Status_Spacer, "Status_Spacer");
            this.Status_Spacer.Spring = true;
            // 
            // Status_CreditsLabel
            // 
            this.Status_CreditsLabel.Name = "Status_CreditsLabel";
            resources.ApplyResources(this.Status_CreditsLabel, "Status_CreditsLabel");
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.ModelMenu,
            this.SettingsMenu,
            this.HelpMenu});
            resources.ApplyResources(this.MenuBar, "MenuBar");
            this.MenuBar.Name = "MenuBar";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_NewButton,
            this.File_OpenButton,
            this.File_SaveButton,
            this.File_SaveAsButton,
            this.toolStripSeparator2,
            this.File_QuitButton});
            this.FileMenu.Name = "FileMenu";
            resources.ApplyResources(this.FileMenu, "FileMenu");
            // 
            // File_NewButton
            // 
            this.File_NewButton.Image = global::MeriseSuite.Resources.New;
            this.File_NewButton.Name = "File_NewButton";
            resources.ApplyResources(this.File_NewButton, "File_NewButton");
            this.File_NewButton.Click += new System.EventHandler(this.Standard_NewButton_Click);
            // 
            // File_OpenButton
            // 
            resources.ApplyResources(this.File_OpenButton, "File_OpenButton");
            this.File_OpenButton.Image = global::MeriseSuite.Resources.Open;
            this.File_OpenButton.Name = "File_OpenButton";
            this.File_OpenButton.Click += new System.EventHandler(this.Standard_OpenButton_Click);
            // 
            // File_SaveButton
            // 
            resources.ApplyResources(this.File_SaveButton, "File_SaveButton");
            this.File_SaveButton.Image = global::MeriseSuite.Resources.Save;
            this.File_SaveButton.Name = "File_SaveButton";
            this.File_SaveButton.Click += new System.EventHandler(this.Standard_SaveButton_Click);
            // 
            // File_SaveAsButton
            // 
            resources.ApplyResources(this.File_SaveAsButton, "File_SaveAsButton");
            this.File_SaveAsButton.Name = "File_SaveAsButton";
            this.File_SaveAsButton.Click += new System.EventHandler(this.File_SaveAsButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // File_QuitButton
            // 
            this.File_QuitButton.Name = "File_QuitButton";
            resources.ApplyResources(this.File_QuitButton, "File_QuitButton");
            this.File_QuitButton.Click += new System.EventHandler(this.File_QuitButton_Click);
            // 
            // ModelMenu
            // 
            this.ModelMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_AddNewEntityButton,
            this.Model_AddNewRelatioButton,
            this.Model_AddNewLinkButton});
            this.ModelMenu.Name = "ModelMenu";
            resources.ApplyResources(this.ModelMenu, "ModelMenu");
            // 
            // Menu_AddNewEntityButton
            // 
            this.Menu_AddNewEntityButton.Image = global::MeriseSuite.Resources.NewEntity;
            this.Menu_AddNewEntityButton.Name = "Menu_AddNewEntityButton";
            resources.ApplyResources(this.Menu_AddNewEntityButton, "Menu_AddNewEntityButton");
            this.Menu_AddNewEntityButton.Click += new System.EventHandler(this.Standard_AddNewEntityButton_Click);
            // 
            // Model_AddNewRelatioButton
            // 
            this.Model_AddNewRelatioButton.Image = global::MeriseSuite.Resources.NewRelation;
            this.Model_AddNewRelatioButton.Name = "Model_AddNewRelatioButton";
            resources.ApplyResources(this.Model_AddNewRelatioButton, "Model_AddNewRelatioButton");
            this.Model_AddNewRelatioButton.Click += new System.EventHandler(this.Standard_AddNewRelation_Click);
            // 
            // Model_AddNewLinkButton
            // 
            this.Model_AddNewLinkButton.Image = global::MeriseSuite.Resources.NewLink;
            this.Model_AddNewLinkButton.Name = "Model_AddNewLinkButton";
            resources.ApplyResources(this.Model_AddNewLinkButton, "Model_AddNewLinkButton");
            this.Model_AddNewLinkButton.Click += new System.EventHandler(this.Standard_AddNewLinkButton_Click);
            // 
            // SettingsMenu
            // 
            this.SettingsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Settings_ColorsMenu,
            this.Settings_QualityMenu});
            this.SettingsMenu.Name = "SettingsMenu";
            resources.ApplyResources(this.SettingsMenu, "SettingsMenu");
            // 
            // Settings_ColorsMenu
            // 
            this.Settings_ColorsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Settings_SetEntityColorButton,
            this.Settings_SetRelationColorButton,
            this.Settings_SetInheritanceColorButton});
            this.Settings_ColorsMenu.Name = "Settings_ColorsMenu";
            resources.ApplyResources(this.Settings_ColorsMenu, "Settings_ColorsMenu");
            // 
            // Settings_SetEntityColorButton
            // 
            this.Settings_SetEntityColorButton.Name = "Settings_SetEntityColorButton";
            resources.ApplyResources(this.Settings_SetEntityColorButton, "Settings_SetEntityColorButton");
            this.Settings_SetEntityColorButton.Click += new System.EventHandler(this.Settings_SetEntityColorButton_Click);
            // 
            // Settings_SetRelationColorButton
            // 
            this.Settings_SetRelationColorButton.Name = "Settings_SetRelationColorButton";
            resources.ApplyResources(this.Settings_SetRelationColorButton, "Settings_SetRelationColorButton");
            this.Settings_SetRelationColorButton.Click += new System.EventHandler(this.Settings_SetRelationColorButton_Click);
            // 
            // Settings_SetInheritanceColorButton
            // 
            this.Settings_SetInheritanceColorButton.Name = "Settings_SetInheritanceColorButton";
            resources.ApplyResources(this.Settings_SetInheritanceColorButton, "Settings_SetInheritanceColorButton");
            this.Settings_SetInheritanceColorButton.Click += new System.EventHandler(this.Settings_SetInheritanceColorButton_Click);
            // 
            // Settings_QualityMenu
            // 
            this.Settings_QualityMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Settings_LowQualityButton,
            this.Settings_HighQualityButton});
            this.Settings_QualityMenu.Name = "Settings_QualityMenu";
            resources.ApplyResources(this.Settings_QualityMenu, "Settings_QualityMenu");
            // 
            // Settings_LowQualityButton
            // 
            this.Settings_LowQualityButton.Name = "Settings_LowQualityButton";
            resources.ApplyResources(this.Settings_LowQualityButton, "Settings_LowQualityButton");
            this.Settings_LowQualityButton.Click += new System.EventHandler(this.Settings_LowQualityButton_Click);
            // 
            // Settings_HighQualityButton
            // 
            this.Settings_HighQualityButton.Checked = true;
            this.Settings_HighQualityButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Settings_HighQualityButton.Name = "Settings_HighQualityButton";
            resources.ApplyResources(this.Settings_HighQualityButton, "Settings_HighQualityButton");
            this.Settings_HighQualityButton.Click += new System.EventHandler(this.Settings_HighQualityButton_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Help_AboutButton});
            this.HelpMenu.Name = "HelpMenu";
            resources.ApplyResources(this.HelpMenu, "HelpMenu");
            // 
            // Help_AboutButton
            // 
            this.Help_AboutButton.Name = "Help_AboutButton";
            resources.ApplyResources(this.Help_AboutButton, "Help_AboutButton");
            this.Help_AboutButton.Click += new System.EventHandler(this.Help_AboutButton_Click);
            // 
            // MainContainer
            // 
            this.MainContainer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.MainContainer.Controls.Add(this.ScrollPanel);
            resources.ApplyResources(this.MainContainer, "MainContainer");
            this.MainContainer.Name = "MainContainer";
            // 
            // ScrollPanel
            // 
            resources.ApplyResources(this.ScrollPanel, "ScrollPanel");
            this.ScrollPanel.Name = "ScrollPanel";
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DataPropertyName = "Type";
            resources.ApplyResources(this.dataGridViewComboBoxColumn1, "dataGridViewComboBoxColumn1");
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.DefaultExt = "model";
            resources.ApplyResources(this.SaveFileDialog, "SaveFileDialog");
            this.SaveFileDialog.SupportMultiDottedExtensions = true;
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.DefaultExt = "model";
            resources.ApplyResources(this.OpenFileDialog, "OpenFileDialog");
            this.OpenFileDialog.SupportMultiDottedExtensions = true;
            // 
            // ColorDialog
            // 
            this.ColorDialog.AnyColor = true;
            this.ColorDialog.FullOpen = true;
            this.ColorDialog.SolidColorOnly = true;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.MainContainer);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.StandardBar);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.StandardBar.ResumeLayout(false);
            this.StandardBar.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.MainContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip StandardBar;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem File_QuitButton;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.Panel MainContainer;
        private System.Windows.Forms.ToolStripButton Standard_NewButton;
        private System.Windows.Forms.ToolStripButton Standard_OpenButton;
        private System.Windows.Forms.ToolStripButton Standard_SaveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton Standard_AddNewEntityButton;
        private System.Windows.Forms.ToolStripSplitButton Standard_NamingStyle_ApplyButton;
        private System.Windows.Forms.ToolStripMenuItem Standard_NamingStyle_PascalCaseButton;
        private System.Windows.Forms.ToolStripMenuItem Standard_NamingStyle_CamelCaseButton;
        private System.Windows.Forms.ToolStripMenuItem Standard_NamingStyle_LowerCaseButton;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.ToolStripButton Standard_NewRelationButton;
        private System.Windows.Forms.ToolStripMenuItem Standard_NamingStyle_UpperCaseButton;
        private System.Windows.Forms.ToolStripSplitButton Standard_PropertyStyle_ApplyButton;
        private System.Windows.Forms.ToolStripMenuItem Standard_PropertyStyle_KeyOnlyButton;
        private System.Windows.Forms.ToolStripMenuItem Standard_PropertyStyle_PrefixButton;
        private System.Windows.Forms.ToolStripMenuItem Standard_PropertyStyle_SuffixButton;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel Status_Spacer;
        private System.Windows.Forms.ToolStripStatusLabel Status_CreditsLabel;
        private System.Windows.Forms.ToolStripMenuItem Help_AboutButton;
        private System.Windows.Forms.ToolStripButton Standard_AddNewLinkButton;
        private System.Windows.Forms.ToolStripMenuItem File_NewButton;
        private System.Windows.Forms.ToolStripMenuItem File_OpenButton;
        private System.Windows.Forms.ToolStripMenuItem File_SaveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ModelMenu;
        private System.Windows.Forms.ToolStripMenuItem Menu_AddNewEntityButton;
        private System.Windows.Forms.ToolStripMenuItem Model_AddNewRelatioButton;
        private System.Windows.Forms.ToolStripMenuItem SettingsMenu;
        private System.Windows.Forms.ToolStripMenuItem Settings_ColorsMenu;
        private System.Windows.Forms.ToolStripMenuItem Settings_SetEntityColorButton;
        private System.Windows.Forms.ToolStripMenuItem Settings_SetRelationColorButton;
        private System.Windows.Forms.ToolStripMenuItem Settings_SetInheritanceColorButton;
        private System.Windows.Forms.ToolStripMenuItem Settings_QualityMenu;
        private System.Windows.Forms.ToolStripMenuItem Settings_LowQualityButton;
        private System.Windows.Forms.ToolStripMenuItem Settings_HighQualityButton;
        private System.Windows.Forms.ToolStripMenuItem Model_AddNewLinkButton;
        private System.Windows.Forms.ColorDialog ColorDialog;
        private System.Windows.Forms.ToolStripMenuItem File_SaveAsButton;
        private System.Windows.Forms.Panel ScrollPanel;
    }
}

