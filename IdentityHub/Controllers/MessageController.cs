using IdentityHub.Context;
using IdentityHub.Entities;
using IdentityHub.Models.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityHub.Controllers
{
	[Authorize]
	public class MessageController : Controller
	{
		private readonly IdentityContext _context;
		private readonly UserManager<AppUser> _userManager;

		public MessageController(IdentityContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> Inbox(int? categoryId)
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			var messages = (from m in _context.Messages
							join u in _context.Users
							on m.SenderEmail equals u.Email into userGroup
							from sender in userGroup.DefaultIfEmpty()

							join c in _context.Categories
							on m.CategoryId equals c.Id into categoryGroup
							from category in categoryGroup.DefaultIfEmpty()

							where m.ReceiverEmail == user.Email && (!categoryId.HasValue || m.CategoryId == categoryId.Value)
							select new MessageWithSenderInfoViewModel
							{
								Id = m.Id,
								Subject = m.Subject,
								MessageDetail = m.MessageDetail,
								SentAt = m.SentAt,
								SenderEmail = m.SenderEmail,
								SenderName = sender != null ? sender.Name : "Bilinmeyen",
								SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
								CategoryName = category != null ? category.Name : "Bilinmeyen Kategori"
							}).ToList();

			return View(messages);
		}

		public async Task<IActionResult> Sendbox(int? categoryId)
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			var messages = (from m in _context.Messages
							join u in _context.Users
							on m.ReceiverEmail equals u.Email into userGroup
							from receiver in userGroup.DefaultIfEmpty()

							join c in _context.Categories
							on m.CategoryId equals c.Id into categoryGroup
							from category in categoryGroup.DefaultIfEmpty()

							where m.SenderEmail == user.Email && (!categoryId.HasValue || m.CategoryId == categoryId.Value)
							select new MessageWithReceiverInfoViewModel
							{
								Id = m.Id,
								Subject = m.Subject,
								MessageDetail = m.MessageDetail,
								SentAt = m.SentAt,
								ReceiverEmail = m.SenderEmail,
								ReceiverName = receiver != null ? receiver.Name : "Bilinmeyen",
								ReceiverSurname = receiver != null ? receiver.Surname : "Kullanıcı",
								CategoryName = category != null ? category.Name : "Bilinmeyen Kategori"
							}).ToList();

			return View(messages);
		}

		public IActionResult Detail(int id)
		{
			var messages = _context.Messages.Where(x => x.Id == id).FirstOrDefault();
			return View(messages);
		}

		[HttpGet]
		public IActionResult Compose()
		{
			var categories = _context.Categories.ToList();
			ViewBag.c = categories.Select(c => new SelectListItem
			{
				Text = c.Name,
				Value = c.Id.ToString()
			}).ToList();

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Compose(Message message)
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			message.SenderEmail = user.Email;
			message.SentAt = DateTime.Now;
			message.IsRead = false;
			_context.Messages.Add(message);
			_context.SaveChanges();
			return RedirectToAction("Sendbox");
		}
	}
}
