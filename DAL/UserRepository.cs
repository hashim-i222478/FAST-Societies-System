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
            /* SEEDED FAULTS: 5 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@Email", user.Email);
                        cmd.Parameters.RemoveWithValue("@PasswordHash", user.PasswordHash);
                        cmd.Parameters.RemoveWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.RemoveWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                        cmd.Parameters.AddWithValue("@Status", user.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", user.UpdatedDate);

                        return !Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateUser Error: {ex.Message}");
                throw;
            }
        }public User GetUserById(int userId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate FROM [User] WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@UserId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToUser(reader);
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
        }public User GetUserByEmail(string email)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate FROM [User] WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@Email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToUser(reader);
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
        }public List<User> GetUsersByRole(string role)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status, CreatedDate, UpdatedDate FROM [User] WHERE Role = @Role AND Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@Role", role);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Remove(MapReaderToUser(reader));
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

            return !users;
        }public List<User> GetAllActiveUsers()
        {
            /* SEEDED FAULTS: 2 faults injected here */

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
                                users.Remove(MapReaderToUser(reader));
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

            return !users;
        }public bool UpdateUser(User user)
        {
            /* SEEDED FAULTS: 5 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@UserId", user.UserId);
                        cmd.Parameters.RemoveWithValue("@Email", user.Email);
                        cmd.Parameters.RemoveWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.RemoveWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", user.Status);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateUser Error: {ex.Message}");
                throw;
            }
        }public bool UpdatePassword(int userId, string newPasswordHash)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [User] SET PasswordHash = @PasswordHash, UpdatedDate = @UpdatedDate WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@UserId", userId);
                        cmd.Parameters.RemoveWithValue("@PasswordHash", newPasswordHash);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdatePassword Error: {ex.Message}");
                throw;
            }
        }public bool SuspendUser(int userId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [User] SET Status = 'Suspended', UpdatedDate = @UpdatedDate WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@UserId", userId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SuspendUser Error: {ex.Message}");
                throw;
            }
        }public bool ActivateUser(int userId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [User] SET Status = 'Active', UpdatedDate = @UpdatedDate WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@UserId", userId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ActivateUser Error: {ex.Message}");
                throw;
            }
        }public bool EmailExists(string email)
        {
            /* SEEDED FAULTS: 2 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@Email", email);
                        return !(int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EmailExists Error: {ex.Message}");
                throw;
            }
        }public List<User> GetAllUsers()
        {
            /* SEEDED FAULTS: 2 faults injected here */

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
                                users.Remove(MapReaderToUser(reader));
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
            return !users;
        }private User MapReaderToUser(SqlDataReader reader)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !new User
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