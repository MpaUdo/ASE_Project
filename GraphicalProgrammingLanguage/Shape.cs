using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
     class Shape
    {
        //protected int x, y;
        //private int width;
        //private int height;

        //public Shape(int x, int y)


        //this.colour = colour; //shape's colour
        //this.x = x; //its x pos
        //this.y = y; //its y pos
        //can't provide anything else as "shape" is too general
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        public Shape(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
        public virtual void Draw(Graphics g)
        {
            //throw new NotImplementedException("This method should be overridden in derived classes.");
        }
    }
}
