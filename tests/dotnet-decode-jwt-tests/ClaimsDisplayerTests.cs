using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xunit;

namespace DotNet.Decode.Jwt.Tests
{
    public class ClaimsDisplayerTests
    {
        private readonly ClaimsDisplayer _target;

        private readonly MockConsole _console;

        private const int ClaimsIndex = 3;
        private const int ExpectedLineCountWhenClaims = 5;
        private const int ExpectedLineCountWhenNoClaims = 3;

        public ClaimsDisplayerTests()
        {
            _console = new MockConsole();

            _target = new ClaimsDisplayer(_console);
        }

        [Fact]
        public void GivenNoClaim_WhenDisplayClaims_ThenMessage()
        {
            // Arrange

            var claims = new JObject();

            // Act

            _target.DisplayClaims(claims);

            // Assert

            Assert.Equal(ExpectedLineCountWhenNoClaims, _console.Actions.Count);
            Assert.Equal("SET FOREGROUND COLOR: DarkGray", _console.Actions[0]);
            Assert.Equal("WRITE: There was no claims in the JWT.", _console.Actions[1]);
            Assert.Equal("RESET COLOR", _console.Actions[2]);
        }

        [Fact]
        public void GivenAnyClaim_WhenDisplayClaims_ThenDisplayClaims()
        {
            // Arrange

            var claims = JObject.Parse(@"
            {
                'iat': 1516239022
            }
            ");

            // Act

            _target.DisplayClaims(claims);

            // Assert

            Assert.Equal(ExpectedLineCountWhenClaims, _console.Actions.Count);
            Assert.Equal("SET FOREGROUND COLOR: Green", _console.Actions[0]);
            Assert.Equal("WRITE: Claims are:", _console.Actions[1]);
            Assert.Equal("RESET COLOR", _console.Actions[2]);
            Assert.Equal("WRITE: {\r\n  \"iat\": 1516239022\r\n}", _console.Actions[ClaimsIndex]);
            Assert.Equal("RESET COLOR", _console.Actions[4]);
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
