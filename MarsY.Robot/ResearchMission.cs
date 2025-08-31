using MarsY.Robot.Instructions;

namespace MarsY.Robot;

public interface IPrinter
{
    void LogBustedRobot(RobotPosition position);

    void LogOnlineRobot(RobotPosition position);
}

public readonly record struct RobotDraft(RobotPosition InitialPosition, string Instructions);

internal class InstructionParser(ISet<RobotPosition> robotScents)
{
    public IEnumerable<IInstruction> Parse(IEnumerable<char> instructionCode)
    {
        foreach (var symbol in instructionCode)
        {
            yield return symbol switch
            {
                'L' => new RotateInstruction(RotateDirection.Left),
                'R' => new RotateInstruction(RotateDirection.Right),
                'F' => new MoveForwardInstruction(robotScents),
                _ => throw new NotImplementedException()
            };
        }
    }
}

public readonly record struct MissionPlan(IBoundary boundary, IEnumerable<RobotDraft> robots);

public sealed class ResearchMission(IPrinter printer, MissionPlan plan)
{
    public void Accomplish()
    {
        var scents = new HashSet<RobotPosition>();
        var instructionParser = new InstructionParser(scents);

        foreach (var draft in plan.robots)
        {
            var robot = new Robot(draft.InitialPosition, plan.boundary, scents);
            robot.Execute(instructionParser.Parse(draft.Instructions));
            if (robot.State is RobotState.Online)
            {
                printer.LogOnlineRobot(robot.Position);
            }
            else
            {
                printer.LogBustedRobot(robot.Position);
            }
        }
    }
}