using System;
namespace getQuote.Models
{
	public class Login
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool KeepLoggedIn { get; set; }
	}
}

