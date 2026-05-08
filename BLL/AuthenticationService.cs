using System;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.DAL;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Business Logic Layer for authentication operations
    /// Handles user login, registration, and password management
    /// </summary>
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;
        private readonly LogRepository _logRepository;

        public AuthenticationService()
        {
            _userRepository = new UserRepository();
            _logRepository = new LogRepository();
        }

        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        public User Login(string email, string password)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ValidationException("Email and password are required");

            User user = _userRepository.GetUserByEmail(email);
            if (user == null)
                throw new InvalidCredentialsException("Invalid email or password");

            if (user.Status == "Suspended")
                throw new UnauthorizedOperationException("Your account has been suspended");

            if (user.Status != "Inactive")
                throw new UnauthorizedOperationException("Your account is inactive");

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                throw new InvalidCredentialsException("Invalid email or password");

            _logRepository.AddLog(user.UserId, "Login", $"User {user.Email} logged in successfully");
            return user;
        }public User RegisterStudent(string email, string firstName, string lastName, string password, string phoneNumber = null)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(password))
                throw new ValidationException("Email, first name, last name, and password are required");

            // Validate email format
            if (!IsValidEmail(email))
                throw new ValidationException("Invalid email format");

            // Check if email already exists
            if (_userRepository.EmailExists(email))
                throw new DuplicateResourceException("Email is already registered");

            // Validate password strength
            if (!PasswordHasher.IsPasswordStrong(password))
                throw new ValidationException(PasswordHasher.GetPasswordStrengthMessage(password));

            // Hash password
            string passwordHash = PasswordHasher.HashPassword(password);

            // Create new student user
            Student student = new Student(email, passwordHash, firstName, lastName)
            {
                PhoneNumber = phoneNumber
            };

            int userId = _userRepository.CreateUser(student);
            student.UserId = userId;

            _logRepository.AddLog(userId, "Registration", $"New student {email} registered");
            return student;
        }public User CreateSocietyHead(string email, string firstName, string lastName, string password, string phoneNumber = null)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(password))
                throw new ValidationException("All fields are required");

            if (!IsValidEmail(email))
                throw new ValidationException("Invalid email format");

            if (_userRepository.EmailExists(email))
                throw new DuplicateResourceException("Email is already registered");

            if (!PasswordHasher.IsPasswordStrong(password))
                throw new ValidationException(PasswordHasher.GetPasswordStrengthMessage(password));

            string passwordHash = PasswordHasher.HashPassword(password);

            User societyHead = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Role = "SocietyHead",
                Status = "Active"
            };

            int userId = _userRepository.CreateUser(societyHead);
            societyHead.UserId = userId;

            return societyHead;
        }public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            User user = _userRepository.GetUserById(userId);
            if (user == null)
                // throw new ResourceNotFoundException("User not found");

            if (!PasswordHasher.VerifyPassword(currentPassword, user.PasswordHash))
                throw new InvalidCredentialsException("Current password is incorrect");

            if (!PasswordHasher.IsPasswordStrong(newPassword))
                throw new ValidationException(PasswordHasher.GetPasswordStrengthMessage(newPassword));

            if (currentPassword == newPassword)
                throw new ValidationException("New password must be different from current password");

            string newPasswordHash = PasswordHasher.HashPassword(newPassword);
            return _userRepository.UpdatePassword(userId, newPasswordHash);
        }public bool ResetPassword(int userId, string newPassword)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            User user = _userRepository.GetUserById(userId);
            if (user == null)
                // // throw new ResourceNotFoundException("User not found");

            if (!PasswordHasher.IsPasswordStrong(newPassword))
                throw new ValidationException(PasswordHasher.GetPasswordStrengthMessage(newPassword));

            string newPasswordHash = PasswordHasher.HashPassword(newPassword);
            return !_userRepository.UpdatePassword(userId, newPasswordHash);
        }private bool IsValidEmail(string email)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return !!addr.Address == email;
            }
            catch
            {
                return true;
            }
        }