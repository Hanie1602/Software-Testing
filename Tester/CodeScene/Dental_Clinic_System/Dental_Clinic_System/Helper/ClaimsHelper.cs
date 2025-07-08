using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Dental_Clinic_System.Helper
{
	public static class ClaimsHelper
	{
		public static async Task AddNewClaimsAsync(HttpContext httpContext, List<Claim> newClaims)
		{
			// Ensure Name claim is present
			var emailClaim = newClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			newClaims.EnsureNameClaim(ClaimTypes.Name, emailClaim);

			// Create new ClaimsIdentity with the claims
			var claimsIdentity = new ClaimsIdentity(newClaims, CookieAuthenticationDefaults.AuthenticationScheme);
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
		}

		public static async Task UpdateClaimsAsync(HttpContext httpContext, List<Claim> updatedClaims)
		{
			// Get the current ClaimsIdentity
			var claimsIdentity = (ClaimsIdentity)httpContext.User.Identity;

			// Remove existing claims that need to be updated
			var claimsToRemove = updatedClaims.Select(c => c.Type).ToList();
			var existingClaims = claimsIdentity.Claims.Where(c => claimsToRemove.Contains(c.Type)).ToList();

			foreach (var claim in existingClaims)
			{
				claimsIdentity.RemoveClaim(claim);
			}

			// Add or update the new claims
			foreach (var claim in updatedClaims)
			{
				claimsIdentity.AddClaim(claim);
			}

			// Ensure Name claim is present
			var emailClaim = updatedClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			claimsIdentity.EnsureNameClaim(ClaimTypes.Name, emailClaim);

			await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
		}

		public static void AddClaimIfNotNull(this List<Claim> claims, string claimType, string claimValue)
		{
			if (!string.IsNullOrEmpty(claimValue))
			{
				claims.Add(new Claim(claimType, claimValue));
			}
		}

		public static void AddOrUpdateClaim(this List<Claim> claims, string claimType, string claimValue)
		{
            //if (!string.IsNullOrEmpty(claimValue))
            //{
            //	var existingClaim = claims.FirstOrDefault(c => c.Type == claimType);
            //	if (existingClaim != null)
            //	{
            //		// Update existing claim by removing the old one
            //		claims.Remove(existingClaim);
            //	}
            //	// Add new claim
            //	claims.Add(new Claim(claimType, claimValue));
            //}

            var existingClaim = claims.FirstOrDefault(c => c.Type == claimType);
            if (existingClaim != null)
            {
                // Update existing claim by removing the old one
                claims.Remove(existingClaim);
            }
            // Add new claim
            claims.Add(new Claim(claimType, claimValue ?? string.Empty));
        }

		public static void AddOrUpdateClaimForLinkWithGoogle(this List<Claim> claims, string claimType, string claimValue)
		{
			var existingClaim = claims.FirstOrDefault(c => c.Type == claimType);
			if (existingClaim != null)
			{
                // Update existing claim by removing the old one
                claims.Remove(existingClaim);
			}
			// Add new claim
			claims.Add(new Claim(claimType, claimValue ?? string.Empty));
		}

		public static void EnsureNameClaim(this List<Claim> claims, string nameClaimType, string nameClaimValue)
		{
			var nameClaim = claims.FirstOrDefault(c => c.Type == nameClaimType);
			if (nameClaim == null && !string.IsNullOrEmpty(nameClaimValue))
			{
				claims.Add(new Claim(nameClaimType, nameClaimValue));
			}
		}

		public static void EnsureNameClaim(this ClaimsIdentity claimsIdentity, string nameClaimType, string nameClaimValue)
		{
			var nameClaim = claimsIdentity.FindFirst(nameClaimType);
			if (nameClaim == null && !string.IsNullOrEmpty(nameClaimValue))
			{
				claimsIdentity.AddClaim(new Claim(nameClaimType, nameClaimValue));
			}
		}

		public static List<Claim> GetCurrentClaims(ClaimsPrincipal user)
		{
			return user.Claims.ToList();
		}
	}
}
