using Sprint_sol1.Models;
namespace Sprint_sol1.Services
{
        /// <summary>
        /// Interface for Azure Blob Storage operations.
        /// </summary>
        public interface IAzureStorage
        {
            /// <summary>
            /// Uploads a file submitted with the request.
            /// </summary>
            /// <param name="file">The file to upload.</param>
            /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="BlobResponseDto"/> with the status and details of the uploaded blob.</returns>
            Task<BlobResponseDto> UploadAsync(IFormFile file);
        Task<IEnumerable<BlobDto>> ListBlobsAsync();
    }
    }
