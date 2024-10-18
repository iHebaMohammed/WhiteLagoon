namespace Demo.Web.Helpers
{
    public static class DocumentSettings
    {
        public async static Task<string> UploadFile(IFormFile file , string folderName)
        {
            ///Steps Of Uploading file
            ///1. Get located foulder parh
            ///2. Get file name and make it uniqe
            ///3. Get file path
            ///4. Save file as stream ==> (Stream) is a Date Per Time
            
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);
            using var fs = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fs);
            return Path.Combine("\\images" , folderName, fileName);
        }

        public static void DeleteFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName.TrimStart('\\'));
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
