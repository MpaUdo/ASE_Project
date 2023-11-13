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
            // Get the text from the TextBox
            string textToDisplay = TextBoxCmdLine.Text.ToLower().Trim();
            string[] commands = TextBoxCmdLine.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            commandParser.ExecuteCommands(commands);
            //commandParser.ClearPicBox();



        }
    }
}