using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Ejiamike's: <br></br>
    /// Parses and executes commands to draw shapes.
    /// </summary>
    class CommandParser
    {
        private Graphics drawingGraphics;
        private Point penPosition;
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParser"/> class.
        /// </summary>
        /// <param name="graphics">The graphics surface on which to draw shapes.</param>
        /// <summary>Point to serve for the cordinates</summary>

        public CommandParser(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException(nameof(graphics), "Graphics object cannot be null.");
            }
            drawingGraphics = graphics;
            penPosition = new Point(0, 0);
        }
        /// <summary>
        /// Commands to draw shapes.
        /// </summary>
        /// <param name="commands">An array of commands to execute.</param>
        public void ExecuteCommands(string[] commands)
        {
            foreach (string command in commands)
            {
                try
                {
                    ExecuteCommand(command.Trim());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"ArgumentException: {ex.Message}");
                    // Log or handle the specific exception
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing command '{command}': {ex.Message}");
                    // Log or handle the general exception
                }
            }
        }
        /// <summary>
        /// Executes a single drawing command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
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
                else
                {
                    throw new ArgumentException("Invalid parameters for drawto command.");
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
                else
                {
                    throw new ArgumentException("Invalid parameters for moveto command.");
                }
            }
            else if (command.StartsWith("tri"))
            {
                // Example: triangle(50,50)
                string[] parameters = ExtractParameters(command);
                var brush = new SolidBrush(color: Color.Red);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
                {
                    Point[] points =
                    {
                    penPosition,
                    new Point(penPosition.X + width, penPosition.Y),
                    new Point(penPosition.X + width / 2, penPosition.Y + height)
                    };
                    drawingGraphics.DrawPolygon(Pens.Black, points);
                    drawingGraphics.FillPolygon(brush, points);
                    SolidBrush b = new SolidBrush(Color.Red);
                    penPosition = new Point(penPosition.X + width, penPosition.Y + width);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for tri command.");
                }
            }
            else if (command.StartsWith("rec"))
            {
                // Example: rectangle(50, 30)
                string[] parameters = ExtractParameters(command);
                var brush = new SolidBrush(color: Color.Blue);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
                {
                    drawingGraphics.DrawRectangle(Pens.Black, penPosition.X, penPosition.Y, width, height);
                    drawingGraphics.FillRectangle(brush, penPosition.X, penPosition.Y, width, height);
                    SolidBrush b = new SolidBrush(Color.Blue);
                    penPosition = new Point(width, height);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for rec command.");
                }
            }
            else if (command.StartsWith("cir"))
            {
                // Example: circle(30)
                string[] parameters = ExtractParameters(command);
                var brush = new SolidBrush(color: Color.Green);
                if (parameters.Length == 1 && int.TryParse(parameters[0], out int radius))
                {
                    drawingGraphics.DrawEllipse(Pens.Black, penPosition.X, penPosition.Y, radius * 2, radius * 2);
                    drawingGraphics.FillEllipse(brush, penPosition.X, penPosition.Y, radius * 2, radius * 2);
                    penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for cir command.");
                }
            }
            else
            {
            throw new ArgumentException($"Unknown command: {command}");
            }

            /// <summary>
            /// Example
            /// </summary>
            /// <param name="command">The command containing parameters.</param>
            /// <returns>An array of parameter values.</returns>
           
        }
        /// <summary>
        /// Extracts parameters from a command string.
        /// </summary>
        /// <param name="command">The command containing parameters.</param>
        /// <returns>An array of parameter values.</returns>
        public void ClearPicBox()
        {
            drawingGraphics.Clear(Color.Gray);
            penPosition = new Point(0, 0);
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
