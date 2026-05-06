using System;
using System.Collections.Generic;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.DAL;

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

        public StudentService()
        {
            _societyRepository = new SocietyRepository();
            _membershipRepository = new MembershipRepository();
            _eventRepository = new EventRepository();
            _eventRegistrationRepository = new EventRegistrationRepository();
        }

        /// <summary>
        /// Gets all active societies that a student can browse
        /// </summary>
        public List<Society> BrowseSocieties()
        {
            return _societyRepository.GetAllActiveSocieties();
        }

        /// <summary>
        /// Applies for membership in a society
        /// </summary>
        public int ApplyForMembership(int studentId, int societyId)
        {
            // Check if society exists and is active
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            if (society.Status != "Active" && society.Status != "Approved")
                throw new ValidationException("This society is not active");

            // Check if student is already a member
            if (_membershipRepository.IsMember(studentId, societyId))
                throw new DuplicateResourceException("You are already a member of this society");

            // Create membership request (Pending status)
            Membership membership = new Membership(studentId, societyId);
            return _membershipRepository.CreateMembership(membership);
        }

        /// <summary>
        /// Gets all societies a student is member of
        /// </summary>
        public List<Membership> GetMyMemberships(int studentId)
        {
            return _membershipRepository.GetStudentMemberships(studentId);
        }

        /// <summary>
        /// Gets membership status in a specific society
        /// </summary>
        public Membership GetMembershipStatus(int studentId, int societyId)
        {
            var memberships = _membershipRepository.GetStudentMemberships(studentId);
            foreach (var m in memberships)
            {
                if (m.SocietyId == societyId)
                    return m;
            }
            return null;
        }

        /// <summary>
        /// Gets all upcoming events
        /// </summary>
        public List<Event> GetUpcomingEvents()
        {
            return _eventRepository.GetUpcomingEvents();
        }

        /// <summary>
        /// Gets upcoming events for a specific society
        /// </summary>
        public List<Event> GetUpcomingEventsBySociety(int societyId)
        {
            List<Event> allEvents = _eventRepository.GetEventsBySociety(societyId);
            List<Event> upcomingEvents = new List<Event>();

            foreach (var evt in allEvents)
            {
                if (evt.IsUpcoming() && (evt.Status == "Approved" || evt.Status == "Scheduled"))
                {
                    upcomingEvents.Add(evt);
                }
            }

            return upcomingEvents;
        }

        /// <summary>
        /// Registers student for an event
        /// </summary>
        public int RegisterForEvent(int studentId, int eventId)
        {
            // Check if event exists
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                throw new ResourceNotFoundException("Event not found");

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
        }

        /// <summary>
        /// Gets all event registrations for a student
        /// </summary>
        public List<EventRegistration> GetMyEventRegistrations(int studentId)
        {
            return _eventRegistrationRepository.GetStudentRegistrations(studentId);
        }

        /// <summary>
        /// Cancels event registration
        /// </summary>
        public bool CancelEventRegistration(int registrationId, int studentId)
        {
            EventRegistration registration = _eventRegistrationRepository.GetRegistrationById(registrationId);
            if (registration == null)
                throw new ResourceNotFoundException("Registration not found");

            if (registration.StudentId != studentId)
                throw new UnauthorizedOperationException("You cannot cancel this registration");

            if (registration.AttendanceStatus == "CheckedIn")
                throw new ValidationException("You cannot cancel registration after check-in");

            return _eventRegistrationRepository.CancelRegistration(registrationId);
        }

        /// <summary>
        /// Retrieves ticket information for an event
        /// </summary>
        public EventRegistration GetEventTicket(string ticketId)
        {
            return _eventRegistrationRepository.GetRegistrationByTicket(ticketId);
        }

        /// <summary>
        /// Leaves a society (removes membership)
        /// </summary>
        public bool LeaveSociety(int membershipId, int studentId)
        {
            Membership membership = _membershipRepository.GetMembershipById(membershipId);
            if (membership == null)
                throw new ResourceNotFoundException("Membership not found");

            if (membership.StudentId != studentId)
                throw new UnauthorizedOperationException("You cannot perform this action");

            return _membershipRepository.RemoveMembership(membershipId);
        }

        /// <summary>
        /// Gets society details by ID
        /// </summary>
        public Society GetSocietyDetails(int societyId)
        {
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            return society;
        }

        /// <summary>
        /// Gets event details by ID
        /// </summary>
        public Event GetEventDetails(int eventId)
        {
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                throw new ResourceNotFoundException("Event not found");

            return eventObj;
        }
    }
}
