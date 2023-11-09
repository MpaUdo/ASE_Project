using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Circle : Shape
    {
        private int centerX;
        private int centerY;
        private int radius;

        public Circle(int x, int y, int r)
        {
            centerX = x;
            centerY = y;
            radius = r;
        }

        public override void Draw(Graphics g)
        {
            g.DrawEllipse(Pens.Black, centerX - radius, centerY - radius, 2 * radius, 2 * radius);
        }
    }
}
