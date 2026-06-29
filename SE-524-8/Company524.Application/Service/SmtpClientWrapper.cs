using Company524.Application.Contracts.Service;
using MimeKit;

namespace Company524.Application.Service
{
    public class SmtpClientWrapper : ISmtpClient
    {
        private readonly MailKit.Net.Smtp.SmtpClient _client = new();

        public async Task AuthenticateAsync(string username, string password) => await _client.AuthenticateAsync(username, password);
        public async Task ConnectAsync(string host, int port, bool useSsl) => await _client.ConnectAsync(host, port, useSsl);
        public async Task DisconnectAsync(bool quit) => await _client.DisconnectAsync(quit);
        public async Task SendAsync(MimeMessage message) => await _client.SendAsync(message);
        public void Dispose() => _client.Dispose();
    }
}
