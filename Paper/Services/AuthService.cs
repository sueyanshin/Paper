using System.Data.SqlClient;
using System.Threading.Tasks;
using Paper.Models;

namespace Paper.Services
{
    public class AuthService
    {
        private readonly string connectionString = "Data Source=SYS\\SQLEXPRESS;Initial Catalog=paper;Integrated Security=True;";
        public async Task<User> Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "SELECT Uid,Name,Email FROM Users WHERE Email=@Email AND Password=@Password";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("Uid")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
