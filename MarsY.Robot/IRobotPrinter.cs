namespace MarsY.Robot;

public interface IRobotPrinter
{
    void LogBustedRobot(RobotPosition position);

    void LogOnlineRobot(RobotPosition position);
}