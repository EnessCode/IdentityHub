using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.ViewComponents.MessageViewComponents
{
	public class MessageSidebarViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
