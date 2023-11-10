using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    class Circle : Shape
    {
        public int Radius { get; set; }

        public Circle(int x, int y, int radius, Color color) : base(x, y, color)
        {
            Radius = radius;
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            using (var brush = new SolidBrush(Color))
            {
                Pen p = new Pen(Color.Black, 2);
                SolidBrush b = new SolidBrush(Color);
                //g.FillEllipse(brush, X - Radius, Y - Radius, 2 * Radius, 2 * Radius);
                g.DrawEllipse(p, X, Y, Radius * 2, Radius * 2);
                base.Draw(g);
            }
        }
    }
}
