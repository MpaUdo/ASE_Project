namespace GraphicalProgrammingLanguage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {
            // Create instances of shapes
            Shape circle = new Circle(100, 100, 50);
            Shape rectangle = new Rectangle(100, 100, 80, 60);
            Point[] trianglePoints = { new Point(50, 60), new Point(150, 150), new Point(20, 30) };
            Shape triangle = new Triangle(trianglePoints);

            // Draw the shapes
            circle.Draw(e.Graphics);
            rectangle.Draw(e.Graphics);
            triangle.Draw(e.Graphics);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            pictureBoxDraw.Enabled = true;
            using (Graphics g = pictureBoxDraw.CreateGraphics())
            {
                g.Clear(Color.White);
            }
        }
    }
}