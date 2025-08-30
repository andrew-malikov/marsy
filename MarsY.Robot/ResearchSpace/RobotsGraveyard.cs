namespace MarsY.Robot.ResearchSpace;

/// <summary>
///   Last positions of robots before they fell off the grid. 
/// </summary>
public sealed class RobotsGraveyard(ISet<RobotPosition>? lostRobots = null) : IRobotsGraveyard
{
    private readonly ISet<RobotPosition> _lostRobots = lostRobots ?? new HashSet<RobotPosition>();

    public void MarkLastRobotPosition(RobotPosition position)
    {
        _lostRobots.Add(position);
    }

    public bool Contains(RobotPosition position)
    {
        return _lostRobots.Contains(position);
    }
}