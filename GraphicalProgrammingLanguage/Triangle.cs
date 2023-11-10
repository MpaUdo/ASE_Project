using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Triangle : Shape
    {

        //private Point[] points;

        //public Triangle(Point[] pts)
        //{
        //    points = pts;
        //}

        //public override void Draw(Graphics g)
        //{
        //    g.DrawPolygon(Pens.Orange, points);
        //}
        public int SideLength { get; set; }

        public Triangle(int x, int y, int sideLength, Color color)
            : base(x, y, color)
        {
            SideLength = sideLength;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            using (var brush = new SolidBrush(Color))
            {
                Point[] points = new Point[3];
                points[0] = new Point(X, Y - SideLength / 2);
                points[1] = new Point(X + SideLength / 2, Y + SideLength / 2);
                points[2] = new Point(X - SideLength / 2, Y + SideLength / 2);
                //g.FillPolygon(brush, points);
                Pen p = new Pen(Color.Black, 2);
                SolidBrush b = new SolidBrush(Color);
                g.DrawPolygon(Pens.Black, points);
                base.Draw(g);
            }
        }
    }
}
