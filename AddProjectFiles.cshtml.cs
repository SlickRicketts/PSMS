
using _2024PSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System;
using System.IO;

namespace _2024PSMS.Pages.ProjectFiles
{
    [BindProperties]
    public class AddProjectFilesModel : PageModel
    {
        public string MessageColor;
        public string Message;
        public string strDateTime = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");

        private readonly _2024PSMSContext _2024PSMSContext;
        private readonly IWebHostEnvironment IWebHostEnvironment;
        public AddProjectFilesModel(_2024PSMSContext PSMS, IWebHostEnvironment IWHE)
        {
            _2024PSMSContext = PSMS;
            IWebHostEnvironment = IWHE;

        }        
        public SelectList ProjectSelectList;

        public ProjectFile ProjectFile { get; set; }
        public Project Project { get; set; }

        public IFormFile ProjectFile1 { get; set; }

        public void OnGet()
        {
            // Set the message.
            MessageColor = "Green";
            Message = "Please add the information below and click Add.";

            //Populate the select list.
            ProjectSelectList = new SelectList(_2024PSMSContext.Project
                .OrderBy(p => p.Title), "ProjectID", "Title");
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            try
            {                   
                    string strFileName = strDateTime + " " + ProjectFile1.FileName;
                    string strProjectPath = Path.Combine(IWebHostEnvironment.WebRootPath, "ProjectFiles");
                    string strFilePath = Path.Combine(strProjectPath, strFileName);
                    FileStream objFileStream = new FileStream(strFilePath, FileMode.Create);
                    ProjectFile1.CopyTo(objFileStream);
                    objFileStream.Close();

                
                ProjectFile.ProjectFile1 = strFileName;
                    

                // Add the row to the table.
                _2024PSMSContext.ProjectFile.Add(ProjectFile);
                    await _2024PSMSContext.SaveChangesAsync();

                    // Set the message.
                    TempData["strMessageColor"] = "Green";
                    TempData["strMessage"] = ProjectFile.ProjectFile1 + " was successfully uploaded.";              

            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception occurred while saving to the
                // database.
                // Set the message.
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = ProjectFile.ProjectFile1 + " was NOT added. Please report this message to Dr. Beasley: rbeasley@franklincollege.edu " + objDbUpdateException.InnerException.Message;
            }
            return Redirect("MaintainProjectFiles");

        }

    }
}

