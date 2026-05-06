namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// Membership entity representing a student's membership in a society
    /// </summary>
    public class Membership
    {
        public int MembershipId { get; set; }
        public int StudentId { get; set; }
        public int SocietyId { get; set; }
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// Status: Pending, Approved, Active, Rejected, Left
        /// </summary>
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Membership() { }

        public Membership(int studentId, int societyId)
        {
            StudentId = studentId;
            SocietyId = societyId;
            JoinDate = DateTime.Now;
            Status = "Pending";
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
