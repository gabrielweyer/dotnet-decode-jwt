using System;
using System.Collections.Generic;
using Xunit;

namespace DotNet.Decode.Jwt.Tests
{
    public class ClaimsDisplayerTests
    {
        private readonly ClaimsDisplayer _target;

        private readonly MockConsole _console;

        public ClaimsDisplayerTests()
        {
            _console = new MockConsole();

            _target = new ClaimsDisplayer(_console);
        }

        [Fact]
        public void GivenNoClaim_WhenDisplayClaims_ThenMessage()
        {
            // Arrange

            var claims = new Dictionary<string, string>();

            // Act

            _target.DisplayClaims(claims);

            // Assert

            Assert.Equal(3, _console.Actions.Count);
            Assert.Equal("SET FOREGROUND COLOR: DarkGray", _console.Actions[0]);
            Assert.Equal("WRITE: There was no claims in the JWT.", _console.Actions[1]);
            Assert.Equal("RESET COLOR", _console.Actions[2]);
        }

        [Fact]
        public void GivenAtLeastOneClaim_WhenDisplayClaims_ThenOpeningAndClosingBraces()
        {
            // Arrange

            var claims = new Dictionary<string, string>
            {
                {"name", "John Doe"}
            };

            // Act

            _target.DisplayClaims(claims);

            // Assert

            Assert.Equal(7, _console.Actions.Count);
            Assert.Equal("WRITE: {", _console.Actions[3]);
            Assert.Equal("WRITE: }", _console.Actions[5]);
        }

        [Fact]
        public void GivenLastClaim_WhenDisplayClaims_ThenNoTrailingComma()
        {
            // Arrange

            var claims = new Dictionary<string, string>
            {
                {"name", "John Doe"}
            };

            // Act

            _target.DisplayClaims(claims);

            // Assert

            Assert.Equal(7, _console.Actions.Count);
            Assert.Equal("WRITE: {", _console.Actions[3]);
            Assert.Equal("WRITE: \t\"name\": \"John Doe\"", _console.Actions[4]);
            Assert.Equal("WRITE: }", _console.Actions[5]);
        }

        [Fact]
        public void GivenIatClaim_WhenDisplayClaims_ThenDisplayIatAsNumber()
        {
            // Arrange

            var claims = new Dictionary<string, string>
            {
                {"iat", "1516239022"}
            };

            // Act

            _target.DisplayClaims(claims);

            // Assert

            Assert.Equal(7, _console.Actions.Count);
            Assert.Equal("WRITE: {", _console.Actions[3]);
            Assert.Equal("WRITE: \t\"iat\": 1516239022", _console.Actions[4]);
            Assert.Equal("WRITE: }", _console.Actions[5]);
        }

        [Fact]
        public void GivenArrayClaim_WhenDisplayClaims_ThenDisplayClaimAsArray()
        {
            // Arrange

            var claims = new Dictionary<string, string>
            {
                {"aud", "[\"audience-one\",\"audience-two\"]"}
            };

            // Act

            _target.DisplayClaims(claims);

            // Assert

            Assert.Equal(7, _console.Actions.Count);
            Assert.Equal("WRITE: {", _console.Actions[3]);
            Assert.Equal("WRITE: \t\"aud\": [\"audience-one\",\"audience-two\"]", _console.Actions[4]);
            Assert.Equal("WRITE: }", _console.Actions[5]);
        }
    }

    internal class MockConsole : IConsole
    {
        private readonly List<string> _actions = new List<string>();

        public IReadOnlyList<string> Actions => _actions;

        public ConsoleColor ForegroundColor
        {
            set => _actions.Add($"SET FOREGROUND COLOR: {value}");
        }

        public void WriteLine(string value)
        {
            _actions.Add($"WRITE: {value}");
        }

        public void ResetColor()
        {
            _actions.Add("RESET COLOR");
        }
    }
}
