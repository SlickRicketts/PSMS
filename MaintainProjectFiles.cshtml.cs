using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.ProjectFiles
{
    public class MaintainProjectFilesModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public MaintainProjectFilesModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public class Result
        {
            public int? UserID;
            public int? ProjectID;         
            public int? ProjectFileID;
            public string? FirstName;           
            public string? LastName;
            public string? ProjectTitle;          
            public string? ProjectFile;
        }

        private IQueryable<Result> ResultIQueryable;
        public IList<Result> ResultIList;
        public async Task OnGetAsync()
        {
            // Set the message.
            if (TempData["strMessage"] == null)
            {
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = "Please choose an option below.";
            }
            else
            {
                MessageColor = TempData["strMessageColor"].ToString();
                Message = TempData["strMessage"].ToString();
            }

            // Define the database query.
            ResultIQueryable = (
                from p in _2024PSMSContext.Project
                join u in _2024PSMSContext.User on p.UserID equals u.UserID
                join pf in _2024PSMSContext.ProjectFile on p.ProjectID equals pf.ProjectID
                orderby u.LastName, u.FirstName
                select new Result
                {
                    UserID = u.UserID,
                    ProjectID = p.ProjectID,
                    ProjectFileID = pf.ProjectFileID,
                    LastName = u.LastName,
                    FirstName = u.FirstName,                  
                    ProjectTitle = p.Title,                  
                    ProjectFile = pf.ProjectFile1,
                });
            // Retrieve the rows for display.
            ResultIList = await ResultIQueryable.ToListAsync();

            //Display Project Files uploaded by who is logged in. 
            if (HttpContext.Session.GetString("Status") == "S" || HttpContext.Session.GetString("Status") == "I")
            {
                // Define the database query.
                ResultIQueryable = (
                    from p in _2024PSMSContext.Project
                    join u in _2024PSMSContext.User on p.UserID equals u.UserID
                    join pf in _2024PSMSContext.ProjectFile on p.ProjectID equals pf.ProjectID
                    where u.UserID == Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                    orderby u.LastName, u.FirstName
                    select new Result
                    {
                        UserID = u.UserID,
                        ProjectID = p.ProjectID,
                        LastName = u.LastName,
                        FirstName = u.FirstName,
                        ProjectFileID = pf.ProjectFileID,
                        ProjectTitle = p.Title,
                        ProjectFile = pf.ProjectFile1,
                    });
                // Retrieve the rows for display.
                ResultIList = await ResultIQueryable.ToListAsync();
            }

        }
    }
}

