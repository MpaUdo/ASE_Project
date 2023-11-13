using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalProgrammingLanguage
{
    public partial class Form1 : Form
    {
        private CommandParser commandParser;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
        public Form1()
        {
            InitializeComponent();
            commandParser = new CommandParser(pictureBoxDraw.CreateGraphics());
            TextBoxSCMDL.KeyDown += TextBoxSCMDL_KeyDown;
            InitializeSaveFileDialog();
            InitializeOpenFileDialog();
        }

        private void pictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnClear_Click(object sender, EventArgs e)
        {

            commandParser.ClearPicBox();
            //pictureBoxDraw.Refresh();

        }
        private void BtnRun_Click(object sender, EventArgs e)
        {
            try
            {
                commandParser.ClearPicBox();
                pictureBoxDraw.Refresh();

                // Get the text from the TextBox
                string textToDisplay = TextBoxCmdLine.Text.ToLower().Trim();
                string[] commands = TextBoxCmdLine.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                commandParser.ExecuteCommands(commands);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument-related exceptions
                MessageBox.Show($"Argument Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBoxSCMDL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevents the "ding" sound
                ProcessCommand(TextBoxSCMDL.Text);
            }
        }

        private void ProcessCommand(string command)
        {
            try
            {
                string[] commands = command.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                commandParser.ExecuteCommands(commands);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            TextBoxSCMDL.Clear();
            TextBoxSCMDL.Focus();
        }

        private void InitializeSaveFileDialog()
        {
            saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Text File",
                DefaultExt = "txt",
                AddExtension = true
            };
        }

        private void InitializeOpenFileDialog()
        {
            openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Open Text File",
                DefaultExt = "txt",
                AddExtension = true
            };
        }
        private void ToolStripMenuItemopen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void ToolStripMenuItemsave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, TextBoxCmdLine.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenFile()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    TextBoxCmdLine.Text = File.ReadAllText(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}