namespace MarsY.Robot.Instructions;

internal sealed class MoveForwardInstruction(ISet<RobotPosition> robotScents) : IInstruction
{
    public RobotPosition Execute(RobotPosition position)
    {
        if (robotScents.Contains(position))
        {
            return position;
        }

        return position.Orientation switch
        {
            Orientation.North => position with { Y = position.Y + 1 },
            Orientation.East => position with { X = position.X + 1 },
            Orientation.South => position with { Y = position.Y - 1 },
            Orientation.West => position with { X = position.X - 1 },
            _ => throw new NotImplementedException("Houston! Are we in a black hole again?")
        };
    }
}