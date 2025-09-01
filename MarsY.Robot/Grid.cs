using System.ComponentModel.DataAnnotations;

namespace MarsY.Robot;

public interface IBoundary
{
    bool Contains(int x, int y);
}

/// <summary>
///     A grid, left and bottom are 0s.
/// </summary>
internal sealed class Grid : IBoundary
{
    private readonly int _x;

    private readonly int _y;

    /// <param name="x">Right grid boundary, a positive integer > 0.</param>
    /// <param name="y">Upper grid boundary, a positive integer > 0.</param>
    private Grid(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public bool Contains(int x, int y)
    {
        return x >= 0 &&
               x <= _x &&
               y >= 0 &&
               y <= _y;
    }

    public static (IBoundary?, ValidationResult?) From(int x, int y)
    {
        if (x < 1)
        {
            return (null, new ValidationResult($"X {x} must be positive."));
        }

        if (y < 1)
        {
            return (null, new ValidationResult($"Y {y} must be positive."));
        }

        return (new Grid(x, y), null);
    }
}