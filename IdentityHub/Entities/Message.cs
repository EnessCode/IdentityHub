namespace IdentityHub.Entities
{
	public class Message
	{
		public int Id { get; set; }
		public string SenderEmail { get; set; }
		public string ReceiverEmail { get; set; }
		public string Subject { get; set; }
		public DateTime SentAt { get; set; }
		public string MessageDetail { get; set; }
		public bool IsRead { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
