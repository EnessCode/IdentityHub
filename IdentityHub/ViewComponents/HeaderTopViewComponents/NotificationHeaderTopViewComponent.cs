using IdentityHub.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityHub.ViewComponents.HeaderTopViewComponents
{
	public class NotificationHeaderTopViewComponent : ViewComponent
	{
		private readonly IdentityContext _context;

		public NotificationHeaderTopViewComponent(IdentityContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var notifications = await _context.Notifications
				.OrderByDescending(n => n.SendDate)
				.Take(5)
				.ToListAsync();
			return View(notifications);
		}
	}
}
