using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2024PSMS.Pages.ProjectTypes
{
    public class MaintainProjectTypesModel : PageModel
    {
        public string MessageColor;
        public string Message;

        private readonly _2024PSMSContext _2024PSMSContext;
        public MaintainProjectTypesModel(_2024PSMSContext PSMS)
        {
            _2024PSMSContext = PSMS;
        }
        public class Result
        {  
            public int? ProjectTypeID;
            public string? ProjectType;
        }

        private IQueryable<Result> ResultIQueryable;
        public IList<Result> ResultIList;

        public async Task OnGetAsync()
        {
            // Set the message.
            if (TempData["strMessage"] == null)
            {
                TempData["strMessageColor"] = "Green";
                TempData["strMessage"] = "Please Choose an option below.";
            }
            else
            {
                MessageColor = TempData["strMessageColor"].ToString();
                Message = TempData["strMessage"].ToString();
            }

            // Define the database query.
            ResultIQueryable = (
                from pt in _2024PSMSContext.ProjectType
                orderby pt.ProjectType1
                select new Result
                {
                    ProjectTypeID = pt.ProjectTypeID,
                    ProjectType = pt.ProjectType1,
                }); ;
            // Retrieve the rows for display.
            ResultIList = await ResultIQueryable.ToListAsync();

        }


    }
}
