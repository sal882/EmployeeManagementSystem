using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MVCApplicationTestPL.Helpers
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            //Get Folder Path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            //Create File Name and make it unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            //Get File Path
            string filePath = Path.Combine(folderPath, fileName);

            //Craete and Save the file using FileStream Class
            using var fs = new FileStream(filePath, FileMode.Create);

            //Copy the file stream to 
            await file.CopyToAsync(fs);

            return fileName;
        }


        public static void DeleteFiel(string folderName, string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", 
                folderName, fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
