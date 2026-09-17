using IdentityHub.Context;
using IdentityHub.Entities;
using IdentityHub.ML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityHub.Controllers
{
	[Authorize] 
	public class CommentController : Controller
	{
		private readonly IdentityContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly IConfiguration _configuration;

		public CommentController(IdentityContext context, UserManager<AppUser> userManager, IConfiguration configuration)
		{
			_context = context;
			_userManager = userManager;
			_configuration = configuration;
		}

		[Authorize(Roles = "Admin,Moderatör")]
		public IActionResult Index()
		{
			var comments = _context.Comments
				.Include(x => x.AppUser)
				.OrderByDescending(x => x.SentAt)
				.ToList();

			return View(comments);
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Moderatör")]
		public IActionResult ChangeStatus(int id, CommentStatus newStatus)
		{
			var comment = _context.Comments.Find(id);
			if (comment != null)
			{
				comment.Status = newStatus;
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Moderatör")]
		public IActionResult UpdateAllStatuses(Dictionary<int, CommentStatus> statuses)
		{
			if (statuses != null && statuses.Count > 0)
			{
				foreach (var kvp in statuses)
				{
					int commentId = kvp.Key;
					var comment = _context.Comments.Find(commentId);
					if (comment != null)
					{
						comment.Status = kvp.Value;
					}
				}
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		[Authorize(Roles = "Admin,Moderatör")]
		public IActionResult Delete(int id)
		{
			var comment = _context.Comments.Find(id);
			if (comment != null)
			{
				_context.Comments.Remove(comment);
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public IActionResult Forum()
		{
			var forumComments = _context.Comments
				.Include(x => x.AppUser)
				.Where(x => x.Status == CommentStatus.Onaylandi)
				.OrderBy(x => x.SentAt)
				.ToList();

			return View(forumComments);
		}

		[HttpPost]
		public async Task<IActionResult> AddComment(Comment comment)
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			if (user != null)
			{
				comment.AppUserId = user.Id;
				comment.SentAt = DateTime.Now;

				var prediction = ToxicityModel.GetPrediction(comment.CommentDetail);

				if (prediction.IsToxic && prediction.Probability < 0.70f)
					comment.Status = CommentStatus.OnayBekliyor;
				else if (prediction.IsToxic && prediction.Probability >= 0.70f)
					comment.Status = CommentStatus.Toksik;
				else
					comment.Status = CommentStatus.Onaylandi;

				_context.Comments.Add(comment);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("Forum");
		}
	}
}
