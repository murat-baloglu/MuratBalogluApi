using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MuratBaloglu.Application.Abstractions.Storage.Local;

namespace MuratBaloglu.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName) => File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, path, fileName));

        public List<string> GetFiles(string path)
        {
            //DirectoryInfo directoryInfo = new DirectoryInfo(path); Hata verince aşağıdaki gibi değiştirdim.
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, path));
            return directoryInfo.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName) => File.Exists($"{path}/{fileName}");

        async Task<bool> CopyFileAsync(string fullPathIncludeFileName, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new FileStream(fullPathIncludeFileName, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception e)
            {
                //Todo log!
                throw e;
            }
        }

        public async Task<List<(string fileName, string pathOrContainerNameIncludeFileName)>> UploadAsync(string path, IFormFileCollection files)
        {
            //path dosyaların kaydedileceği klasör ismi.(image-files isimdeki klasörün içine at dosyaları demek. Veya resources/blog-images gibi.)

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            //string fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", path);

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            var datas = new List<(string fileName, string pathIncludeFileName)>();

            foreach (IFormFile file in files)
            {
                string newFileName = RenameFileName(file.FileName);

                await CopyFileAsync($"{fullPath}/{newFileName}", file); //Local Storage a dosya kaydedilir. Yani uygulamanın çalıştığı host yada server a.

                datas.Add((newFileName, $"{path}/{newFileName}")); //Database e kaydetmek için geri döndürülen datalar.
            }

            return datas;
        }
    }
}
