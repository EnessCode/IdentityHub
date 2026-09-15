namespace IdentityHub.Entities
{
	public class Notification
	{
		public int Id { get; set; }
		public string Detail { get; set; }
		public string ImageUrl { get; set; }
		public DateTime SendDate { get; set; } = DateTime.Now;
		public bool Status { get; set; }
	}
}
