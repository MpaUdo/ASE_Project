using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class CommandParser
    {
        private Graphics g;

        public CommandParser(Graphics graphics)
        {
            g = graphics;
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
            if (command.StartsWith("drawTo"))
            {
                // Example: drawTo(100,100)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int x) && int.TryParse(parameters[1], out int y))
                {
                    g.DrawLine(Pens.Black, g.ClipBounds.X, g.ClipBounds.Y, x, y);
                }
            }
            else if (command.StartsWith("moveTo"))
            {
                // Example: moveTo(50,50)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int x) && int.TryParse(parameters[1], out int y))
                {
                    g.TranslateTransform(x, y);
                }
            }
            else if (command.StartsWith("triangle"))
            {
                // Example: triangle(50,50)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
                {
                    Point[] points =
                    {
                    new Point(0, 0),
                    new Point(width, 0),
                    new Point(width / 2, height)
                };
                    g.DrawPolygon(Pens.Black, points);
                }
            }
            // Add more commands as needed
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
