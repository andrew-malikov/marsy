using NSubstitute;
using Shouldly;

namespace MarsY.Robot.UnitTests;

public class ResearchMissionUnitTests
{
    private readonly IRobotPrinter _printer = Substitute.For<IRobotPrinter>();

    [Fact]
    public void Accomplish_ValidPlan_NullValidationResultAndValidPrints()
    {
        // Arrange
        var (plan, _) = MissionPlan.From("5 3\n1 1 E\nRFRFRFRF\n3 2 N\nFRRFLLFFRRFLL\n0 3 W\nLLFFFLFLFL");

        // Act
        var missionValidationResult = new ResearchMission(_printer, plan!).Accomplish();

        // Assert
        missionValidationResult.ShouldBeNull();
        _printer.ReceivedCalls().Count().ShouldBe(3);
        _printer.Received(1).LogOnlineRobot(new RobotPosition(1, 1, Orientation.East));
        _printer.Received(1).LogBustedRobot(new RobotPosition(3, 3, Orientation.North));
        _printer.Received(1).LogOnlineRobot(new RobotPosition(2, 3, Orientation.South));
    }

    [Theory]
    [InlineData("5 3\n1 1 E\nDFRL")]
    public void Accomplish_MalformedInstructioSet_NotNullValidationResult(string planCode)
    {
        // Arrange
        var (plan, _) = MissionPlan.From(planCode);

        // Act
        var missionValidationResult = new ResearchMission(_printer, plan!).Accomplish();

        // Assert
        missionValidationResult.ShouldNotBeNull();
        _printer.ReceivedCalls().ShouldBeEmpty();
    }
}