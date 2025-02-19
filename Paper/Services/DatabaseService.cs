using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Paper.Models;

namespace Paper.Services
{
    public class DatabaseService
    {
        private readonly string connectionString = "Data Source=SYS\\SQLEXPRESS;Initial Catalog=paper;Integrated Security=True;";

        public async Task<int> SaveContent(Content content, string sourceFilePath)
        {
            // create upload directory if it doesn't exist
            string uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
            Directory.CreateDirectory(uploadDir);

            // Copy file to upload directory
            string fileName = Path.GetFileName(sourceFilePath);
            string destPath = Path.Combine(uploadDir, fileName);
            File.Copy(sourceFilePath, destPath, true);

            SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = @"INSERT INTO Contents (UserId, FileName, FilePath, Summary) VALUES (@UserId,@FileName,@FilePath,@Summary)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", content.UserId);
            cmd.Parameters.AddWithValue("@FileName", content.UserId);
            cmd.Parameters.AddWithValue("@FilePath", content.UserId);
            cmd.Parameters.AddWithValue("@Summary", content.UserId);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
    }
}
