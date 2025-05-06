using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2024PSMS.Pages.Mentors
{
    public class CancelMentorsModel : PageModel
    {
        public RedirectResult OnGet()
        {
            // Set the message.
            TempData["strMessageColor"] = "Red";
            TempData["strMessage"] = "The operation was cancelled. No data was affected.";
            return Redirect("MaintainMentors");
        }
    }
}
