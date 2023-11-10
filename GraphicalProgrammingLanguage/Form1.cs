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
            // Create instances of shapes
            //Shape circle = new Circle(100, 100, 50);
            //Shape rectangle = new Rectangle(100, 100, 80, 60);
            //Point[] trianglePoints = { new Point(0, 60), new Point(60, 150), new Point(150, 0) };
            //Shape triangle = new Triangle(trianglePoints);
            //Shape square = new Square(100,150,60);

            //// Draw the shapes
            //circle.Draw(e.Graphics);
            //rectangle.Draw(e.Graphics);
            //triangle.Draw(e.Graphics);
            //square.Draw(e.Graphics);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            pictureBoxDraw.Enabled = true;
            Graphics g = pictureBoxDraw.CreateGraphics();

            g.Clear(color: pictureBoxDraw.BackColor);

        }
        private void BtnRun_Click(object sender, EventArgs e)
        {
            // Get the text from the TextBox
            string textToDisplay = TextBoxCmdLine.Text;

            // Display the text in the PictureBox
            //pictureBoxDraw.Image = DrawTextOnImage(textToDisplay);

            // Get the size and background color of the PictureBox
            int pictureBoxWidth = pictureBoxDraw.Width;
            int pictureBoxHeight = pictureBoxDraw.Height;
            Color pictureBoxBackColor = pictureBoxDraw.BackColor;

            // Display the text in the PictureBox
            pictureBoxDraw.Image = DrawTextOnImage(textToDisplay, pictureBoxWidth, pictureBoxHeight, pictureBoxBackColor);

            string[] commands = TextBoxCmdLine.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            commandParser.ExecuteCommands(commands);
        }

        // Function to create an image with the given text
        //private System.Drawing.Image DrawTextOnImage(string text)
        //{
        //    // Create a bitmap with a white background
        //    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(pictureBoxDraw.Size());
        //    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
        //    {
        //        g.Clear(System.Drawing.Color.White);

        //        // Draw the text in black
        //        using (System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
        //        {
        //            g.DrawString(text, this.Font, brush, new System.Drawing.PointF(10, 10));
        //        }
        //    }

        //    return bitmap;
        //}

        // Function to create an image with the given text, size, and background color
        private Image DrawTextOnImage(string text, int width, int height, Color backgroundColor)
        {
            // Create a bitmap with the specified width, height, and background color
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(backgroundColor);

                // Draw the text in black
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString(text, Font, brush, new PointF(10, 10));
                }
            }

            return bitmap;
        }
    }
}