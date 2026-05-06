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
                        cmd.Parameters.AddWithValue("@RequestType", request.RequestType);
                        cmd.Parameters.AddWithValue("@RequesterId", request.RequesterId);
                        cmd.Parameters.AddWithValue("@TargetId", request.TargetId);
                        cmd.Parameters.AddWithValue("@Description", request.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", request.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", request.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", request.UpdatedDate);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateApprovalRequest Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves an approval request by ID
        /// </summary>
        public ApprovalRequest GetApprovalRequestById(int approvalId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT ApprovalId, RequestType, RequesterId, TargetId, Description, Status, ApprovedBy, ApprovalDate, RejectionReason, CreatedDate, UpdatedDate
                                   FROM [ApprovalRequest] WHERE ApprovalId = @ApprovalId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ApprovalId", approvalId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToApprovalRequest(reader);
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
        }

        /// <summary>
        /// Retrieves all pending approval requests of a specific type
        /// </summary>
        public List<ApprovalRequest> GetPendingApprovalRequests(string requestType)
        {
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
                        cmd.Parameters.AddWithValue("@RequestType", requestType);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Add(MapReaderToApprovalRequest(reader));
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

            return requests;
        }

        /// <summary>
        /// Retrieves all approval requests by a requester
        /// </summary>
        public List<ApprovalRequest> GetApprovalRequestsByRequester(int requesterId)
        {
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
                        cmd.Parameters.AddWithValue("@RequesterId", requesterId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Add(MapReaderToApprovalRequest(reader));
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

            return requests;
        }

        /// <summary>
        /// Retrieves all pending approval requests
        /// </summary>
        public List<ApprovalRequest> GetAllPendingApprovalRequests()
        {
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
                                requests.Add(MapReaderToApprovalRequest(reader));
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

            return requests;
        }

        /// <summary>
        /// Approves an approval request
        /// </summary>
        public bool ApproveRequest(int approvalId, int approvedBy)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [ApprovalRequest] SET Status = 'Approved', ApprovedBy = @ApprovedBy, ApprovalDate = @ApprovalDate, UpdatedDate = @UpdatedDate 
                                   WHERE ApprovalId = @ApprovalId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ApprovalId", approvalId);
                        cmd.Parameters.AddWithValue("@ApprovedBy", approvedBy);
                        cmd.Parameters.AddWithValue("@ApprovalDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveRequest Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Rejects an approval request
        /// </summary>
        public bool RejectRequest(int approvalId, int approvedBy, string rejectionReason)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [ApprovalRequest] SET Status = 'Rejected', ApprovedBy = @ApprovedBy, ApprovalDate = @ApprovalDate, RejectionReason = @RejectionReason, UpdatedDate = @UpdatedDate 
                                   WHERE ApprovalId = @ApprovalId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ApprovalId", approvalId);
                        cmd.Parameters.AddWithValue("@ApprovedBy", approvedBy);
                        cmd.Parameters.AddWithValue("@ApprovalDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@RejectionReason", rejectionReason ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RejectRequest Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to ApprovalRequest object
        /// </summary>
        private ApprovalRequest MapReaderToApprovalRequest(SqlDataReader reader)
        {
            return new ApprovalRequest
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
    }
}
