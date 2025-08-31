namespace MarsY.Robot.Instructions;

public interface IInstruction
{
    RobotPosition Execute(RobotPosition position);
}