using System;
using System.Data;
using System.Data.SQLite.Core; // TODO(hekj): Replace with MSSQL.
using System.IO;
using Dapper;
using DbLogStorage.Model;

namespace DbLogStorage
{
    public class LogStorage
    {
        private const string createTableSql =
            @"CREATE TABLE logs (
            ID integer identity primary key AUTOINCREMENT,
            json varchar(1024) not null
            )";

        public LogStorage()
        {
            
        }

        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.sqlite"; }
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }

        private static void CreateDatabase()
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Execute(createTableSql);
            }
        }

        public void SaveLog(LogEntry logEntry) 
        {
            using (var conn = SimpleDbConnection())
            {
                conn.Open();
                logEntry.Id = conn.Query<long>(
                    @"INSERT INTO logs 
                    ( json ) VALUES ( @json );
                    select last_insert_rowid()", logEntry).First();
            }
            
        }
    }
}