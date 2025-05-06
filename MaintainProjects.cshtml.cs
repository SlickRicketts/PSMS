using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Projects
{
    public class MaintainProjectsModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public MaintainProjectsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }
        public class Result
        {          
            public int? UserID;
            public int? ProjectID;
            public int? ProjectTypeID;
            public int? MentorID;
            public string? FirstName;
            public string? MiddleInitial;
            public string? LastName;
            public string? ProjectTitle;
            public string? ProjectType;
            public string? Description;
            public DateTime? StartDate;
            public DateTime? EndDate;         
            public string? Status;
            public string? MentorLastName;
            public string MentorName;
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
                join pt in _2024PSMSContext.ProjectType on p.ProjectTypeID equals pt.ProjectTypeID
                join m in _2024PSMSContext.Mentor on p.MentorID equals m.MentorID
                orderby u.LastName, u.FirstName
                select new Result
                {
                    MentorID = m.MentorID,
                    ProjectID = p.ProjectID,
                    UserID = u.UserID,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    ProjectTitle = p.Title,
                    ProjectType = pt.ProjectType1,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                    MentorLastName = m.LastName,
                    MentorName = m.LastName+ ", "+  m.FirstName,
                });
            // Retrieve the rows for display.
            ResultIList = await ResultIQueryable.ToListAsync();

            //Display Projects that are submitted by who is logged in. 
            if (HttpContext.Session.GetString("Status") == "S" || HttpContext.Session.GetString("Status") == "I")
            {
                ResultIQueryable = (
                from p in _2024PSMSContext.Project
                join u in _2024PSMSContext.User on p.UserID equals u.UserID
                join pt in _2024PSMSContext.ProjectType on p.ProjectTypeID equals pt.ProjectTypeID
                join m in _2024PSMSContext.Mentor on p.MentorID equals m.MentorID
                where u.UserID == Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                orderby u.LastName, u.FirstName
                select new Result
                {
                    MentorID = m.MentorID,
                    ProjectID = p.ProjectID,
                    UserID = u.UserID,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    ProjectTitle = p.Title,
                    ProjectType = pt.ProjectType1,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                    MentorLastName = m.LastName,
                    MentorName = m.LastName + ", " + m.FirstName,
                });
                // Retrieve the rows for display.
                ResultIList = await ResultIQueryable.ToListAsync();
            }
        }
    }
}