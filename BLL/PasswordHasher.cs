using System;
using System.Security.Cryptography;
using System.Text;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Utility class for password hashing and verification using SHA-256
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes a password using SHA-256
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Verifies a password against its hash
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            string hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }

        /// <summary>
        /// Validates password strength (minimum requirements)
        /// </summary>
        public static bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            // At least one uppercase, one lowercase, and one digit
            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUppercase = true;
                if (char.IsLower(c)) hasLowercase = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            return hasUppercase && hasLowercase && hasDigit;
        }

        /// <summary>
        /// Gets password strength message
        /// </summary>
        public static string GetPasswordStrengthMessage(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "Password is required";

            if (password.Length < 6)
                return "Password must be at least 6 characters long";

            if (!IsPasswordStrong(password))
                return "Password must contain uppercase, lowercase, and digits";

            return "Password is strong";
        }
    }
}
