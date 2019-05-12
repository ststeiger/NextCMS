
// using Microsoft.AspNet.FileProviders;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage.Blob;


namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{
    public class AzureFileInfo : IFileInfo
    {
        private readonly CloudBlockBlob _blockBlob;

        public AzureFileInfo(IListBlobItem blob)
        {
            IsDirectory = (blob is CloudBlobDirectory);

            if (!IsDirectory)
            {
                _blockBlob = (CloudBlockBlob)blob;

                System.Threading.Tasks.Task<bool> task = System.Threading.Tasks.Task.Run<bool>(async () => await _blockBlob.ExistsAsync());
                Exists = task.Result;

                if (Exists)
                {
                    Length = _blockBlob.Properties.Length;
                    PhysicalPath = _blockBlob.Uri.ToString();
                    Name = _blockBlob.Name;
                    LastModified = _blockBlob.Properties.LastModified.HasValue ? _blockBlob.Properties.LastModified.Value : System.DateTimeOffset.MinValue;
                }
            }
        }

        public System.IO.Stream CreateReadStream()
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(async () => await _blockBlob.DownloadToStreamAsync(stream));
            stream.Position = 0;
            return stream;
        }

        public bool Exists { get; }
        public long Length { get; }
        public string PhysicalPath { get; }
        public string Name { get; }
        public System.DateTimeOffset LastModified { get; }
        public bool IsDirectory { get; }
    }


}
