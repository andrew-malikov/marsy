using MarsY.Robot.Instructions;
using Shouldly;

namespace MarsY.Robot.UnitTests;

public class RobotUnitTests
{
    public static IEnumerable<object[]> OnlineRobot_Data()
    {
        var grid = new Grid(5, 3);
        HashSet<RobotPosition> scents = [];
        yield return
        [
            new Robot(1, new RobotPosition(1, 1, Orientation.East), grid, scents), new IInstruction[]
            {
                new RotateInstruction(RotateDirection.Right), new MoveForwardInstruction(scents),
                new RotateInstruction(RotateDirection.Right), new MoveForwardInstruction(scents),
                new RotateInstruction(RotateDirection.Right), new MoveForwardInstruction(scents),
                new RotateInstruction(RotateDirection.Right), new MoveForwardInstruction(scents),
            },
            new RobotPosition(1, 1, Orientation.East),
        ];
        scents = [];
        yield return
        [
            new Robot(2, new RobotPosition(3, 3, Orientation.North), grid, scents), new IInstruction[]
            {
                new RotateInstruction(RotateDirection.Left), new MoveForwardInstruction(scents),
                new MoveForwardInstruction(scents), new MoveForwardInstruction(scents)
            },
            new RobotPosition(0, 3, Orientation.West),
        ];
        scents = [new(3, 3, Orientation.North)];
        yield return
        [
            new Robot(1, new RobotPosition(0, 3, Orientation.West), grid, scents), new IInstruction[]
            {
                new RotateInstruction(RotateDirection.Left), new RotateInstruction(RotateDirection.Left),
                new MoveForwardInstruction(scents), new MoveForwardInstruction(scents),
                new MoveForwardInstruction(scents), new RotateInstruction(RotateDirection.Left),
                new MoveForwardInstruction(scents), new RotateInstruction(RotateDirection.Left),
                new MoveForwardInstruction(scents), new RotateInstruction(RotateDirection.Left),
            },
            new RobotPosition(2, 3, Orientation.South),
        ];
    }

    [Theory]
    [MemberData(nameof(OnlineRobot_Data))]
    internal void Execute_Online(Robot robot, IEnumerable<IInstruction> instructions, RobotPosition expectedPosition)
    {
        // Act
        robot.Execute(instructions);

        // Assert
        robot.State.ShouldBe(RobotState.Online);
        robot.Position.ShouldBe(expectedPosition);
    }

    public static IEnumerable<object[]> BustedRobot_Data()
    {
        var grid = new Grid(5, 3);
        HashSet<RobotPosition> scents = [];
        yield return
        [
            new Robot(1, new RobotPosition(3, 2, Orientation.North), grid, scents), new IInstruction[]
            {
                new MoveForwardInstruction(scents), new RotateInstruction(RotateDirection.Right),
                new RotateInstruction(RotateDirection.Right), new MoveForwardInstruction(scents),
                new RotateInstruction(RotateDirection.Left), new RotateInstruction(RotateDirection.Left),
                new MoveForwardInstruction(scents), new MoveForwardInstruction(scents),
                new RotateInstruction(RotateDirection.Right), new RotateInstruction(RotateDirection.Right),
                new MoveForwardInstruction(scents), new RotateInstruction(RotateDirection.Left),
                new RotateInstruction(RotateDirection.Left),
            },
            new RobotPosition(3, 3, Orientation.North),
            scents
        ];
    }

    [Theory]
    [MemberData(nameof(BustedRobot_Data))]
    internal void Execute_Busted(Robot robot, IEnumerable<IInstruction> instructions, RobotPosition expectedPosition,
        ISet<RobotPosition> scents)
    {
        // Act
        robot.Execute(instructions);

        // Assert
        robot.State.ShouldBe(RobotState.Busted);
        robot.Position.ShouldBe(expectedPosition);
        scents.ShouldHaveSingleItem().ShouldBe(expectedPosition);
    }
}