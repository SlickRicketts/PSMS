using _2024PSMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Okta.AspNetCore;
using System.Security.Claims;

namespace _2024PSMS.Pages.Home;

public class InitializeModel : PageModel
{

    // IMPORTANT: The Okta.AspCore by Okta, Inc. NuGet package must
    // be installed in the project for Single Sign On to work.

    private readonly _2024PSMSContext _2024PSMSContext;
    public InitializeModel(_2024PSMSContext PSMS)
    {
        _2024PSMSContext = PSMS;
    }

    // In this page model class only, call the User object "MyUser"
    // because User is an object that is already being used by Okta.
    private User MyUser;

    private string strEmailAddress;
    private string strFirstName;
    private string strLastName;

    public async Task<IActionResult> OnGetAsync()
    {

        // Okta: If the user has not been authenticated,
        // display the LogIn page. Otherwise, continue.
       //if (!HttpContext.User.Identity.IsAuthenticated)
       // {
       //    return Challenge(OktaDefaults.MvcAuthenticationScheme);
       // }
       


        // Okta: Get the user's email address.
        //strEmailAddress = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(c => c.Type.Equals("preferred_username")).Value.ToString();
        // Okta: If needed for some reason, get the user's first name and last name.
        //strFirstName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(c => c.Type.Equals("given_name")).Value.ToString();
         //strLastName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(c => c.Type.Equals("family_name")).Value.ToString();

        // Hardcode the email address when bypassing the Okta login.
         strEmailAddress = "zachary.ricketts@franklincollege.edu";

        // Use the email address to look up the user in the application's database.
        MyUser = await _2024PSMSContext.User
           .Where(u => u.EmailAddress == strEmailAddress)
           .AsNoTracking()
           .FirstOrDefaultAsync();

        if (MyUser != null)
        {
            // The user was found in the application's database.
            // Save the user's information in session variables.
            HttpContext.Session.SetString("FirstName", MyUser.FirstName);
            HttpContext.Session.SetString("LastName", MyUser.LastName);
            HttpContext.Session.SetString("EmailAddress", MyUser.EmailAddress);
            HttpContext.Session.SetString("Status", MyUser.Status);
            HttpContext.Session.SetString("UserID",MyUser.UserID.ToString());
            // Authenticate the user.
            List<Claim> objClaimList = new List<Claim> { new Claim(ClaimTypes.Role, MyUser.Status) };
            ClaimsIdentity objClaimsIdentity = new ClaimsIdentity(objClaimList, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(objClaimsIdentity));
            // Redirect the user.
            return RedirectToAction("ViewWelcome", "Common");
        }
        else
        {
            // The user was not found in the application's database.
            // Redirect the user.
            return RedirectToAction("ViewProjects", "Projects");
        }

    }

}
