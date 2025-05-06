using _2024PSMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Okta.AspNetCore;
using System.Globalization;
using System.Security.Claims;

namespace _2024PSMS.Pages.Home
{
    public class LandingModel : PageModel
    {
        public void OnGet()
        {
        }

        public RedirectResult OnPostInitialize()
        {
            // Check if the user is authenticated

            if (!HttpContext.User.Identity.IsAuthenticated)

            {
                // If not authenticated, redirect to login page

                return Redirect("/Home/Initialize");
            }

            // If authenticated, include authentication information in the redirect URL.

            var initializeUrl = Url.Page("Initialize");

            // Append any necessary query parameters to pass authentication information, if needed.

            return Redirect(initializeUrl);

        }
    }
}
