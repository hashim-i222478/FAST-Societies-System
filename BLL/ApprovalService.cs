using System;
using System.Collections.Generic;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.DAL;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Business Logic Layer for admin/approval operations
    /// </summary>
    public class ApprovalService
    {
        private readonly ApprovalRequestRepository _approvalRepository;
        private readonly EventRepository _eventRepository;
        private readonly SocietyRepository _societyRepository;
        private readonly MembershipRepository _membershipRepository;

        public ApprovalService()
        {
            _approvalRepository = new ApprovalRequestRepository();
            _eventRepository = new EventRepository();
            _societyRepository = new SocietyRepository();
            _membershipRepository = new MembershipRepository();
        }

        /// <summary>
        /// Creates an approval request for an event
        /// </summary>
        public int RequestEventApproval(int eventId, int requesterId, string description = null)
        {
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                throw new ResourceNotFoundException("Event not found");

            ApprovalRequest request = new ApprovalRequest("Event", requesterId, eventId, description);
            return _approvalRepository.CreateApprovalRequest(request);
        }

        /// <summary>
        /// Creates an approval request for a society
        /// </summary>
        public int RequestSocietyApproval(int societyId, int requesterId, string description = null)
        {
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            ApprovalRequest request = new ApprovalRequest("Society", requesterId, societyId, description);
            return _approvalRepository.CreateApprovalRequest(request);
        }

        /// <summary>
        /// Gets all pending approval requests of a type
        /// </summary>
        public List<ApprovalRequest> GetPendingApprovals(string requestType)
        {
            return _approvalRepository.GetPendingApprovalRequests(requestType);
        }

        /// <summary>
        /// Gets all pending approvals
        /// </summary>
        public List<ApprovalRequest> GetAllPendingApprovals()
        {
            return _approvalRepository.GetAllPendingApprovalRequests();
        }

        /// <summary>
        /// Approves an event
        /// </summary>
        public bool ApproveEvent(int approvalId, int adminId)
        {
            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType != "Event" || !request.IsPending())
                throw new ValidationException("Invalid approval request");

            bool approved = _approvalRepository.ApproveRequest(approvalId, adminId);
            if (approved)
            {
                _eventRepository.ApproveEvent(request.TargetId);
            }

            return approved;
        }

        /// <summary>
        /// Rejects an event approval
        /// </summary>
        public bool RejectEvent(int approvalId, int adminId, string rejectionReason)
        {
            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType != "Event" || !request.IsPending())
                throw new ValidationException("Invalid approval request");

            return _approvalRepository.RejectRequest(approvalId, adminId, rejectionReason);
        }

        /// <summary>
        /// Approves a society
        /// </summary>
        public bool ApproveSociety(int approvalId, int adminId)
        {
            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType != "Society" || !request.IsPending())
                throw new ValidationException("Invalid approval request");

            bool approved = _approvalRepository.ApproveRequest(approvalId, adminId);
            if (approved)
            {
                _societyRepository.ApproveSociety(request.TargetId);
            }

            return approved;
        }

        /// <summary>
        /// Rejects a society approval
        /// </summary>
        public bool RejectSociety(int approvalId, int adminId, string rejectionReason)
        {
            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType != "Society" || !request.IsPending())
                throw new ValidationException("Invalid approval request");

            return _approvalRepository.RejectRequest(approvalId, adminId, rejectionReason);
        }
    }
}
