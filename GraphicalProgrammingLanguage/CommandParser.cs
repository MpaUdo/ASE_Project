using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class CommandParser
    {
        private Graphics drawingGraphics;
        private Point penPosition;
        //private PenAndPointer penAndPointer;

        public CommandParser(Graphics graphics)
        {
            drawingGraphics = graphics;
            penPosition = new Point(0, 0);
            //penAndPointer = new PenAndPointer(graphics, penSize, penColor);
        }

        public void ExecuteCommands(string[] commands)
        {
            foreach (string command in commands)
            {
                ExecuteCommand(command.Trim());
            }
        }

        private void ExecuteCommand(string command)
        {
            // Update the pen position at the start of each command
           // penPosition = new Point(0, 0);
            if (command.StartsWith("drawto"))
            {
                // Example: drawTo(100,100)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int x) && int.TryParse(parameters[1], out int y))
                {
                    drawingGraphics.DrawLine(Pens.Black, penPosition, new Point(x, y));
                    penPosition = new Point(x, y);
                }
            }
            else if (command.StartsWith("moveto"))
            {
                // Example: moveTo(50,50)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int x) && int.TryParse(parameters[1], out int y))
                {
                    penPosition = new Point(x, y);
                }
            }
            else if (command.StartsWith("tri"))
            {
                // Example: triangle(50,50)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
                {
                    Point[] points =
                    {
                    penPosition,
                    new Point(penPosition.X + width / 2, penPosition.Y + height),
                    new Point(penPosition.X + width, penPosition.Y),
                    
                };
                    drawingGraphics.DrawPolygon(Pens.Black, points);
                   // penPosition = new Point(penPosition.X + width, penPosition.Y);
                }
            }
            else if (command.StartsWith("rec"))
            {
                // Example: rectangle(50, 30)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
                {
                    drawingGraphics.DrawRectangle(Pens.Black, penPosition.X, penPosition.Y, width, height);
                    //penPosition = new Point(penPosition.X + width, penPosition.Y);
                }
            }
            else if (command.StartsWith("cir"))
            {
                // Example: circle(30)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 1 && int.TryParse(parameters[0], out int radius))
                {
                    drawingGraphics.DrawEllipse(Pens.Black, penPosition.X, penPosition.Y, radius * 2, radius * 2);
                   // penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
                }
            }
            else if (command.StartsWith("sqr"))
            {
                // Example: square(20)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 1 && int.TryParse(parameters[0], out int side))
                {
                    drawingGraphics.DrawRectangle(Pens.Black, penPosition.X, penPosition.Y, side, side);
                    penPosition = new Point(penPosition.X + side, penPosition.Y);
                }
            }
            
        }

        private string[] ExtractParameters(string command)
        {
            int startIndex = command.IndexOf('(') + 1;
            int endIndex = command.IndexOf(')');
            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                string parameterString = command.Substring(startIndex, endIndex - startIndex);
                return parameterString.Split(',');
            }
            return new string[0];
        }

    }
}
