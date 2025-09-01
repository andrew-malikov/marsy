using AutoFixture.Xunit2;
using MarsY.Robot.Instructions;
using NSubstitute;
using Shouldly;

namespace MarsY.Robot.UnitTests.Instructions;

public class MoveForwardInstructionUnitTests
{
    private readonly ISet<RobotPosition> _scents;
    private readonly IInstruction _instruction;

    public MoveForwardInstructionUnitTests()
    {
        _scents = Substitute.For<ISet<RobotPosition>>();
        _instruction = new MoveForwardInstruction(_scents);
    }

    public static IEnumerable<object[]> NewPosition =>
        new List<object[]>
        {
            new object[]
            {
                new RobotPosition { X = 0, Y = 0, Orientation = Orientation.North },
                new RobotPosition { X = 0, Y = 1, Orientation = Orientation.North }
            },
            new object[]
            {
                new RobotPosition { X = 0, Y = 0, Orientation = Orientation.East },
                new RobotPosition { X = 1, Y = 0, Orientation = Orientation.East }
            },
            new object[]
            {
                new RobotPosition { X = 23, Y = 1, Orientation = Orientation.South },
                new RobotPosition { X = 23, Y = 0, Orientation = Orientation.South }
            },
            new object[]
            {
                new RobotPosition { X = 1, Y = 12, Orientation = Orientation.West },
                new RobotPosition { X = 0, Y = 12, Orientation = Orientation.West }
            }
        };

    [Theory]
    [MemberData(nameof(NewPosition))]
    public void Execute_NoScent_NewPosition(RobotPosition initialPosition, RobotPosition expectedPosition)
    {
        // Arrange
        _scents.Contains(default).ReturnsForAnyArgs(false);

        // Act
        var actualResult = _instruction.Execute(initialPosition);

        // Assert
        actualResult.ShouldBe(expectedPosition);
        _scents.Received(1).Contains(initialPosition);
    }

    [Theory]
    [AutoData]
    public void Execute_HasScent_SamePosition(RobotPosition position)
    {
        // Arrange
        _scents.Contains(position).ReturnsForAnyArgs(true);

        // Act
        var actualResult = _instruction.Execute(position);

        // Assert
        actualResult.ShouldBe(position);
        _scents.Received(1).Contains(position);
    }
}