using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Rectangle : Shape
    {
        private int x;
        private int y;
        private int width;
        private int height;

        public Rectangle(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            width = w;
            height = h;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Black, x, y, width, height);
        }
    }
}
