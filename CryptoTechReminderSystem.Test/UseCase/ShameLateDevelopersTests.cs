using System;
using System.Linq;
using CryptoTechReminderSystem.Boundary;
using CryptoTechReminderSystem.Test.TestDouble;
using CryptoTechReminderSystem.UseCase;
using FluentAssertions;
using NUnit.Framework;

namespace CryptoTechReminderSystem.Test.UseCase
{
    public class ShameLateDevelopersTests
    {
        private SendReminderSpy _sendReminderSpy;
        private GetLateDevelopersSpy _getLateDevelopersSpy;
        private ClockStub _clock;

        [SetUp]
        public void SetUp()
        {
            _sendReminderSpy = new SendReminderSpy();
            _getLateDevelopersSpy = new GetLateDevelopersSpy();
            _clock = new ClockStub(
                new DateTimeOffset(
                    new DateTime(2019, 03, 01, 13, 30, 0)
                )
            );
        }
        
        [Test]
        public void CanGetLateDevelopers()
        {
            var shameLateDevelopers = new ShameLateDevelopers(_getLateDevelopersSpy, _sendReminderSpy);

            shameLateDevelopers.Execute(new ShameLateDevelopersRequest());

            _getLateDevelopersSpy.Called.Should().BeTrue();
        }

        [Test]
        public void CanRemindDevelopers()
        {
            var shameLateDevelopers = new ShameLateDevelopers(_getLateDevelopersSpy, _sendReminderSpy);

            shameLateDevelopers.Execute(
                new ShameLateDevelopersRequest
                {
                    ShameMessage = "TIMESHEETS ARE GOOD YO!"
                }
            );

            _sendReminderSpy.Called.Should().BeTrue();
        }

        [Test]
        [TestCase( "W0123CHAN", "W123AMON", "W789ROSS")]
        [TestCase( "W0123CHAN", "W123AMON")]
        public void CanCheckShameMessageHasAllUsers(params string[] userId)
        {
            
            var getLateDevelopersStub = new GetLateDevelopersStub(userId.ToList());
            var shameLateDevelopers = new ShameLateDevelopers(getLateDevelopersStub, _sendReminderSpy);
            var expectedShameMessage = "TIMESHEETS ARE GOOD YO!";

            shameLateDevelopers.Execute(
                new ShameLateDevelopersRequest
                {
                    ShameMessage = expectedShameMessage
                }
            );

            foreach (var user in userId)
            {
                expectedShameMessage += $"\n• <@{user}>";
            }
            
            _sendReminderSpy.Text.Should().Be(expectedShameMessage);
        }

        [Test]
        public void CanCheckMessageHadChannel()
        {
            var shameLateDevelopers = new ShameLateDevelopers(_getLateDevelopersSpy, _sendReminderSpy);

            const string expectedChannel = "CH123456";
            
            shameLateDevelopers.Execute(
                new ShameLateDevelopersRequest
                {
                    Channel = expectedChannel
                }
            );
            
            _sendReminderSpy.Channels.First().Should().Be(expectedChannel);
        }
    }
}
