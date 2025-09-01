using MarsY.Robot;

namespace MarsY.Mission.Cli;

internal sealed class RobotStdOutPrinter : IRobotPrinter
{
    public void LogBustedRobot(RobotPosition position)
    {
        Console.WriteLine("{0} {1} {2} LOST", position.X, position.Y, position.Orientation.ToString()[0]);
    }

    public void LogOnlineRobot(RobotPosition position)
    {
        Console.WriteLine("{0} {1} {2}", position.X, position.Y, position.Orientation.ToString()[0]);
    }
}