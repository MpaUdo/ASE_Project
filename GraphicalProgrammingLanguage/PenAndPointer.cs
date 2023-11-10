using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class PenAndPointer
    {
        private Graphics graphics;
        private Point penPosition;
        private int penSize;
        private Color penColor;

        public PenAndPointer(Graphics graphics, int penSize = 1, Color? penColor = null)
        {
            this.graphics = graphics;
            this.penPosition = new Point(0, 0);
            this.penSize = penSize;
            this.penColor = penColor ?? Color.Black;
        }

        public void DrawLineTo(int x, int y)
        {
            using (Pen pen = new Pen(penColor, penSize))
            {
                graphics.DrawLine(pen, penPosition, new Point(x, y));
                penPosition = new Point(x, y);
            }
        }

        //public void MoveTo(int x, int y)
        //{
        //    penPosition = new Point(x, y);
        //}

        //public void DrawCircle(int radius)
        //{
        //    using (Pen pen = new Pen(penColor, penSize))
        //    {
        //        graphics.DrawEllipse(pen, penPosition.X, penPosition.Y, radius * 2, radius * 2);
        //        penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
        //    }
        //}

        //public void DrawRectangle(int width, int height)
        //{
        //    using (Pen pen = new Pen(penColor, penSize))
        //    {
        //        graphics.DrawRectangle(pen, penPosition.X, penPosition.Y, width, height);
        //        penPosition = new Point(penPosition.X + width, penPosition.Y);
        //    }
        //}

        public void DrawPointer()
        {
            // Draw a small circle at the current pen position to represent the pointer
            using (SolidBrush brush = new SolidBrush(penColor))
            {
                graphics.FillEllipse(brush, penPosition.X - penSize, penPosition.Y - penSize, penSize * 2, penSize * 2);
            }
        }
    }

//    public class CommandParser
//    {
//        private PenAndPointer penAndPointer;

//        public CommandParser(Graphics graphics, int penSize = 1, Color? penColor = null)
//        {
//            penAndPointer = new PenAndPointer(graphics, penSize, penColor);
//        }

//        public void ExecuteCommands(string[] commands)
//        {
//            foreach (string command in commands)
//            {
//                ExecuteCommand(command.Trim());
//            }
//        }

//        private void ExecuteCommand(string command)
//        {
//            if (command.StartsWith("drawTo"))
//            {
//                // Example: drawTo(100,100)
//                string[] parameters = ExtractParameters(command);
//                if (parameters.Length == 2 && int.TryParse(parameters[0], out int x) && int.TryParse(parameters[1], out int y))
//                {
//                    penAndPointer.DrawLineTo(x, y);
//                }
//            }
//            else if (command.StartsWith("moveTo"))
//            {
//                // Example: moveTo(50,50)
//                string[] parameters = ExtractParameters(command);
//                if (parameters.Length == 2 && int.TryParse(parameters[0], out int x) && int.TryParse(parameters[1], out int y))
//                {
//                    penAndPointer.MoveTo(x, y);
//                }
//            }
//            else if (command.StartsWith("triangle"))
//            {
//                // Example: triangle(50,50)
//                // Implement triangle drawing logic here
//            }
//            else if (command.StartsWith("rectangle"))
//            {
//                // Example: rectangle(50, 30)
//                string[] parameters = ExtractParameters(command);
//                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
//                {
//                    penAndPointer.DrawRectangle(width, height);
//                }
//            }
//            else if (command.StartsWith("circle"))
//            {
//                // Example: circle(30)
//                string[] parameters = ExtractParameters(command);
//                if (parameters.Length == 1 && int.TryParse(parameters[0], out int radius))
//                {
//                    penAndPointer.DrawCircle(radius);
//                }
//            }
//            else if (command.StartsWith("square"))
//            {
//                // Example: square(20)
//                // Implement square drawing logic here
//            }
//            // Add more commands as needed
//        }

//        private string[] ExtractParameters(string command)
//        {
//            int startIndex = command.IndexOf('(') + 1;
//            int endIndex = command.IndexOf(')');
//            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
//            {
//                string parameterString = command.Substring(startIndex, endIndex - startIndex);
//                return parameterString.Split(',');
//            }
//            return new string[0];
        
//    }

//}
}
