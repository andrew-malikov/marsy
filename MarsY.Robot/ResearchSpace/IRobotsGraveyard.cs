namespace MarsY.Robot.ResearchSpace;

public interface IRobotsGraveyard
{
    void MarkLastRobotPosition(RobotPosition position);

    bool Contains(RobotPosition position);
}