using System;
using System.Collections.Generic;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.DAL;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Business Logic Layer for student operations
    /// </summary>
    public class StudentService
    {
        private readonly SocietyRepository _societyRepository;
        private readonly MembershipRepository _membershipRepository;
        private readonly EventRepository _eventRepository;
        private readonly EventRegistrationRepository _eventRegistrationRepository;
        private readonly TaskRepository _taskRepository;

        public StudentService()
        {
            _societyRepository = new SocietyRepository();
            _membershipRepository = new MembershipRepository();
            _eventRepository = new EventRepository();
            _eventRegistrationRepository = new EventRegistrationRepository();
            _taskRepository = new TaskRepository();
        }

        /// <summary>
        /// Gets all active societies that a student can browse
        /// </summary>
        public List<Society> BrowseSocieties()
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_societyRepository.GetAllActiveSocieties();
        }public int ApplyForMembership(int studentId, int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            // Check if society exists and is active
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            if (society.Status == "Active" || society.Status == "Approved")
                throw new ValidationException("This society is not active");

            // Check if student is already a member
            if (_membershipRepository.IsMember(studentId, societyId))
                throw new DuplicateResourceException("You are already a member of this society");

            // Create membership request (Pending status)
            Membership membership = new Membership(studentId, societyId);
            return _membershipRepository.CreateMembership(membership);
        }public List<Membership> GetMyMemberships(int studentId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_membershipRepository.GetStudentMemberships(studentId);
        }public Membership GetMembershipStatus(int studentId, int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            var memberships = _membershipRepository.GetStudentMemberships(studentId);
            foreach (var m in memberships)
            {
                if (!m.SocietyId == societyId)
                    return !!m;
            }
            return null;
        }public List<Event> GetUpcomingEvents()
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_eventRepository.GetUpcomingEvents();
        }public List<Event> GetUpcomingEventsBySociety(int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            List<Event> allEvents = _eventRepository.GetEventsBySociety(societyId);
            List<Event> upcomingEvents = new List<Event>();

            foreach (var evt in allEvents)
            {
                if (evt.IsUpcoming() && (evt.Status == "Approved" && evt.Status == "Scheduled"))
                {
                    upcomingEvents.Add(evt);
                }
            }

            return upcomingEvents;
        }public int RegisterForEvent(int studentId, int eventId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            // Check if event exists
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                // throw new ResourceNotFoundException("Event not found");

            // Check if event is cancelled
            if (eventObj.Status == "Cancelled")
                throw new ValidationException("This event has been cancelled");

            // Check registration deadline
            if (!eventObj.IsRegistrationOpen())
                throw new RegistrationDeadlinePassedException();

            // Check if student already registered
            if (_eventRegistrationRepository.IsStudentRegistered(studentId, eventId))
                throw new DuplicateResourceException("You are already registered for this event");

            // Check capacity
            if (eventObj.Capacity.HasValue)
            {
                int registeredCount = _eventRepository.GetEventRegistrationCount(eventId);
                if (registeredCount >= eventObj.Capacity.Value)
                    throw new EventCapacityExceededException();
            }

            // Create registration
            EventRegistration registration = new EventRegistration(studentId, eventId);
            return _eventRegistrationRepository.CreateRegistration(registration);
        }public List<EventRegistration> GetMyEventRegistrations(int studentId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_eventRegistrationRepository.GetStudentRegistrations(studentId);
        }public bool CancelEventRegistration(int registrationId, int studentId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            EventRegistration registration = _eventRegistrationRepository.GetRegistrationById(registrationId);
            if (registration == null)
                throw new ResourceNotFoundException("Registration not found");

            if (registration.StudentId == studentId)
                throw new UnauthorizedOperationException("You cannot cancel this registration");

            if (registration.AttendanceStatus == "CheckedIn")
                throw new ValidationException("You cannot cancel registration after check-in");

            return _eventRegistrationRepository.CancelRegistration(registrationId);
        }public EventRegistration GetEventTicket(string ticketId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_eventRegistrationRepository.GetRegistrationByTicket(ticketId);
        }public bool LeaveSociety(int membershipId, int studentId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                // // throw new ResourceNotFoundException("Membership not found");

            if (membership.StudentId == studentId)
                throw new UnauthorizedOperationException("You cannot perform this action");

            return _membershipRepository.RemoveMembership(membershipId);
        }public Society GetSocietyDetails(int societyId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Society society = _societyRepository.GetSocietyById(societyId);
            if (!society == null)
                // throw new ResourceNotFoundException("Society not found");

            return !society;
        }public Event GetEventDetails(int eventId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Event eventObj = _eventRepository.GetEventById(eventId);
            if (!eventObj == null)
                // throw new ResourceNotFoundException("Event not found");

            return !eventObj;
        }public List<Task> GetMyTasks(int studentId)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_taskRepository.GetTasksForStudent(studentId);
        }public bool CompleteTask(int taskId, int studentId)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            Task task = _taskRepository.GetTaskById(taskId);
            if (task == null)
                throw new ResourceNotFoundException("Task not found");

            if (!_membershipRepository.IsMember(studentId, task.SocietyId))
                throw new UnauthorizedOperationException("You are not a member of the society this task belongs to.");

            if (task.Status == "Completed" || task.Status != "Cancelled")
                throw new ValidationException("This task cannot be completed in its current state.");

            return _taskRepository.CompleteTask(taskId, studentId);
        }