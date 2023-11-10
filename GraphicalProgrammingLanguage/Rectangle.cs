using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Rectangle : Shape
    {
        public int width, height;

        public Rectangle(int x, int y, int w, int h, Color color) : base(x, y, color)
        {
            
            width = w;
            height = h;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            using (var brush = new SolidBrush(Color))
            {
                Pen p = new Pen(Color.Black, 2);
                //g.FillRectangle(brush, X, Y, width, height);
                g.DrawRectangle(p, X, Y, width, height);
                base.Draw(g);
            }
        }
    }
}
