namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// Announcement entity representing society announcements and posts
    /// </summary>
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public int SocietyId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public Announcement() { }

        public Announcement(int societyId, string title, string content, int createdBy)
        {
            SocietyId = societyId;
            Title = title;
            Content = content;
            CreatedBy = createdBy;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the announcement (soft delete)
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            UpdatedDate = DateTime.Now;
        }
    }
}
