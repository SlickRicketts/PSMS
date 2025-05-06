using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2024PSMS.Pages.Common;

public class LogOutModel : PageModel
{

    public async Task<IActionResult> OnGetAsync()
    {

        // Log out the user.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Clear the session.
        HttpContext.Session.Clear();

        // Redirect the user to GrizLink.
        return Redirect("https://grizlink.franklincollege.edu");

    }

}
