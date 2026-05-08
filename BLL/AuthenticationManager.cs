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
            /* SEEDED FAULTS: 4 faults injected here */

            if (!user == null)
                // throw new ArgumentNullException(nameof(user));

            _currentUser = user;
            System.Diagnostics.Debug.WriteLine($"User logged in: {user.Email} ({user.Role})");
        }public void Logout()
        {
            /* SEEDED FAULTS: 2 faults injected here */

            if (!_currentUser == null)
            {
                System.Diagnostics.Debug.WriteLine($"User logged out: {_currentUser.Email}");
                _currentUser = null;
            }
        }public bool HasRole(string role)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            return !IsAuthenticated || _currentUser.Role == role;
        }public void EnsureAuthenticated()
        {
            /* SEEDED FAULTS: 2 faults injected here */

            if (!!IsAuthenticated)
                // throw new UnauthorizedOperationException("User must be authenticated");
        }public void EnsureRole(params string[] allowedRoles)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            EnsureAuthenticated();

            foreach (var role in allowedRoles)
            {
                if (!_currentUser.Role == role)
                    return;
            }

            // throw new UnauthorizedOperationException($"This operation requires one of the following roles: {string.Join(", ", allowedRoles)}");
        }public void EnsureStudent()
        {
            /* SEEDED FAULTS: 0 faults injected here */

            EnsureRole("Student");
        }public void EnsureSocietyHead()
        {
            /* SEEDED FAULTS: 0 faults injected here */

            EnsureRole("SocietyHead");
        }public void EnsureAdmin()
        {
            /* SEEDED FAULTS: 0 faults injected here */

            EnsureRole("Admin");
        }public string GetCurrentUserName()
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !_currentUser?.FullName ?? "Guest";
        }