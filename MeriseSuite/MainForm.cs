using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using MeriseSuite.Modelling;
using MeriseSuite.Properties;
using MeriseSuite.Styling;
using MeriseSuite.History;
using System.Collections;
using System.Xml.Linq;
using System.Threading;

namespace MeriseSuite
{
    public enum NamingStyle
    {
        PascalCase,
        CamelCase,
        LowerCase,
        UpperCase
    }
    public enum PropertyStyle
    {
        KeyOnly,
        KeyPrefix,
        KeySuffix
    }

    public partial class MainForm : Form
    {
        private ModelVisualizer ModelVisualizer;
        private string CurrentFileName;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ModelVisualizer = new ModelVisualizer();
            ModelVisualizer.BackgroundImage = Resources.Grid;
            ModelVisualizer.BorderColor = Color.Gray;
            ModelVisualizer.AllowDrop = true;
            ModelVisualizer.DragEnter += ModelVisualizer_DragEnter;
            ModelVisualizer.DragDrop += ModelVisualizer_DragDrop;
            ModelVisualizer.Click += ModelVisualizer_Click;
            ScrollPanel.Controls.Add(ModelVisualizer);

            MainForm_Resize(sender, e);

            ModelVisualizer.Quality = Settings.Default.Quality;
            Settings_HighQualityButton.Checked = ModelVisualizer.Quality;
            Settings_LowQualityButton.Checked = !ModelVisualizer.Quality;

            Standard_NamingStyle_PascalCaseButton.Checked = Standard_NamingStyle_CamelCaseButton.Checked = Standard_NamingStyle_LowerCaseButton.Checked = Standard_NamingStyle_UpperCaseButton.Checked = false;
            switch (Settings.Default.NamingStyle)
            {
                case NamingStyle.PascalCase: Standard_NamingStyle_PascalCaseButton.Checked = true; Standard_NamingStyle_ApplyButton.Image = Resources.PascalCase; break;
                case NamingStyle.CamelCase: Standard_NamingStyle_CamelCaseButton.Checked = true; Standard_NamingStyle_ApplyButton.Image = Resources.CamelCase; break;
                case NamingStyle.LowerCase: Standard_NamingStyle_LowerCaseButton.Checked = true; Standard_NamingStyle_ApplyButton.Image = Resources.LowerCase; break;
                case NamingStyle.UpperCase: Standard_NamingStyle_UpperCaseButton.Checked = true; Standard_NamingStyle_ApplyButton.Image = Resources.UpperCase; break;
            }

            Standard_PropertyStyle_KeyOnlyButton.Checked = Standard_PropertyStyle_PrefixButton.Checked = Standard_PropertyStyle_SuffixButton.Checked = false;
            switch (Settings.Default.PropertyStyle)
            {
                case PropertyStyle.KeyOnly: Standard_PropertyStyle_KeyOnlyButton.Checked = true; Standard_PropertyStyle_ApplyButton.Image = Resources.KeyOnly; break;
                case PropertyStyle.KeyPrefix: Standard_PropertyStyle_PrefixButton.Checked = true; Standard_PropertyStyle_ApplyButton.Image = Resources.KeyPrefix; break;
                case PropertyStyle.KeySuffix: Standard_PropertyStyle_SuffixButton.Checked = true; Standard_PropertyStyle_ApplyButton.Image = Resources.KeySuffix; break;
            }

            ModelVisualizer.EntityColor = Settings.Default.EntityColor;
            ModelVisualizer.RelationColor = Settings.Default.RelationColor;
            ModelVisualizer.InheritanceColor = Settings.Default.InheritanceColor;

            #region Chargement du fichier passé en paramètre

            Standard_NewButton_Click(sender, e);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                OpenFile(args[1]);

            #endregion

