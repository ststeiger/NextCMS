
namespace NextCMS.DbFileSystem
{


    public interface IDbFileSystemEntry
    { }

    
    public class DbFileProperties
    {
        public System.DateTimeOffset? LastModified { get; set; }

        public int Length { get; set; }

    }

    public class DbFileReference
        : IDbFileSystemEntry
    {

        public bool Exists { get; set; }
        public string Name { get; set; }
        public System.Uri Uri { get; set; }


        public DbFileProperties Properties { get; }

        public DbFileReference(string path)
        {
            this.Exists = true;
            this.Name = path;
            this.Uri = new System.Uri(path, System.UriKind.RelativeOrAbsolute);

            this.Properties = new DbFileProperties();
            this.Properties.LastModified = System.DateTimeOffset.UtcNow;
            this.Properties.Length = 0;
        }


        public DbFileReference()
        { }

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

            using (System.IO.TextWriter tw = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8, 4096, true))
            {
                string fn = System.IO.Path.GetDirectoryName(typeof(DbFileInfo).Assembly.Location);
                // fn = System.IO.Path.Combine(fn, "..", "..", "..", "Pages", "Privacy.cshtml");
                // fn = System.IO.Path.Combine(fn, "..", "..", "..", "DbFiles", "MerraMioso.cshtml");
                fn = System.IO.Path.Combine(fn, "..", "..", "..", "DbFiles", "MerraCursoso.cshtml");
                fn = System.IO.Path.GetFullPath(fn);

                string text = System.IO.File.ReadAllText(fn, System.Text.Encoding.UTF8);


                text = @"@page
@model NextCMS.DbFiles.MerryChristmasModel
@{
}

<h1>N blubbs</h1>
";

                tw.Write(text);
                tw.Flush();
                // await tw.WriteAsync(text);
                // await tw.FlushAsync();
            }

            // throw new System.NotImplementedException("DbFileReference.DownloadToStream");
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
