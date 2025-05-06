using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.ProjectFiles
{
    [BindProperties]
    public class ModifyProjectFilesModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public ModifyProjectFilesModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public SelectList ProjectSelectList;
        public SelectList SupplierSelectList;

        public ProjectFile ProjectFile { get; set; }
        public Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int intProjectFileID)
        {

            // Set the message.
            MessageColor = "Green";
            Message = "Please modify the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            ProjectFile = await _2024PSMSContext.ProjectFile.FindAsync(intProjectFileID);
            if (User != null)
            {
                //Populate the project title select list.
                ProjectSelectList = new SelectList(_2024PSMSContext.Project
                .OrderBy(p => p.Title), "ProjectID", "Title");
                return Page();
            }
            else
            {
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = "The selected project file was deleted by someone else.";
                return Redirect("MaintainProjectFiles");
            }

        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                _2024PSMSContext.ProjectFile.Update(ProjectFile);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = ProjectFile.ProjectFile1 + " was successfully modified.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = ProjectFile.ProjectFile1 + " was NOT modified. Please report this message to...: " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainProjectFiles");
        }
    }
}
