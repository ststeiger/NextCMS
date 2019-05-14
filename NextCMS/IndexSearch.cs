
namespace NextCMS
{
    public static class Crap
    {
        public static System.IO.DriveInfo GetDriveInfo(this System.IO.FileInfo file)
        {
            return new System.IO.DriveInfo(file.Directory.Root.FullName);
        }
    }


    public class SQL
    {
        private System.Data.Common.DbProviderFactory fac;


        public SQL()
        {
            this.fac = System.Data.SqlClient.SqlClientFactory.Instance;
        }


        public string GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.DataSource = System.Environment.MachineName;
            
            if("COR".Equals(System.Environment.UserDomainName, System.StringComparison.InvariantCultureIgnoreCase))
                csb.DataSource += @"\SQLExpress";

                
            csb.InitialCatalog = "TestDb";
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

            return connectionString;
        }



        public System.Data.Common.DbCommand CreateCommand(string sql)
        {
            return CreateCommand(sql, 30);
        } // End Function CreateCommand


        public System.Data.Common.DbCommand CreateCommand(string sql, int timeout)
        {
            System.Data.Common.DbCommand cmd = fac.CreateCommand();
            cmd.CommandTimeout = timeout;
            cmd.CommandText = sql;

            return cmd;
        } // End Function CreateCommand


