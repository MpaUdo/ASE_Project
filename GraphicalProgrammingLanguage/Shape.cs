using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Shape
    {
        private int x;
        private int y;
        //private int width;
        //private int height;


        public virtual void Draw(Graphics graphics)
        {
            throw new NotImplementedException("This method should be overridden in derived classes.");
        }

    }
}
