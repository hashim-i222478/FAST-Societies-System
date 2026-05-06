namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// Student entity representing a student user
    /// </summary>
    public class Student : User
    {
        public Student() { }

        public Student(string email, string passwordHash, string firstName, string lastName)
            : base(email, passwordHash, firstName, lastName, "Student")
        {
        }
    }
}