        public System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue)
        {
            return AddParameter(command, strParameterName, objValue, System.Data.ParameterDirection.Input);
        } // End Function AddParameter


        public System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad)
        {
            if (objValue == null)
            {
                //throw new ArgumentNullException("objValue");
                objValue = System.DBNull.Value;
            } // End if (objValue == null)

            System.Type tDataType = objValue.GetType();
            System.Data.DbType dbType = GetDbType(tDataType);

            return AddParameter(command, strParameterName, objValue, pad, dbType);
        } // End Function AddParameter


        public System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad, System.Data.DbType dbType)
        {
            System.Data.IDbDataParameter parameter = command.CreateParameter();

            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            } // End if (!strParameterName.StartsWith("@"))

            parameter.ParameterName = strParameterName;
            parameter.DbType = dbType;
            parameter.Direction = pad;

            // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
            // No association  DbType UInt64 to a known SqlDbType

            if (objValue == null)
                parameter.Value = System.DBNull.Value;
            else
                parameter.Value = objValue;

            command.Parameters.Add(parameter);
            return parameter;
        } // End Function AddParameter


        // From Type to DBType
        public System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType DBtype = System.Data.DbType.String; // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return DBtype;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }

                DBtype = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);

                // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
                // http://msdn.microsoft.com/en-us/library/bbw6zyha(v=vs.71).aspx
                if (DBtype == System.Data.DbType.UInt64)
                    DBtype = System.Data.DbType.Int64;
            }
            catch (System.Exception)
            {
                // Add error handling to suit your taste
            }

            return DBtype;
        } // End Function GetDbType


        public System.Data.Common.DbConnection GetConnection()
        {
            System.Data.Common.DbConnection cn = this.fac.CreateConnection();
            cn.ConnectionString = this.GetConnectionString();

            return cn;
        } // End Function GetConnection 


        public int ExecuteNonQuery(string strSQL)
        {
            int retVal = 0;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                retVal = ExecuteNonQuery(cmd);
            } // End Using cmd 

            return retVal;
        } // End Function ExecuteNonQuery


        public int ExecuteNonQuery(System.Data.IDbCommand cmd)
        {
            int iAffected = -1;
            using (System.Data.IDbConnection idbConn = GetConnection())
            {

                lock (idbConn)
                {

                    lock (cmd)
                    {
                        cmd.Connection = idbConn;

                        if (cmd.Connection.State != System.Data.ConnectionState.Open)
                            cmd.Connection.Open();

                        using (System.Data.IDbTransaction idbtTrans = idbConn.BeginTransaction())
                        {

                            try
                            {
                                cmd.Transaction = idbtTrans;

                                iAffected = cmd.ExecuteNonQuery();
                                idbtTrans.Commit();
                            } // End Try
                            catch (System.Data.Common.DbException ex)
                            {
                                if (idbtTrans != null)
                                    idbtTrans.Rollback();

                                iAffected = -2;
                                
                                    throw;
                            } // End catch
                            finally
                            {
                                if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                    cmd.Connection.Close();
                            } // End Finally

                        } // End Using idbtTrans

                    } // End lock cmd

                } // End lock idbConn

            } // End Using idbConn

            return iAffected;
        } // End Function ExecuteNonQuery

    } // End Class SQL 


    // var ids = new NextCMS.IndexSearch();
    // ids.Test();
    public class IndexSearch
    {


        public void Test()
        {
            string sql = @"
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileIndex]') AND type in (N'U'))
DROP TABLE [dbo].[FileIndex]
GO



CREATE TABLE dbo.FileIndex
(
     uid uniqueidentifier NOT NULL 
    ,root nvarchar(4000) NOT NULL 
    ,path nvarchar(4000) NOT NULL 
    ,directory nvarchar(4000) 
    ,fileName nvarchar(4000) 
    ,fileNameWithoutExtension nvarchar(4000) 
    ,extension nvarchar(100) 
    ,creationTime datetime
    ,accessTime datetime
    ,writeTime datetime
    ,size bigint 
);


SELECT 
	 COUNT(*) AS cnt 
	,extension 
FROM FileIndex 
WHERE (1=1) 
AND fileName NOT LIKE '%designer.vb' 
AND fileNameWithoutExtension NOT LIKE '%.FileListAbsolute'
AND fileNameWithoutExtension NOT IN ('DO_NOT_ADD_FILES_HERE', '_WPPLastBuildInfo')
AND fileName NOT IN ('AssemblyInfo.cs')
AND extension NOT IN 
( 
	  '.dll','.pdb', '.map', '.wsdl', '.disco', '.xml' 
	, '.vssscc', '.vspscc', '.ide', '.ide-shm', '.ide-wal', '.CopyComplete' 
	, '.sln', '.suo', '.user', '.vbproj', '.csproj', '.tfignore', '.pubxml', '.lock', '.cache'
	, '.myapp', '.datasource' ,'.force' 
	, '.set', '.settings', '.cd', '.htc' 
	,'.resx', '.resources' 

	,'.png', '.gif', '.jpg'
	, '.docx', '.pdf' 
	-- , '.sql'
) 

GROUP BY extension 

ORDER BY cnt DESC 




SELECT 
     fileName
    ,fileNameWithoutExtension
    ,extension
FROM FileIndex 
WHERE extension LIKE '.master' 



";

            IndexDirectory(@"D:\username\Documents\Visual Studio 2017\TFS\COR-Basic\COR-Basic\Basicv3");
            System.Console.WriteLine("indexed");
        }
        
        
        public void IndexDirectory(string rootPath)
        {
            SQL sql = new SQL();


            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(rootPath);
            System.IO.FileInfo[] fis = di.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            
            for (int i = 0; i < fis.Length; ++i)
            {
                try
                {
                    // bool rdo = fis[i].IsReadOnly;
                    // System.IO.DriveInfo drive = fis[i].GetDriveInfo();
                    string root = fis[i].Directory.Root.FullName;
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fis[i].Name);


                    using (System.Data.Common.DbCommand cmd = sql.CreateCommand(@"
INSERT INTO FileIndex
(
	 uid
	,root 
    ,path
	,directory
	,fileName
	,fileNameWithoutExtension
	,extension
	,creationTime
	,accessTime
	,writeTime
	,size
) 
SELECT
     NEWID() AS uid 
    ,@root 
    ,@path
    ,@dir
    ,@fn
    ,@fileNameWithoutExtension
    ,@ext
    ,@creationTime
    ,@accessTime
    ,@writeTime
    ,@size
;
"))
                    {
                        sql.AddParameter(cmd, "root", root);
                        sql.AddParameter(cmd, "path", fis[i].FullName);
                        sql.AddParameter(cmd, "dir", fis[i].DirectoryName);
                        sql.AddParameter(cmd, "fn", fis[i].Name);
                        sql.AddParameter(cmd, "fileNameWithoutExtension", fileName);
                        sql.AddParameter(cmd, "ext", fis[i].Extension);
                        sql.AddParameter(cmd, "creationTime", fis[i].CreationTimeUtc);
                        sql.AddParameter(cmd, "accessTime", fis[i].LastAccessTimeUtc);
                        sql.AddParameter(cmd, "writeTime", fis[i].LastWriteTimeUtc);
                        sql.AddParameter(cmd, "size", fis[i].Length);

                        sql.ExecuteNonQuery(cmd);
                    }

                        
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }
            
        }


    }
}