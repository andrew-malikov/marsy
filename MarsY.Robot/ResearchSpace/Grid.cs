namespace MarsY.Robot.ResearchSpace;

/// <param name="x"> Right grid boundary. </param>
/// <param name="y">Upper grid boundary.</param>
public sealed class Grid(int x, int y) : IBoundary
{
    private int X { get; } = x;

    private int Y { get; } = y;

    public bool Contains(int x, int y)
    {
        return x >= 0 &&
               x <= X &&
               y >= 0 &&
               y <= Y;
    }
}