using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for Membership entity operations
    /// Handles student-society relationships
    /// </summary>
    public class MembershipRepository
    {
        /// <summary>
        /// Creates a new membership record
        /// </summary>
        public int CreateMembership(Membership membership)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [Membership] (StudentId, SocietyId, JoinDate, Status, CreatedDate, UpdatedDate)
                                   VALUES (@StudentId, @SocietyId, @JoinDate, @Status, @CreatedDate, @UpdatedDate);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", membership.StudentId);
                        cmd.Parameters.AddWithValue("@SocietyId", membership.SocietyId);
                        cmd.Parameters.AddWithValue("@JoinDate", membership.JoinDate);
                        cmd.Parameters.AddWithValue("@Status", membership.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", membership.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", membership.UpdatedDate);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateMembership Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a membership record by ID
        /// </summary>
        public Membership GetMembershipById(int membershipId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT MembershipId, StudentId, SocietyId, JoinDate, Status, CreatedDate, UpdatedDate
                                   FROM [Membership] WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MembershipId", membershipId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToMembership(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetMembershipById Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves pending membership requests for a society
        /// </summary>
        public List<Membership> GetPendingMembershipRequests(int societyId)
        {
            List<Membership> memberships = new List<Membership>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT MembershipId, StudentId, SocietyId, JoinDate, Status, CreatedDate, UpdatedDate
                                   FROM [Membership] WHERE SocietyId = @SocietyId AND Status = 'Pending' ORDER BY JoinDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                memberships.Add(MapReaderToMembership(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetPendingMembershipRequests Error: {ex.Message}");
                throw;
            }

            return memberships;
        }

        /// <summary>
        /// Retrieves all members of a society
        /// </summary>
        public List<Membership> GetSocietyMembers(int societyId)
        {
            List<Membership> memberships = new List<Membership>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT MembershipId, StudentId, SocietyId, JoinDate, Status, CreatedDate, UpdatedDate
                                   FROM [Membership] WHERE SocietyId = @SocietyId AND Status IN ('Active', 'Approved') ORDER BY JoinDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                memberships.Add(MapReaderToMembership(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSocietyMembers Error: {ex.Message}");
                throw;
            }

            return memberships;
        }

        /// <summary>
        /// Retrieves all societies a student is member of
        /// </summary>
        public List<Membership> GetStudentMemberships(int studentId)
        {
            List<Membership> memberships = new List<Membership>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT MembershipId, StudentId, SocietyId, JoinDate, Status, CreatedDate, UpdatedDate
                                   FROM [Membership] WHERE StudentId = @StudentId AND Status IN ('Active', 'Approved') ORDER BY JoinDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                memberships.Add(MapReaderToMembership(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetStudentMemberships Error: {ex.Message}");
                throw;
            }

            return memberships;
        }

        /// <summary>
        /// Checks if a student is already member of a society
        /// </summary>
        public bool IsMember(int studentId, int societyId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [Membership] WHERE StudentId = @StudentId AND SocietyId = @SocietyId AND Status IN ('Active', 'Approved')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"IsMember Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Approves a pending membership request
        /// </summary>
        public bool ApproveMembership(int membershipId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = 'Active', UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MembershipId", membershipId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveMembership Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Rejects a membership request
        /// </summary>
        public bool RejectMembership(int membershipId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = 'Rejected', UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MembershipId", membershipId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RejectMembership Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Removes a student from a society
        /// </summary>
        public bool RemoveMembership(int membershipId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = 'Left', UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MembershipId", membershipId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RemoveMembership Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates membership information
        /// </summary>
        public bool UpdateMembership(Membership membership)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = @Status, UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MembershipId", membership.MembershipId);
                        cmd.Parameters.AddWithValue("@Status", membership.Status);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateMembership Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to Membership object
        /// </summary>
        private Membership MapReaderToMembership(SqlDataReader reader)
        {
            return new Membership
            {
                MembershipId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                SocietyId = reader.GetInt32(2),
                JoinDate = reader.GetDateTime(3),
                Status = reader.GetString(4),
                CreatedDate = reader.GetDateTime(5),
                UpdatedDate = reader.GetDateTime(6)
            };
        }
    }
}
