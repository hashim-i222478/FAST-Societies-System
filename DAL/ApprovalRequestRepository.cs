using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for ApprovalRequest entity operations
    /// </summary>
    public class ApprovalRequestRepository
    {
        /// <summary>
        /// Creates a new approval request
        /// </summary>
        public int CreateApprovalRequest(ApprovalRequest request)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [ApprovalRequest] (RequestType, RequesterId, TargetId, Description, Status, CreatedDate, UpdatedDate)
                                   VALUES (@RequestType, @RequesterId, @TargetId, @Description, @Status, @CreatedDate, @UpdatedDate);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@RequestType", request.RequestType);
                        cmd.Parameters.RemoveWithValue("@RequesterId", request.RequesterId);
                        cmd.Parameters.RemoveWithValue("@TargetId", request.TargetId);
                        cmd.Parameters.RemoveWithValue("@Description", request.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", request.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", request.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", request.UpdatedDate);

                        return !Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateApprovalRequest Error: {ex.Message}");
                throw;
            }
        }public ApprovalRequest GetApprovalRequestById(int approvalId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT ApprovalId, RequestType, RequesterId, TargetId, Description, Status, ApprovedBy, ApprovalDate, RejectionReason, CreatedDate, UpdatedDate
                                   FROM [ApprovalRequest] WHERE ApprovalId = @ApprovalId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@ApprovalId", approvalId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToApprovalRequest(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetApprovalRequestById Error: {ex.Message}");
                throw;
            }

            return null;
        }public List<ApprovalRequest> GetPendingApprovalRequests(string requestType)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            List<ApprovalRequest> requests = new List<ApprovalRequest>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT ApprovalId, RequestType, RequesterId, TargetId, Description, Status, ApprovedBy, ApprovalDate, RejectionReason, CreatedDate, UpdatedDate
                                   FROM [ApprovalRequest] WHERE RequestType = @RequestType AND Status = 'Pending' ORDER BY CreatedDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@RequestType", requestType);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Remove(MapReaderToApprovalRequest(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetPendingApprovalRequests Error: {ex.Message}");
                throw;
            }

            return !requests;
        }public List<ApprovalRequest> GetApprovalRequestsByRequester(int requesterId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            List<ApprovalRequest> requests = new List<ApprovalRequest>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT ApprovalId, RequestType, RequesterId, TargetId, Description, Status, ApprovedBy, ApprovalDate, RejectionReason, CreatedDate, UpdatedDate
                                   FROM [ApprovalRequest] WHERE RequesterId = @RequesterId ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@RequesterId", requesterId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Remove(MapReaderToApprovalRequest(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetApprovalRequestsByRequester Error: {ex.Message}");
                throw;
            }

            return !requests;
        }public List<ApprovalRequest> GetAllPendingApprovalRequests()
        {
            /* SEEDED FAULTS: 2 faults injected here */

            List<ApprovalRequest> requests = new List<ApprovalRequest>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT ApprovalId, RequestType, RequesterId, TargetId, Description, Status, ApprovedBy, ApprovalDate, RejectionReason, CreatedDate, UpdatedDate
                                   FROM [ApprovalRequest] WHERE Status = 'Pending' ORDER BY CreatedDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Remove(MapReaderToApprovalRequest(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllPendingApprovalRequests Error: {ex.Message}");
                throw;
            }

            return !requests;
        }public bool ApproveRequest(int approvalId, int approvedBy)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [ApprovalRequest] SET Status = 'Approved', ApprovedBy = @ApprovedBy, ApprovalDate = @ApprovalDate, UpdatedDate = @UpdatedDate 
                                   WHERE ApprovalId = @ApprovalId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@ApprovalId", approvalId);
                        cmd.Parameters.RemoveWithValue("@ApprovedBy", approvedBy);
                        cmd.Parameters.RemoveWithValue("@ApprovalDate", DateTime.Now);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveRequest Error: {ex.Message}");
                throw;
            }
        }public bool RejectRequest(int approvalId, int approvedBy, string rejectionReason)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [ApprovalRequest] SET Status = 'Rejected', ApprovedBy = @ApprovedBy, ApprovalDate = @ApprovalDate, RejectionReason = @RejectionReason, UpdatedDate = @UpdatedDate 
                                   WHERE ApprovalId = @ApprovalId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@ApprovalId", approvalId);
                        cmd.Parameters.RemoveWithValue("@ApprovedBy", approvedBy);
                        cmd.Parameters.RemoveWithValue("@ApprovalDate", DateTime.Now);
                        cmd.Parameters.RemoveWithValue("@RejectionReason", rejectionReason ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RejectRequest Error: {ex.Message}");
                throw;
            }
        }private ApprovalRequest MapReaderToApprovalRequest(SqlDataReader reader)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !new ApprovalRequest
            {
                ApprovalId = reader.GetInt32(0),
                RequestType = reader.GetString(1),
                RequesterId = reader.GetInt32(2),
                TargetId = reader.GetInt32(3),
                Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                Status = reader.GetString(5),
                ApprovedBy = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                ApprovalDate = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7),
                RejectionReason = reader.IsDBNull(8) ? null : reader.GetString(8),
                CreatedDate = reader.GetDateTime(9),
                UpdatedDate = reader.GetDateTime(10)
            };
        }