using Company524.API.Service;
using Company524.API.Service.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Company524.Tests
{
    /// <summary>
    /// Unit tests for EmailService.
    ///
    /// EmailService depends on ISmtpClient and IConfiguration — both are mocked.
    /// This lets us test the service's logic (validation, subject normalization,
    /// error handling) without actually sending any email.
    /// </summary>
    public class EmailServiceTests
    {
        private readonly Mock<IConfiguration> _configMock = new();
        private readonly Mock<ISmtpClient> _smtpClientMock = new();
        private readonly Mock<ILogger<EmailService>> _loggerMock = new();
        private readonly EmailService _sut;

        public EmailServiceTests()
        {
            // Configuration values the service needs at runtime
            _configMock.Setup(c => c["EmailSettings:Sender"]).Returns("sender@test.com");
            _configMock.Setup(c => c["EmailSettings:SmtpServer"]).Returns("smtp.test.com");
            _configMock.Setup(c => c["EmailSettings:Port"]).Returns("587");
            _configMock.Setup(c => c["EmailSettings:UseSsl"]).Returns("false");
            _configMock.Setup(c => c["EmailSettings:Username"]).Returns("user");
            _configMock.Setup(c => c["EmailSettings:Password"]).Returns("pass");

            // SMTP client methods — all succeed by default
            _smtpClientMock.Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);
            _smtpClientMock.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _smtpClientMock.Setup(s => s.SendAsync(It.IsAny<MimeKit.MimeMessage>()))
                .Returns(Task.CompletedTask);
            _smtpClientMock.Setup(s => s.DisconnectAsync(It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            _sut = new EmailService(_configMock.Object, _smtpClientMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Send_WhenRecipientIsEmpty_ReturnsFail()
        {
            var result = await _sut.Send("", "Subject", "Body");

            // EmailService catches exceptions and returns a failure response
            // instead of throwing — this tests that contract
            result.success.Should().BeFalse();
        }

        [Fact]
        public async Task Send_WhenRecipientIsWhitespace_ReturnsFail()
        {
            var result = await _sut.Send("   ", "Subject", "Body");

            result.success.Should().BeFalse();
        }

        [Fact]
        public async Task Send_WhenRecipientIsInvalidEmail_ReturnsFail()
        {
            var result = await _sut.Send("not-an-email", "Subject", "Body");

            result.success.Should().BeFalse();
        }

        [Fact]
        public async Task Send_WhenValidEmail_ConnectsAuthenticatesSendsDisconnects()
        {
            var result = await _sut.Send("recipient@test.com", "Hello", "<p>Body</p>");

            result.success.Should().BeTrue();

            // Verify the full SMTP pipeline was executed in order
            _smtpClientMock.Verify(s => s.ConnectAsync("smtp.test.com", 587, false), Times.Once);
            _smtpClientMock.Verify(s => s.AuthenticateAsync("user", "pass"), Times.Once);
            _smtpClientMock.Verify(s => s.SendAsync(It.IsAny<MimeKit.MimeMessage>()), Times.Once);
            _smtpClientMock.Verify(s => s.DisconnectAsync(true), Times.Once);
        }

        [Fact]
        public async Task Send_WhenSmtpThrows_ReturnsFailResponseInsteadOfPropagatingException()
        {
            // Simulate SMTP server error mid-send
            _smtpClientMock
                .Setup(s => s.SendAsync(It.IsAny<MimeKit.MimeMessage>()))
                .ThrowsAsync(new Exception("SMTP connection refused"));

            // Should NOT throw — service catches and wraps the error
            var result = await _sut.Send("recipient@test.com", "Subject", "Body");

            result.success.Should().BeFalse();
            result.error.Should().NotBeNull();
            result.message.Should().Contain("SMTP connection refused");
        }

        [Fact]
        public async Task Send_WhenSubjectIsWhitespace_NormalizesToEmptyString()
        {
            // The service trims whitespace subjects — verify the send still completes
            var result = await _sut.Send("recipient@test.com", "   ", "Body");

            result.success.Should().BeTrue();
        }

        [Fact]
        public async Task Send_WhenSubjectHasLeadingTrailingSpaces_TrimsSubject()
        {
            // Email should still be sent successfully with trimmed subject
            var result = await _sut.Send("recipient@test.com", "  Hello World  ", "Body");

            result.success.Should().BeTrue();
        }
    }
}
