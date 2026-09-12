using Microsoft.AspNetCore.Mvc;
using IdentityHub.Context;

namespace IdentityHub.Controllers
{
	public class MessageController : Controller
	{
		private readonly IdentityContext _context;

		public MessageController(IdentityContext context)
		{
			_context = context;
		}

		public IActionResult Inbox()
		{
			var messages = _context.Messages.Where(x => x.ReceiverEmail == "ali@gmail.com").ToList();
			return View(messages);
		}
		public IActionResult Sendbox()
		{
			var messages = _context.Messages.Where(x => x.SenderEmail == "ali@gmail.com").ToList();
			return View(messages);
		}

		public IActionResult MessageDetail()
		{
			var messages = _context.Messages.Where(x => x.Id == 1).FirstOrDefault();
			return View(messages);
		}

		[HttpGet]
		public IActionResult ComposeMessage()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ComposeMessage(int id)
		{
			return View();
		}
	}
}
