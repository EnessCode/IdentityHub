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

	public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
	{
		var emailSettings = _configuration.GetSection("EmailSettings");

		MimeMessage mimeMessage = new MimeMessage();
		mimeMessage.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
		mimeMessage.To.Add(new MailboxAddress("Kullanıcı", toEmail));

		mimeMessage.Subject = "Identity Hub - Şifre Sıfırlama Talebi";

		var bodyBuilder = new BodyBuilder
		{
			HtmlBody = $@"
				<h3>Şifre Sıfırlama Talebi</h3>
				<p>Hesabınız için şifre sıfırlama talebinde bulunulmuştur.</p>
				<p>Yeni şifrenizi belirlemek için <a href='{resetLink}'><strong>buraya tıklayarak</strong></a> ilgili sayfaya gidebilirsiniz.</p>
				<br/>
				<p><small>Eğer bu talebi siz yapmadıysanız, bu e-postayı dikkate almayınız.</small></p>"
		};
		mimeMessage.Body = bodyBuilder.ToMessageBody();

		using var smtpClient = new SmtpClient();
		await smtpClient.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), false);
		await smtpClient.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
		await smtpClient.SendAsync(mimeMessage);
		await smtpClient.DisconnectAsync(true);
	}
}