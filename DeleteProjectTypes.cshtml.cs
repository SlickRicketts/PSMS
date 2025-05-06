using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.ProjectTypes
{
    public class DeleteProjectTypesModel : PageModel
    {
        private readonly _2024PSMSContext _2024PSMSContext;
        public DeleteProjectTypesModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        private ProjectType ProjectType { get; set; }

        public async Task<IActionResult> OnGetAsync(int intProjectTypeID)
        {

            // Look up the row in the table to see if it still exists.
            ProjectType = await _2024PSMSContext.ProjectType.FindAsync(intProjectTypeID);
            if (ProjectType != null)
            {
                try
                {
                    // Delete the row from the table.
                    _2024PSMSContext.ProjectType.Remove(ProjectType);
                    await _2024PSMSContext.SaveChangesAsync();
                    // Set the message.
                    TempData["strMessageColor"] = "Green";
                    TempData["strMessage"] = ProjectType.ProjectType1 + " was successfully deleted.";
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
                        TempData["strMessage"] = ProjectType.ProjectType1 + " was NOT deleted because it is associated with one or more On-Going Projects. To delete this project, you must first delete the On-Going project.";
                    }
                    else
                    {
                        // A database exception occurred while saving to
                        // the database.
                        // Set the message.
                        TempData["strMessageColor"] = "Red";
                        TempData["strMessage"] = ProjectType.ProjectType1 + " was NOT deleted. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
                    }
                }
            }
            else
            {
                // Even though someone else deleted the item first, still
                // inform the user that the item was deleted successfully.
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = " The Project Type was successfully deleted.";
            }
            return Redirect("MaintainProjectTypes");
        }
    }
}
