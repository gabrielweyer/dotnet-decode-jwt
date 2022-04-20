namespace Gabo.DotNet.DecodeJwt.Tests;

public class ProgramTests
{
    private const string ValidJwtInput = "eyJhbGciOiJub25lIn0.ewogICAgImlzcyI6ICJiZXN0LWlzc3VlciIsCiAgICAic3ViIjogIm5pY2Utc3ViamVjdCIsCiAgICAiYXVkIjogWyJhdWRpZW5jZS1vbmUiLCAiYXVkaWVuY2UtdHdvIl0sCiAgICAiZXhwIjogMTUyODY5MTM1MCwKICAgICJuYmYiOiAxNTI4NjkwNzUwLAogICAgImlhdCI6IDE1Mjg2OTA3NTAsCiAgICAianRpIjogImMzMTk3ZGNiLWUxMTMtNDc3OC04OTc5LWI5NTZmNjg0MDA3ZiIsCiAgICAiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjogImhpQG1lLmNvbSIsCiAgICAic29tZS1udW1iZXIiOiAxMi41NiwKICAgICJuZXN0ZWQtY2xhaW0iOiB7CiAgICAgICAgImhpIjogIkknbSIsCiAgICAgICAgImEiOiAibmVzdGVkIGNsYWltIgogICAgfQp9Cg==.";
    private const string InvalidJwtInput = "cQ==";

    [Fact]
    public void GivenNoArgumentProvided_ThenFailureExitCode()
    {
        // Arrange
        var input = Array.Empty<string>();

        // Act
        var actualExitCode = Program.Main(input);

        // Assert
        actualExitCode.Should().Be(Program.FailureExitCode);
    }

    [Fact]
    public void GivenMoreThanOneArgumentProvided_ThenFailureExitCode()
    {
        // Arrange
        var input = new[] { ValidJwtInput, ValidJwtInput };

        // Act
        var actualExitCode = Program.Main(input);

        // Assert
        actualExitCode.Should().Be(Program.FailureExitCode);
    }

    [Fact]
    public void GivenInvalidJwtInput_ThenFailureExitCode()
    {
        // Arrange
        var input = new[] { InvalidJwtInput };

        // Act
        var actualExitCode = Program.Main(input);

        // Assert
        actualExitCode.Should().Be(Program.FailureExitCode);
    }

    [Fact]
    public void GivenValidJwtInput_ThenSuccessExitCode()
    {
        // Arrange
        var input = new[] { ValidJwtInput };

        // Act
        var actualExitCode = Program.Main(input);

        // Assert
        actualExitCode.Should().Be(Program.SuccessExitCode);
    }
}
