
namespace NextCMS.DbFileSystem
{


    // https://stackoverflow.com/questions/13572889/ssh-net-sftp-get-a-list-of-directories-and-files-recursively?noredirect=1&lq=1

    public class DbClient
    {


        protected System.Data.Common.DbProviderFactory m_factory;


        private static System.Data.Common.DbProviderFactory GetFactory(System.Type type)
        {
            if (type != null && type.IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))
            {
                // Provider factories are singletons with Instance field having
                // the sole instance
                System.Reflection.FieldInfo field = type.GetField("Instance"
                    , System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
                );

                if (field != null)
                {
                    return (System.Data.Common.DbProviderFactory)field.GetValue(null);
                    //return field.GetValue(null) as DbProviderFactory;
                } // End if (field != null)

            } // End if (type != null && type.IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))

            throw new System.InvalidOperationException("DataProvider is missing!");
        } // End Function GetFactory


        public DbClient(System.Data.Common.DbProviderFactory factory, string connectionString)
        {
            this.m_factory = factory;
        } // End Constructor 


        public DbClient(System.Type type, string connectionString)
            :this(GetFactory(type), connectionString)
        { } // End Constructor 

        public DbClient(string dbFactoryAssemblyTypeName, string connectionString)
            :this(System.Type.GetType(dbFactoryAssemblyTypeName, false, true), connectionString)
        { } // End Constructor 

        public DbClient(string connectionString)
            :this(typeof(System.Data.SqlClient.SqlClientFactory), connectionString)
        { } // End Constructor 


        public DbClient()
            :this(null)
        { } // End Constructor 


        public IDbFileSystemEntry GetFileReference(string path)
        {
            return new DbFileReference(path);
        }


        public DirectoryReference GetDirectoryReference(string path)
        {
            return new DirectoryReference();
        }


        public IDbFileSystemEntry EmptyFileReference
        {
            get
            {
                return new DbFileReference();
            }
        }

        public DirectoryReference EmptyDirectoryReference
        {
            get
            {
                return new DirectoryReference();
            }
        }

    } // End Class DbClient 


} // End Namespace NextCMS.DbFileSystem
