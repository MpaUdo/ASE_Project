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
            commandParser = new CommandParser(pictureBoxDraw.CreateGraphics());
        }

        private void pictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            pictureBoxDraw.Enabled = true;
            Graphics g = pictureBoxDraw.CreateGraphics();

            g.Clear(color: pictureBoxDraw.BackColor);
            pictureBoxDraw.Refresh();

        }
        private void BtnRun_Click(object sender, EventArgs e)
        {
            // Get the text from the TextBox
            string textToDisplay = TextBoxCmdLine.Text.ToLower().Trim();

            // Display the text in the PictureBox
            //pictureBoxDraw.Image = DrawTextOnImage(textToDisplay);

            // Get the size and background color of the PictureBox
            int pictureBoxWidth = pictureBoxDraw.Width;
            int pictureBoxHeight = pictureBoxDraw.Height;
            Color pictureBoxBackColor = pictureBoxDraw.BackColor;

            // Display the text in the PictureBox
            //pictureBoxDraw.Image = DrawTextOnImage(textToDisplay, pictureBoxWidth, pictureBoxHeight, pictureBoxBackColor);

            string[] commands = TextBoxCmdLine.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            commandParser.ExecuteCommands(commands);
            

        }
        //private Image DrawTextOnImage(string text, int width, int height, Color backgroundColor)
        //{
        //    // Create a bitmap with the specified width, height, and background color
        //    Bitmap bitmap = new Bitmap(width, height);
        //    using (Graphics g = Graphics.FromImage(bitmap))
        //    {
        //        g.Clear(backgroundColor);

        //        // Draw the text in black
        //        using (Brush brush = new SolidBrush(Color.Black))
        //        {
        //            g.DrawString(text, Font, brush, new PointF(10, 10));
        //        }
        //    }

        //    return bitmap;
        //}
    }
}