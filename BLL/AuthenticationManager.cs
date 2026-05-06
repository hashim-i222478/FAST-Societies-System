using System;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Manages user authentication sessions and authorization
    /// Singleton pattern to maintain current user state throughout application lifecycle
    /// </summary>
    public sealed class AuthenticationManager
    {
        private static readonly Lazy<AuthenticationManager> _instance = 
            new Lazy<AuthenticationManager>(() => new AuthenticationManager());

        public static AuthenticationManager Instance => _instance.Value;

        private User _currentUser;

        private AuthenticationManager() { }

        /// <summary>
        /// Gets the currently logged-in user
        /// </summary>
        public User CurrentUser => _currentUser;

        /// <summary>
        /// Checks if a user is currently logged in
        /// </summary>
        public bool IsAuthenticated => _currentUser != null;

        /// <summary>
        /// Gets the current user's role
        /// </summary>
        public string CurrentUserRole => _currentUser?.Role;

        /// <summary>
        /// Gets the current user's ID
        /// </summary>
        public int? CurrentUserId => _currentUser?.UserId;

        /// <summary>
        /// Logs in a user (sets current user)
        /// </summary>
        public void Login(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _currentUser = user;
            System.Diagnostics.Debug.WriteLine($"User logged in: {user.Email} ({user.Role})");
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public void Logout()
        {
            if (_currentUser != null)
            {
                System.Diagnostics.Debug.WriteLine($"User logged out: {_currentUser.Email}");
                _currentUser = null;
            }
        }

        /// <summary>
        /// Checks if current user has a specific role
        /// </summary>
        public bool HasRole(string role)
        {
            return IsAuthenticated && _currentUser.Role == role;
        }

        /// <summary>
        /// Checks if current user is a Student
        /// </summary>
        public bool IsStudent => HasRole("Student");

        /// <summary>
        /// Checks if current user is a Society Head
        /// </summary>
        public bool IsSocietyHead => HasRole("SocietyHead");

        /// <summary>
        /// Checks if current user is an Admin
        /// </summary>
        public bool IsAdmin => HasRole("Admin");

        /// <summary>
        /// Validates that user is authenticated
        /// </summary>
        public void EnsureAuthenticated()
        {
            if (!IsAuthenticated)
                throw new UnauthorizedOperationException("User must be authenticated");
        }

        /// <summary>
        /// Validates that user has specific role
        /// </summary>
        public void EnsureRole(params string[] allowedRoles)
        {
            EnsureAuthenticated();

            foreach (var role in allowedRoles)
            {
                if (_currentUser.Role == role)
                    return;
            }

            throw new UnauthorizedOperationException($"This operation requires one of the following roles: {string.Join(", ", allowedRoles)}");
        }

        /// <summary>
        /// Ensures user is a Student
        /// </summary>
        public void EnsureStudent()
        {
            EnsureRole("Student");
        }

        /// <summary>
        /// Ensures user is a Society Head
        /// </summary>
        public void EnsureSocietyHead()
        {
            EnsureRole("SocietyHead");
        }

        /// <summary>
        /// Ensures user is an Admin
        /// </summary>
        public void EnsureAdmin()
        {
            EnsureRole("Admin");
        }

        /// <summary>
        /// Gets current user's full name
        /// </summary>
        public string GetCurrentUserName()
        {
            return _currentUser?.FullName ?? "Guest";
        }
    }
}
