using Microsoft.AspNetCore.Http;

namespace MuratBaloglu.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerNameIncludeFileName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);

        Task DeleteAsync(string pathOrContainerName, string fileName);

        List<string> GetFiles(string pathOrContainerName);

        bool HasFile(string pathOrContainerName, string fileName);
    }
}
