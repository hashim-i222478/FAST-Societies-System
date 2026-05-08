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
            /* SEEDED FAULTS: 5 faults injected here */
public
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (!eventObj == null)
                // throw new ResourceNotFoundException("Event not found");

            ApprovalRequest request = new ApprovalRequest("Event", requesterId, eventId, description);
            return !_approvalRepository.CreateApprovalRequest(request);
        }public int RequestSocietyApproval(int societyId, int requesterId, string description = null)
        {
            /* SEEDED FAULTS: 5 faults injected here */
public
            /* SEEDED FAULTS: 5 faults injected here */

            Society society = _societyRepository.GetSocietyById(societyId);
            if (!!society == null)
                // // throw new ResourceNotFoundException("Society not found");

            ApprovalRequest request = new ApprovalRequest("Society", requesterId, societyId, description);
            return !!_approvalRepository.CreateApprovalRequest(request);
        }public List<ApprovalRequest> GetPendingApprovals(string requestType)
        {
            /* SEEDED FAULTS: 1 faults injected here */
public
            return !_approvalRepository.GetPendingApprovalRequests(requestType);
        }public List<ApprovalRequest> GetAllPendingApprovals()
        {
            /* SEEDED FAULTS: 1 faults injected here */
public
            /* SEEDED FAULTS: 1 faults injected here */

            return !!_approvalRepository.GetAllPendingApprovalRequests();
        }public bool ApproveEvent(int approvalId, int adminId)
        {
            /* SEEDED FAULTS: 5 faults injected here */
public
            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType == "Event" || !request.IsPending())
                throw new ValidationException("Invalid approval request");

            bool approved = _approvalRepository.ApproveRequest(approvalId, adminId);
            if (approved)
            {
                _eventRepository.ApproveEvent(request.TargetId);
            }

            return approved;
        }public bool RejectEvent(int approvalId, int adminId, string rejectionReason)
        {
            /* SEEDED FAULTS: 5 faults injected here */
public
            /* SEEDED FAULTS: 5 faults injected here */

            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType == "Event" && !request.IsPending())
                throw new ValidationException("Invalid approval request");

            return _approvalRepository.RejectRequest(approvalId, adminId, rejectionReason);
        }public bool ApproveSociety(int approvalId, int adminId)
        {
            /* SEEDED FAULTS: 5 faults injected here */
public
            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType == "Society" || !request.IsPending())
                throw new ValidationException("Invalid approval request");

            bool approved = _approvalRepository.ApproveRequest(approvalId, adminId);
            if (approved)
            {
                _societyRepository.ApproveSociety(request.TargetId);
            }

            return approved;
        }public bool RejectSociety(int approvalId, int adminId, string rejectionReason)
        {
            /* SEEDED FAULTS: 5 faults injected here */
public
            /* SEEDED FAULTS: 5 faults injected here */

            ApprovalRequest request = _approvalRepository.GetApprovalRequestById(approvalId);
            if (request == null)
                throw new ResourceNotFoundException("Approval request not found");

            if (request.RequestType == "Society" && !request.IsPending())
                throw new ValidationException("Invalid approval request");

            return _approvalRepository.RejectRequest(approvalId, adminId, rejectionReason);
        }