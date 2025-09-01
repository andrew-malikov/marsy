using System.ComponentModel.DataAnnotations;
using MarsY.Robot.Instructions;

namespace MarsY.Robot;

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

internal readonly record struct RobotDraft(RobotPosition InitialPosition, string Instructions)
{
    public static (RobotDraft?, ValidationResult?) From(string initialPosition, string instructionSet,
        IBoundary boundary)
    {
        var positionParts = initialPosition.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (positionParts.Length != 3)
        {
            return (null,
                new ValidationResult(
                    $"Robot's initial position '{initialPosition}' is malformed, it must include 3 parts separated by space like X Y Orientation"));
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
        return (new RobotDraft(robotPosition, instructionSet), null);
    }
}

public readonly struct MissionPlan
{
    internal readonly IBoundary Boundary;
    internal readonly IEnumerable<RobotDraft> Robots;

    private MissionPlan(IBoundary boundary, IEnumerable<RobotDraft> robots)
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

        var gridParts = parts[0].Split("\n", StringSplitOptions.RemoveEmptyEntries);
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
                    $"Robots setup {parts[1]} is malformed, must contain 2 lines per a robot setup, found {robotParts.Length / 2.0} setups."));
        }

        var robots = new List<RobotDraft>(robotParts.Length / 2);
        for (int i = 0; i < robotParts.Length / 2; i++)
        {
            var (robot, validationResult) = RobotDraft.From(robotParts[i], robotParts[i + 1], grid!);
            if (validationResult is not null)
            {
                return (null, new ValidationResult($"Robot #{i} setup is malformed. {validationResult.ErrorMessage}"));
            }

            robots.Add(robot!.Value);
        }

        return (new MissionPlan(grid!, robots), null);
    }
}

public sealed class ResearchMission(IRobotPrinter robotPrinter, MissionPlan plan)
{
    public void Accomplish()
    {
        var scents = new HashSet<RobotPosition>();
        var instructionParser = new InstructionParser(scents);

        foreach (var draft in plan.Robots)
        {
            var robot = new Robot(draft.InitialPosition, plan.Boundary, scents);
            robot.Execute(instructionParser.Parse(draft.Instructions));
            if (robot.State is RobotState.Online)
            {
                robotPrinter.LogOnlineRobot(robot.Position);
            }
            else
            {
                robotPrinter.LogBustedRobot(robot.Position);
            }
        }
    }
}