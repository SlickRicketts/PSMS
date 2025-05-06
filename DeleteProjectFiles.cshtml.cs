using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.ProjectFiles
{
    [BindProperties]
    public class DeleteProjectFilesModel : PageModel
    {
        private readonly _2024PSMSContext _2024PSMSContext;
        private readonly IWebHostEnvironment IWebHostEnvironment;
        public string strDateTime = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
        public DeleteProjectFilesModel(_2024PSMSContext PSMS, IWebHostEnvironment IWHE)
        {
            _2024PSMSContext = PSMS;
            IWebHostEnvironment = IWHE;
        }
       
        private ProjectFile ProjectFile { get; set; }
        public IFormFile PFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int intProjectFileID)
        {

            // Look up the row in the table to see if it still exists.
            ProjectFile = await _2024PSMSContext.ProjectFile.FindAsync(intProjectFileID);
            if (ProjectFile != null)
            {
                try
                {

                    string strProjectPath = Path.Combine(IWebHostEnvironment.WebRootPath, "ProjectFiles");
                    string strFilePath = Path.Combine(strProjectPath, ProjectFile.ProjectFile1);
                    System.IO.File.Delete(strFilePath);

                    // Delete the row from the table.
                    _2024PSMSContext.ProjectFile.Remove(ProjectFile);
                        await _2024PSMSContext.SaveChangesAsync();
                                                   
                                          
                    // Set the message.
                    TempData["strMessageColor"] = "Green";
                    TempData["strMessage"] = ProjectFile.ProjectFile1 + " was successfully deleted.";
                 
                                      
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
                        TempData["strMessage"] = ProjectFile.ProjectFile1 + " was NOT deleted because it is associated with an On-Going Project . To delete this project file, you must first delete the associated project.";
                    }
                    else
                    {
                        // A database exception occurred while saving to
                        // the database.
                        // Set the message.
                        TempData["strMessageColor"] = "Red";
                        TempData["strMessage"] = ProjectFile.ProjectFile1 + " was NOT deleted. Please report this message to Dr. Beasley: rbealsey@franklincollege.edu " + objDbUpdateException.InnerException.Message;
                    }
                }
                
            }
            else
            {
                // Even though someone else deleted the item first, still
                // inform the user that the item was deleted successfully.
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = "The project file was successfully deleted.";
            }
            return Redirect("MaintainProjectFiles");

        }
    }
}
