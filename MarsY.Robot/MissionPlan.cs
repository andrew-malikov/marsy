using System.ComponentModel.DataAnnotations;

namespace MarsY.Robot;

public record MissionPlan
{
    internal readonly IBoundary Boundary;
    internal readonly IEnumerable<RobotDraft> Robots;

    internal MissionPlan(IBoundary boundary, IEnumerable<RobotDraft> robots)
    {
        Boundary = boundary;
        Robots = robots;
    }

    public static (MissionPlan?, ValidationResult?) From(string planCode)
    {
        var parts = planCode.Split("\n", 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return (null,
                new ValidationResult(
                    $"Research plan '{planCode}' must contain several lines, first is the grid and on robot setups on the rest."));
        }

        var gridParts = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (gridParts.Length != 2)
        {
            return (null,
                new ValidationResult(
                    $"The grid setup {parts[0]} is malformed, must contain 2 integers separated by space."));
        }

        if (!int.TryParse(gridParts[0], out int x))
        {
            return (null, new ValidationResult($"Grid's X size {gridParts[0]} is malformed, must be an integer."));
        }

        if (!int.TryParse(gridParts[1], out int y))
        {
            return (null, new ValidationResult($"Grid's Y size {gridParts[1]} is malformed, must be an integer."));
        }

        var (grid, gridValidationResult) = Grid.From(x, y);
        if (gridValidationResult is not null)
        {
            return (null, new ValidationResult($"Grid's size is malformed. {gridValidationResult.ErrorMessage}"));
        }

        var robotParts = parts[1].Split("\n");
        if (robotParts.Length / 2.0 % 1 != 0)
        {
            return (null,
                new ValidationResult(
                    $"Robots' setup {parts[1]} is malformed, must contain 2 lines per a robot setup, found {robotParts.Length / 2.0} setups."));
        }

        var robots = new List<RobotDraft>(robotParts.Length / 2);
        for (int i = 0; i < robotParts.Length / 2; i++)
        {
            var (robot, validationResult) = RobotDraft.From(i, robotParts[i * 2], robotParts[i * 2 + 1], grid!);
            if (validationResult is not null)
            {
                return (null,
                    new ValidationResult($"Robot's #{i} setup is malformed. {validationResult.ErrorMessage}"));
            }

            robots.Add(robot!.Value);
        }

        return (new MissionPlan(grid!, robots), null);
    }
}