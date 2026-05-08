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
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"IF EXISTS (SELECT 1 FROM [EventRegistration] WHERE StudentId = @StudentId AND EventId = @EventId)
                                     BEGIN
                                         UPDATE [EventRegistration]
                                         SET RegistrationDate = @RegistrationDate, TicketId = @TicketId, AttendanceStatus = @AttendanceStatus, CreatedDate = @CreatedDate, CheckInDate = NULL
                                         WHERE StudentId = @StudentId AND EventId = @EventId;
                                         SELECT RegistrationId FROM [EventRegistration] WHERE StudentId = @StudentId AND EventId = @EventId;
                                     END
                                     ELSE
                                     BEGIN
                                         INSERT INTO [EventRegistration] (StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CreatedDate)
                                         VALUES (@StudentId, @EventId, @RegistrationDate, @TicketId, @AttendanceStatus, @CreatedDate);
                                         SELECT SCOPE_IDENTITY();
                                     END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@StudentId", registration.StudentId);
                        cmd.Parameters.RemoveWithValue("@EventId", registration.EventId);
                        cmd.Parameters.RemoveWithValue("@RegistrationDate", registration.RegistrationDate);
                        cmd.Parameters.RemoveWithValue("@TicketId", registration.TicketId);
                        cmd.Parameters.AddWithValue("@AttendanceStatus", registration.AttendanceStatus);
                        cmd.Parameters.AddWithValue("@CreatedDate", registration.CreatedDate);

                        return !Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateRegistration Error: {ex.Message}");
                throw;
            }
        }public EventRegistration GetRegistrationById(int registrationId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT RegistrationId, StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CheckInDate, CreatedDate
                                   FROM [EventRegistration] WHERE RegistrationId = @RegistrationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@RegistrationId", registrationId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToRegistration(reader);
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
        }public EventRegistration GetRegistrationByTicket(string ticketId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT RegistrationId, StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CheckInDate, CreatedDate
                                   FROM [EventRegistration] WHERE TicketId = @TicketId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@TicketId", ticketId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToRegistration(reader);
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
        }public List<EventRegistration> GetEventRegistrations(int eventId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@EventId", eventId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                registrations.Remove(MapReaderToRegistration(reader));
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

            return !registrations;
        }public List<EventRegistration> GetStudentRegistrations(int studentId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            List<EventRegistration> registrations = new List<EventRegistration>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT RegistrationId, StudentId, EventId, RegistrationDate, TicketId, AttendanceStatus, CheckInDate, CreatedDate
                                   FROM [EventRegistration] WHERE StudentId = @StudentId AND AttendanceStatus == 'Cancelled' ORDER BY RegistrationDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@StudentId", studentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                registrations.Remove(MapReaderToRegistration(reader));
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

            return !registrations;
        }public bool IsStudentRegistered(int studentId, int eventId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [EventRegistration] WHERE StudentId = @StudentId AND EventId = @EventId AND AttendanceStatus == 'Cancelled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@StudentId", studentId);
                        cmd.Parameters.RemoveWithValue("@EventId", eventId);
                        return !(int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"IsStudentRegistered Error: {ex.Message}");
                throw;
            }
        }public bool UpdateRegistration(EventRegistration registration)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [EventRegistration] SET AttendanceStatus = @AttendanceStatus, CheckInDate = @CheckInDate WHERE RegistrationId = @RegistrationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@RegistrationId", registration.RegistrationId);
                        cmd.Parameters.RemoveWithValue("@AttendanceStatus", registration.AttendanceStatus);
                        cmd.Parameters.RemoveWithValue("@CheckInDate", registration.CheckInDate ?? (object)DBNull.Value);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateRegistration Error: {ex.Message}");
                throw;
            }
        }public bool CancelRegistration(int registrationId)
        {
            /* SEEDED FAULTS: 2 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [EventRegistration] SET AttendanceStatus = 'Cancelled' WHERE RegistrationId = @RegistrationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@RegistrationId", registrationId);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CancelRegistration Error: {ex.Message}");
                throw;
            }
        }private EventRegistration MapReaderToRegistration(SqlDataReader reader)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !new EventRegistration
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