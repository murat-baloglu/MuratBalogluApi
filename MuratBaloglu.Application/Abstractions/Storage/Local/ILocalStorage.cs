using Microsoft.AspNetCore.Http;

namespace MuratBaloglu.Application.Abstractions.Storage.Local
{
    public interface ILocalStorage : IStorage
    {
        //public Task<bool> CopyFileAsync(string fullPath, IFormFile file);
    }
}
