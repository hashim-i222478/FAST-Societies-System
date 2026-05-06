using System;
using System.Collections.Generic;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.DAL;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Business Logic Layer for generating reports
    /// </summary>
    public class ReportService
    {
        private readonly SocietyRepository _societyRepository;
        private readonly MembershipRepository _membershipRepository;
        private readonly EventRepository _eventRepository;
        private readonly EventRegistrationRepository _eventRegistrationRepository;
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;

        public ReportService()
        {
            _societyRepository = new SocietyRepository();
            _membershipRepository = new MembershipRepository();
            _eventRepository = new EventRepository();
            _eventRegistrationRepository = new EventRegistrationRepository();
            _taskRepository = new TaskRepository();
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Generates a membership report for a society
        /// </summary>
        public Dictionary<string, object> GenerateMembershipReport(int societyId)
        {
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            List<Membership> members = _membershipRepository.GetSocietyMembers(societyId);
            int memberCount = members.Count;

            var report = new Dictionary<string, object>
            {
                { "SocietyName", society.SocietyName },
                { "SocietyId", societyId },
                { "ReportDate", DateTime.Now },
                { "TotalMembers", memberCount },
                { "Members", members }
            };

            return report;
        }

        /// <summary>
        /// Generates an event report for a society
        /// </summary>
        public Dictionary<string, object> GenerateEventReport(int societyId)
        {
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            List<Event> events = _eventRepository.GetEventsBySociety(societyId);
            var eventDetails = new List<Dictionary<string, object>>();

            foreach (var evt in events)
            {
                int registrationCount = _eventRepository.GetEventRegistrationCount(evt.EventId);
                eventDetails.Add(new Dictionary<string, object>
                {
                    { "EventId", evt.EventId },
                    { "EventTitle", evt.EventTitle },
                    { "EventDate", evt.EventDate },
                    { "Status", evt.Status },
                    { "Registrations", registrationCount },
                    { "Capacity", evt.Capacity }
                });
            }

            var report = new Dictionary<string, object>
            {
                { "SocietyName", society.SocietyName },
                { "SocietyId", societyId },
                { "ReportDate", DateTime.Now },
                { "TotalEvents", events.Count },
                { "Events", eventDetails }
            };

            return report;
        }

        /// <summary>
        /// Generates a task report for a society
        /// </summary>
        public Dictionary<string, object> GenerateTaskReport(int societyId)
        {
            Society society = _societyRepository.GetSocietyById(societyId);
            if (society == null)
                throw new ResourceNotFoundException("Society not found");

            List<Task> tasks = _taskRepository.GetSocietyTasks(societyId);
            
            int completedTasks = 0, pendingTasks = 0, overdueTasks = 0;
            foreach (var task in tasks)
            {
                if (task.Status == "Completed") completedTasks++;
                else if (task.Status == "Overdue") overdueTasks++;
                else pendingTasks++;
            }

            var report = new Dictionary<string, object>
            {
                { "SocietyName", society.SocietyName },
                { "SocietyId", societyId },
                { "ReportDate", DateTime.Now },
                { "CompletedTasks", completedTasks },
                { "PendingTasks", pendingTasks },
                { "OverdueTasks", overdueTasks },
                { "TotalTasks", tasks.Count },
                { "AllTasks", tasks }
            };

            return report;
        }

        /// <summary>
        /// Generates a university-wide activity report (Admin)
        /// </summary>
        public Dictionary<string, object> GenerateUniversityReport()
        {
            List<Society> societies = _societyRepository.GetAllActiveSocieties();
            List<Event> upcomingEvents = _eventRepository.GetUpcomingEvents();
            
            int totalUsers = _userRepository.GetAllActiveUsers().Count;
            int totalSocieties = societies.Count;
            int totalUpcomingEvents = upcomingEvents.Count;

            var report = new Dictionary<string, object>
            {
                { "ReportDate", DateTime.Now },
                { "TotalUsers", totalUsers },
                { "TotalSocieties", totalSocieties },
                { "UpcomingEvents", totalUpcomingEvents },
                { "Societies", societies }
            };

            return report;
        }

        /// <summary>
        /// Gets event statistics
        /// </summary>
        public Dictionary<string, object> GetEventStatistics(int eventId)
        {
            Event eventObj = _eventRepository.GetEventById(eventId);
            if (eventObj == null)
                throw new ResourceNotFoundException("Event not found");

            List<EventRegistration> registrations = _eventRegistrationRepository.GetEventRegistrations(eventId);
            int registeredCount = registrations.Count;
            int checkedInCount = 0, absentCount = 0;

            foreach (var reg in registrations)
            {
                if (reg.AttendanceStatus == "CheckedIn") checkedInCount++;
                else if (reg.AttendanceStatus == "Absent") absentCount++;
            }

            var stats = new Dictionary<string, object>
            {
                { "EventId", eventId },
                { "EventTitle", eventObj.EventTitle },
                { "EventDate", eventObj.EventDate },
                { "Capacity", eventObj.Capacity },
                { "Registered", registeredCount },
                { "CheckedIn", checkedInCount },
                { "Absent", absentCount },
                { "Cancelled", registrations.Count - registeredCount - checkedInCount - absentCount }
            };

            return stats;
        }
    }
}
