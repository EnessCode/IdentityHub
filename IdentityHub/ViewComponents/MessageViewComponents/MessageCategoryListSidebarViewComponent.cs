using Microsoft.AspNetCore.Mvc;
using IdentityHub.Context;

namespace IdentityHub.ViewComponents.MessageViewComponents
{
	public class MessageCategoryListSidebarViewComponent : ViewComponent
	{
		private readonly IdentityContext _context;

		public MessageCategoryListSidebarViewComponent(IdentityContext context)
		{
			_context = context;
		}

		public IViewComponentResult Invoke()
		{
			var categories = _context.Categories.ToList();
			return View(categories);
		}
	}
}