            Standard_OpenButton.Enabled = File_OpenButton.Enabled = true;
            Standard_SaveButton.Enabled = File_SaveButton.Enabled = File_SaveAsButton.Enabled = true;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ModelVisualizer.Actions.Count > 0)
            {
                switch (MessageBox.Show("Le modèle actuel a été modifié, voulez-vous l'enregistrer ?", "MeriseSuite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes: Standard_SaveButton_Click(sender, e); break;
                    case DialogResult.Cancel: e.Cancel = true; return;
                }
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (ModelVisualizer != null)
            {
                ModelVisualizer.MinimumSize = ScrollPanel.Size;
                ModelVisualizer.Redraw();
            }
        }

        #region MenuBar

        private void File_SaveAsButton_Click(object sender, EventArgs e)
        {
            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                if (CurrentFileName == null)
                    CurrentFileName = SaveFileDialog.FileName;

                ModelVisualizer.Redraw();
                if (File.Exists(SaveFileDialog.FileName))
                    File.Move(SaveFileDialog.FileName, SaveFileDialog.FileName + ".bak");
                IO.Save(SaveFileDialog.FileName, ModelVisualizer.Model, ModelVisualizer.Styles);
                File.Delete(SaveFileDialog.FileName + ".bak");

                Text = "MeriseSuite - " + Path.GetFileName(CurrentFileName);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Erreur lors de l'enregistrement du modèle", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void File_QuitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Settings_SetEntityColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ModelVisualizer.EntityColor;
            if (ColorDialog.ShowDialog() != DialogResult.OK)
                return;

            foreach (Style style in ModelVisualizer.Styles.Values)
                if (style.Element is Entity && style.Color == ModelVisualizer.EntityColor)
                    style.Color = ColorDialog.Color;

            ModelVisualizer.EntityColor = ColorDialog.Color;
            ModelVisualizer.Redraw();

            Settings.Default.EntityColor = ColorDialog.Color;
            Settings.Default.Save();
        }
        private void Settings_SetRelationColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ModelVisualizer.RelationColor;
            if (ColorDialog.ShowDialog() != DialogResult.OK)
                return;

            foreach (Style style in ModelVisualizer.Styles.Values)
                if (style.Element is Relation && style.Color == ModelVisualizer.RelationColor)
                    style.Color = ColorDialog.Color;

            ModelVisualizer.RelationColor = ColorDialog.Color;
            ModelVisualizer.Redraw();

            Settings.Default.RelationColor = ColorDialog.Color;
            Settings.Default.Save();
        }
        private void Settings_SetInheritanceColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ModelVisualizer.InheritanceColor;
            if (ColorDialog.ShowDialog() != DialogResult.OK)
                return;

            foreach (Style style in ModelVisualizer.Styles.Values)
                if (style.Element is Inheritance && style.Color == ModelVisualizer.InheritanceColor)
                    style.Color = ColorDialog.Color;

            ModelVisualizer.InheritanceColor = ColorDialog.Color;
            ModelVisualizer.Redraw();

