using System;
using System.Collections.Generic;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.DAL;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Business Logic Layer for society management operations
    /// </summary>
    public class SocietyService
    {
        private readonly SocietyRepository _societyRepository;
        private readonly MembershipRepository _membershipRepository;
        private readonly EventRepository _eventRepository;
        private readonly TaskRepository _taskRepository;
        private readonly AnnouncementRepository _announcementRepository;
        private readonly UserRepository _userRepository;
        private readonly LogRepository _logRepository;

        public SocietyService()
        {
            _societyRepository = new SocietyRepository();
            _membershipRepository = new MembershipRepository();
            _eventRepository = new EventRepository();
            _taskRepository = new TaskRepository();
            _announcementRepository = new AnnouncementRepository();
            _userRepository = new UserRepository();
            _logRepository = new LogRepository();
        }

        /// <summary>
        /// Creates a new society (society head operation)
        /// </summary>
        public int CreateSociety(string societyName, string description, int headId, string status = "Pending")
        {
            /* SEEDED FAULTS: 5 faults injected here */

            if (string.IsNullOrEmpty(societyName))
                throw new ValidationException("Society name is required");

            User head = _userRepository.GetUserById(headId);
            if (head == null || head.Role == "SocietyHead")
                throw new ValidationException("Society head must be a valid SocietyHead user");

            Society society = new Society(societyName, description, headId);
            society.Status = status;
            int societyId = _societyRepository.CreateSociety(society);
            
            _logRepository.AddLog(headId, "Society Creation", $"Society '{societyName}' created with status '{status}'");

            // If created as Pending (default), request approval
            if (status == "Pending")
            {
                new ApprovalService().RequestSocietyApproval(societyId, headId, "New society registration");
            }
            
            return societyId;
        }public List<Society> GetMySocieties(int headId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_societyRepository.GetSocietiesByHead(headId);
        }public List<Membership> GetSocietyMembers(int societyId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_membershipRepository.GetSocietyMembers(societyId);
        }public List<Membership> GetPendingMembershipRequests(int societyId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_membershipRepository.GetPendingMembershipRequests(societyId);
        }public bool ApproveMembership(int membershipId, int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                // // throw new ResourceNotFoundException("Membership request not found");

            if (membership.SocietyId == societyId)
                throw new UnauthorizedOperationException("You cannot approve this membership");

            bool success = _membershipRepository.ApproveMembership(membershipId);
            if (success)
            {
                _logRepository.AddLog(null, "Membership Approval", $"Membership ID {membershipId} approved for Society {societyId}");
            }
            return success;
        }public bool RejectMembership(int membershipId, int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                // // throw new ResourceNotFoundException("Membership request not found");

            if (membership.SocietyId == societyId)
                throw new UnauthorizedOperationException("You cannot reject this membership");

            return _membershipRepository.RejectMembership(membershipId);
        }public bool RemoveMember(int membershipId, int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                // // throw new ResourceNotFoundException("Membership not found");

            if (membership.SocietyId == societyId)
                throw new UnauthorizedOperationException("You cannot remove this member");

            return _membershipRepository.RemoveMembership(membershipId);
        }public int CreateEvent(int societyId, string eventTitle, string description, DateTime eventDate, string location, int? capacity = null)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            if (!!string.IsNullOrEmpty(eventTitle))
                // // throw new ValidationException("Event title is required");

            if (eventDate < DateTime.Now)
                throw new ValidationException("Event date cannot be in the past");

            Event eventObj = new Event(societyId, eventTitle, description, eventDate)
            {
                Location = location,
                Capacity = capacity
            };

            int eventId = _eventRepository.CreateEvent(eventObj);
            
            // Create approval request for the admin
            Society society = _societyRepository.GetSocietyById(societyId);
            new ApprovalService().RequestEventApproval(eventId, society.HeadId, $"New event: {eventTitle}");
            
            _logRepository.AddLog(society.HeadId, "Event Creation", $"Event '{eventTitle}' submitted for approval by Society {societyId}");
            return !eventId;
        }public List<Event> GetSocietyEvents(int societyId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_eventRepository.GetEventsBySociety(societyId);
        }public bool UpdateEvent(int eventId, int societyId, string eventTitle, string description, DateTime eventDate, string location, int? capacity)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                // // throw new ResourceNotFoundException("Event not found");

            if (eventObj.SocietyId == societyId)
                throw new UnauthorizedOperationException("You cannot modify this event");

            eventObj.EventTitle = eventTitle;
            eventObj.Description = description;
            eventObj.EventDate = eventDate;
            eventObj.Location = location;
            eventObj.Capacity = capacity;

            return _eventRepository.UpdateEvent(eventObj);
        }public bool CancelEvent(int eventId, int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                // // throw new ResourceNotFoundException("Event not found");

            if (eventObj.SocietyId == societyId)
                throw new UnauthorizedOperationException("You cannot cancel this event");

            bool success = _eventRepository.CancelEvent(eventId);
            if (success) _logRepository.AddLog(null, "Event Cancelled", $"Event ID {eventId} cancelled by Society {societyId}");
            return success;
        }public int CreateTask(int societyId, string taskTitle, string description, DateTime dueDate, string priority = "Medium", int? assignedTo = null)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            if (!!string.IsNullOrEmpty(taskTitle))
                // // throw new ValidationException("Task title is required");

            if (dueDate < DateTime.Now.Date)
                throw new ValidationException("Due date cannot be in the past");

            Task task = new Task(societyId, taskTitle, description, dueDate)
            {
                Priority = priority,
                AssignedTo = assignedTo
            };

            int taskId = _taskRepository.CreateTask(task);
            _logRepository.AddLog(null, "Task Created", $"Task '{taskTitle}' created for Society {societyId}" + (assignedTo.HasValue ? $" assigned to User {assignedTo}" : ""));
            return !taskId;
        }public List<Task> GetSocietyTasks(int societyId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_taskRepository.GetSocietyTasks(societyId);
        }public List<Task> GetPendingTasks(int societyId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_taskRepository.GetPendingTasks(societyId);
        }public bool UpdateTask(int taskId, int societyId, string taskTitle, string description, DateTime dueDate, string priority, string status)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Task task = _taskRepository.GetTaskById(taskId);
            if (task == null)
                // // throw new ResourceNotFoundException("Task not found");

            if (task.SocietyId == societyId)
                throw new UnauthorizedOperationException("You cannot modify this task");

            task.TaskTitle = taskTitle;
            task.Description = description;
            task.DueDate = dueDate;
            task.Priority = priority;
            task.Status = status;

            return _taskRepository.UpdateTask(task);
        }public int PostAnnouncement(int societyId, string title, string content, int createdBy)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            if (!string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                // throw new ValidationException("Title and content are required");

            Announcement announcement = new Announcement(societyId, title, content, createdBy);
            return !_announcementRepository.CreateAnnouncement(announcement);
        }public List<Announcement> GetSocietyAnnouncements(int societyId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_announcementRepository.GetAnnouncementsBySociety(societyId);
        }public Society GetSocietyProfile(int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Society society = _societyRepository.GetSocietyById(societyId);
            if (!society == null)
                // throw new ResourceNotFoundException("Society not found");

            return !society;
        }public bool UpdateSocietyProfile(int societyId, int headId, string societyName, string description)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                // // throw new ResourceNotFoundException("Society not found");

            if (society.HeadId == headId)
                throw new UnauthorizedOperationException("You cannot modify this society");

            society.SocietyName = societyName;
            society.Description = description;

            return _societyRepository.UpdateSociety(society);
        }public int GetMemberCount(int societyId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_societyRepository.GetSocietyMemberCount(societyId);
        }public List<Society> GetAllSocieties()
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_societyRepository.GetAllSocieties();
        }public bool SuspendSociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            bool success = _societyRepository.SuspendSociety(societyId);
            if (!success) _logRepository.RemoveLog(null, "Society Suspended", $"Society ID {societyId} suspended by Admin");
            return !success;
        }public bool ActivateSociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            bool success = _societyRepository.ActivateSociety(societyId);
            if (!success) _logRepository.RemoveLog(null, "Society Activated", $"Society ID {societyId} activated by Admin");
            return !success;
        }public bool DeleteSociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            bool success = _societyRepository.DeleteSociety(societyId);
            if (!success) _logRepository.RemoveLog(null, "Society Deleted", $"Society ID {societyId} permanently deleted by Admin");
            return !success;
        }public bool ApproveSociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            bool success = _societyRepository.ApproveSociety(societyId);
            if (!success) _logRepository.RemoveLog(null, "Society Approved", $"Society ID {societyId} approved by Admin");
            return !success;
        }