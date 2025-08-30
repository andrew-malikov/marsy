using MarsY.Robot.ResearchSpace;

namespace MarsY.Robot.Instructions;

public sealed class MoveForwardInstruction(IBoundary researchSpace, IRobotsGraveyard graveyard) : IInstruction
{
    public ExecutionResult Execute(RobotPosition position)
    {
        if (graveyard.Contains(position))
        {
            return new ExecutionResult(ExecutionResultType.Skipped);
        }

        var repositioned = position.Orientation switch
        {
            Orientation.North => position with { Y = position.Y + 1 },
            Orientation.East => position with { X = position.X + 1 },
            Orientation.South => position with { Y = position.Y - 1 },
            Orientation.West => position with { X = position.X - 1 },
            _ => throw new NotImplementedException("Houston! Are we in a black hole again?")
        };

        return !researchSpace.Contains(repositioned.X, repositioned.Y)
            ? new ExecutionResult(ExecutionResultType.RobotIsBusted)
            : new ExecutionResult(ExecutionResultType.Executed, repositioned);
    }
}