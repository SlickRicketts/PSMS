using _2024PSMS.Models;
using _2024PSMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.SendNotification
{
    [BindProperties]
    public class SendNotificationModel : PageModel
    {
        private readonly IEmailService IEmailService;
        private readonly _2024PSMSContext _2024PSMSContext;

        public SendNotificationModel(IEmailService IES, _2024PSMSContext PSMS)
        {
            IEmailService = IES;
            _2024PSMSContext = PSMS;
        }
      
        private IQueryable<Result> ResultIQueryable;
        public IList<Result> ResultIList;

        public string MessageColor;
        public string Message;

        public SelectList ProjectSelectList;
 

        public string EmailAddress { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
        public Mentor Mentor { get; set; }
        public class Result
        {
            public int ProjectID;
            public int UserID;
            public int MentorID;
            public string? FirstName;
            public string? LastName;
            public string? EmailAddress;
            public string? Title;
            public string? Organization;
            public string? MentorFirstName;
            public string? MentorLastName;            
        }
        public void OnGet()
        {
            //Set the Message
            if (TempData["strMessage"] == null)
            {
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = "Please select each field below.";
            }
            else
            {
                MessageColor = TempData["strMessageColor"].ToString();
                Message = TempData["strMessage"].ToString();
            }

            //Populate the project title select list.
            ProjectSelectList = new SelectList(_2024PSMSContext.Project
                .OrderBy(p => p.Title), "ProjectID", "Title");
                  
           
        }
        public async Task OnPostSendEmailAsync()
        {         
            ResultIQueryable = (
                from p in _2024PSMSContext.Project
                join u in _2024PSMSContext.User on p.UserID equals u.UserID
                join m in _2024PSMSContext.Mentor on p.MentorID equals m.MentorID
                where p.ProjectID == Project.ProjectID              
                select new Result
                {
                    ProjectID = p.ProjectID,
                    UserID = u.UserID,
                    MentorID = m.MentorID,
                    FirstName = u.FirstName,
                    LastName= u.LastName,
                    EmailAddress = u.EmailAddress,
                    Title = p.Title,
                    Organization = m.Organization,
                    MentorFirstName = m.FirstName,
                    MentorLastName = m.LastName,
                });
           
            ResultIList = ResultIQueryable.ToList();
            
            foreach(var project in ResultIList)
            {
                // Configure the email message and send it.
                string strToName = project.FirstName + " " + project.LastName;
                string strToAddress = project.EmailAddress;
                string strSubject = "Project Interest";
                string strBody = "Dear " + strToName + "," +
                                 "<br /><br />" +
                                 project.MentorFirstName + " " + project.MentorLastName + " from " + project.Organization + " is interested in your project: " + project.Title + ". " +
                                 "Please email jvanandel@franklincollege.edu or kbixler@franklincollege.edu if you are interested in collaborating with this mentor.<br /><br />" +
                                 "Thank you,<br /><br />The Project Submission Management System";

                await IEmailService.SendEmail(strToName, strToAddress, strSubject, strBody);

                // Set the message.
                MessageColor = "Green";
                Message = "An email message was successfully sent to " + project.FirstName + " "+ project.LastName + "."; 

            }          
        }
    }
}
