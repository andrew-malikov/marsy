using Shouldly;

namespace MarsY.Robot.UnitTests;

public class RobotDraftUnitTests
{
    public static IEnumerable<object[]> ValidRobotSetup_Data()
    {
        var grid = new Grid(5, 5);
        yield return
        [
            1, "1 2 E", "FRFRF", grid, new RobotDraft(1, new RobotPosition(1, 2, Orientation.East), "FRFRF")
        ];
        yield return
        [
            2, "5 5 N", "L", grid, new RobotDraft(2, new RobotPosition(5, 5, Orientation.North), "L")
        ];
    }

    [Theory]
    [MemberData(nameof(ValidRobotSetup_Data))]
    internal void From_ValidRobotSetup_NotNullRobotDraft(int id, string initialPosition, string instructionSet,
        IBoundary grid, RobotDraft expectedRobotDraft)
    {
        // Act
        var (draft, validationResult) = RobotDraft.From(id, initialPosition, instructionSet, grid);

        // Assert
        validationResult.ShouldBeNull();
        draft.ShouldBeEquivalentTo(expectedRobotDraft);
    }

    public static IEnumerable<object[]> MalformedRobotSetup_Data()
    {
        var grid = new Grid(5, 5);
        yield return [1, "1 E", "FRFRF", grid];
        yield return [2, "5 5", "L", grid];
        yield return [2, "5 5", "L", grid];
    }

    [Theory]
    [InlineData(1, "1 E", "FRFRF")]
    [InlineData(2, "5 5", "F")]
    [InlineData(3, "E", "RF")]
    [InlineData(4, "", "RF")]
    [InlineData(6, "  ", "RF")]
    [InlineData(7, "1 7 S", "RF")]
    [InlineData(8, "2 1 N", "")]
    internal void From_MalformedRobotSetup_NotNullValidationResult(int id, string initialPosition, string instructionSet)
    {
        // Arrange
        var grid = new Grid(5, 5);
        
        // Act
        var (draft, validationResult) = RobotDraft.From(id, initialPosition, instructionSet, grid);

        // Assert
        validationResult.ShouldNotBeNull();
        draft.ShouldBeNull();
    }
}