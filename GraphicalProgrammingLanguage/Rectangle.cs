using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Rectangle : Shape
    {
        protected int width, height;

        public Rectangle(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Red, x, y, width, height);
        }
    }
}
