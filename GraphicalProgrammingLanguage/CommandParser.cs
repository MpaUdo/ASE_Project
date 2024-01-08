using System.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace GraphicalProgrammingLanguage
{
    internal class CommandParser
    {
        private Graphics drawingGraphics;
        private Point penPosition;
        private Dictionary<string, int> variables;
        //private PenAndPointer penAndPointer;

        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public CommandParser(Graphics graphics, int penSize = 1, Color? penColor = null)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException(nameof(graphics), "Graphics object cannot be null.");
            }
            drawingGraphics = graphics;
            penPosition = new Point(0, 0);
            variables = new Dictionary<string, int>();
            //penAndPointer = new PenAndPointer(graphics, penSize, penColor);
        }

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
                    semaphore.Wait();
                    drawingGraphics.DrawLine(Pens.Black, penPosition, new Point(x, y));
                    penPosition = new Point(x, y);
                    semaphore.Release();
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
               // var brush = new SolidBrush(color: Color.Red);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
                {
                    semaphore.Wait();
                    Point[] points =
                    {
                    penPosition,
                    new Point(penPosition.X + width, penPosition.Y),
                    new Point(penPosition.X + width / 2, penPosition.Y + height)
                    };
                    drawingGraphics.DrawPolygon(Pens.Black, points);
                   // drawingGraphics.FillPolygon(brush, points);
                    //SolidBrush b = new SolidBrush(Color.Red);
                    penPosition = new Point(penPosition.X + width, penPosition.Y + width);
                    semaphore.Release();
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
               // var brush = new SolidBrush(color: Color.Blue);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height))
                {
                    semaphore.Wait();
                    drawingGraphics.DrawRectangle(Pens.Black, penPosition.X, penPosition.Y, width, height);
                   // drawingGraphics.FillRectangle(brush, penPosition.X, penPosition.Y, width, height);
                    //SolidBrush b = new SolidBrush(Color.Blue);
                    penPosition = new Point(width, height);
                    semaphore.Release();
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
                //var brush = new SolidBrush(color: Color.Green);
                if (parameters.Length == 1 && int.TryParse(parameters[0], out int radius))
                {
                    semaphore.Wait();
                    drawingGraphics.DrawEllipse(Pens.Black, penPosition.X, penPosition.Y, radius * 2, radius * 2);
                    //drawingGraphics.FillEllipse(brush, penPosition.X, penPosition.Y, radius * 2, radius * 2);
                    penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
                    semaphore.Release();
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for cir command.");
                }
            }
            else if (command.StartsWith("if"))
            {
                // Example: if(condition) { /* commands */ }
                string condition = ExtractCondition(command);
                if (CheckCondition(condition))
                {
                    // If the condition is true, execute the commands inside the block
                    ExecuteConditionalBlock(command.Substring(command.IndexOf('{') + 1, command.LastIndexOf('}') - command.IndexOf('{') - 1).Trim());
                }
            }
            else if (command.StartsWith("for"))
            {
                // Example: for(initialization; condition; iteration) { /* commands */ }
                string initialization = ExtractForPart(command, "for", "(", ";");
                string condition = ExtractForPart(command, ";", ";", ")");
                string iteration = ExtractForPart(command, ";", "{", ")");

                // Parse and execute the for loop
                ExecuteForLoop(initialization, condition, iteration, command.Substring(command.IndexOf('{') + 1, command.LastIndexOf('}') - command.IndexOf('{') - 1).Trim());
            }
            else if (command.StartsWith("endloop"))
            {
                // Do nothing for "endloop"; it's just a marker for the end of the loop block
            }

            else
            {
                throw new ArgumentException($"Unknown command: {command}");
            }
        }




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
        private string ExtractCondition(string command)
        {
            int startIndex = command.IndexOf('(') + 1;
            int endIndex = command.IndexOf(')');
            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                return command.Substring(startIndex, endIndex - startIndex).Trim();
            }
            throw new ArgumentException("Invalid if statement format");
        }
        private bool CheckCondition(string condition)
        {
            // You can implement a more sophisticated condition checking logic here
            // For simplicity, let's assume that any non-empty condition is considered true
            return !string.IsNullOrWhiteSpace(condition);
        }
        private void ExecuteConditionalBlock(string block)
        {
            string[] commands = block.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            ExecuteCommands(commands);
        }
        private string ExtractForPart(string command, string startDelimiter, string endDelimiter, string stopDelimiter)
        {
            int startIndex = command.IndexOf(startDelimiter) + startDelimiter.Length;
            int endIndex = command.IndexOf(endDelimiter);
            int stopIndex = command.IndexOf(stopDelimiter);

            if (startIndex >= 0 && endIndex >= 0 && stopIndex >= 0)
            {
                return command.Substring(startIndex, endIndex - startIndex).Trim();
            }
            throw new ArgumentException($"Invalid 'for' loop statement format: {command}");
        }
        private void ExecuteForLoop(string initialization, string condition, string iteration, string block)
        {
            // Execute initialization
            ExecuteCommand(initialization);

            // Continue loop while the condition is true
            while (CheckCondition(condition))
            {
                // Execute the block of commands inside the loop
                ExecuteConditionalBlock(block);

                // Execute the iteration
                ExecuteCommand(iteration);
            }
        }
        private void SetVariable(string variableName, int value)
        {
            if (variables.ContainsKey(variableName))
            {
                variables[variableName] = value;
            }
            else
            {
                variables.Add(variableName, value);
            }
        }
        private int GetVariable(string variableName)
        {
            if (variables.ContainsKey(variableName))
            {
                return variables[variableName];
            }
            else
            {
                throw new ArgumentException($"Variable '{variableName}' not found");
            }
        }
    }
}
