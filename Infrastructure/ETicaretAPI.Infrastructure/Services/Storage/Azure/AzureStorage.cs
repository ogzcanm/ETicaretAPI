using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient;//ilgili azure storage accountuna bağlanmayı sağlar
        BlobContainerClient _blobContainerClient;
        //accountda ki hedef container üzerinde dosya işlemi yapmamızı sağlar
        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);
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

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _blobContainerClient =  _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();//ilgili container var mı? yoksa oluştur
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);//blob containera erişim izni
            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewNAme = await FileRenameAsync(containerName, HasFile, file.Name);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewNAme);

                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewNAme, containerName));
            }
            return datas;
        }
    }
}
