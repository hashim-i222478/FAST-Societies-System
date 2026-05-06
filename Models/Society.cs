namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// Society entity representing a student society/club
    /// </summary>
    public class Society
    {
        public int SocietyId { get; set; }
        public string SocietyName { get; set; }
        public string Description { get; set; }
        public int HeadId { get; set; }
        public string Logo { get; set; }

        /// <summary>
        /// Status: Pending, Approved, Active, Suspended, Inactive
        /// </summary>
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Society() { }

        public Society(string societyName, string description, int headId)
        {
            SocietyName = societyName;
            Description = description;
            HeadId = headId;
            Status = "Pending";
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
