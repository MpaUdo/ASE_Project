﻿using System.Drawing;
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
        //private void SetDrawingColor(Color color)
        //{
        //    currentDrawingColor = color;
        //}
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
        public void RotateRectangle(int degrees)
        {
            semaphore.Wait();
            drawingGraphics.TranslateTransform(penPosition.X, penPosition.Y);
            drawingGraphics.RotateTransform(degrees);
            drawingGraphics.TranslateTransform(-penPosition.X, -penPosition.Y);
            semaphore.Release();
        }
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
       
        //private string ExtractFunctionName(string command)
        //{
        //    int startIndex = command.IndexOf("function") + "function".Length;
        //    int endIndex = command.IndexOf("(");
        //    if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
        //    {
        //        return command.Substring(startIndex, endIndex - startIndex).Trim();
        //    }
        //    throw new ArgumentException("Invalid function definition format");
        //}

        //private string ExtractFunctionBody(string command)
        //{
        //    int startIndex = command.IndexOf("{") + 1;
        //    int endIndex = command.LastIndexOf("}");
        //    if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
        //    {
        //        return command.Substring(startIndex, endIndex - startIndex).Trim();
        //    }
        //    throw new ArgumentException("Invalid function definition format");
        //}

        //private List<string> ParseFunctionBody(string functionBody)
        //{
        //    // Split the function body into individual commands
        //    return functionBody.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //}
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

                // Add more predefined shapes as needed

                default:
                    throw new ArgumentException($"Unknown shape: {shapeName}");
            }
        }
    }
}
