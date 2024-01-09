using System.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Data;
using System.Linq.Expressions;
using System.Linq;

namespace GraphicalProgrammingLanguage
{
    internal class CommandParser
    {
        private Graphics drawingGraphics;
        private Point penPosition;
        private Dictionary<string, int> variables;
        //private Dictionary<string, string> functions = new Dictionary<string, string>();
        private Dictionary<string, List<string>> functions = new Dictionary<string, List<string>>();
        private bool inFunctionBlock = false;
        private Color currentColor = Color.Black;
        private Color currentDrawingColor = Color.Black;
        //private double currentRotationAngle;
        private float currentRotationAngle = 0;
        private Random random = new Random();
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
        private void ExecuteIfStatement(string condition, string block)
        {
            if (CheckCondition(condition))
            {
                ExecuteConditionalBlock(block);
            }
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
        //private int EvaluateExpression(string expression)
        //{
        //    try
        //    {
        //        // Use a basic approach for expression evaluation for demonstration purposes
                
        //        DataTable table = new DataTable();
        //        table.Columns.Add("expression", typeof(string), expression);
        //        DataRow row = table.NewRow();
        //        table.Rows.Add(row);
        //        return int.Parse((string)row["expression"]);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException($"Error evaluating expression '{expression}': {ex.Message}");
        //    }
        //}
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
                if (command.StartsWith("set"))
                {
                    // Example: set sizex = 10
                    SetVariableFromCommand(command);
                }
                else if (command.StartsWith("cir"))
                {
                    // Example: circle(size)
                    DrawCircle(command);
                }
                else if (command.StartsWith("rec"))
                {
                    // Example: rectangle(width, height)
                    DrawRectangle(command);
                }
                else if (command.StartsWith("tri"))
                {
                    // Example: triangle(base, height)
                    DrawTriangle(command);
                }
                else if (command.StartsWith("drawto"))
                {
                    // Example: drawto(x, y)
                    DrawTo(command);
                }
                else if (command.StartsWith("moveto"))
                {
                    // Example: moveto(x, y)
                    MoveTo(command);
                }
            else if (command.StartsWith("while"))
            {
                // Example: while(condition) { /* commands */ }
                string condition = ExtractCondition(command);
                string block = command.Substring(command.IndexOf('{') + 1, command.LastIndexOf('}') - command.IndexOf('{') - 1).Trim();

                // Execute the while loop
                while (CheckCondition(condition))
                {
                    ExecuteConditionalBlock(block);
                }
            }
            else if (command.StartsWith("do"))
            {
                // Example: do { /* commands */ } while(condition);
                string block = command.Substring(command.IndexOf('{') + 1, command.LastIndexOf('}') - command.IndexOf('{') - 1).Trim();
                string condition = ExtractCondition(command);

                // Execute the do-while loop at least once
                do
                {
                    ExecuteConditionalBlock(block);
                } while (CheckCondition(condition));
            }
            else if (command.StartsWith("if"))
                {
                    // Example: if(condition) { /* commands */ }
                    string condition = ExtractCondition(command);
                    ExecuteIfStatement(condition, command.Substring(command.IndexOf('{') + 1, command.LastIndexOf('}') - command.IndexOf('{') - 1).Trim());
                }
                else if (command.StartsWith("for"))
                {
                    // Example: for(initialization; condition; iteration) { /* commands */ }
                    string initialization = ExtractForPart(command, "for", "(", ";");
                    string condition = ExtractForPart(command, ";", ";", ")");
                    string iteration = ExtractForPart(command, ";", "{", ")");

                    ExecuteForLoop(initialization, condition, iteration, command.Substring(command.IndexOf('{') + 1, command.LastIndexOf('}') - command.IndexOf('{') - 1).Trim());
                }
            else if (command.StartsWith("rot"))
            {
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 1 && float.TryParse(parameters[0], out float angleDegrees))
                {
                    Rotate(angleDegrees);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for rotate command.");
                }
            }
            else if (command.StartsWith("col"))
            {
                // Example: color(255,0,0) for red
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 3 && int.TryParse(parameters[0], out int red) && int.TryParse(parameters[1], out int green) && int.TryParse(parameters[2], out int blue))
                {
                    SetDrawingColor(Color.FromArgb(red, green, blue));
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for color command.");
                }
            }
            else if (command.StartsWith("rds"))
            {
                DrawRandomShape();
            }
            else if (command.StartsWith("ani"))
            {
                Animate(command);
            }
            else if (command.StartsWith("function"))
            {
                inFunctionBlock = true;
                string functionName = ExtractFunctionName(command);
                string functionBody = ExtractFunctionBody(command);
                functions[functionName] = new List<string>();
            }
            else if (inFunctionBlock)
            {
                if (command.StartsWith("endfunction"))
                {
                    inFunctionBlock = false;
                }
                else
                {
                    functions.Last().Value.Add(command);
                }
            }
            else if (command.EndsWith("()"))
            {
                // Example: myFunction()
                string functionName = command.Substring(0, command.Length - 2);
                if (functions.ContainsKey(functionName))
                {
                    ExecuteCommands(functions[functionName].ToArray());
                }
                else
                {
                    throw new ArgumentException($"Undefined function: {functionName}");
                }
            }

