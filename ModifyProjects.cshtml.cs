using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Projects
{
    [BindProperties]
    public class ModifyProjectsModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public ModifyProjectsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;

            
        }

        public Project Project { get; set; }
        public ProjectType ProjectType { get; set; }
        public Mentor Mentor { get; set; }

        public SelectList MentorSelectList;
        public SelectList ProjectTypeSelectList;

        public async Task<IActionResult> OnGetAsync(int intProjectID, int intUserID)
        {
            // Set the message.
            MessageColor = "Green";
            Message = "Please modify the information below and click Modify.";

            //Populate the project type select list.
            ProjectTypeSelectList = new SelectList(_2024PSMSContext.ProjectType
                .OrderBy(pt => pt.ProjectTypeID), "ProjectTypeID", "ProjectType1");

            //Populate the mentor name select list.
            //This should list mentors "Last Name, First Name"
            MentorSelectList = new SelectList(_2024PSMSContext.Mentor
                .OrderBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .Select(m => new
                {
                    MentorID = m.MentorID,
                    FullName = m.LastName + ", " + m.FirstName
                }), "MentorID", "FullName");

            // Attempt to retrieve the row from the table.
            Project = await _2024PSMSContext.Project.FindAsync(intProjectID);

            if (Project != null)
            {                
                return Page();
            }
            else
            {
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = "The selected project was deleted by someone else.";
                return Redirect("MaintainProjects");
            }

        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                Project.UserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                // Modify the row in the table.
                _2024PSMSContext.Project.Update(Project);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = Project.Title + " was successfully modified.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = Project.Title + " was NOT modified. Please report this message to...: " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainProjects");

        }
    }
}
