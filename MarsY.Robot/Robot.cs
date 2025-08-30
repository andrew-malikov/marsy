using MarsY.Robot.Instructions;

namespace MarsY.Robot;

public enum Orientation
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public readonly record struct RobotPosition(int X, int Y, Orientation Orientation);

public enum RobotState
{
    Online = 0,
    Busted = 1
}

public sealed class Robot(RobotPosition position, RobotState state = RobotState.Online)
{
    /// <summary>
    ///     Last trackable position.
    /// </summary>
    public RobotPosition Position { get; private set; } = position;

    public RobotState State { get; private set; } = state;

    public void Execute(IEnumerable<IInstruction> instructions)
    {
        if (State != RobotState.Online)
        {
            return;
        }

        foreach (var instruction in instructions)
        {
            Execute(instruction);
        }
    }

    public void Execute(IInstruction instruction)
    {
        if (State != RobotState.Online)
        {
            return;
        }

        var result = instruction.Execute(Position);
        switch (result.Type)
        {
            case ExecutionResultType.Executed:
                Position = result.Position!.Value;
                break;

            case ExecutionResultType.Skipped:
                break;

            case ExecutionResultType.RobotIsBusted:
                State = RobotState.Busted;
                return;

            default:
                throw new NotImplementedException();
        }
    }
}