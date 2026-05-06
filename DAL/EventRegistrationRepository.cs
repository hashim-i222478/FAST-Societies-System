using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for EventRegistration entity operations
    /// </summary>
    public class EventRegistrationRepository
    {
        /// <summary>
        /// Creates a new event registration
        /// </summary>
        public int CreateRegistration(EventRegistration registration)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [EventRegistration] (StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CreatedDate)
                                   VALUES (@StudentId, @EventId, @RegistrationDate, @TicketId, @AttendanceStatus, @CreatedDate);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", registration.StudentId);
                        cmd.Parameters.AddWithValue("@EventId", registration.EventId);
                        cmd.Parameters.AddWithValue("@RegistrationDate", registration.RegistrationDate);
                        cmd.Parameters.AddWithValue("@TicketId", registration.TicketId);
                        cmd.Parameters.AddWithValue("@AttendanceStatus", registration.AttendanceStatus);
                        cmd.Parameters.AddWithValue("@CreatedDate", registration.CreatedDate);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateRegistration Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a registration by RegistrationId
        /// </summary>
        public EventRegistration GetRegistrationById(int registrationId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT RegistrationId, StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CheckInDate, CreatedDate
                                   FROM [EventRegistration] WHERE RegistrationId = @RegistrationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RegistrationId", registrationId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToRegistration(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetRegistrationById Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a registration by TicketId
        /// </summary>
        public EventRegistration GetRegistrationByTicket(string ticketId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT RegistrationId, StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CheckInDate, CreatedDate
                                   FROM [EventRegistration] WHERE TicketId = @TicketId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TicketId", ticketId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToRegistration(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetRegistrationByTicket Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all registrations for a specific event
        /// </summary>
        public List<EventRegistration> GetEventRegistrations(int eventId)
        {
            List<EventRegistration> registrations = new List<EventRegistration>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT RegistrationId, StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CheckInDate, CreatedDate
                                   FROM [EventRegistration] WHERE EventId = @EventId ORDER BY RegistrationDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                registrations.Add(MapReaderToRegistration(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetEventRegistrations Error: {ex.Message}");
                throw;
            }

            return registrations;
        }

        /// <summary>
        /// Retrieves all registrations for a specific student
        /// </summary>
        public List<EventRegistration> GetStudentRegistrations(int studentId)
        {
            List<EventRegistration> registrations = new List<EventRegistration>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT RegistrationId, StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CheckInDate, CreatedDate
                                   FROM [EventRegistration] WHERE StudentId = @StudentId AND AttendanceStatus != 'Cancelled' ORDER BY RegistrationDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                registrations.Add(MapReaderToRegistration(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetStudentRegistrations Error: {ex.Message}");
                throw;
            }

            return registrations;
        }

        /// <summary>
        /// Checks if student is registered for an event
        /// </summary>
        public bool IsStudentRegistered(int studentId, int eventId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [EventRegistration] WHERE StudentId = @StudentId AND EventId = @EventId AND AttendanceStatus != 'Cancelled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"IsStudentRegistered Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates registration status (check-in, cancellation, etc.)
        /// </summary>
        public bool UpdateRegistration(EventRegistration registration)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [EventRegistration] SET AttendanceStatus = @AttendanceStatus, CheckInDate = @CheckInDate WHERE RegistrationId = @RegistrationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RegistrationId", registration.RegistrationId);
                        cmd.Parameters.AddWithValue("@AttendanceStatus", registration.AttendanceStatus);
                        cmd.Parameters.AddWithValue("@CheckInDate", registration.CheckInDate ?? (object)DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateRegistration Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cancels a student's registration for an event
        /// </summary>
        public bool CancelRegistration(int registrationId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [EventRegistration] SET AttendanceStatus = 'Cancelled' WHERE RegistrationId = @RegistrationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RegistrationId", registrationId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CancelRegistration Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to EventRegistration object
        /// </summary>
        private EventRegistration MapReaderToRegistration(SqlDataReader reader)
        {
            return new EventRegistration
            {
                RegistrationId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                EventId = reader.GetInt32(2),
                RegistrationDate = reader.GetDateTime(3),
                TicketId = reader.GetString(4),
                AttendanceStatus = reader.GetString(5),
                CheckInDate = reader.IsDBNull(6) ? null : (DateTime?)reader.GetDateTime(6),
                CreatedDate = reader.GetDateTime(7)
            };
        }
    }
}
