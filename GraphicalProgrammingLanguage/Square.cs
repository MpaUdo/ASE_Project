using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    class Square : Rectangle
    {
        protected int size;
        public Square(int x, int y, int size) : base(x, y, size, size)
        {
            this.size = size;
        }
    }
}
