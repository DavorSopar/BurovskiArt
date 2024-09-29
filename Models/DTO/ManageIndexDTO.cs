using System;
using Microsoft.AspNetCore.Identity;

namespace ArtistPortfolio.Models.DTO
{
	public class ManageIndexDTO
	{
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo>? Logins { get; set; }

        public string? PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public string? AuthenticatorKey { get; set; }
    }
}

