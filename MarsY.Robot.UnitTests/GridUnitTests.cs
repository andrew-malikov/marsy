using Shouldly;

namespace MarsY.Robot.UnitTests;

public class GridUnitTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 3)]
    [InlineData(432, 3921321)]
    public void Grid_From_NullValidionResult(int x, int y)
    {
        // Act
        var (grid, validationResult) = Grid.From(x, y);

        // Assert
        grid.ShouldNotBeNull();
        validationResult.ShouldBeNull();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 3)]
    [InlineData(-232, 3921321)]
    [InlineData(23, -32)]
    [InlineData(0, -5)]
    public void Grid_From_NullGrid(int x, int y)
    {
        // Act
        var (grid, validationResult) = Grid.From(x, y);

        // Assert
        grid.ShouldBeNull();
        validationResult.ShouldNotBeNull();
    }
}