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
        public Form1()
        {
            InitializeComponent();
            commandParser = new CommandParser(pictureBoxDraw.CreateGraphics(), penSize: 5, penColor: Color.Red);
            TextBoxSCMDL.KeyDown += TextBoxSCMDL_KeyDown;
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
                string[] commands = command.Split(new char[] {}, StringSplitOptions.RemoveEmptyEntries);
                commandParser.ExecuteCommands(commands);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            TextBoxSCMDL.Clear();
            TextBoxSCMDL.Focus();
        }
    
    }
}