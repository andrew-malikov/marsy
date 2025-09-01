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

internal enum RobotState
{
    Online = 0,
    Busted = 1
}

internal sealed class Robot(
    int id,
    RobotPosition position,
    IBoundary boundary,
    ISet<RobotPosition> robotScents,
    RobotState state = RobotState.Online)
{
    public int Id { get; private set; } = id;

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

        var newPosition = instruction.Execute(Position);
        if (boundary.Contains(newPosition.X, newPosition.Y))
        {
            Position = newPosition;
            return;
        }

        State = RobotState.Busted;
        robotScents.Add(Position);
    }
}