namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// Base User class representing all users in the system (Student, SocietyHead, Admin)
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Role: Student, SocietyHead, or Admin
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Status: Active, Inactive, or Suspended
        /// </summary>
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets the full name of the user
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        public User() { }

        public User(string email, string passwordHash, string firstName, string lastName, string role)
        {
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Status = "Active";
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
