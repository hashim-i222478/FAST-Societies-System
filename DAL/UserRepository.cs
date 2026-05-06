using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for User entity operations
    /// Handles CRUD operations, authentication, and user queries
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Creates a new user in the database
        /// </summary>
        public int CreateUser(User user)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [User] (Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate)
                                   VALUES (@Email, @PasswordHash, @FirstName, @LastName, @PhoneNumber, @Role, @Status, @CreatedDate, @UpdatedDate);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 120; // 2 minutes
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                        cmd.Parameters.AddWithValue("@Status", user.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", user.UpdatedDate);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateUser Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user by UserId
        /// </summary>
        public User GetUserById(int userId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate FROM [User] WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToUser(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUserById Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a user by email
        /// </summary>
        public User GetUserByEmail(string email)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate FROM [User] WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToUser(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUserByEmail Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all users by role
        /// </summary>
        public List<User> GetUsersByRole(string role)
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate FROM [User] WHERE Role = @Role AND Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Role", role);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(MapReaderToUser(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUsersByRole Error: {ex.Message}");
                throw;
            }

            return users;
        }

        /// <summary>
        /// Retrieves all active users
        /// </summary>
        public List<User> GetAllActiveUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate FROM [User] WHERE Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(MapReaderToUser(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllActiveUsers Error: {ex.Message}");
                throw;
            }

            return users;
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        public bool UpdateUser(User user)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [User] SET Email = @Email, FirstName = @FirstName, LastName = @LastName, 
                                   PhoneNumber = @PhoneNumber, Status = @Status, UpdatedDate = @UpdatedDate
                                   WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", user.UserId);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", user.Status);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateUser Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        public bool UpdatePassword(int userId, string newPasswordHash)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [User] SET PasswordHash = @PasswordHash, UpdatedDate = @UpdatedDate WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@PasswordHash", newPasswordHash);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdatePassword Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Suspends or deactivates a user account
        /// </summary>
        public bool SuspendUser(int userId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [User] SET Status = 'Suspended', UpdatedDate = @UpdatedDate WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SuspendUser Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Checks if email exists in the database
        /// </summary>
        public bool EmailExists(string email)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EmailExists Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all users from the database
        /// </summary>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate 
                                   FROM [User] ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(MapReaderToUser(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllUsers Error: {ex.Message}");
                throw;
            }
            return users;
        }

        /// <summary>
        /// Maps SqlDataReader to User object
        /// </summary>
        private User MapReaderToUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Email = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                FirstName = reader.GetString(3),
                LastName = reader.GetString(4),
                PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                Role = reader.GetString(6),
                Status = reader.GetString(7),
                CreatedDate = reader.GetDateTime(8),
                UpdatedDate = reader.GetDateTime(9)
            };
        }
    }
}
