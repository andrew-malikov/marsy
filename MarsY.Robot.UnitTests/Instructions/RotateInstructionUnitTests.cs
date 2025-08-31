using MarsY.Robot.Instructions;
using Shouldly;

namespace MarsY.Robot.UnitTests.Instructions;

public class RotateInstructionUnitTests
{
    [Theory]
    [InlineData(Orientation.North, RotateDirection.Left, Orientation.West)]
    [InlineData(Orientation.North, RotateDirection.Right, Orientation.East)]
    [InlineData(Orientation.East, RotateDirection.Left, Orientation.North)]
    [InlineData(Orientation.East, RotateDirection.Right, Orientation.South)]
    [InlineData(Orientation.South, RotateDirection.Left, Orientation.East)]
    [InlineData(Orientation.South, RotateDirection.Right, Orientation.West)]
    [InlineData(Orientation.West, RotateDirection.Left, Orientation.South)]
    [InlineData(Orientation.West, RotateDirection.Right, Orientation.North)]
    public void Execute_Executed(Orientation initial, RotateDirection rotate, Orientation expected)
    {
        // Arrange
        var position = new RobotPosition { X = 3, Y = 3, Orientation = initial };
        var instruction = new RotateInstruction(rotate);

        // Act
        var actualResult = instruction.Execute(position);

        // Assert
        actualResult.Orientation.ShouldBe(expected);
        actualResult.X.ShouldBe(3);
        actualResult.Y.ShouldBe(3);
    }
}