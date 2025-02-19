using System;
using System.Collections.Generic;
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
            // Create upload directory if it doesn't exist
            string uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
            Directory.CreateDirectory(uploadDir);

            // Copy file to upload directory
            string fileName = Path.GetFileName(sourceFilePath);
            string destPath = Path.Combine(uploadDir, fileName);
            File.Copy(sourceFilePath, destPath, true);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string sql = @"INSERT INTO Contents (UserId, FileName, Summary) 
                             VALUES (@UserId, @FileName, @Summary);
                             SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", content.UserId);
                    cmd.Parameters.AddWithValue("@FileName", content.FileName);
                    cmd.Parameters.AddWithValue("@Summary", content.Summary);

                    return Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
        }

        public async Task SaveFlashcards(int contentId, List<Flashcard> flashcards)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                foreach (var flashcard in flashcards)
                {
                    string sql = @"INSERT INTO Flashcards (Cid, Question, Answer) 
                                 VALUES (@Cid, @Question, @Answer)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Cid", contentId);
                        cmd.Parameters.AddWithValue("@Question", flashcard.Question);
                        cmd.Parameters.AddWithValue("@Answer", flashcard.Answer);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task<Content> GetContent(int contentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string sql = "SELECT * FROM Contents WHERE Cid = @ContentId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ContentId", contentId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Content
                            {
                                ContentId = reader.GetInt32(reader.GetOrdinal("Cid")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                FileName = reader.GetString(reader.GetOrdinal("FileName")),
                                Summary = reader.GetString(reader.GetOrdinal("Summary")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")).ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
