using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json.Linq;
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

            _target = new ClaimsDisplayer(_console, TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"));
        }

        [Fact]
        public void GivenNoClaim_WhenDisplayClaims_ThenMessage()
        {
            // Arrange

            var claims = new JObject();

            // Act

            _target.DisplayClaims(claims);

            // Assert

            var expected = new List<string>
            {
                "SET FOREGROUND COLOR: DarkGray",
                "WRITE: There was no claims in the JWT.",
                "RESET COLOR"
            };

            _console.Actions.Should().BeEquivalentTo(expected);
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

            var expected = new List<string>
            {
                "WRITE: ",
                "SET FOREGROUND COLOR: Yellow",
                "WRITE: Expiration Time (exp): N/A",
                "WRITE: Not Before (nbf): N/A",
                "WRITE: Issued At (iat): Thursday, 18 January 2018 01:30:22 UTC / Thursday, 18 January 2018 12:30:22 (UTC+10:00) Canberra, Melbourne, Sydney",
                "SET FOREGROUND COLOR: Green",
                "WRITE: ",
                "WRITE: Claims are:",
                "WRITE: ",
                "RESET COLOR",
                "WRITE: {\r\n  \"iat\": 1516239022\r\n}",
                "RESET COLOR"
            };

            _console.Actions.Should().BeEquivalentTo(expected);
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
