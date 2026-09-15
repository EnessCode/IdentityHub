using IdentityHub.Context;
using IdentityHub.Entities;
using IdentityHub.Models.Layout;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.ViewComponents.HeaderTopViewComponents
{
	public class HeaderTopViewComponent : ViewComponent
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IdentityContext _context;

		public HeaderTopViewComponent(UserManager<AppUser> userManager, IdentityContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = new HeaderTopViewModel();

			if (!User.Identity.IsAuthenticated)
			{
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user == null)
			{
				return View(model);
			}

			model.MessageCount = _context.Messages
				.Count(x => x.ReceiverEmail == user.Email && x.IsRead == false);

			model.NotificationCount = _context.Notifications
				.Count(x => x.Status == true);

			//model.TaskCount = 2;

			return View(model);
		}
	}
}
