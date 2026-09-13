using IdentityHub.Context;
using IdentityHub.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.ViewComponents.MessageViewComponents
{
	public class MessageSidebarViewComponent : ViewComponent
	{
		private readonly IdentityContext _context;
		private readonly UserManager<AppUser> _userManager;

		public MessageSidebarViewComponent(IdentityContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			ViewBag.sendMessageCount = _context.Messages.Count(m => m.IsRead == false && m.SenderEmail == user.Email);
			ViewBag.receiverMessageCount = _context.Messages.Count(m => m.IsRead == false && m.ReceiverEmail == user.Email);
			return View();
		}
	}
}
