using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Users
{
    public class DeleteUsersModel : PageModel
    {
        private readonly _2024PSMSContext _2024PSMSContext;
        public DeleteUsersModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        private User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int intUserID)
        {

            // Look up the row in the table to see if it still exists.
            User = await _2024PSMSContext.User.FindAsync(intUserID);
            if (User != null)
            {
                try
                {
                    // Delete the row from the table.
                    _2024PSMSContext.User.Remove(User);
                    await _2024PSMSContext.SaveChangesAsync();
                    // Set the message.
                    TempData["strMessageColor"] = "Green";
                    TempData["strMessage"] = User.FirstName + " " + User.LastName + " was successfully deleted.";
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
                        TempData["strMessage"] = User.FirstName + " " + User.LastName + " was NOT deleted because it is associated with one or more On-Going projects. To delete this User, you must first delete the On-Going Projects.";
                    }
                    else
                    {
                        // A database exception occurred while saving to
                        // the database.
                        // Set the message.
                        TempData["strMessageColor"] = "Red";
                        TempData["strMessage"] = User.FirstName + " " + User.LastName + " was NOT deleted. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
                    }
                }
            }
            else
            {
                // Even though someone else deleted the item first, still
                // inform the user that the item was deleted successfully.
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = "The User was successfully deleted.";
            }
            return Redirect("MaintainUsers");

        }
    }
}
