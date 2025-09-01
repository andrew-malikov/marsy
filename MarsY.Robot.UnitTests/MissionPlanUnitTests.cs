using Shouldly;

namespace MarsY.Robot.UnitTests;

public class MissionPlanUnitTests
{
    public static IEnumerable<object[]> ValidPlanCode_Data()
    {
        yield return
        [
            "5 3\n1 1 E\nFRFRFRFRF",
            new MissionPlan(new Grid(5, 3),
                new List<RobotDraft> { new(0, new RobotPosition(1, 1, Orientation.East), "FRFRFRFRF") })
        ];
        yield return
        [
            "10 2\n10 2 N\nF\n0 0 S\nR",
            new MissionPlan(new Grid(10, 2),
                new List<RobotDraft>
                {
                    new(0, new RobotPosition(10, 2, Orientation.North), "F"),
                    new(1, new RobotPosition(0, 0, Orientation.South), "R")
                })
        ];
    }

    [Theory]
    [MemberData(nameof(ValidPlanCode_Data))]
    public void From_ValidPlanCode_NotNullMissionPlan(string planCode, MissionPlan expectedMissionPlan)
    {
        // Act
        var (missionPlan, validationResult) = MissionPlan.From(planCode);

        // Assert
        validationResult.ShouldBeNull();
        missionPlan.ShouldBeEquivalentTo(expectedMissionPlan);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("5 E\n1 1 E\nL")]
    [InlineData("\n1 1 E\nL")]
    [InlineData("E \n1 1 E\nL")]
    [InlineData("5 -3\n1 1 E\nL")]
    [InlineData("-3 0\n1 1 E\nL")]
    [InlineData("5 5 E\n1 1 E\nL\n3 3 S")]
    public void From_MalformedPlanCode_NotNullValidationResult(string planCode)
    {
        // Act
        var (missionPlan, validationResult) = MissionPlan.From(planCode);

        // Assert
        validationResult.ShouldNotBeNull();
        missionPlan.ShouldBeNull();
    }
}