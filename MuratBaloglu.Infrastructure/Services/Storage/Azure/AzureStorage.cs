using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MuratBaloglu.Application.Abstractions.Storage.Azure;

namespace MuratBaloglu.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient? _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["Storage:Azure"]);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerNameIncludeFileName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            //containerName dosyaların kaydedileceği klasör ismi/container name. (image-files isimdeki klasörün içine at dosyaları demek.) Azure da Data stroge da bu isimde bir container oluşturur. 

            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            var datas = new List<(string fileName, string containerNameIncludeFileName)>();

            foreach (IFormFile file in files)
            {
                string newFileName = RenameFileName(file.FileName);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName); //blobName, file name demektir.
                await blobClient.UploadAsync(file.OpenReadStream()); //Azure Storage da verilen container adının içine dosya/dosyalar kaydedilir.

                datas.Add((newFileName, $"{containerName}/{newFileName}")); //Database e kaydetmek için geri döndürülen datalar.
            }

            return datas;
        }
    }
}
