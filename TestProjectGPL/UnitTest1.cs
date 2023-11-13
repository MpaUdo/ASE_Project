using GraphicalProgrammingLanguage;
using System.Drawing;
using System.Windows.Forms;

namespace TestProjectGPL
{
    [TestClass]
public class CommandParserTests
{
    public CommandParser commandParser;
    public PictureBox pictureBox;

        [TestInitialize]
    public void TestInitialize()
    {
        // Initialize the CommandParser with a dummy PictureBox for testing
        pictureBox = new PictureBox();
        //commandParser = new CommandParser(Graphics.FromImage(new Bitmap(1, 1)), pictureBox);
    }

    [TestMethod]
    public void TestExecuteDrawToCommand()
    {
        // Arrange
        string[] commands = { "drawTo(50,50);" };

        // Act
        commandParser.ExecuteCommands(commands);

        // Assert
       // Assert.AreEqual(new Point(50, 50), commandParser.PenPosition);
    }

    [TestMethod]
    public void TestExecuteMoveToCommand()
    {
        // Arrange
        string[] commands = { "moveTo(30,30);" };

        // Act
        commandParser.ExecuteCommands(commands);

        // Assert
       // Assert.AreEqual(new Point(30, 30), commandParser.PenPosition);
    }

    [TestMethod]
    public void TestExecuteTriangleCommand()
    {
        // Arrange
        string[] commands = { "triangle(50,50);" };

        // Act
        commandParser.ExecuteCommands(commands);

        // Assert
        Assert.IsTrue(VerifyTriangleHasBeenDrawn());
    }

    [TestMethod]
    public void TestExecuteRectangleCommand()
    {
        // Arrange
        string[] commands = { "rectangle(40, 30);" };

        // Act
        commandParser.ExecuteCommands(commands);

        // Assert
        Assert.IsTrue(VerifyRectangleHasBeenDrawn());
    }

    [TestMethod]
    public void TestExecuteCircleCommand()
    {
        // Arrange
        string[] commands = { "circle(20);" };

        // Act
        commandParser.ExecuteCommands(commands);

        // Assert
        Assert.IsTrue(VerifyCircleHasBeenDrawn());
    }

    [TestMethod]
    public void TestExecuteSquareCommand()
    {
        // Arrange
        string[] commands = { "square(25);" };

        // Act
        commandParser.ExecuteCommands(commands);

        // Assert
        Assert.IsTrue(VerifySquareHasBeenDrawn());
    }

    // Add more test methods for other commands as needed

    private bool VerifyTriangleHasBeenDrawn()
    {
        // Implement logic to verify that a triangle has been drawn in the PictureBox
        // Example: Check if the PictureBox contains a triangle based on its expected shape and position
        // You might need to access internal state or properties of the CommandParser or PictureBox
        // for verification. It's good practice to expose relevant methods or properties for testing.
        return false;
    }

    private bool VerifyRectangleHasBeenDrawn()
    {
        // Implement logic to verify that a rectangle has been drawn in the PictureBox
        // Similar to VerifyTriangleHasBeenDrawn, but for rectangles
        return false;
    }

    private bool VerifyCircleHasBeenDrawn()
    {
        // Implement logic to verify that a circle has been drawn in the PictureBox
        // Similar to VerifyTriangleHasBeenDrawn, but for circles
        return false;
    }

    private bool VerifySquareHasBeenDrawn()
    {
        // Implement logic to verify that a square has been drawn in the PictureBox
        // Similar to VerifyTriangleHasBeenDrawn, but for squares
        return false;
    }
}
}