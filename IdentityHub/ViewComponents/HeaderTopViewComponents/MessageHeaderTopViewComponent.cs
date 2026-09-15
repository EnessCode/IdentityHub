using IdentityHub.Context;
using IdentityHub.Entities;
using IdentityHub.Models.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.ViewComponents.HeaderTopViewComponents
{
	public class MessageHeaderTopViewComponent : ViewComponent
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IdentityContext _context;

		public MessageHeaderTopViewComponent(UserManager<AppUser> userManager, IdentityContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return View(new List<MessageWithUserInfoViewModel>());
			}

			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user == null)
			{
				return View(new List<MessageWithUserInfoViewModel>());
			}

			var messages = (from message in _context.Messages
							join senderUser in _context.Users
							on message.SenderEmail equals senderUser.Email
							where message.ReceiverEmail == user.Email && message.IsRead == false
							select new MessageWithUserInfoViewModel
							{
								FulName = senderUser.Name + " " + senderUser.Surname,
								ImageUrl = senderUser.ImageUrl,
								Subject = message.Subject,
								MessageDetail = message.MessageDetail,
								SentAt = message.SentAt
							}).ToList();

			return View(messages);
		}
	}
}
