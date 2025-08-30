namespace MarsY.Robot.Instructions;

public enum ExecutionResultType
{
    Executed = 0,
    Skipped = 1,
    RobotIsBusted = 2
}

public readonly struct ExecutionResult(ExecutionResultType type, RobotPosition? position = null)
{
    public ExecutionResultType Type { get; } = type;

    public RobotPosition? Position { get; } = position;
}

public interface IInstruction
{
    ExecutionResult Execute(RobotPosition position);
}