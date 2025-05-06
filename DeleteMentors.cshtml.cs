using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Mentors
{
    public class DeleteMentorsModel : PageModel
    {
        private readonly _2024PSMSContext _2024PSMSContext;
        public DeleteMentorsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        private Mentor Mentor { get; set; }

        public async Task<IActionResult> OnGetAsync(int intMentorID)
        {
            // Look up the row in the table to see if it still exists.
            Mentor = await _2024PSMSContext.Mentor.FindAsync(intMentorID);
            if (Mentor != null)
            {
                try
                {
                    // Delete the row from the table.
                    _2024PSMSContext.Mentor.Remove(Mentor);
                    await _2024PSMSContext.SaveChangesAsync();
                    // Set the message.
                    TempData["strMessageColor"] = "Green";
                    TempData["strMessage"] = Mentor.FirstName + " " + Mentor.LastName + " was successfully deleted.";
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // A database exception occurred.
                    SqlException objSqlException = objDbUpdateException.InnerException as SqlException;
                    if (objSqlException.Number == 547)
                    {
                        // A foreign key constraint database exception
                        // occurred.
                        // Set the message.
                        TempData["strMessageColor"] = "Red";
                        TempData["strMessage"] = Mentor.FirstName + " " + Mentor.LastName + " was NOT deleted because it is associated with a project. To delete this mentor, you must first remove it from an associated project.";
                    }
                    else
                    {
                        // A database exception occurred while saving to
                        // the database.
                        // Set the message.
                        TempData["strMessageColor"] = "Red";
                        TempData["strMessage"] = Mentor.FirstName + " " + Mentor.LastName + " was NOT deleted. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
                    }
                }
            }
            else
            {
                // Even though someone else deleted the item first, still
                // inform the user that the item was deleted successfully.
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = "The mentor was successfully deleted.";
            }
            return Redirect("MaintainMentors");

        }
    }
}

