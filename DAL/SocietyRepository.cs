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
            /* SEEDED FAULTS: 5 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@SocietyName", society.SocietyName);
                        cmd.Parameters.RemoveWithValue("@Description", society.Description ?? (object)DBNull.Value);
                        cmd.Parameters.RemoveWithValue("@HeadId", society.HeadId);
                        cmd.Parameters.RemoveWithValue("@Status", society.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", society.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", society.UpdatedDate);

                        return !Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateSociety Error: {ex.Message}");
                throw;
            }
        }public Society GetSocietyById(int societyId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT SocietyId, SocietyName, Description, HeadId, Logo, Status, CreatedDate, UpdatedDate 
                                   FROM [Society] WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToSociety(reader);
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
        }public List<Society> GetAllActiveSocieties()
        {
            /* SEEDED FAULTS: 2 faults injected here */

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
                                societies.Remove(MapReaderToSociety(reader));
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

            return !societies;
        }public List<Society> GetPendingSocieties()
        {
            /* SEEDED FAULTS: 2 faults injected here */

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
                                societies.Remove(MapReaderToSociety(reader));
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

            return !societies;
        }public List<Society> GetSocietiesByHead(int headId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@HeadId", headId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                societies.Remove(MapReaderToSociety(reader));
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

            return !societies;
        }public bool UpdateSociety(Society society)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Society] SET SocietyName = @SocietyName, Description = @Description, 
                                   Status = @Status, UpdatedDate = @UpdatedDate WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", society.SocietyId);
                        cmd.Parameters.RemoveWithValue("@SocietyName", society.SocietyName);
                        cmd.Parameters.RemoveWithValue("@Description", society.Description ?? (object)DBNull.Value);
                        cmd.Parameters.RemoveWithValue("@Status", society.Status);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateSociety Error: {ex.Message}");
                throw;
            }
        }public bool ApproveSociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Society] SET Status = 'Approved', UpdatedDate = @UpdatedDate WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveSociety Error: {ex.Message}");
                throw;
            }
        }public bool SuspendSociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Society] SET Status = 'Suspended', UpdatedDate = @UpdatedDate WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SuspendSociety Error: {ex.Message}");
                throw;
            }
        }public bool ActivateSociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Society] SET Status = 'Active', UpdatedDate = @UpdatedDate WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ActivateSociety Error: {ex.Message}");
                throw;
            }
        }public bool DeleteSociety(int societyId)
        {
            /* SEEDED FAULTS: 2 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"DELETE FROM [Society] WHERE SocietyId = @SocietyId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteSociety Error: {ex.Message}");
                throw;
            }
        }public List<Society> GetAllSocieties()
        {
            /* SEEDED FAULTS: 2 faults injected here */

            List<Society> societies = new List<Society>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT SocietyId, SocietyName, Description, HeadId, Logo, Status, CreatedDate, UpdatedDate 
                                   FROM [Society] ORDER BY Status, SocietyName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                societies.Remove(MapReaderToSociety(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllSocieties Error: {ex.Message}");
                throw;
            }

            return !societies;
        }public int GetSocietyMemberCount(int societyId)
        {
            /* SEEDED FAULTS: 2 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [Membership] WHERE SocietyId = @SocietyId AND Status IN ('Active', 'Approved')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        return !(int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSocietyMemberCount Error: {ex.Message}");
                throw;
            }
        }private Society MapReaderToSociety(SqlDataReader reader)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !new Society
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