            else
                    {
                    throw new ArgumentException($"Unknown command: {command}");
                    }
                }

            private void DrawTo(string command)
            {
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2)
                {
                    int x = GetParameterValue(parameters[0]);
                    int y = GetParameterValue(parameters[1]);
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

            private void MoveTo(string command)
            {
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2)
                {
                    int x = GetParameterValue(parameters[0]);
                    int y = GetParameterValue(parameters[1]);
                    penPosition = new Point(x, y);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for moveto command.");
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
        private int GetVariableValue(string variableName)
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

        private void SetVariableFromCommand(string command)
        {
            string[] parts = command.Split('=');
            if (parts.Length == 2)
            {
                string variableName = parts[0].Substring(3).Trim(); // Remove "set" and trim
                if (int.TryParse(parts[1], out int value))
                {
                    SetVariable(variableName, value);
                }
                else
                {
                    throw new ArgumentException($"Invalid value for variable '{variableName}'");
                }
            }
            else
            {
                throw new ArgumentException("Invalid 'set' command format");
            }
        }
        private void SetDrawingColor(Color color)
        {
            currentDrawingColor = color;
        }
        private void DrawTriangle(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length == 2)
            {
                int baseLength = GetParameterValue(parameters[0]);
                int height = GetParameterValue(parameters[1]);
                semaphore.Wait();
                Point[] points =
                {
            penPosition,
            new Point(penPosition.X + baseLength, penPosition.Y),
            new Point(penPosition.X, penPosition.Y + height)
        };
                drawingGraphics.DrawPolygon(Pens.Black, points);
                penPosition = new Point(penPosition.X + baseLength, penPosition.Y);
                semaphore.Release();
            }
            else
            {
                throw new ArgumentException("Invalid parameters for tri command.");
            }
        }
        private void DrawCircle(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length == 1)
            {
                int radius = GetParameterValue(parameters[0]);
                semaphore.Wait();
                drawingGraphics.DrawEllipse(Pens.Black, penPosition.X, penPosition.Y, radius * 2, radius * 2);
                penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
                semaphore.Release();
            }
            else
            {
                throw new ArgumentException("Invalid parameters for cir command.");
            }
        }

        private void DrawRectangle(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length == 2)
            {
                int width = GetParameterValue(parameters[0]);
                int height = GetParameterValue(parameters[1]);
                semaphore.Wait();
                drawingGraphics.DrawRectangle(Pens.Black, penPosition.X, penPosition.Y, width, height);
                penPosition = new Point(penPosition.X + width, penPosition.Y);
                semaphore.Release();
            }
            else
            {
                throw new ArgumentException("Invalid parameters for rec command.");
            }
        }
        private int GetParameterValue(string parameter)
        {
            if (int.TryParse(parameter, out int value))
            {
                return value;
            }
            else if (variables.ContainsKey(parameter))
            {
                return variables[parameter];
            }
            else
            {
                throw new ArgumentException($"Invalid parameter: {parameter}");
            }
        }
        private void Rotate(float angleDegrees)
        {
            currentRotationAngle += angleDegrees;
        }
        private void DrawRotatedCircle(int radius)
        {
            semaphore.Wait();
            drawingGraphics.TranslateTransform(penPosition.X, penPosition.Y);
            drawingGraphics.RotateTransform(currentRotationAngle);
            drawingGraphics.DrawEllipse(new Pen(currentDrawingColor), -radius, -radius, radius * 2, radius * 2);
            drawingGraphics.ResetTransform();
            penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
            semaphore.Release();
        }

        private void DrawRotatedRectangle(int width, int height)
        {
            semaphore.Wait();
            drawingGraphics.TranslateTransform(penPosition.X, penPosition.Y);
            drawingGraphics.RotateTransform(currentRotationAngle);
            drawingGraphics.DrawRectangle(new Pen(currentDrawingColor), -width / 2, -height / 2, width, height);
            drawingGraphics.ResetTransform();
            penPosition = new Point(penPosition.X + width, penPosition.Y);
            semaphore.Release();
        }

        private void DrawRotatedTriangle(int baseLength, int height)
        {
            semaphore.Wait();
            Point[] points =
            {
                penPosition,
                new Point(penPosition.X + baseLength, penPosition.Y),
                new Point(penPosition.X + baseLength / 2, penPosition.Y + height)
            };
            drawingGraphics.TranslateTransform(penPosition.X, penPosition.Y);
            drawingGraphics.RotateTransform(currentRotationAngle);
            drawingGraphics.DrawPolygon(new Pen(currentDrawingColor), points);
            drawingGraphics.ResetTransform();
            penPosition = new Point(penPosition.X + baseLength, penPosition.Y);
            semaphore.Release();
        }

        private void DrawRandomShape()
        {
            int randomSize = random.Next(10, 50);
            int randomX = random.Next(0, (int)(drawingGraphics.VisibleClipBounds.Width - randomSize));
            int randomY = random.Next(0, (int)(drawingGraphics.VisibleClipBounds.Height - randomSize));
            int randomRed = random.Next(0, 256);
            int randomGreen = random.Next(0, 256);
            int randomBlue = random.Next(0, 256);
            Color randomColor = Color.FromArgb(randomRed, randomGreen, randomBlue);

            string[] commands = { $"color({randomRed},{randomGreen},{randomBlue})", $"cir({randomSize})", $"drawto({randomX},{randomY})" };
            ExecuteCommands(commands);
        }

        //private void Animate(string command)
        //{
        //    string[] parameters = ExtractParameters(command);
        //    if (parameters.Length == 3 && parameters.All(int.TryParse))
        //    {
        //        int xStart = int.Parse(parameters[0]);
        //        int xEnd = int.Parse(parameters[1]);
        //        int durationInSeconds = int.Parse(parameters[2]);

        //        int framesPerSecond = 30;
        //        int totalFrames = durationInSeconds * framesPerSecond;

        //        for (int frame = 0; frame < totalFrames; frame++)
        //        {
        //            int currentX = (int)Math.Round((double)frame / totalFrames * (xEnd - xStart) + xStart);
        //            ExecuteCommand($"drawto({currentX},{penPosition.Y})");
        //            System.Threading.Thread.Sleep(1000 / framesPerSecond);
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid parameters for animate command.");
        //    }
        //}
        private void Animate(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length == 4 &&
                int.TryParse(parameters[0], out int targetX) &&
                int.TryParse(parameters[1], out int targetY) &&
                int.TryParse(parameters[2], out int duration) &&
                int.TryParse(parameters[3], out int steps))
            {
                int deltaX = (targetX - penPosition.X) / steps;
                int deltaY = (targetY - penPosition.Y) / steps;

                for (int i = 0; i < steps; i++)
                {
                    MoveTo($"{penPosition.X + deltaX},{penPosition.Y + deltaY}");
                    Thread.Sleep(duration / steps);
                }
            }
            else
            {
                throw new ArgumentException("Invalid parameters for animate command.");
            }
        }

        private int EvaluateExpression(string expression)
        {
            // Basic arithmetic expression evaluation (for simplicity)
            string[] tokens = expression.Split(' ');
            int result = 0;

            if (tokens.Length == 3)
            {
                int operand1 = GetOperandValue(tokens[0]);
                int operand2 = GetOperandValue(tokens[2]);

                switch (tokens[1])
                {
                    case "+":
                        result = operand1 + operand2;
                        break;
                    case "-":
                        result = operand1 - operand2;
                        break;
                    case "*":
                        result = operand1 * operand2;
                        break;
                    case "/":
                        if (operand2 != 0)
                            result = operand1 / operand2;
                        else
                            throw new ArgumentException("Division by zero.");
                        break;
                    default:
                        throw new ArgumentException("Invalid operator in expression.");
                }
            }
            else
            {
                // Single operand case
                result = GetOperandValue(expression);
            }

            return result;
        }

        private int GetOperandValue(string operand)
        {
            if (int.TryParse(operand, out int value))
            {
                return value;
            }
            else if (variables.ContainsKey(operand))
            {
                return variables[operand];
            }
            else
            {
                throw new ArgumentException($"Variable '{operand}' not found");
            }
        }
        private string ExtractFunctionName(string command)
        {
            int startIndex = command.IndexOf("function") + "function".Length;
            int endIndex = command.IndexOf("(");
            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                return command.Substring(startIndex, endIndex - startIndex).Trim();
            }
            throw new ArgumentException("Invalid function definition format");
        }

        private string ExtractFunctionBody(string command)
        {
            int startIndex = command.IndexOf("{") + 1;
            int endIndex = command.LastIndexOf("}");
            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                return command.Substring(startIndex, endIndex - startIndex).Trim();
            }
            throw new ArgumentException("Invalid function definition format");
        }

        private List<string> ParseFunctionBody(string functionBody)
        {
            // Split the function body into individual commands
            return functionBody.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
