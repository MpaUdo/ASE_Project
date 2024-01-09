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
    /// <summary>
    /// Represents a command parser for a graphical programming language.
    /// </summary>
    internal class CommandParser
    {
        private Graphics drawingGraphics;
        private Point penPosition;
        private Dictionary<string, int> variables;
        private Dictionary<string, string> functions = new Dictionary<string, string>();
        //private Dictionary<string, List<string>> functions = new Dictionary<string, List<string>>();
        private Dictionary<string, Action<string[]>> userDefinedFunctions;
        private bool inFunctionBlock = false;
        //private Color currentColor = Color.Black;
        private Color currentDrawingColor = Color.Black;
        //private double currentRotationAngle;
        //private float currentRotationAngle = 0;
        private Random random = new Random();
        //private PenAndPointer penAndPointer;
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParser"/> class.
        /// </summary>
        /// <param name="graphics">The graphics object to draw on.</param>
        /// <param name="penSize">The size of the pen.</param>
        /// <param name="penColor">The color of the pen.</param>
        public CommandParser(Graphics graphics, int penSize = 1, Color? penColor = null)
        {
            userDefinedFunctions = new Dictionary<string, Action<string[]>>();
            if (graphics == null)
            {
                throw new ArgumentNullException(nameof(graphics), "Graphics object cannot be null.");
            }
            drawingGraphics = graphics;
            penPosition = new Point(0, 0);
            variables = new Dictionary<string, int>();
            //penAndPointer = new PenAndPointer(graphics, penSize, penColor);
        }
        /// <summary>
        /// Checks if a given condition is true.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <returns><c>true</c> if the condition is true; otherwise, <c>false</c>.</returns>
        private bool CheckCondition(string condition)
        {
            // You can implement a more sophisticated condition checking logic here
            // For simplicity, let's assume that any non-empty condition is considered true
            return !string.IsNullOrWhiteSpace(condition);
        }
        /// <summary>
        /// Executes a block of commands if the specified condition is true.
        /// </summary>
        /// <param name="block">The block of commands to execute.</param>
        private void ExecuteConditionalBlock(string block)
        {
            string[] commands = block.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            ExecuteCommands(commands);
        }
        /// <summary>
        /// Executes an if statement with the provided condition and block.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="block">The block of commands to execute if the condition is true.</param>
        private void ExecuteIfStatement(string condition, string block)
        {
            if (CheckCondition(condition))
            {
                ExecuteConditionalBlock(block);
            }
        }
        /// <summary>
        /// Defines a user-defined function.
        /// </summary>
        /// <param name="functionName">The name of the function.</param>
        /// <param name="functionBody">The body of the function.</param>
        public void DefineFunction(string functionName, Action<string[]> functionBody)
        {
            if (userDefinedFunctions.ContainsKey(functionName))
            {
                userDefinedFunctions[functionName] = functionBody;
            }
            else
            {
                userDefinedFunctions.Add(functionName, functionBody);
            }
        }
        /// <summary>
        /// Calls a user-defined function with the specified arguments.
        /// </summary>
        /// <param name="functionName">The name of the function to call.</param>
        /// <param name="arguments">The arguments to pass to the function.</param>
        public void CallFunction(string functionName, string[] arguments)
        {
            if (userDefinedFunctions.ContainsKey(functionName))
            {
                userDefinedFunctions[functionName]?.Invoke(arguments);
            }
            else
            {
                throw new ArgumentException($"Function '{functionName}' not defined.");
            }
        }
        /// <summary>
        /// Executes a for loop with the specified initialization, condition, iteration, and block of commands.
        /// </summary>
        /// <param name="initialization">The initialization part of the for loop.</param>
        /// <param name="condition">The condition to check in each iteration.</param>
        /// <param name="iteration">The iteration part of the for loop.</param>
        /// <param name="block">The block of commands inside the for loop.</param>
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

        /// <summary>
        /// Executes a series of commands.
        /// </summary>
        /// <param name="commands">The array of commands to execute.</param>
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
        /// Executes an individual command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
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
            else if (command.StartsWith("draw"))
            {
                // Example: draw circle
                string shapeName = ExtractParameters(command).FirstOrDefault()?.ToLower();
                if (!string.IsNullOrWhiteSpace(shapeName))
                {
                    DrawPredefinedShape(shapeName);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for draw command.");
                }
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
                // Example: rotate(90)
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 1 && int.TryParse(parameters[0], out int degrees))
                {
                    RotateRectangle(degrees);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for rotate command.");
                }
            }
            //else if (command.StartsWith("colrec"))
            //{
            //    // Example: colrect(100, 50, Color.Red)
            //    string[] parameters = ExtractParameters(command);
            //    if (parameters.Length == 3 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height) && Color.FromName(parameters[2]) != null)
            //    {
            //        FillRectangle(width, height, Color.FromName(parameters[2]));
            //    }
            //    else
            //    {
            //        throw new ArgumentException("Invalid parameters for colrect command.");
            //    }
            //}
            //else if (command.StartsWith("coltri"))
            //{
            //    // Example: coltri(100, 50, Color.Blue)
            //    string[] parameters = ExtractParameters(command);
            //    if (parameters.Length == 3 && int.TryParse(parameters[0], out int width) && int.TryParse(parameters[1], out int height) && Color.FromName(parameters[2]) != null)
            //    {
            //        FillTriangle(width, height, Color.FromName(parameters[2]));
            //    }
            //    else
            //    {
            //        throw new ArgumentException("Invalid parameters for coltri command.");
            //    }
            //}
            //else if (command.StartsWith("colcir"))
            //{
            //    // Example: colcir(30, Color.Green)
            //    string[] parameters = ExtractParameters(command);
            //    if (parameters.Length == 2 && int.TryParse(parameters[0], out int radius) && Color.FromName(parameters[1]) != null)
            //    {
            //        FillCircle(radius, Color.FromName(parameters[1]));
            //    }
            //    else
            //    {
            //        throw new ArgumentException("Invalid parameters for fillcir command.");
            //    }
            //}
            else if (command.StartsWith("rds"))
            {
                DrawRandomShape();
            }
            else if (command.StartsWith("ani"))
            {
                // Example: ani(5, 10) - animate for 5 seconds at 10 frames per second
                string[] parameters = ExtractParameters(command);
                if (parameters.Length == 2 && int.TryParse(parameters[0], out int duration) && int.TryParse(parameters[1], out int framesPerSecond))
                {
                    AnimateShapes(duration, framesPerSecond);
                }
                else
                {
                    throw new ArgumentException("Invalid parameters for ani command.");
                }
            }
            else if (command.StartsWith("def"))
            {
                // Example: def myFunction(arg1, arg2) { /* commands */ }
                DefineFunction(command);
            }
            else if (command.StartsWith("call"))
            {
                // Example: call myFunction(50, Color.Red)
                CallFunction(command);
            }
            else
            {
                throw new ArgumentException($"Unknown command: {command}");
            }
        }
        /// <summary>
        /// Draws a line to the specified position.
        /// </summary>
        /// <param name="command">The drawto command.</param>
        private void DrawTo(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length >= 2)
            {
                int x = GetParameterValue(parameters[0]);
                int y = GetParameterValue(parameters[1]);
                Color color = GetColorParameter(parameters.Length > 2 ? parameters[2] : null);

                semaphore.Wait();
                drawingGraphics.DrawLine(new Pen(color), penPosition, new Point(x, y));
                penPosition = new Point(x, y);
                semaphore.Release();
            }
            else
            {
                throw new ArgumentException("Invalid parameters for drawto command.");
            }
        }
        /// <summary>
        /// Moves the pen to the specified position.
        /// </summary>
        /// <param name="command">The moveto command.</param>
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
        /// <summary>
        /// Clears the drawing area.
        /// </summary>
        public void ClearPicBox()
        {
            drawingGraphics.Clear(Color.Gray);
            penPosition = new Point(0, 0);
        }
        /// <summary>
        /// Extracts parameters from a command string.
        /// </summary>
        /// <param name="command">The command string.</param>
        /// <returns>An array of parameters.</returns>
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
        /// <summary>
        /// Extracts parameters from a condition command string.
        /// </summary>
        /// <param name="command">The condition command string.</param>
        /// <returns>An array of parameters representing the condition.</returns>
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
        /// <summary>
        /// Sets variables based on a command that assigns values to them.
        /// </summary>
        /// <param name="command">The command to set variables from.</param>
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
        /// <summary>
        /// Gets the list of variable names defined in the program.
        /// </summary>
        /// <returns>A list of variable names.</returns>
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
        /// <summary>
        /// Gets the values of all variables defined in the program.
        /// </summary>
        /// <returns>A dictionary containing variable names and their corresponding values.</returns>
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
        /// <summary>
        /// Sets a variable value based on the provided command.
        /// </summary>
        /// <param name="command">The command to set the variable value.</param>
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
        //private void SetDrawingColor(Color color)
        //{
        //    currentDrawingColor = color;
        //}
        /// <summary>
        /// Draws a triangle based on the provided command.
        /// </summary>
        /// <param name="command">The command containing triangle parameters.</param>
        private void DrawTriangle(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length >= 2)
            {
                int baseLength = GetParameterValue(parameters[0]);
                int height = GetParameterValue(parameters[1]);
                Color penColor = GetColorParameter(parameters.Length > 2 ? parameters[2] : null);
                Color fillColor = GetColorParameter(parameters.Length > 3 ? parameters[3] : null);

                semaphore.Wait();
                Point[] points =
                {
            penPosition,
            new Point(penPosition.X + baseLength, penPosition.Y),
            new Point(penPosition.X, penPosition.Y + height)
        };
                drawingGraphics.FillPolygon(new SolidBrush(fillColor), points);
                drawingGraphics.DrawPolygon(new Pen(penColor), points);
                penPosition = new Point(penPosition.X + baseLength, penPosition.Y);
                semaphore.Release();
            }
            else
            {
                throw new ArgumentException("Invalid parameters for tri command.");
            }
        }
        /// <summary>
        /// Draws a circle based on the provided command.
        /// </summary>
        /// <param name="command">The command containing circle parameters.</param>
        private void DrawCircle(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length >= 1)
            {
                int radius = GetParameterValue(parameters[0]);
                Color penColor = GetColorParameter(parameters.Length > 1 ? parameters[1] : null);
                Color fillColor = GetColorParameter(parameters.Length > 2 ? parameters[2] : null);

                semaphore.Wait();
                drawingGraphics.FillEllipse(new SolidBrush(fillColor), penPosition.X, penPosition.Y, radius * 2, radius * 2);
                drawingGraphics.DrawEllipse(new Pen(penColor), penPosition.X, penPosition.Y, radius * 2, radius * 2);
                penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
                semaphore.Release();
            }
            else
            {
                throw new ArgumentException("Invalid parameters for cir command.");
            }
        }
        /// <summary>
        /// Draws a rectangle based on the provided command.
        /// </summary>
        /// <param name="command">The command containing rectangle parameters.</param>
        private void DrawRectangle(string command)
        {
            string[] parameters = ExtractParameters(command);
            if (parameters.Length >= 2)
            {
                int width = GetParameterValue(parameters[0]);
                int height = GetParameterValue(parameters[1]);
                Color penColor = GetColorParameter(parameters.Length > 2 ? parameters[2] : null);
                Color fillColor = GetColorParameter(parameters.Length > 3 ? parameters[3] : null);

                semaphore.Wait();
                drawingGraphics.FillRectangle(new SolidBrush(fillColor), penPosition.X, penPosition.Y, width, height);
                drawingGraphics.DrawRectangle(new Pen(penColor), penPosition.X, penPosition.Y, width, height);
                penPosition = new Point(penPosition.X + width, penPosition.Y);
                semaphore.Release();
            }
            else
            {
                throw new ArgumentException("Invalid parameters for rec command.");
            }
        }
        /// <summary>
        /// Gets the numerical value of a parameter, either a variable or a literal value.
        /// </summary>
        /// <param name="parameter">The parameter to evaluate.</param>
        /// <returns>The numerical value of the parameter.</returns>
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
        public void FillRectangle(int width, int height, Color fillColor)
        {
            semaphore.Wait();
            var brush = new SolidBrush(fillColor);
            drawingGraphics.FillRectangle(brush, penPosition.X, penPosition.Y, width, height);
            penPosition = new Point(penPosition.X + width, penPosition.Y + height);
            semaphore.Release();
        }

        public void FillTriangle(int width, int height, Color fillColor)
        {
            semaphore.Wait();
            var brush = new SolidBrush(fillColor);
            Point[] points =
            {
        penPosition,
        new Point(penPosition.X + width, penPosition.Y),
        new Point(penPosition.X + width / 2, penPosition.Y + height)
    };
            drawingGraphics.FillPolygon(brush, points);
            penPosition = new Point(penPosition.X + width, penPosition.Y + width);
            semaphore.Release();
        }

        public void FillCircle(int radius, Color fillColor)
        {
            semaphore.Wait();
            var brush = new SolidBrush(fillColor);
            drawingGraphics.FillEllipse(brush, penPosition.X, penPosition.Y, radius * 2, radius * 2);
            penPosition = new Point(penPosition.X + radius * 2, penPosition.Y);
            semaphore.Release();
        }
        /// <summary>
        /// Rotates a rectangle drawn on the graphics object.
        /// </summary>
        /// <param name="angle">The rotation angle in degrees.</param>
        public void RotateRectangle(int degrees)
        {
            semaphore.Wait();
            drawingGraphics.TranslateTransform(penPosition.X, penPosition.Y);
            drawingGraphics.RotateTransform(degrees);
            drawingGraphics.TranslateTransform(-penPosition.X, -penPosition.Y);
            semaphore.Release();
        }
        /// <summary>
        /// Draws a randomly selected shape on the graphics object.
        /// </summary>
        public void DrawRandomShape()
        {
            Random random = new Random();
            int shapeType = random.Next(3); // 0: rectangle, 1: circle, 2: triangle

            int size1 = random.Next(20, 100); // Random size for dimension 1
            int size2 = random.Next(20, 100); // Random size for dimension 2

            Color fillColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)); // Random fill color

            semaphore.Wait();
            switch (shapeType)
            {
                case 0:
                    // Rectangle
                    drawingGraphics.DrawRectangle(Pens.Black, penPosition.X, penPosition.Y, size1, size2);
                    break;
                case 1:
                    // Circle
                    drawingGraphics.DrawEllipse(Pens.Black, penPosition.X, penPosition.Y, size1, size1);
                    break;
                case 2:
                    // Triangle
                    Point[] points =
                    {
                penPosition,
                new Point(penPosition.X + size1, penPosition.Y),
                new Point(penPosition.X + size1 / 2, penPosition.Y + size2)
            };
                    drawingGraphics.DrawPolygon(Pens.Black, points);
                    break;
            }
            semaphore.Release();
        }
        /// <summary>
        /// Animates shapes for a specified duration and frames per second.
        /// </summary>
        /// <param name="duration">The duration of the animation in seconds.</param>
        /// <param name="fps">The frames per second for the animation.</param>
        public void AnimateShapes(int duration, int fps)
        {
            int totalFrames = duration * fps;
            for (int frame = 0; frame < totalFrames; frame++)
            {
                // Clear the drawing area in each frame
                ClearPicBox();

                // Draw shapes at their animated positions
                DrawAnimatedShapes(frame, totalFrames);

                // Delay to control the animation speed
                Thread.Sleep(1000 / fps);
            }
        }
        /// <summary>
        /// Draws animated shapes on the graphics object for a specified duration and frames per second.
        /// </summary>
        /// <param name="duration">The duration of the animation in seconds.</param>
        /// <param name="fps">The frames per second for the animation.</param>
        private void DrawAnimatedShapes(int currentFrame, int totalFrames)
        {
            Random random = new Random();

            for (int i = 0; i < 5; i++) // Draw 5 random shapes in each frame for demonstration
            {
                // Random position and size for each shape
                int x = random.Next((int)(drawingGraphics.VisibleClipBounds.Width - 50));
                int y = random.Next((int)(drawingGraphics.VisibleClipBounds.Height - 50));
                int size1 = random.Next(20, 50);
                int size2 = random.Next(20, 50);

                // Random fill color for each shape
                Color fillColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

                // Draw the shapes at animated positions
                penPosition = new Point(x, y);
                DrawRandomShape(); // You can modify this to use specific shapes based on your needs
            }
        }
        /// <summary>
        /// Gets the numerical value of an operand, either a variable or a literal value.
        /// </summary>
        /// <param name="operand">The operand to evaluate.</param>
        /// <returns>The numerical value of the operand.</returns>
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
        /// <summary>
        /// Defines a user-defined function with the specified name and body.
        /// </summary>
        /// <param name="functionName">The name of the function.</param>
        /// <param name="functionBody">The body of the function.</param>
        private void DefineFunction(string command)
        {
            int startIndex = command.IndexOf("def") + "def".Length;
            int endIndex = command.IndexOf("(");

            if (startIndex >= 0 && endIndex >= 0)
            {
                string functionName = command.Substring(startIndex, endIndex - startIndex).Trim();
                int bodyStartIndex = command.IndexOf("{");
                int bodyEndIndex = command.LastIndexOf("}");

                if (bodyStartIndex >= 0 && bodyEndIndex >= 0)
                {
                    string body = command.Substring(bodyStartIndex + 1, bodyEndIndex - bodyStartIndex - 1).Trim();
                    Action<string[]> functionBody = (args) => ExecuteCommands(body.Split(';'));
                    DefineFunction(functionName, functionBody);
                }
                else
                {
                    throw new ArgumentException($"Invalid function definition syntax: {command}");
                }
            }
            else
            {
                throw new ArgumentException($"Invalid function definition syntax: {command}");
            }
        }
        /// <summary>
        /// Calls a user-defined function with the specified name and arguments.
        /// </summary>
        /// <param name="functionName">The name of the function to call.</param>
        /// <param name="arguments">The arguments to pass to the function.</param>
        private void CallFunction(string command)
        {
            int startIndex = command.IndexOf("call") + "call".Length;
            int endIndex = command.IndexOf("(");

            if (startIndex >= 0 && endIndex >= 0)
            {
                string functionName = command.Substring(startIndex, endIndex - startIndex).Trim();
                int argumentsStartIndex = command.IndexOf("(") + 1;
                int argumentsEndIndex = command.IndexOf(")");

                if (argumentsStartIndex >= 0 && argumentsEndIndex >= 0)
                {
                    string argumentsString = command.Substring(argumentsStartIndex, argumentsEndIndex - argumentsStartIndex).Trim();
                    string[] arguments = argumentsString.Split(',');

                    CallFunction(functionName, arguments);
                }
                else
                {
                    throw new ArgumentException($"Invalid function call syntax: {command}");
                }
            }
            else
            {
                throw new ArgumentException($"Invalid function call syntax: {command}");
            }
        }
        /// <summary>
        /// Gets the color value from a parameter, either a predefined color or a variable.
        /// </summary>
        /// <param name="parameter">The parameter to evaluate.</param>
        /// <returns>The color value.</returns>
        private Color GetColorParameter(string colorParameter)
        {
            if (colorParameter != null)
            {
                try
                {
                    return Color.FromName(colorParameter);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Invalid color name.");
                }
            }
            return Color.Black; // Default color if not specified
        }
        /// <summary>
        /// Draws a predefined shape based on the specified shape name.
        /// </summary>
        /// <param name="shapeName">The name of the predefined shape.</param>
        public void DrawPredefinedShape(string shapeName)
        {
            Random random = new Random();

            switch (shapeName.ToLower())
            {
                case "circle":
                    int circleRadius = 50;
                    Color circleColor = Color.Red;
                    semaphore.Wait();
                    drawingGraphics.DrawEllipse(Pens.Black, penPosition.X, penPosition.Y, circleRadius * 2, circleRadius * 2);
                    drawingGraphics.FillEllipse(new SolidBrush(circleColor), penPosition.X, penPosition.Y, circleRadius * 2, circleRadius * 2);
                    penPosition = new Point(penPosition.X + circleRadius * 2, penPosition.Y);
                    semaphore.Release();
                    break;

                case "rectangle":
                    int rectangleWidth = 50;
                    int rectangleHeight = 30;
                    Color rectangleColor = Color.Blue;
                    semaphore.Wait();
                    drawingGraphics.DrawRectangle(Pens.Black, penPosition.X, penPosition.Y, rectangleWidth, rectangleHeight);
                    drawingGraphics.FillRectangle(new SolidBrush(rectangleColor), penPosition.X, penPosition.Y, rectangleWidth, rectangleHeight);
                    penPosition = new Point(penPosition.X + rectangleWidth, penPosition.Y);
                    semaphore.Release();
                    break;

                default:
                    throw new ArgumentException($"Unknown shape: {shapeName}");
            }
        }
        /// <summary>
        /// END.
        /// </summary>
    }
}
