namespace IdentityHub.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public string CommentDetail { get; set; }
		public DateTime SentAt { get; set; }
		public CommentStatus Status { get; set; }
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
