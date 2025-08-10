namespace RestroLogic.Infrastructure.Storage
{
    public class AzureBlobOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = "product-images";
        // host público custom/CDN:
        public string? PublicBaseUrl { get; set; }
    }
}
