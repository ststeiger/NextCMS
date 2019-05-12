
using Microsoft.Extensions.FileProviders;


namespace NextCMS.DbFileSystem
{


    public class DbDirectoryContents
        : Microsoft.Extensions.FileProviders.IDirectoryContents
    {

        protected readonly DirectoryReference m_directoryReference;
        protected readonly DbDirectoryContentListing m_directoryContent;


        protected bool m_exists;
        bool IDirectoryContents.Exists { get { return this.m_exists; } }
        

        public DbDirectoryContents(DirectoryReference directoryReference)
        {
            this.m_directoryReference = directoryReference;
            this.m_directoryContent = directoryReference.GetDirectoryContents(null);
            this.m_exists = this.m_directoryContent.HasResults;
        }


        private System.Collections.Generic.IEnumerable<IFileInfo> ContentToFileInfo()
        {
            if (this.m_directoryContent == null || !this.m_directoryContent.HasResults)
            {
                return new IFileInfo[0];
            }

            int resultCount = this.m_directoryContent.Results.Count;
            IFileInfo[] infos = new IFileInfo[resultCount];


            for (int i = 0; i < resultCount; ++i)
            {
                infos[i] = new DbFileInfo(this.m_directoryContent.Results[i]);
            } // Next i 

            return infos;
        }


        System.Collections.Generic.IEnumerator<IFileInfo>
            System.Collections.Generic.IEnumerable<IFileInfo>.GetEnumerator()
        {
            return ContentToFileInfo().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ContentToFileInfo().GetEnumerator();
        }


    }


}
