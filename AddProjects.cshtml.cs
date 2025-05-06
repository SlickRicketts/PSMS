using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Projects
{
    [BindProperties]
    public class AddProjectsModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public AddProjectsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public SelectList ProjectTypeSelectList;
        public SelectList MentorSelectList;

        public Project Project { get; set; }
        public int UserID { get; set; }

        public void OnGet()
        {

            // Set the message.
            MessageColor = "Green";
            Message = "Please add the information below and click Add.";

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
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            try
            {
               Project.UserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                // Add the row to the table.
                _2024PSMSContext.Project.Add(Project);
                await _2024PSMSContext.SaveChangesAsync();
                // Set the message.
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = Project.Title + " was successfully added.";
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = Project.Title + " was NOT added. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu" + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainProjects");

        }
    }
}
