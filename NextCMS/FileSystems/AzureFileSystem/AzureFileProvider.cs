
// using Microsoft.AspNet.FileProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{
    public class AzureFileProvider : IFileProvider
    {
        private readonly CloudBlobContainer _container;

        public AzureFileProvider(Microsoft.Extensions.Configuration.IConfiguration configurationRoot)
        {
            string connectionString = configurationRoot.GetValue<string>("Azure:StorageConnectionString");
            string containerName = configurationRoot.GetValue<string>("Azure:BlobContainerName");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            string azurePath = ConvertPath(subpath);
            CloudBlockBlob blob = _container.GetBlockBlobReference(azurePath);
            return new AzureFileInfo(blob);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            string azurePath = ConvertPath(subpath);
            CloudBlobDirectory blob = _container.GetDirectoryReference(azurePath);
            return new AzureDirectoryContents(blob);
        }

        public Microsoft.Extensions.Primitives.IChangeToken Watch(string filter)
        {
            return new NoWatchChangeToken();
        }

        private string ConvertPath(string path)
        {
            if (path.StartsWith("/Views/", System.StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(7);
            }
            if (path.StartsWith("Views/", System.StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(6);
            }
            if (path.StartsWith("/", System.StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(1);
            }
            return path;
        }
    }
}
