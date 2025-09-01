namespace MarsY.Robot.Instructions;

public enum RotateDirection
{
    Left = -1,
    Right = 1
}

internal sealed class RotateInstruction(RotateDirection direction) : IInstruction
{
    public RobotPosition Execute(RobotPosition position)
    {
        return position with { Orientation = (Orientation)((4 + (int)position.Orientation + (int)direction) % 4) };
    }
}