            Settings.Default.InheritanceColor = ColorDialog.Color;
            Settings.Default.Save();
        }
        private void Settings_LowQualityButton_Click(object sender, EventArgs e)
        {
            Settings_LowQualityButton.Checked = true;
            Settings_HighQualityButton.Checked = false;

            Settings.Default.Quality = ModelVisualizer.Quality = false;
            Settings.Default.Save();

            ModelVisualizer.Redraw();
        }
        private void Settings_HighQualityButton_Click(object sender, EventArgs e)
        {
            Settings_HighQualityButton.Checked = true;
            Settings_LowQualityButton.Checked = false;

            Settings.Default.Quality = ModelVisualizer.Quality = true;
            Settings.Default.Save();

            ModelVisualizer.Redraw();
        }

        private void Help_AboutButton_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        #endregion
        #region StandardBar

        private void Standard_NewButton_Click(object sender, EventArgs e)
        {
            if (ModelVisualizer.Actions.Count > 0)
            {
                switch (MessageBox.Show("Le modèle actuel a été modifié, voulez-vous l'enregistrer ?", "MeriseSuite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes: Standard_SaveButton_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }

            ModelVisualizer.Actions.Clear();

            ModelVisualizer.Model = new Model();
            ModelVisualizer.Quality = Settings.Default.Quality;
            ModelVisualizer.EntityColor = Settings.Default.EntityColor;
            ModelVisualizer.RelationColor = Settings.Default.RelationColor;
            ModelVisualizer.InheritanceColor = Settings.Default.InheritanceColor;
            ModelVisualizer.Styles = new Dictionary<object, Style>();
            ModelVisualizer.Redraw();

            CurrentFileName = null;

            Text = "MeriseSuite";
        }
        private void Standard_OpenButton_Click(object sender, EventArgs e)
        {
            if (ModelVisualizer.Actions.Count > 0)
            {
                switch (MessageBox.Show("Le modèle actuel a été modifié, voulez-vous l'enregistrer ?", "MeriseSuite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes: Standard_SaveButton_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }

            if (OpenFileDialog.ShowDialog() != DialogResult.OK)
                return;

            OpenFile(OpenFileDialog.FileName);
        }
        private void Standard_SaveButton_Click(object sender, EventArgs e)
        {
            if (CurrentFileName == null || Path.GetExtension(CurrentFileName) != ".model")
                if (SaveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

            try
            {
                if (CurrentFileName == null)
                    CurrentFileName = SaveFileDialog.FileName;

                ModelVisualizer.Redraw();
                if (File.Exists(CurrentFileName))
                    File.Move(CurrentFileName, CurrentFileName + ".bak");
                IO.Save(CurrentFileName, ModelVisualizer.Model, ModelVisualizer.Styles);
                File.Delete(CurrentFileName + ".bak");

                ModelVisualizer.Actions.Clear();
                Text = "MeriseSuite - " + Path.GetFileName(CurrentFileName);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Erreur lors de l'enregistrement du modèle", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Standard_SaveButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ModelVisualizer.Model != null)
            {
                string fileName = Path.GetFileName(CurrentFileName ?? "Nouveau modèle.model");
                if (fileName.Contains('.'))
                    fileName = fileName.Remove(fileName.LastIndexOf('.'));
                fileName = Path.GetTempPath() + fileName + ".model";

                try
                {
                    ModelVisualizer.Redraw();
                    IO.Save(fileName, ModelVisualizer.Model, ModelVisualizer.Styles);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Erreur lors de l'enregistrement du modèle", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }

                DoDragDrop(new DataObject(DataFormats.FileDrop, new string[] { fileName }), DragDropEffects.Copy | DragDropEffects.Move);
            }

        }

        private void Standard_AddNewEntityButton_Click(object sender, EventArgs e)
        {
            ModelVisualizer.Actions.Execute(new EntityCreation(ModelVisualizer, new Entity()));
        }
        private void Standard_AddNewRelation_Click(object sender, EventArgs e)
        {
            ModelVisualizer.Actions.Execute(new RelationCreation(ModelVisualizer, new Relation()));
        }
        private void Standard_AddNewLinkButton_Click(object sender, EventArgs e)
        {
            ModelVisualizer.Linking = !ModelVisualizer.Linking;
            Standard_AddNewLinkButton.Checked = ModelVisualizer.Linking;
        }

        private void Standard_NamingStyle_ApplyButton_ButtonClick(object sender, EventArgs e)
        {
            NamingStyle style = NamingStyle.PascalCase;
            if (Standard_NamingStyle_PascalCaseButton.Checked) style = NamingStyle.PascalCase;
            if (Standard_NamingStyle_CamelCaseButton.Checked) style = NamingStyle.CamelCase;
            if (Standard_NamingStyle_LowerCaseButton.Checked) style = NamingStyle.LowerCase;
            if (Standard_NamingStyle_UpperCaseButton.Checked) style = NamingStyle.UpperCase;

            Settings.Default.NamingStyle = style;
            Settings.Default.Save();

            string name;
            ActionGroup group = new ActionGroup(ModelVisualizer);

            foreach (Entity entity in ModelVisualizer.Model.Entities)
            {
                if (entity.Name != (name = ApplyNamingStyle(entity.Name, style)))
                    group.Add(new ElementNameModification(ModelVisualizer, entity, name));
                foreach (Property property in entity.Properties)
                    if (property.Name != (name = ApplyNamingStyle(property.Name, style)))
                        group.Add(new PropertyNameModification(ModelVisualizer, property, name));
            }
            foreach (Relation relation in ModelVisualizer.Model.Relations)
            {
                if (relation.Name != (name = ApplyNamingStyle(relation.Name, style)))
                    group.Add(new ElementNameModification(ModelVisualizer, relation, name));
                foreach (Property property in relation.Properties)
                    if (property.Name != (name = ApplyNamingStyle(property.Name, style)))
                        group.Add(new PropertyNameModification(ModelVisualizer, property, name));
            }

            if (group.Actions.Count > 0)
                ModelVisualizer.Actions.Execute(group);
        }
        private void Standard_NamingStyle_PascalCaseButton_Click(object sender, EventArgs e)
        {
            Standard_NamingStyle_CamelCaseButton.Checked = Standard_NamingStyle_LowerCaseButton.Checked = Standard_NamingStyle_UpperCaseButton.Checked = false;
            Standard_NamingStyle_ApplyButton.Image = Resources.PascalCase;
            Standard_NamingStyle_ApplyButton_ButtonClick(sender, e);
        }
        private void Standard_NamingStyle_CamelCaseButton_Click(object sender, EventArgs e)
        {
            Standard_NamingStyle_PascalCaseButton.Checked = Standard_NamingStyle_LowerCaseButton.Checked = Standard_NamingStyle_UpperCaseButton.Checked = false;
            Standard_NamingStyle_ApplyButton.Image = Resources.CamelCase;
            Standard_NamingStyle_ApplyButton_ButtonClick(sender, e);
        }
        private void Standard_NamingStyle_LowerCaseButton_Click(object sender, EventArgs e)
        {
            Standard_NamingStyle_PascalCaseButton.Checked = Standard_NamingStyle_CamelCaseButton.Checked = Standard_NamingStyle_UpperCaseButton.Checked = false;
            Standard_NamingStyle_ApplyButton.Image = Resources.LowerCase;
            Standard_NamingStyle_ApplyButton_ButtonClick(sender, e);
        }
        private void Standard_NamingStyle_UpperCaseButton_Click(object sender, EventArgs e)
        {
            Standard_NamingStyle_PascalCaseButton.Checked = Standard_NamingStyle_CamelCaseButton.Checked = Standard_NamingStyle_LowerCaseButton.Checked = false;
            Standard_NamingStyle_ApplyButton.Image = Resources.UpperCase;
            Standard_NamingStyle_ApplyButton_ButtonClick(sender, e);
        }

        private void Standard_PropertyStyle_ApplyButton_ButtonClick(object sender, EventArgs e)
        {
            NamingStyle namingStyle = NamingStyle.PascalCase;
            if (Standard_NamingStyle_PascalCaseButton.Checked) namingStyle = NamingStyle.PascalCase;
            if (Standard_NamingStyle_CamelCaseButton.Checked) namingStyle = NamingStyle.CamelCase;
            if (Standard_NamingStyle_LowerCaseButton.Checked) namingStyle = NamingStyle.LowerCase;
            if (Standard_NamingStyle_UpperCaseButton.Checked) namingStyle = NamingStyle.UpperCase;

            PropertyStyle propertyStyle = PropertyStyle.KeyOnly;
            if (Standard_PropertyStyle_KeyOnlyButton.Checked) propertyStyle = PropertyStyle.KeyOnly;
            if (Standard_PropertyStyle_PrefixButton.Checked) propertyStyle = PropertyStyle.KeyPrefix;
            if (Standard_PropertyStyle_SuffixButton.Checked) propertyStyle = PropertyStyle.KeySuffix;

            Settings.Default.PropertyStyle = propertyStyle;
            Settings.Default.Save();

            string name;
            ActionGroup group = new ActionGroup(ModelVisualizer);

            foreach (Entity entity in ModelVisualizer.Model.Entities)
                foreach (Property property in entity.Properties)
                    if (property.Name != (name = ApplyPropertyStyle(property.Name, entity.Name, propertyStyle, namingStyle)))
                        group.Add(new PropertyNameModification(ModelVisualizer, property, name));

            foreach (Relation relation in ModelVisualizer.Model.Relations)
                foreach (Property property in relation.Properties)
                    if (property.Name != (name = ApplyPropertyStyle(property.Name, relation.Name, propertyStyle, namingStyle)))
                        group.Add(new PropertyNameModification(ModelVisualizer, property, name));

            if (group.Actions.Count > 0)
                ModelVisualizer.Actions.Execute(group);
        }
        private void Standard_PropertyStyle_KeyOnlyButton_Click(object sender, EventArgs e)
        {
            Standard_PropertyStyle_PrefixButton.Checked = Standard_PropertyStyle_SuffixButton.Checked = false;
            Standard_PropertyStyle_ApplyButton.Image = Resources.KeyOnly;
            Standard_PropertyStyle_ApplyButton_ButtonClick(sender, e);
        }
        private void Standard_PropertyStyle_PrefixButton_Click(object sender, EventArgs e)
        {
            Standard_PropertyStyle_KeyOnlyButton.Checked = Standard_PropertyStyle_SuffixButton.Checked = false;
            Standard_PropertyStyle_ApplyButton.Image = Resources.KeyPrefix;
            Standard_PropertyStyle_ApplyButton_ButtonClick(sender, e);
        }
        private void Standard_PropertyStyle_SuffixButton_Click(object sender, EventArgs e)
        {
            Standard_PropertyStyle_KeyOnlyButton.Checked = Standard_PropertyStyle_PrefixButton.Checked = false;
            Standard_PropertyStyle_ApplyButton.Image = Resources.KeySuffix;
            Standard_PropertyStyle_ApplyButton_ButtonClick(sender, e);
        }

        #endregion

        #region ModelVisualizer

        private void ModelVisualizer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                if (filenames.Length == 1 && (filenames[0].EndsWith(".model") | filenames[0].EndsWith(".asi")))
                    e.Effect = DragDropEffects.Copy;
            }
        }
        private void ModelVisualizer_DragDrop(object sender, DragEventArgs e)
        {
            if (ModelVisualizer.Actions.Count > 0)
            {
                switch (MessageBox.Show("Le modèle actuel a été modifié, voulez-vous l'enregistrer ?", "MeriseSuite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes: Standard_SaveButton_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }

            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            string filename = filenames[0];

            OpenFile(filename);
        }
        private void ModelVisualizer_Click(object sender, EventArgs e)
        {
            Standard_AddNewLinkButton.Checked = false;
        }

        #endregion

        public void OpenFile(string path)
        {
            try
            {
                KeyValuePair<Model, Dictionary<object, Style>> model;

                switch (Path.GetExtension(path))
                {
                    case ".asi":
                        model = IO.ImportAsi(path);
                        break;

                    case ".model":
                    default:
                        model = IO.Load(path);
                        break;
                }

                ModelVisualizer.Model = model.Key;
                ModelVisualizer.Styles = model.Value;

                foreach (Style style in ModelVisualizer.Styles.Values)
                    style.Visualizer = ModelVisualizer;

                ModelVisualizer.Unselect();
                ModelVisualizer.Redraw();

                CurrentFileName = path;
                Text = "MeriseSuite - " + Path.GetFileName(CurrentFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de l'ouverture du modèle, il est peut-être endommagé ou incompatible avec MeriseSuite.", "MeriseSuite", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string ApplyNamingStyle(string text, NamingStyle style)
        {
            text = text.Trim();
            if (text == "")
                return "";

            // Détection des préfixes
            string prefix = "";
            if (text.StartsWith("@"))
            {
                prefix = "@";
                text = text.Substring(1);
            }

            // Analyse du nom
            int lowers = text.Count(c => char.IsLower(c));
            int uppers = text.Count(c => char.IsUpper(c));

            // Découpage des mots
            if (lowers > 0 && uppers > 0)
                text = string.Join("", text.Select(c => char.IsUpper(c) ? "_" + c : c.ToString()).ToArray());
            IEnumerable<string> words = text.Split(new char[] { '_', '.' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Application des conventions
            string result = "";
            switch (style)
            {
                case NamingStyle.PascalCase:
                    result = string.Join("", words.Select(w => w.Length > 1 ? w.Remove(1).ToUpper() + w.Substring(1).ToLower() : w.ToUpper()).ToArray());
                    return prefix + result;
                case NamingStyle.CamelCase:
                    result = string.Join("", words.Select(w => w.Length > 1 ? w.Remove(1).ToUpper() + w.Substring(1).ToLower() : w.ToUpper()).ToArray());
                    return prefix + result.Remove(1).ToLower() + result.Substring(1);
                case NamingStyle.LowerCase:
                    return prefix + string.Join("_", words.Select(w => w.ToLower()).ToArray());
                case NamingStyle.UpperCase:
                    return prefix + string.Join("_", words.Select(w => w.ToUpper()).ToArray());
            }

            return null;
        }
        public string ApplyPropertyStyle(string text, string word, PropertyStyle propertyStyle, NamingStyle namingStyle)
        {
            text = text.Trim();
            word = word.ToLower();
            if (text == "")
                return "";
            if (word == "")
                return text;

            // Analyse du nom
            int lowers = text.Count(c => char.IsLower(c));
            int uppers = text.Count(c => char.IsUpper(c));

            // Découpage des mots
            if (lowers > 0 && uppers > 0)
                text = string.Join("", text.Select(c => char.IsUpper(c) ? "_" + c : c.ToString()).ToArray());
            List<string> words = text.Split(new char[] { '_', '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Détection des préfixes et suffixes
            if (RemoveAccents(words.First().ToLower()) == RemoveAccents(word.ToLower()))
                words.RemoveAt(0);
            if (RemoveAccents(words.Last().ToLower()) == RemoveAccents(word.ToLower()))
                words.RemoveAt(words.Count - 1);

            // Application du préfixe ou du suffixe
            switch (propertyStyle)
            {
                case PropertyStyle.KeyPrefix: words.Insert(0, word); break;
                case PropertyStyle.KeySuffix: words.Add(word); break;
            }

            // Application des conventions
            switch (namingStyle)
            {
                case NamingStyle.PascalCase:
                    return string.Join("", words.Select(w => w.Length > 1 ? w.Remove(1).ToUpper() + w.Substring(1).ToLower() : w.ToUpper()).ToArray());
                case NamingStyle.CamelCase:
                    string result = string.Join("", words.Select(w => w.Length > 1 ? w.Remove(1).ToUpper() + w.Substring(1).ToLower() : w.ToUpper()).ToArray());
                    return result.Remove(1).ToLower() + result.Substring(1);
                case NamingStyle.LowerCase:
                    return string.Join("_", words.Select(w => w.ToLower()).ToArray());
                case NamingStyle.UpperCase:
                    return string.Join("_", words.Select(w => w.ToUpper()).ToArray());
            }

            return null;
        }
        public string RemoveAccents(string text)
        {
            string result;
            StringBuilder builder = new StringBuilder();
            char[] car = text.ToCharArray();
            foreach (char c in car)
            {
                if (c == 'ä' || c == 'á' || c == 'à' || c == 'â' || c == 'ã' || c == 'å')
                    builder.Append('a');
                else if (c == 'é' || c == 'è' || c == 'ê' || c == 'ë')
                    builder.Append('e');
                else if (c == 'ì' || c == 'í' || c == 'ï' || c == 'î')
                    builder.Append('i');
                else if (c == 'ò' || c == 'ó' || c == 'ô' || c == 'ö')
                    builder.Append('o');
                else if (c == 'ù' || c == 'ú' || c == 'ü' || c == 'û')
                    builder.Append('u');
                else if (c == 'ÿ' || c == 'ý')
                    builder.Append('y');
                else if (c == 'ç')
                    builder.Append('c');
                else if (c == 'ñ')
                    builder.Append('n');
                else
                    builder.Append(c);
            }
            result = builder.ToString();
            return result;
        }
    }
}