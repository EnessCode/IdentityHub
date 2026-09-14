namespace IdentityHub.Service.EmailService
{
	public interface IEmailService
	{
		Task SendActivationEmailAsync(string toEmail, int activationCode);
		Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
	}
}
