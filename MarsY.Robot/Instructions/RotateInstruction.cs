namespace MarsY.Robot.Instructions;

public enum RotateDirection
{
    Left = -1,
    Right = 1
}

public sealed class RotateInstruction(RotateDirection direction) : IInstruction
{
    public ExecutionResult Execute(RobotPosition position)
    {
        return new ExecutionResult(ExecutionResultType.Executed,
            position with { Orientation = (Orientation)((4 + (int)position.Orientation + (int)direction) % 4) });
    }
}