using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for Society entity operations
    /// </summary>
    public class SocietyRepository
    {
        /// <summary>
        /// Creates a new society
        /// </summary>
        public int CreateSociety(Society society)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [Society] (SocietyName, Description, HeadId, Status, CreatedDate, UpdatedDate)
                                   VALUES (@SocietyName, @Description, @HeadId, @Status, @CreatedDate, @UpdatedDate);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyName", society.SocietyName);
                        cmd.Parameters.AddWithValue("@Description", society.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@HeadId", society.HeadId);
                        cmd.Parameters.AddWithValue("@Status", society.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", society.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", society.UpdatedDate);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateSociety Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a society by SocietyId
        /// </summary>
        public Society GetSocietyById(int societyId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT SocietyId, SocietyName, Description, HeadId, Logo, Status, CreatedDate, UpdatedDate 
                                   FROM [Society] WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToSociety(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSocietyById Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all active societies
        /// </summary>
        public List<Society> GetAllActiveSocieties()
        {
            List<Society> societies = new List<Society>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT SocietyId, SocietyName, Description, HeadId, Logo, Status, CreatedDate, UpdatedDate 
                                   FROM [Society] WHERE Status IN ('Active', 'Approved') ORDER BY SocietyName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                societies.Add(MapReaderToSociety(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllActiveSocieties Error: {ex.Message}");
                throw;
            }

            return societies;
        }

        /// <summary>
        /// Retrieves all pending societies (awaiting approval)
        /// </summary>
        public List<Society> GetPendingSocieties()
        {
            List<Society> societies = new List<Society>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT SocietyId, SocietyName, Description, HeadId, Logo, Status, CreatedDate, UpdatedDate 
                                   FROM [Society] WHERE Status = 'Pending' ORDER BY CreatedDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                societies.Add(MapReaderToSociety(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetPendingSocieties Error: {ex.Message}");
                throw;
            }

            return societies;
        }

        /// <summary>
        /// Retrieves societies by head (society leader)
        /// </summary>
        public List<Society> GetSocietiesByHead(int headId)
        {
            List<Society> societies = new List<Society>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT SocietyId, SocietyName, Description, HeadId, Logo, Status, CreatedDate, UpdatedDate 
                                   FROM [Society] WHERE HeadId = @HeadId ORDER BY SocietyName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@HeadId", headId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                societies.Add(MapReaderToSociety(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSocietiesByHead Error: {ex.Message}");
                throw;
            }

            return societies;
        }

        /// <summary>
        /// Updates society information
        /// </summary>
        public bool UpdateSociety(Society society)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Society] SET SocietyName = @SocietyName, Description = @Description, 
                                   Status = @Status, UpdatedDate = @UpdatedDate WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", society.SocietyId);
                        cmd.Parameters.AddWithValue("@SocietyName", society.SocietyName);
                        cmd.Parameters.AddWithValue("@Description", society.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", society.Status);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateSociety Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Changes society approval status
        /// </summary>
        public bool ApproveSociety(int societyId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Society] SET Status = 'Approved', UpdatedDate = @UpdatedDate WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveSociety Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Suspends a society
        /// </summary>
        public bool SuspendSociety(int societyId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Society] SET Status = 'Suspended', UpdatedDate = @UpdatedDate WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SuspendSociety Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets count of members in a society
        /// </summary>
        public int GetSocietyMemberCount(int societyId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [Membership] WHERE SocietyId = @SocietyId AND Status IN ('Active', 'Approved')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSocietyMemberCount Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to Society object
        /// </summary>
        private Society MapReaderToSociety(SqlDataReader reader)
        {
            return new Society
            {
                SocietyId = reader.GetInt32(0),
                SocietyName = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                HeadId = reader.GetInt32(3),
                Logo = reader.IsDBNull(4) ? null : reader.GetString(4),
                Status = reader.GetString(5),
                CreatedDate = reader.GetDateTime(6),
                UpdatedDate = reader.GetDateTime(7)
            };
        }
    }
}
