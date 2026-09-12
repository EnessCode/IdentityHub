using MailKit.Net.Smtp;
using MimeKit;
using IdentityHub.Service.EmailService;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;

	public EmailService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task SendActivationEmailAsync(string toEmail, int activationCode)
	{
		var emailSettings = _configuration.GetSection("EmailSettings");

		MimeMessage mimeMessage = new MimeMessage();
		mimeMessage.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
		mimeMessage.To.Add(new MailboxAddress("User", toEmail));

		mimeMessage.Subject = "Hesap Doğrulama";
		mimeMessage.Body = new BodyBuilder { TextBody = $"Doğrulama Kodunuz: {activationCode}" }.ToMessageBody();

		using var smtpClient = new SmtpClient();
		await smtpClient.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), false);
		await smtpClient.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
		await smtpClient.SendAsync(mimeMessage);
		await smtpClient.DisconnectAsync(true);
	}
}