using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Projects
{
    public class DeleteProjectsModel : PageModel
    {
        private readonly _2024PSMSContext _2024PSMSContext;
        public DeleteProjectsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        private Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int intProjectID)
        {

            // Look up the row in the table to see if it still exists.
            Project = await _2024PSMSContext.Project.FindAsync(intProjectID);
            if (Project != null)
            {
                try
                {
                    // Delete the row from the table.
                    _2024PSMSContext.Project.Remove(Project);
                    await _2024PSMSContext.SaveChangesAsync();
                    // Set the message.
                    TempData["strMessageColor"] = "Green";
                    TempData["strMessage"] = Project.Title + " was successfully deleted.";
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
                        TempData["strMessage"] = Project.Title + " was NOT deleted because it is currently On-Going. To delete this project, you must set the project status to Open or Closed.";
                    }
                    else
                    {
                        // A database exception occurred while saving to
                        // the database.
                        // Set the message.
                        TempData["strMessageColor"] = "Red";
                        TempData["strMessage"] = Project.Title + " was NOT deleted. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
                    }
                }
            }
            else
            {
                // Even though someone else deleted the item first, still
                // inform the user that the item was deleted successfully.
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = Project.Title + " was successfully deleted.";
            }
            return Redirect("MaintainProjects");
        }
    }
}
