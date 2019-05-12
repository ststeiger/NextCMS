
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;


namespace NextCMS.DbFileSystem
{


    public class DbFileProvider
        : IFileProvider

    {

        protected readonly DbClient _container;
        protected readonly System.Collections.Generic.HashSet<string> m_ignoreFolders;
        protected readonly System.Collections.Generic.HashSet<string> m_ignoreFiles;

        public DbFileProvider(Microsoft.Extensions.Configuration.IConfiguration configurationRoot)
        {
            this.m_ignoreFolders = new System.Collections.Generic.HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
            this.m_ignoreFiles = new System.Collections.Generic.HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);

            this.m_ignoreFolders.Add("/Areas");
            this.m_ignoreFolders.Add("/Pages");
            this.m_ignoreFolders.Add("/Pages/Shared");

            this.m_ignoreFiles.Add("/Areas/_ViewImports.cshtml");
            this.m_ignoreFiles.Add("/_ViewImports.cshtml");
            this.m_ignoreFiles.Add("/_ViewStart.cshtml");
            
            this.m_ignoreFiles.Add("/Pages/_Layout.cshtml");
            this.m_ignoreFiles.Add("/Pages/_CookieConsentPartial.cshtml");



            // string connectionString = configurationRoot.GetValue<string>("Azure:StorageConnectionString");
            // string containerName = configurationRoot.GetValue<string>("Azure:BlobContainerName");

            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.DataSource = System.Environment.MachineName;
            csb.InitialCatalog = "";
            csb.IntegratedSecurity = true;
            if (!csb.IntegratedSecurity)
            {
                csb.UserID = "";
                csb.Password = "";
            }

            csb.PacketSize = 4096;
            csb.PersistSecurityInfo = false;
            csb.WorkstationID = System.Environment.MachineName;
            csb.ApplicationName = "NextCMS";
            csb.ApplicationIntent = System.Data.SqlClient.ApplicationIntent.ReadWrite;

            string connectionString = csb.ConnectionString;


            // var storageAccount = CloudStorageAccount.Parse(connectionString);
            // var blobClient = storageAccount.CreateCloudBlobClient();

            // _container = blobClient.GetContainerReference(containerName);
            this._container = new DbClient(connectionString);
            
        }

        IDirectoryContents IFileProvider.GetDirectoryContents(string subpath)
        {
            if(this.m_ignoreFolders.Contains(subpath))
                return new DbDirectoryContents(_container.EmptyDirectoryReference);

            DirectoryReference blob = _container.GetDirectoryReference(subpath);
            return new DbDirectoryContents(blob);
        }



        IFileInfo IFileProvider.GetFileInfo(string subpath)
        {
            if (this.m_ignoreFiles.Contains(subpath))
                return new DbFileInfo(_container.EmptyFileReference);

            IDbFileSystemEntry blob = _container.GetFileReference(subpath);
            return new DbFileInfo(blob);
        }

        Microsoft.Extensions.Primitives.IChangeToken IFileProvider.Watch(string filter)
        {
            return new NoWatchChangeToken();
        }


        // string azurePath = ConvertPath(subpath);
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
