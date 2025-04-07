using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ServerWeather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private const string SMTP_HOST = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        private const string SMTP_USERNAME = "tharun.ks.cse.2021@snsce.ac.in";
        private const string SMTP_PASSWORD = "qhsi pmvd fpvf crvy";

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Weather Alert System", SMTP_USERNAME));
                email.To.Add(new MailboxAddress("User", request.ToEmail));
                email.Subject = request.Subject;
                email.Body = new TextPart("plain") { Text = request.Body };

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(SMTP_HOST, SMTP_PORT, SecureSocketOptions.StartTls);

                // Clear any existing authentication
                client.AuthenticationMechanisms.Clear();
                client.AuthenticationMechanisms.Add("XOAUTH2");
                client.AuthenticationMechanisms.Add("PLAIN");

                await client.AuthenticateAsync(SMTP_USERNAME, SMTP_PASSWORD);
                
                Console.WriteLine($"Connected: {client.IsConnected}");
                Console.WriteLine($"Authenticated: {client.IsAuthenticated}");

                await client.SendAsync(email);
                await client.DisconnectAsync(true);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Detailed error: {ex}");
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class EmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
