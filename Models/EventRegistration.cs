namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// EventRegistration entity representing a student's registration for an event
    /// </summary>
    public class EventRegistration
    {
        public int RegistrationId { get; set; }
        public int StudentId { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string TicketId { get; set; }

        /// <summary>
        /// Status: Registered, CheckedIn, Absent, Cancelled
        /// </summary>
        public string AttendanceStatus { get; set; }

        public DateTime? CheckInDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public EventRegistration() { }

        public EventRegistration(int studentId, int eventId)
        {
            StudentId = studentId;
            EventId = eventId;
            RegistrationDate = DateTime.Now;
            AttendanceStatus = "Registered";
            CreatedDate = DateTime.Now;
            TicketId = GenerateTicketId();
        }

        /// <summary>
        /// Generates a unique ticket ID
        /// </summary>
        private string GenerateTicketId()
        {
            return $"TICKET-{EventId}-{StudentId}-{DateTime.Now.Ticks}";
        }

        /// <summary>
        /// Marks student as checked-in to the event
        /// </summary>
        public void CheckIn()
        {
            AttendanceStatus = "CheckedIn";
            CheckInDate = DateTime.Now;
        }

        /// <summary>
        /// Cancels the registration
        /// </summary>
        public void Cancel()
        {
            AttendanceStatus = "Cancelled";
        }
    }
}
