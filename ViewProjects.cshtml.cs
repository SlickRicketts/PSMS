using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Projects
{
    public class ViewProjectsModel : PageModel
    {

        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public ViewProjectsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public class Result
        {
            public int? UserID;
            public int? ProjectID;
            public int? ProjectTypeID;
            public string? FirstName;
            public string? MiddleInitial;
            public string? LastName;
            public string? ProjectTitle;
            public string? ProjectType;
            public string? Description;
            public DateTime? StartDate;
            public DateTime? EndDate;
            public string? Status;
        }

        private IQueryable<Result> ResultIQueryable;
        public IList<Result> ResultIList;

        public async Task OnGetAsync()
        {

            // Set the message.           
                MessageColor = "Green";
                Message = "If interested in an ''Open'' project, please email jvanandel@franklincollege.edu or kbixler@franklincollege.edu. Please provide the project author and title.";
                      

            // Define the database query.
            ResultIQueryable = (
                from p in _2024PSMSContext.Project
                join u in _2024PSMSContext.User on p.UserID equals u.UserID                
                join pt in _2024PSMSContext.ProjectType on p.ProjectTypeID equals pt.ProjectTypeID
                orderby u.LastName, u.FirstName
                select new Result
                {
                    UserID = u.UserID,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    ProjectTitle = p.Title,
                    ProjectType = pt.ProjectType1,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                });
            // Retrieve the rows for display.
            ResultIList = await ResultIQueryable.ToListAsync();
        }
    }
}
