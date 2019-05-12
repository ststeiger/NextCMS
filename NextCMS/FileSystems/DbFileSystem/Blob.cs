
namespace NextCMS.DbFileSystem
{


    public interface IDbFileSystemEntry
    { }

    
    public class DbFileProperties
    {
        public System.DateTimeOffset? LastModified { get; }

        public int Length { get; }

    }

    public class DbFileReference
        : IDbFileSystemEntry
    {

        public bool Exists { get; }
        public string Name { get; }

        public System.Uri Uri { get; }


        public DbFileProperties Properties { get; }


        // public System.Threading.Tasks.Task DownloadToStream(System.IO.Stream stream)
        public void DownloadToStream(System.IO.Stream stream)
        {
            // byte[] fileContents = new byte[0];
            // stream.Write(fileContents, 0, fileContents.Length);

            // System.IO.Stream input = null;
            // await input.CopyToAsync(stream);
            // byte[] buffer = new byte[32768];
            // int read;
            // while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            // {
            //     stream.Write(buffer, 0, read);
            // }

            using (System.IO.TextWriter tw = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8))
            {
                string text = System.IO.File.ReadAllText(@"C:\Users\Administrator\Documents\Visual Studio 2017\Projects\NextCMS\NextCMS\Pages\Privacy.cshtml", System.Text.Encoding.UTF8);
                tw.Write(text);
                tw.Flush();
                // await tw.WriteAsync(text);
                // await tw.FlushAsync();
            }

            throw new System.NotImplementedException("DbFileReference.DownloadToStream");
        }


    }

    public class DirectoryReference
        : IDbFileSystemEntry
    {
        public DbDirectoryContentListing GetDirectoryContents(string abc)
        {
            return new DbDirectoryContentListing();
        }
    }


    public class DbDirectoryContentListing
    {
        public System.Collections.Generic.List<IDbFileSystemEntry> Results;



        public bool HasResults
        {
            get
            {
                if (this.Results == null)
                    return false;


                using (System.Collections.Generic.IEnumerator<IDbFileSystemEntry> e = this.Results.GetEnumerator())
                {
                    if (e.MoveNext()) return true;
                }

                return false;
            }
        }



    }




}
