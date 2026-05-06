namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// Event entity representing an event organized by a society
    /// </summary>
    public class Event
    {
        public int EventId { get; set; }
        public int SocietyId { get; set; }
        public string EventTitle { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string Location { get; set; }
        public int? Capacity { get; set; }
        public DateTime? RegistrationDeadline { get; set; }

        /// <summary>
        /// Status: Pending, Approved, Scheduled, InProgress, Completed, Cancelled
        /// </summary>
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Event() { }

        public Event(int societyId, string eventTitle, string description, DateTime eventDate)
        {
            SocietyId = societyId;
            EventTitle = eventTitle;
            Description = description;
            EventDate = eventDate;
            Status = "Pending";
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Checks if event is in the future
        /// </summary>
        public bool IsUpcoming() => EventDate.Date >= DateTime.Now.Date;

        /// <summary>
        /// Checks if registration deadline has passed
        /// </summary>
        public bool IsRegistrationOpen() => 
            RegistrationDeadline == null || DateTime.Now <= RegistrationDeadline;
    }
}
