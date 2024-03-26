using Microsoft.AspNetCore.Http;
using MuratBaloglu.Application.Abstractions.Storage;

namespace MuratBaloglu.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName => _storage.GetType().Name;

        public async Task DeleteAsync(string pathOrContainerName, string fileName) => await _storage.DeleteAsync(pathOrContainerName, fileName);

        public List<string> GetFiles(string pathOrContainerName) => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName) => _storage.HasFile(pathOrContainerName, fileName);

        public Task<List<(string fileName, string pathOrContainerNameIncludeFileName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            //pathOrContainerName dosyaların kaydedileceği klasör ismi.(image-files isimdeki klasörün içine at dosyaları demek. Veya resources/blog-images gibi.)
            return _storage.UploadAsync(pathOrContainerName, files);
        }
    }
}
