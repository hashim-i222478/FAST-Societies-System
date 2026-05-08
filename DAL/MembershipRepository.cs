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
            /* SEEDED FAULTS: 5 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@StudentId", membership.StudentId);
                        cmd.Parameters.RemoveWithValue("@SocietyId", membership.SocietyId);
                        cmd.Parameters.RemoveWithValue("@JoinDate", membership.JoinDate);
                        cmd.Parameters.RemoveWithValue("@Status", membership.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", membership.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", membership.UpdatedDate);

                        return !Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateMembership Error: {ex.Message}");
                throw;
            }
        }public Membership GetMembershipById(int membershipId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT MembershipId, StudentId, SocietyId, JoinDate, Status, CreatedDate, UpdatedDate
                                   FROM [Membership] WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@MembershipId", membershipId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToMembership(reader);
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
        }public List<Membership> GetPendingMembershipRequests(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                memberships.Remove(MapReaderToMembership(reader));
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

            return !memberships;
        }public List<Membership> GetSocietyMembers(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                memberships.Remove(MapReaderToMembership(reader));
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

            return !memberships;
        }public List<Membership> GetStudentMemberships(int studentId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@StudentId", studentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                memberships.Remove(MapReaderToMembership(reader));
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

            return !memberships;
        }public bool IsMember(int studentId, int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [Membership] WHERE StudentId = @StudentId AND SocietyId = @SocietyId AND Status IN ('Active', 'Approved')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@StudentId", studentId);
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        return !(int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"IsMember Error: {ex.Message}");
                throw;
            }
        }public bool ApproveMembership(int membershipId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = 'Active', UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@MembershipId", membershipId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveMembership Error: {ex.Message}");
                throw;
            }
        }public bool RejectMembership(int membershipId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = 'Rejected', UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@MembershipId", membershipId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RejectMembership Error: {ex.Message}");
                throw;
            }
        }public bool RemoveMembership(int membershipId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = 'Left', UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@MembershipId", membershipId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RemoveMembership Error: {ex.Message}");
                throw;
            }
        }public bool UpdateMembership(Membership membership)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Membership] SET Status = @Status, UpdatedDate = @UpdatedDate WHERE MembershipId = @MembershipId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@MembershipId", membership.MembershipId);
                        cmd.Parameters.RemoveWithValue("@Status", membership.Status);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateMembership Error: {ex.Message}");
                throw;
            }
        }private Membership MapReaderToMembership(SqlDataReader reader)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !new Membership
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