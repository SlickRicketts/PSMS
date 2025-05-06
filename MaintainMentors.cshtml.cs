using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.Mentors
{
    public class MaintainMentorsModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public MaintainMentorsModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public class Result
        {
            public int? MentorID;
            public string? FirstName;          
            public string? LastName;
            public string? EmailAddress;
            public string? PhoneNumber;
            public string? Organization;
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
                from m in _2024PSMSContext.Mentor
                orderby m.LastName, m.FirstName
                select new Result
                {
                    MentorID = m.MentorID,
                    LastName = m.LastName,
                    FirstName = m.FirstName,                    
                    EmailAddress = m.EmailAddress,
                    PhoneNumber = m.PhoneNumber,
                    Organization = m.Organization,
                });
            // Retrieve the rows for display.
            ResultIList = await ResultIQueryable.ToListAsync();

        }
    }
}

