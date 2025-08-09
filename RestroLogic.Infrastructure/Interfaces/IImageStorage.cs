namespace RestroLogic.Infrastructure.Interfaces
{
    public interface IImageStorage
    {
        Task<string> UploadAsync(Stream content, string contentType, string fileName, CancellationToken ct = default);
        Task DeleteAsync(string objectKey, CancellationToken ct = default);
    }
}
