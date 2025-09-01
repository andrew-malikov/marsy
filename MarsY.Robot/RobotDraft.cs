using System.ComponentModel.DataAnnotations;

namespace MarsY.Robot;

internal readonly record struct RobotDraft(int Id, RobotPosition InitialPosition, string Instructions)
{
    public static (RobotDraft?, ValidationResult?) From(int id, string initialPosition, string instructionSet,
        IBoundary boundary)
    {
        var positionParts = initialPosition.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (positionParts.Length != 3)
        {
            return (null,
                new ValidationResult(
                    $"Robot's initial position '{initialPosition}' is malformed, it must include 3 parts separated by space like 5 3 E."));
        }

        if (!int.TryParse(positionParts[0], out int x))
        {
            return (null,
                new ValidationResult(
                    $"Robot's initial position X coordinate '{positionParts[0]}' is malformed, must be an integer."));
        }

        if (!int.TryParse(positionParts[1], out int y))
        {
            return (null,
                new ValidationResult(
                    $"Robot's initial position Y coordinate '{positionParts[1]}' is malformed, must be an integer."));
        }

        if (!boundary.Contains(x, y))
        {
            return (null,
                new ValidationResult($"Robot's initial position {x} X and {y} Y must not be outside of the grid."));
        }

        Orientation? orientation = positionParts[2] switch
        {
            "N" => Orientation.North,
            "E" => Orientation.East,
            "S" => Orientation.South,
            "W" => Orientation.West,
            _ => null
        };
        if (orientation is null)
        {
            return (null,
                new ValidationResult(
                    $"Robot's initial position orientation '{positionParts[2]}' is malformed, must a letter of N,E,S or W."));
        }

        if (string.IsNullOrEmpty(instructionSet))
        {
            return (null, new ValidationResult("Robot's instruction set mustn't be empty."));
        }

        var robotPosition = new RobotPosition(x, y, orientation.Value);
        return (new RobotDraft(id, robotPosition, instructionSet), null);
    }
}