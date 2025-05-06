using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Mentors
{
    [BindProperties]
    public class ModifyMentorsModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public ModifyMentorsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public SelectList CategorySelectList;
        public SelectList SupplierSelectList;

        public Mentor Mentor { get; set; }

        public async Task<IActionResult> OnGetAsync(int intMentorID)
        {
            // Set the message.
            MessageColor = "Green";
            Message = "Please modify the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            Mentor = await _2024PSMSContext.Mentor.FindAsync(intMentorID);
            if (User != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = "The selected mentor was deleted by someone else.";
                return Redirect("MaintainMentors");
            }
        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                _2024PSMSContext.Mentor.Update(Mentor);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = Mentor.FirstName + " " + Mentor.LastName + " was successfully modified.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = Mentor.FirstName + " " + Mentor.LastName + " was NOT modified. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainMentors");

        }
    }
}
