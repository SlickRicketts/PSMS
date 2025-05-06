using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _2024PSMS.Pages.Users
{
    [BindProperties]
    public class AddUsersModel : PageModel
    {

        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public AddUsersModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }
     
        public SelectList StatusSelectList;

        public User User { get; set; }    
        public void OnGet()
        {
            // Set the message.
            MessageColor = "Green";
            Message = "Please add the information below and click Add.";
        }
        public async Task<IActionResult> OnPostAddAsync()
        {
            try
            {
                // Add the row to the table.
                _2024PSMSContext.User.Add(User);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = User.FirstName + " " + User.LastName + " was successfully added.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = User.FirstName + " " + User.LastName + " was NOT added. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainUsers");

        }

    }
}
