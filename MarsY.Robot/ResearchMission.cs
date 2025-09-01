using System.ComponentModel.DataAnnotations;
using MarsY.Robot.Instructions;

namespace MarsY.Robot;

internal class InstructionParser(ISet<RobotPosition> robotScents)
{
    private static readonly HashSet<char> Instructions = ['L', 'R', 'F'];

    public (IEnumerable<IInstruction>?, ValidationResult?) Parse(IEnumerable<char> instructionCode)
    {
        foreach (var symbol in instructionCode)
        {
            if (!Instructions.Contains(symbol))
            {
                return (null, new ValidationResult($"Instruction {symbol} isn't supported yet."));
            }
        }

        return (ParseNaked(instructionCode), null);
    }

    private IEnumerable<IInstruction> ParseNaked(IEnumerable<char> instructionCode)
    {
        foreach (var symbol in instructionCode)
        {
            yield return symbol switch
            {
                'L' => new RotateInstruction(RotateDirection.Left),
                'R' => new RotateInstruction(RotateDirection.Right),
                'F' => new MoveForwardInstruction(robotScents),
                _ => throw new NotImplementedException("Should not happen!")
            };
        }
    }
}

public sealed class ResearchMission(IRobotPrinter robotPrinter, MissionPlan plan)
{
    public ValidationResult? Accomplish()
    {
        var scents = new HashSet<RobotPosition>();
        var instructionParser = new InstructionParser(scents);

        foreach (var draft in plan.Robots)
        {
            var robot = new Robot(draft.Id, draft.InitialPosition, plan.Boundary, scents);
            var (instructions, validationResult) = instructionParser.Parse(draft.Instructions);
            if (validationResult is not null)
            {
                return new ValidationResult(
                    $"Robot #{robot.Id} has a malformed instruction. {validationResult.ErrorMessage}");
            }

            robot.Execute(instructions!);
            if (robot.State is RobotState.Online)
            {
                robotPrinter.LogOnlineRobot(robot.Position);
            }
            else
            {
                robotPrinter.LogBustedRobot(robot.Position);
            }
        }

        return null;
    }
}