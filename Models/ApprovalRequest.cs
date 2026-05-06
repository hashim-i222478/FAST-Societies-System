namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// ApprovalRequest entity representing requests that require administrative approval
    /// (Events, Societies, Memberships)
    /// </summary>
    public class ApprovalRequest
    {
        public int ApprovalId { get; set; }

        /// <summary>
        /// Type: Event, Society, Membership
        /// </summary>
        public string RequestType { get; set; }

        public int RequesterId { get; set; }
        public int TargetId { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Status: Pending, Approved, Rejected
        /// </summary>
        public string Status { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RejectionReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ApprovalRequest() { }

        public ApprovalRequest(string requestType, int requesterId, int targetId, string description)
        {
            RequestType = requestType;
            RequesterId = requesterId;
            TargetId = targetId;
            Description = description;
            Status = "Pending";
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Approves the request
        /// </summary>
        public void Approve(int approvedBy)
        {
            Status = "Approved";
            ApprovedBy = approvedBy;
            ApprovalDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Rejects the request
        /// </summary>
        public void Reject(int approvedBy, string reason)
        {
            Status = "Rejected";
            ApprovedBy = approvedBy;
            RejectionReason = reason;
            ApprovalDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Checks if request is still pending
        /// </summary>
        public bool IsPending() => Status == "Pending";
    }
}
