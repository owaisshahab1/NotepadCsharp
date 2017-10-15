using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad_WinForm
{
    public partial class NotepadForm : Form
    {
        private bool isFileAlreadySaved;
        private bool isFileDirty;
        private string currOpenFileName;
        private FontDialog fontDialog = new FontDialog();

        public NotepadForm()
        {
            InitializeComponent();
        }





        #region events

        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutNotepadXPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("All right reserved with the Autor", "About Notepad XP",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        #endregion


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFileMenu();
        }

        private void NewFileMenu()
        {
            if (isFileDirty)
            {
                DialogResult result = MessageBox.Show("do you want to save your changes ?", "File Save",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                switch (result)
                {
                    case DialogResult.None:
                        break;
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Abort:
                        break;
                    case DialogResult.Retry:
                        break;
                    case DialogResult.Ignore:
                        break;
                    case DialogResult.Yes:
                        SaveAsFileMenu();
                        ClearScreen();
                        break;
                    case DialogResult.No:
                        ClearScreen();
                        break;
                    default:
                        break;
                }

            }
            ClearScreen();
            isFileAlreadySaved = false;
            currOpenFileName = "";
            EnableDisableUndoRedoProcess(false);

            MessageToolStripStatusLabel.Text = "New Document is created";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileMenu();

        }

        private void OpenFileMenu()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                {
                    MainRichTextBox.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                }
                if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                {
                    MainRichTextBox.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
                }

                this.Text = Path.GetFileName(openFileDialog.FileName) + " - Notepad XP";

                isFileAlreadySaved = true;
                isFileDirty = false;
                currOpenFileName = openFileDialog.FileName;
                EnableDisableUndoRedoProcess(false);

                MessageToolStripStatusLabel.Text = "File is Opened";
            }

        }

        private void EnableDisableUndoRedoProcess(bool Enabled)
        {
            undoToolStripMenuItem.Enabled = Enabled;
            redoToolStripMenuItem.Enabled = Enabled;
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileMenu();
        }

        private void SaveFileMenu()
        {
            if (isFileAlreadySaved)
            {
                if (Path.GetExtension(currOpenFileName) == ".rtf")
                {
                    MainRichTextBox.SaveFile(currOpenFileName, RichTextBoxStreamType.RichText);
                }
                if (Path.GetExtension(currOpenFileName) == ".txt")
                {
                    MainRichTextBox.SaveFile(currOpenFileName, RichTextBoxStreamType.PlainText);
                }
                isFileDirty = false;
            }
            else
            {
                if (isFileDirty)
                {
                    SaveAsFileMenu();
                }
                else
                {
                    ClearScreen();
                }
            }
        }

        private void ClearScreen()
        {
            MainRichTextBox.Clear();
            this.Text = "Untitled - Notepad XP";
            isFileDirty = false;
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsFileMenu();
        }




        #region Methods

        private void SaveAsFileMenu()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (Path.GetExtension(saveFileDialog.FileName) == ".txt")
                {
                    MainRichTextBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                }
                if (Path.GetExtension(saveFileDialog.FileName) == ".rft")
                {
                    MainRichTextBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
                }
                this.Text = Path.GetFileName(saveFileDialog.FileName) + " - Notepad XP";

                isFileAlreadySaved = true;
                isFileDirty = false;
                currOpenFileName = saveFileDialog.FileName;

            }
        }

        #endregion

        private void NotepadForm_Load(object sender, EventArgs e)
        {
            isFileAlreadySaved = false;
            isFileDirty = false;
            currOpenFileName = "";

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                capsToolStripStatusLabel.Text = "Caps ON";
            }
            else
            {
                capsToolStripStatusLabel.Text = "Caps OFF";
            }
        }

        private void MainRichTextBox_TextChanged(object sender, EventArgs e)
        {
            isFileDirty = true;
            undoToolStripMenuItem.Enabled = true;
            UndoToolStripButton.Enabled = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoEditMenu();
        }

        private void UndoEditMenu()
        {
            MainRichTextBox.Undo();
            UndoToolStripButton.Enabled = false;
            RedoToolStripButton.Enabled = true;
            redoToolStripMenuItem.Enabled = true;
            undoToolStripMenuItem.Enabled = false;
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoEditMenu();
        }

        private void RedoEditMenu()
        {
            MainRichTextBox.Redo();
            UndoToolStripButton.Enabled = true;
            RedoToolStripButton.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = true;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainRichTextBox.SelectAll();
        }

        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainRichTextBox.SelectedText = DateTime.Now.ToString();
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Bold);
        }

        private void FormatText(FontStyle fontstyle)
        {
            MainRichTextBox.SelectionFont = new Font(MainRichTextBox.Font, fontstyle);
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Italic);
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Underline);
        }

        private void strikethroughToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Strikeout);
        }

        private void regularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Regular);
        }

        private void formatFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormatFontMenu();
        }

        private void FormatFontMenu()
        {
            fontDialog.ShowColor = true;
            fontDialog.ShowApply = true;

            fontDialog.Apply += new System.EventHandler(fontDialog_Apply);

            DialogResult result = fontDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (MainRichTextBox.SelectionLength > 0)
                {
                    MainRichTextBox.SelectionFont = fontDialog.Font;
                    MainRichTextBox.SelectionColor = fontDialog.Color;
                }
            }
        }

        private void fontDialog_Apply(object sender, EventArgs e)
        {

            if (MainRichTextBox.SelectionLength > 0)
            {
                MainRichTextBox.SelectionFont = fontDialog.Font;
                MainRichTextBox.SelectionColor = fontDialog.Color;
            }
        }

        private void changeTheTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (MainRichTextBox.SelectionLength > 0)
                {
                    MainRichTextBox.SelectionColor = colorDialog.Color;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            NewFileMenu();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileMenu();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileMenu();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SaveAsFileMenu();
        }

        private void UndoToolStripButton_Click(object sender, EventArgs e)
        {
            UndoEditMenu();
        }

        private void RedoToolStripButton_Click(object sender, EventArgs e)
        {
            RedoEditMenu();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Bold);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Italic);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Underline);
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Strikeout);
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            FormatFontMenu();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MainRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                capsToolStripStatusLabel.Text = "Caps ON";
            }
            else
            {
                capsToolStripStatusLabel.Text = "Caps OFF";
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Regular);
        }

        private void boldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Bold);
        }

        private void italicToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Italic);
        }

        private void underlineToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FormatText(FontStyle.Underline);
        }

        private void underlineToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UndoEditMenu();
        }

        private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RedoEditMenu();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MainRichTextBox.Cut();
            Cut();
        }

        private void Cut()
        {
            if (MainRichTextBox.SelectionLength > 0)
            {
                Clipboard.SetText(MainRichTextBox.SelectedText);
                MainRichTextBox.SelectedText = "";
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void Copy()
        {
            if (MainRichTextBox.SelectionLength > 0)
            {
                Clipboard.SetText(MainRichTextBox.SelectedText);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void Paste()
        {
            if (Clipboard.ContainsText())
            {
                MainRichTextBox.SelectedText = Clipboard.GetText();
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Paste();
        }
    }
}
