using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Sprint_sol1.Models;
using Sprint_sol1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sprint_sol1.Repository
{
    public class AzureStorage : IAzureStorage
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureStorage> _logger;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("BlobContainerName");
            _logger = logger;
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile file)
        {
            var response = new BlobResponseDto();
            var container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            try
            {
                await container.CreateIfNotExistsAsync();
                var client = container.GetBlobClient(file.FileName);

                await using (Stream data = file.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }

                response.Status = $"File {file.FileName} uploaded successfully";
                response.Error = false;
                response.Blob = new BlobDto
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name
                };
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {file.FileName} already exists in container.");
                response.Status = $"File with name {file.FileName} already exists. Please use another name.";
                response.Error = true;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError($"Unhandled Exception: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
            }

            return response;
        }

        public async Task<IEnumerable<BlobDto>> ListBlobsAsync()
        {
            var container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            var blobs = new List<BlobDto>();

            await foreach (var blobItem in container.GetBlobsAsync())
            {
                var blobClient = container.GetBlobClient(blobItem.Name);
                blobs.Add(new BlobDto
                {
                    Name = blobItem.Name,
                    Uri = blobClient.Uri.AbsoluteUri
                });
            }

            return blobs;
        }
    }
}
