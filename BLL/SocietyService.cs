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

        public SocietyService()
        {
            _societyRepository = new SocietyRepository();
            _membershipRepository = new MembershipRepository();
            _eventRepository = new EventRepository();
            _taskRepository = new TaskRepository();
            _announcementRepository = new AnnouncementRepository();
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Creates a new society (society head operation)
        /// </summary>
        public int CreateSociety(string societyName, string description, int headId, string status = "Pending")
        {
            if (string.IsNullOrEmpty(societyName))
                throw new ValidationException("Society name is required");

            User head = _userRepository.GetUserById(headId);
            if (head == null || head.Role != "SocietyHead")
                throw new ValidationException("Society head must be a valid SocietyHead user");

            Society society = new Society(societyName, description, headId);
            society.Status = status;
            int societyId = _societyRepository.CreateSociety(society);
            
            // If created as Pending (default), request approval
            if (status == "Pending")
            {
                new ApprovalService().RequestSocietyApproval(societyId, headId, "New society registration");
            }
            
            return societyId;
        }

        /// <summary>
        /// Gets all societies managed by a specific head
        /// </summary>
        public List<Society> GetMySocieties(int headId)
        {
            return _societyRepository.GetSocietiesByHead(headId);
        }

        /// <summary>
        /// Gets all members of a society
        /// </summary>
        public List<Membership> GetSocietyMembers(int societyId)
        {
            return _membershipRepository.GetSocietyMembers(societyId);
        }

        /// <summary>
        /// Gets pending membership requests
        /// </summary>
        public List<Membership> GetPendingMembershipRequests(int societyId)
        {
            return _membershipRepository.GetPendingMembershipRequests(societyId);
        }

        /// <summary>
        /// Approves a membership request
        /// </summary>
        public bool ApproveMembership(int membershipId, int societyId)
        {
            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                throw new ResourceNotFoundException("Membership request not found");

            if (membership.SocietyId != societyId)
                throw new UnauthorizedOperationException("You cannot approve this membership");

            return _membershipRepository.ApproveMembership(membershipId);
        }

        /// <summary>
        /// Rejects a membership request
        /// </summary>
        public bool RejectMembership(int membershipId, int societyId)
        {
            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                throw new ResourceNotFoundException("Membership request not found");

            if (membership.SocietyId != societyId)
                throw new UnauthorizedOperationException("You cannot reject this membership");

            return _membershipRepository.RejectMembership(membershipId);
        }

        /// <summary>
        /// Removes a member from the society
        /// </summary>
        public bool RemoveMember(int membershipId, int societyId)
        {
            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                throw new ResourceNotFoundException("Membership not found");

            if (membership.SocietyId != societyId)
                throw new UnauthorizedOperationException("You cannot remove this member");

            return _membershipRepository.RemoveMembership(membershipId);
        }

        /// <summary>
        /// Creates a new event for the society
        /// </summary>
        public int CreateEvent(int societyId, string eventTitle, string description, DateTime eventDate, string location, int? capacity = null)
        {
            if (string.IsNullOrEmpty(eventTitle))
                throw new ValidationException("Event title is required");

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
            
            return eventId;
        }

        /// <summary>
        /// Gets all events of a society
        /// </summary>
        public List<Event> GetSocietyEvents(int societyId)
        {
            return _eventRepository.GetEventsBySociety(societyId);
        }

        /// <summary>
        /// Updates event details
        /// </summary>
        public bool UpdateEvent(int eventId, int societyId, string eventTitle, string description, DateTime eventDate, string location, int? capacity)
        {
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                throw new ResourceNotFoundException("Event not found");

            if (eventObj.SocietyId != societyId)
                throw new UnauthorizedOperationException("You cannot modify this event");

            eventObj.EventTitle = eventTitle;
            eventObj.Description = description;
            eventObj.EventDate = eventDate;
            eventObj.Location = location;
            eventObj.Capacity = capacity;

            return _eventRepository.UpdateEvent(eventObj);
        }

        /// <summary>
        /// Cancels an event
        /// </summary>
        public bool CancelEvent(int eventId, int societyId)
        {
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                throw new ResourceNotFoundException("Event not found");

            if (eventObj.SocietyId != societyId)
                throw new UnauthorizedOperationException("You cannot cancel this event");

            return _eventRepository.CancelEvent(eventId);
        }

        /// <summary>
        /// Creates a task within the society
        /// </summary>
        public int CreateTask(int societyId, string taskTitle, string description, DateTime dueDate, string priority = "Medium")
        {
            if (string.IsNullOrEmpty(taskTitle))
                throw new ValidationException("Task title is required");

            if (dueDate < DateTime.Now.Date)
                throw new ValidationException("Due date cannot be in the past");

            Task task = new Task(societyId, taskTitle, description, dueDate)
            {
                Priority = priority
            };

            return _taskRepository.CreateTask(task);
        }

        /// <summary>
        /// Gets all tasks for a society
        /// </summary>
        public List<Task> GetSocietyTasks(int societyId)
        {
            return _taskRepository.GetSocietyTasks(societyId);
        }

        /// <summary>
        /// Gets pending tasks
        /// </summary>
        public List<Task> GetPendingTasks(int societyId)
        {
            return _taskRepository.GetPendingTasks(societyId);
        }

        /// <summary>
        /// Updates a task
        /// </summary>
        public bool UpdateTask(int taskId, int societyId, string taskTitle, string description, DateTime dueDate, string priority, string status)
        {
            Task task = _taskRepository.GetTaskById(taskId);
            if (task == null)
                throw new ResourceNotFoundException("Task not found");

            if (task.SocietyId != societyId)
                throw new UnauthorizedOperationException("You cannot modify this task");

            task.TaskTitle = taskTitle;
            task.Description = description;
            task.DueDate = dueDate;
            task.Priority = priority;
            task.Status = status;

            return _taskRepository.UpdateTask(task);
        }

        /// <summary>
        /// Posts an announcement
        /// </summary>
        public int PostAnnouncement(int societyId, string title, string content, int createdBy)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                throw new ValidationException("Title and content are required");

            Announcement announcement = new Announcement(societyId, title, content, createdBy);
            return _announcementRepository.CreateAnnouncement(announcement);
        }

        /// <summary>
        /// Gets announcements for a society
        /// </summary>
        public List<Announcement> GetSocietyAnnouncements(int societyId)
        {
            return _announcementRepository.GetAnnouncementsBySociety(societyId);
        }

        /// <summary>
        /// Gets society profile
        /// </summary>
        public Society GetSocietyProfile(int societyId)
        {
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            return society;
        }

        /// <summary>
        /// Updates society profile
        /// </summary>
        public bool UpdateSocietyProfile(int societyId, int headId, string societyName, string description)
        {
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            if (society.HeadId != headId)
                throw new UnauthorizedOperationException("You cannot modify this society");

            society.SocietyName = societyName;
            society.Description = description;

            return _societyRepository.UpdateSociety(society);
        }

        /// <summary>
        /// Gets member count
        /// </summary>
        public int GetMemberCount(int societyId)
        {
            return _societyRepository.GetSocietyMemberCount(societyId);
        }

        /// <summary>
        /// Gets all societies in the system (Admin only)
        /// </summary>
        public List<Society> GetAllSocieties()
        {
            return _societyRepository.GetAllSocieties();
        }

        /// <summary>
        /// Suspends a society
        /// </summary>
        public bool SuspendSociety(int societyId)
        {
            return _societyRepository.SuspendSociety(societyId);
        }

        /// <summary>
        /// Activates/Unsuspends a society
        /// </summary>
        public bool ActivateSociety(int societyId)
        {
            return _societyRepository.ActivateSociety(societyId);
        }

        /// <summary>
        /// Deletes a society
        /// </summary>
        public bool DeleteSociety(int societyId)
        {
            return _societyRepository.DeleteSociety(societyId);
        }

        /// <summary>
        /// Approves a society
        /// </summary>
        public bool ApproveSociety(int societyId)
        {
            return _societyRepository.ApproveSociety(societyId);
        }
    }
}
