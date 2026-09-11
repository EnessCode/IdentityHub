using Microsoft.AspNetCore.Identity;

namespace IdentityHub.Models.Validator
{
	public class CustomIdentityValidator : IdentityErrorDescriber
	{
		public override IdentityError PasswordTooShort(int length)
		{
			return new IdentityError
			{
				Code = nameof(PasswordTooShort),
				Description = $"Şifre en az {length} karakter olmalıdır."
			};
		}

		public override IdentityError PasswordRequiresLower()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresLower),
				Description = "Şifre en az 1 tane küçük harf içermelidir."
			};
		}

		public override IdentityError PasswordRequiresUpper()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresUpper),
				Description = "Şifre en az 1 tane büyük harf içermelidir."
			};
		}

		public override IdentityError PasswordRequiresDigit()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresDigit),
				Description = "Şifre en az 1 tane rakam içermelidir."
			};
		}

		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresNonAlphanumeric),
				Description = "Şifre en az 1 tane sembol içermelidir."
			};
		}

		public override IdentityError DuplicateUserName(string userName)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateUserName),
				Description = $"{userName} adlı kullanıcı adı zaten alınmış, farklı bir kullanıcı adı deneyiniz."
			};
		}
	}
}

