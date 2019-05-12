
using Microsoft.Extensions.FileProviders;


namespace NextCMS.DbFileSystem
{

    


    public class DbFileInfo
        : Microsoft.Extensions.FileProviders.IFileInfo
    {

        protected readonly DbFileReference m_fileReference;

        protected bool m_exists;
        bool IFileInfo.Exists { get { return this.m_exists; } }


        protected long m_length;

        long IFileInfo.Length { get { return this.m_length; } }


        protected string m_physicalPath;
        string IFileInfo.PhysicalPath { get { return this.m_physicalPath; } }

        protected string m_name;
        string IFileInfo.Name { get { return this.m_name; } }


        protected System.DateTimeOffset m_lastModified;
        System.DateTimeOffset IFileInfo.LastModified { get { return this.m_lastModified; } }


        protected bool m_isDirectory;
        bool IFileInfo.IsDirectory { get { return this.m_isDirectory; } }

        

        public DbFileInfo(IDbFileSystemEntry blob)
        {
            this.m_isDirectory = (blob is DirectoryReference);
            IFileInfo thisFile = (IFileInfo)this;


            if (!thisFile.IsDirectory)
            {
                this.m_fileReference = (DbFileReference)blob;
                this.m_exists = this.m_fileReference.Exists;
                
                if (thisFile.Exists)
                {
                    this.m_length = this.m_fileReference.Properties.Length;
                    this.m_physicalPath = this.m_fileReference.Uri.ToString();
                    this.m_name = this.m_fileReference.Name;
                    this.m_lastModified = this.m_fileReference.Properties.LastModified.HasValue ? 
                          this.m_fileReference.Properties.LastModified.Value 
                        : System.DateTimeOffset.MinValue;
                } // End if (thisFile.Exists) 
            } // End if (!thisFile.IsDirectory) 

        } // End Constructor 


        System.IO.Stream IFileInfo.CreateReadStream()
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.m_fileReference.DownloadToStream(stream);
            stream.Position = 0;
            return stream;
        } // End Function CreateReadStream 


    } // End Cass DbFileInfo 
    

}
