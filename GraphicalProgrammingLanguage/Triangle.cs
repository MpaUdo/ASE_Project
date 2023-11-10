using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Triangle : Shape
    {
        
            private Point[] points;

            public Triangle(Point[] pts)
            {
                points = pts;
            }

            public override void Draw(Graphics g)
            {
                g.DrawPolygon(Pens.Orange, points);
            }
        
    }
}
