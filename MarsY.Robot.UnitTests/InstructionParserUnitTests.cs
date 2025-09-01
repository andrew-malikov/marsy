using MarsY.Robot.Instructions;
using Shouldly;

namespace MarsY.Robot.UnitTests;

public class InstructionParserUnitTests
{
    public static IEnumerable<object[]> ValidInstructions_Data()
    {
        var scents = new HashSet<RobotPosition>();
        var parser = new InstructionParser(scents);
        yield return
        [
            parser,
            "RFLFLR",
            new IInstruction[]
            {
                new RotateInstruction(RotateDirection.Right), new MoveForwardInstruction(scents),
                new RotateInstruction(RotateDirection.Left), new MoveForwardInstruction(scents),
                new RotateInstruction(RotateDirection.Left), new RotateInstruction(RotateDirection.Right)
            }
        ];
        yield return
        [
            parser,
            "F",
            new IInstruction[] { new MoveForwardInstruction(scents) }
        ];
        yield return
        [
            parser,
            "",
            new IInstruction[] { }
        ];
    }

    [Theory]
    [MemberData(nameof(ValidInstructions_Data))]
    internal void Parse_ValidInstructions_NotNullInstructions(InstructionParser parser, string instructionCode,
        IInstruction[] expectedInstructions)
    {
        // Act
        var (instructions, validationResult) = parser.Parse(instructionCode);

        // Assert
        validationResult.ShouldBeNull();
        instructions.ShouldNotBeNull();
        instructions.ToArray().ShouldBeEquivalentTo(expectedInstructions);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("  \n  F")]
    [InlineData(" F")]
    [InlineData("F ")]
    [InlineData("F F")]
    [InlineData("FF  RF")]
    [InlineData("A")]
    [InlineData("AF")]
    [InlineData("FRFA")]
    [InlineData("F1")]
    internal void Parse_MalformedInstructions_NotNullValidationResult(string instructionCode)
    {
        // Arrange
        var parser = new InstructionParser(new HashSet<RobotPosition>());

        // Act
        var (instructions, validationResult) = parser.Parse(instructionCode);

        // Assert
        validationResult.ShouldNotBeNull();
        instructions.ShouldBeNull();
    }
}