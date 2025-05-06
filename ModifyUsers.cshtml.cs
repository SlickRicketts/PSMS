using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Users
{
    [BindProperties]
    public class ModifyUsersModel : PageModel
    {

        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public ModifyUsersModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }
      
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int intUserID)
        {
            // Set the message.
            MessageColor = "Green";
            Message = "Please modify the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            User = await _2024PSMSContext.User.FindAsync(intUserID);
            if (User != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = "The selected user was deleted by someone else.";
                return Redirect("MaintainUsers");
            }
        }
        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                _2024PSMSContext.User.Update(User);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = User.FirstName + " " + User.LastName + " was successfully modified.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = User.FirstName + " " + User.LastName + " was NOT modified. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainUsers");

        }
    }
}
