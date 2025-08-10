using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using RestroLogic.Infrastructure.Interfaces;

namespace RestroLogic.Infrastructure.Storage
{
    public class AzureBlobImageStorage : IImageStorage
    {
        private readonly BlobContainerClient _container;
        private readonly AzureBlobOptions _options;

        public AzureBlobImageStorage(IOptions<AzureBlobOptions> options)
        {
            _options = options.Value;
            var service = new BlobServiceClient(_options.ConnectionString);
            _container = service.GetBlobContainerClient(_options.ContainerName);
            _container.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadAsync(Stream content, string contentType, string fileName, CancellationToken ct = default)
        {
            var blob = _container.GetBlobClient(fileName);
            var headers = new BlobHttpHeaders { ContentType = contentType };

            await blob.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers }, ct);

            // URL final
            return _options.PublicBaseUrl is { Length: > 0 }
                ? $"{_options.PublicBaseUrl.TrimEnd('/')}/{_options.ContainerName}/{fileName}"
                : blob.Uri.ToString();
        }

        public async Task DeleteAsync(string objectKey, CancellationToken ct = default)
        {
            var blob = _container.GetBlobClient(objectKey);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, conditions: null, cancellationToken: ct);
        }
    }
}
