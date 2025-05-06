using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.ProjectTypes
{
    [BindProperties]
    public class ModifyProjectTypesModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public ModifyProjectTypesModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public SelectList CategorySelectList;
        public SelectList SupplierSelectList;
        public ProjectType ProjectType { get; set; }

        public async Task<IActionResult> OnGetAsync(int intProjectTypeID)
        {

            // Set the message.
            MessageColor = "Green";
            Message = "Please modify the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            ProjectType = await _2024PSMSContext.ProjectType.FindAsync(intProjectTypeID);
            if (ProjectType != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = "The selected project type was deleted by someone else.";
                return Redirect("MaintainProjectTypes");
            }

        }
        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                _2024PSMSContext.ProjectType.Update(ProjectType);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = ProjectType.ProjectType1 + " was successfully modified.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = ProjectType.ProjectType1 + " was NOT modified. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainProjectTypes");

        }
    }
}
