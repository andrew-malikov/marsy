using AutoFixture.Xunit2;
using MarsY.Robot.Instructions;
using NSubstitute;
using Shouldly;

namespace MarsY.Robot.UnitTests.Instructions;

public class MoveForwardInstructionUnitTests
{
    private readonly ISet<RobotPosition> _scents = Substitute.For<ISet<RobotPosition>>();

    public static IEnumerable<object[]> WithinBoundary =>
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
    [MemberData(nameof(WithinBoundary))]
    public void Execute_WithinBoundary_Executed(RobotPosition initialPosition, RobotPosition expectedPosition)
    {
        // Arrange
        _scents.Contains(default).ReturnsForAnyArgs(false);
        var instruction = new MoveForwardInstruction(_scents);

        // Act
        var actualResult = instruction.Execute(initialPosition);

        // Assert
        actualResult.ShouldBe(expectedPosition);
        _scents.Received(1).Contains(initialPosition);
    }

    [Theory]
    [AutoData]
    public void Execute_OnTheBoundaryEdge_RobotIsBusted(RobotPosition position)
    {
        // Arrange
        _scents.Contains(default).ReturnsForAnyArgs(false);
        var instruction = new MoveForwardInstruction(_scents);

        // Act
        var actualResult = instruction.Execute(position);

        // Assert
        actualResult.ShouldBe(position);
        _scents.Received(1).Contains(position);
    }

    // [Theory]
    // [AutoData]
    // public void Execute_OnTheBoundaryEdgeWhereOtherRobotGotBusted_Skipped(RobotPosition position)
    // {
    //     // Arrange
    //     var graveyard = Substitute.For<IRobotsGraveyard>();
    //     graveyard.Contains(default).ReturnsForAnyArgs(true);
    //
    //     var boundary = Substitute.For<IBoundary>();
    //     var instruction = new MoveForwardInstruction(boundary, graveyard);
    //
    //     // Act
    //     var actualResult = instruction.Execute(position);
    //
    //     // Assert
    //     actualResult.Type.ShouldBe(ExecutionResultType.Skipped);
    //     actualResult.Position.ShouldBeNull();
    //     boundary.ReceivedCalls().ShouldBeEmpty();
    //     graveyard.Received(1).Contains(position);
    // }
}