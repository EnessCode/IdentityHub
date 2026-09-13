namespace IdentityHub.Models.Message
{
	public class MessageWithSenderInfoViewModel
	{
		public int Id { get; set; }
		public string Subject { get; set; }
		public string MessageDetail { get; set; }
		public DateTime SentAt { get; set; }
		public string SenderEmail { get; set; }
		public string SenderName { get; set; }
		public string SenderSurname { get; set; }
		public string CategoryName { get; set; }
	}
}
