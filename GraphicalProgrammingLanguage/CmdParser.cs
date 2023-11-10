//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraphicalProgrammingLanguage
//{
//    internal class CmdParser
//    {
//        private Point currentLocation = new Point(0, 0);
//        private void InterpretMiniLanguage(string command)
//        {
//            // Parse the command and execute drawing actions on the PictureBox
//            string[] tokens = command.Split(' ');
//            if (tokens.Length >= 2)
//            {
//                string action = tokens[0].ToLower();
//                int x, y;

//                if (action == "moveto" && tokens.Length == 3)
//                {
//                    if (int.TryParse(tokens[1], out x) && int.TryParse(tokens[2], out y))
//                    {
//                        // Update the cursor position
//                        currentLocation = new Point(x, y);
//                    }
//                }
//                else if (action == "drawrectangle" && tokens.Length == 5)
//                {
//                    if (int.TryParse(tokens[1], out x) && int.TryParse(tokens[2], out y))
//                    {
//                        int width = int.Parse(tokens[3]);
//                        int height = int.Parse(tokens[4]);

//                        // Get the Graphics object to draw on the PictureBox
//                        using (Graphics g = PictureBoxDraw.CreateGraphics())
//                        {
//                            // Move the cursor to the specified location
//                            currentLocation = new Point(x, y);

//                            // Draw a rectangle
//                            g.DrawRectangle(Pens.Black, x, y, width, height);
//                        }

//                        // Force the PictureBox to repaint to show the changes
//                        PictureBoxDraw.Invalidate();
//                    }
//                }
//            }
//        }
//    }
//}
