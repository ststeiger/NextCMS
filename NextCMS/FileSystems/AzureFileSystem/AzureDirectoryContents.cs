
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage.Blob;


namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{
    public class AzureDirectoryContents 
        : IDirectoryContents
    {
        private readonly CloudBlobDirectory _blob;
        private readonly BlobResultSegment _directoryContent;

        public AzureDirectoryContents(CloudBlobDirectory blob)
        {
            _blob = blob;

            System.Threading.Tasks.Task<BlobResultSegment> task = System.Threading.Tasks.Task.Run<BlobResultSegment>(async () => await blob.ListBlobsSegmentedAsync(null));
            _directoryContent = task.Result;
            Exists = _directoryContent.Results != null && _directoryContent.Results.Any();
        }

        public System.Collections.Generic.IEnumerator<IFileInfo> GetEnumerator()
        {
            return ContentToFileInfo().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ContentToFileInfo().GetEnumerator();
        }

        public bool Exists { get; }

        private System.Collections.Generic.IEnumerable<IFileInfo> ContentToFileInfo()
        {
            if (_directoryContent == null || _directoryContent.Results == null || !_directoryContent.Results.Any())
            {
                return new IFileInfo[0];
            }

            return _directoryContent.Results.Select(blob => new AzureFileInfo(blob));
        }
    }
}