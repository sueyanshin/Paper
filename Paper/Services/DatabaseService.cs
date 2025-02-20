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
                                ContentId = reader.GetInt32(reader.GetOrdinal("CId")),
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

        public async Task<List<Content>> GetContentsByUserId(int userId)
        {
            var contents = new List<Content>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string sql = @"SELECT Cid, FileName, Summary, CreatedAt 
                              FROM Contents 
                              WHERE UserId = @UserId 
                              ORDER BY CreatedAt DESC ,Cid DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            contents.Add(new Content
                            {
                                ContentId = reader.GetInt32(reader.GetOrdinal("CId")),
                                FileName = reader.GetString(reader.GetOrdinal("FileName")),
                                Summary = reader.GetString(reader.GetOrdinal("Summary")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")).ToString()
                            });
                        }
                    }
                }
            }
            return contents;
        }

        public async Task<Content> GetContentById(int contentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string sql = @"SELECT CId, FileName, Summary, CreatedAt 
                              FROM Contents 
                              WHERE CId = @ContentId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ContentId", contentId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Content
                            {
                                ContentId = reader.GetInt32(reader.GetOrdinal("CId")),
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


        public async Task<List<Flashcard>> GetFlashcardsByContentId(int contentId)
        {
            var flashcards = new List<Flashcard>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string sql = "SELECT FId, Question, Answer FROM Flashcards WHERE CId = @ContentId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ContentId", contentId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            flashcards.Add(new Flashcard
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("FId")),
                                Question = reader.GetString(reader.GetOrdinal("Question")),
                                Answer = reader.GetString(reader.GetOrdinal("Answer")),
                                IsFlipped = false
                            });
                        }
                    }
                }
            }
            return flashcards;
        }

        public async Task DeleteContent(int contentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Delete flashcards first (due to foreign key constraint)
                        string deleteFlashcardsSQL = "DELETE FROM Flashcards WHERE CId = @ContentId";
                        using (SqlCommand cmd = new SqlCommand(deleteFlashcardsSQL, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ContentId", contentId);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Get file name before deleting content
                        string getFileNameSQL = "SELECT FileName FROM Contents WHERE CId = @ContentId";
                        string fileName = "";
                        using (SqlCommand cmd = new SqlCommand(getFileNameSQL, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ContentId", contentId);
                            fileName = (string)await cmd.ExecuteScalarAsync();
                        }

                        // Delete content
                        string deleteContentSQL = "DELETE FROM Contents WHERE CId = @ContentId";
                        using (SqlCommand cmd = new SqlCommand(deleteContentSQL, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ContentId", contentId);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Delete PDF file
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            string uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
                            string filePath = Path.Combine(uploadDir, fileName);
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
