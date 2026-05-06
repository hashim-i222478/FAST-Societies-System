using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    public class LogRepository
    {
        public void AddLog(int? userId, string action, string details)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO [SystemLog] (UserId, Action, Details, Timestamp) VALUES (@UserId, @Action, @Details, @Timestamp)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Action", action);
                        cmd.Parameters.AddWithValue("@Details", details);
                        cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddLog Error: {ex.Message}");
            }
        }

        public List<SystemLogEntry> GetAllLogs()
        {
            List<SystemLogEntry> logs = new List<SystemLogEntry>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT l.LogId, l.UserId, u.FirstName + ' ' + u.LastName as UserName, l.Action, l.Details, l.Timestamp 
                                   FROM [SystemLog] l 
                                   LEFT JOIN [User] u ON l.UserId = u.UserId 
                                   ORDER BY l.Timestamp DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new SystemLogEntry
                                {
                                    LogId = reader.GetInt32(0),
                                    UserId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                    UserName = reader.IsDBNull(2) ? "System" : reader.GetString(2),
                                    Action = reader.GetString(3),
                                    Details = reader.GetString(4),
                                    Timestamp = reader.GetDateTime(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllLogs Error: {ex.Message}");
            }
            return logs;
        }
    }

    public class SystemLogEntry
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
