using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.ProjectTypes
{
    [BindProperties]
    public class AddProjectTypeModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public AddProjectTypeModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public SelectList StatusSelectList;

        public ProjectType ProjectTypes { get; set; }

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
                _2024PSMSContext.ProjectType.Add(ProjectTypes);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = ProjectTypes.ProjectType1 + " was successfully added.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = ProjectTypes.ProjectType1 + " was NOT added. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainProjectTypes");
        }
    }
}
