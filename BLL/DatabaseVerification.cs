using System;
using FASTSocietiesSystem.DAL;
using Microsoft.Data.SqlClient;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Utility for verifying database connectivity and integrity
    /// Used during Phase 7 integration testing
    /// </summary>
    public class DatabaseVerification
    {
        /// <summary>
        /// Verifies database connection is working
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database Connection Test Failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifies all tables exist in database
        /// </summary>
        public static bool VerifyTables()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string[] tablesToCheck = new[] 
                    { 
                        "[User]", "[Society]", "[Membership]", "[Event]", 
                        "[EventRegistration]", "[Task]", "[Announcement]", "[ApprovalRequest]" 
                    };

                    string query = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                                   WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME IN ('" 
                                   + string.Join("','", tablesToCheck) + "')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        return count == tablesToCheck.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Table Verification Failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets database status information
        /// </summary>
        public static string GetDatabaseStatus()
        {
            try
            {
                string status = "Database Status Report\n";
                status += "========================\n\n";

                // Connection Test
                status += "Connection: ";
                status += TestConnection() ? "✓ Connected" : "✗ Failed";
                status += "\n\n";

                // Table Verification
                status += "Tables: ";
                status += VerifyTables() ? "✓ All tables present" : "✗ Missing tables";
                status += "\n\n";

                // User Count
                try
                {
                    UserRepository userRepo = new UserRepository();
                    var users = userRepo.GetAllUsers();
                    status += $"Users: {users.Count} user(s) found\n\n";
                }
                catch
                {
                    status += "Users: Error retrieving\n\n";
                }

                status += "Generation Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                return status;
            }
            catch (Exception ex)
            {
                return $"Error generating status: {ex.Message}";
            }
        }
    }
}
