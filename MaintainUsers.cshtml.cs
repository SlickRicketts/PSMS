using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.ExceptionServices;

namespace _2024PSMS.Pages.Users
{
    public class MaintainUsersModel : PageModel
    {

        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public MaintainUsersModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }

        public class Result
        {
            public int? UserID;
            public string? FirstName;
            public string? MiddleInitial;
            public string? LastName;
            public string? EmailAddress;
            public string? PhoneNumber;
            public string? Status;
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
                from u in _2024PSMSContext.User
                orderby u.LastName, u.FirstName
                select new Result
                {
                    UserID = u.UserID,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    MiddleInitial = u.MiddleInitial,
                    EmailAddress = u.EmailAddress,
                    PhoneNumber = u.PhoneNumber,
                    Status = u.Status,
                });
            // Retrieve the rows for display.
            ResultIList = await ResultIQueryable.ToListAsync();
        }
    }
}
