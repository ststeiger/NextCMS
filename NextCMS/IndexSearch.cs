
using System;

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
        
        
        public static string cs()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.DataSource = System.Environment.MachineName;
            
            if("COR".Equals(System.Environment.UserDomainName, StringComparison.InvariantCultureIgnoreCase))
                csb.DataSource += @"\SQLExpress";

                
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

            return connectionString;
        }


    }


    public class IndexSearch
    {


        public void Test()
        {
            string sql = @"

CREATE TABLE dbo.FileIndex
(
     uid uniqueidentifier NOT NULL,
    ,path nvarchar(4000) NOT NULL 
    ,directory nvarchar(4000) 
    ,fileName nvarchar(4000) 
    ,extension nvarchar(100) 
    ,creationTime as datetime
    ,accessTime as datetime
    ,writeTime as datetime
    ,size bigint 
);


SELECT
     NEWID() AS uid 
    ,@path
    ,@dir
    ,@fn
    ,@ext
    ,@creationTime
    ,@accessTime
    ,@writeTime
    ,@size
;

";
        }
        
        
        public void IndexDirectory(string rootPath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(rootPath);
            System.IO.FileInfo[] fis = di.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            
            for (int i = 0; i < fis.Length; ++i)
            {
                try
                {
                    long l = fis[i].Length;
                    string dn = fis[i].DirectoryName;
                    string fn = fis[i].Directory.Root.FullName;

                    System.DateTime ct = fis[i].CreationTimeUtc;
                    System.DateTime at = fis[i].LastAccessTimeUtc;
                    System.DateTime wt = fis[i].LastWriteTimeUtc;
                    bool rdo = fis[i].IsReadOnly;
                    string ext = fis[i].Extension;
                
                
                    System.IO.DriveInfo drive = fis[i].GetDriveInfo();
                    System.IO.DriveInfo drivei = new System.IO.DriveInfo(fis[i].Directory.Root.FullName);
                    
                    
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }
            
        }


    }
}