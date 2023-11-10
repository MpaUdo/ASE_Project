using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    class Circle : Shape
    {
        private int radius;

        public Circle(int x, int y, int r)
        {
            this.x = x;
            this.y = y;
            this.radius = r;
        }
        public override void Draw(Graphics g)
        {
            g.DrawEllipse(Pens.Black, x - radius, y - radius, 2 * radius, 2 * radius);
            //base.Draw(g);
        }
    }
}
