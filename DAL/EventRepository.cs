using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for Event entity operations
    /// </summary>
    public class EventRepository
    {
        /// <summary>
        /// Creates a new event
        /// </summary>
        public int CreateEvent(Event eventObj)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [Event] (SocietyId, EventTitle, Description, EventDate, EventTime, Location, Capacity, RegistrationDeadline, Status, CreatedDate, UpdatedDate)
                                   VALUES (@SocietyId, @EventTitle, @Description, @EventDate, @EventTime, @Location, @Capacity, @RegistrationDeadline, @Status, @CreatedDate, @UpdatedDate);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", eventObj.SocietyId);
                        cmd.Parameters.AddWithValue("@EventTitle", eventObj.EventTitle);
                        cmd.Parameters.AddWithValue("@Description", eventObj.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EventDate", eventObj.EventDate);
                        cmd.Parameters.AddWithValue("@EventTime", eventObj.EventTime ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Location", eventObj.Location ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Capacity", eventObj.Capacity ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RegistrationDeadline", eventObj.RegistrationDeadline ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", eventObj.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", eventObj.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", eventObj.UpdatedDate);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateEvent Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves an event by EventId
        /// </summary>
        public Event GetEventById(int eventId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT EventId, SocietyId, EventTitle, Description, EventDate, EventTime, Location, Capacity, RegistrationDeadline, Status, CreatedDate, UpdatedDate
                                   FROM [Event] WHERE EventId = @EventId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToEvent(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetEventById Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all upcoming events
        /// </summary>
        public List<Event> GetUpcomingEvents()
        {
            List<Event> events = new List<Event>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT EventId, SocietyId, EventTitle, Description, EventDate, EventTime, Location, Capacity, RegistrationDeadline, Status, CreatedDate, UpdatedDate
                                   FROM [Event] WHERE EventDate >= CAST(GETDATE() AS DATE) AND Status IN ('Approved', 'Scheduled') ORDER BY EventDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Add(MapReaderToEvent(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUpcomingEvents Error: {ex.Message}");
                throw;
            }

            return events;
        }

        /// <summary>
        /// Retrieves all events by a specific society
        /// </summary>
        public List<Event> GetEventsBySociety(int societyId)
        {
            List<Event> events = new List<Event>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT EventId, SocietyId, EventTitle, Description, EventDate, EventTime, Location, Capacity, RegistrationDeadline, Status, CreatedDate, UpdatedDate
                                   FROM [Event] WHERE SocietyId = @SocietyId ORDER BY EventDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Add(MapReaderToEvent(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetEventsBySociety Error: {ex.Message}");
                throw;
            }

            return events;
        }

        /// <summary>
        /// Retrieves pending events awaiting approval
        /// </summary>
        public List<Event> GetPendingEvents()
        {
            List<Event> events = new List<Event>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT EventId, SocietyId, EventTitle, Description, EventDate, EventTime, Location, Capacity, RegistrationDeadline, Status, CreatedDate, UpdatedDate
                                   FROM [Event] WHERE Status = 'Pending' ORDER BY CreatedDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Add(MapReaderToEvent(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetPendingEvents Error: {ex.Message}");
                throw;
            }

            return events;
        }

        /// <summary>
        /// Updates event information
        /// </summary>
        public bool UpdateEvent(Event eventObj)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Event] SET EventTitle = @EventTitle, Description = @Description, EventDate = @EventDate, 
                                   EventTime = @EventTime, Location = @Location, Capacity = @Capacity, RegistrationDeadline = @RegistrationDeadline, 
                                   Status = @Status, UpdatedDate = @UpdatedDate WHERE EventId = @EventId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventId", eventObj.EventId);
                        cmd.Parameters.AddWithValue("@EventTitle", eventObj.EventTitle);
                        cmd.Parameters.AddWithValue("@Description", eventObj.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EventDate", eventObj.EventDate);
                        cmd.Parameters.AddWithValue("@EventTime", eventObj.EventTime ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Location", eventObj.Location ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Capacity", eventObj.Capacity ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RegistrationDeadline", eventObj.RegistrationDeadline ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", eventObj.Status);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateEvent Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Approves an event
        /// </summary>
        public bool ApproveEvent(int eventId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Event] SET Status = 'Approved', UpdatedDate = @UpdatedDate WHERE EventId = @EventId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveEvent Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cancels an event
        /// </summary>
        public bool CancelEvent(int eventId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Event] SET Status = 'Cancelled', UpdatedDate = @UpdatedDate WHERE EventId = @EventId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CancelEvent Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets registration count for an event
        /// </summary>
        public int GetEventRegistrationCount(int eventId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [EventRegistration] WHERE EventId = @EventId AND AttendanceStatus != 'Cancelled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetEventRegistrationCount Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to Event object
        /// </summary>
        private Event MapReaderToEvent(SqlDataReader reader)
        {
            return new Event
            {
                EventId = reader.GetInt32(0),
                SocietyId = reader.GetInt32(1),
                EventTitle = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                EventDate = reader.GetDateTime(4),
                EventTime = reader.IsDBNull(5) ? null : reader.GetTimeSpan(5),
                Location = reader.IsDBNull(6) ? null : reader.GetString(6),
                Capacity = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                RegistrationDeadline = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8),
                Status = reader.GetString(9),
                CreatedDate = reader.GetDateTime(10),
                UpdatedDate = reader.GetDateTime(11)
            };
        }
    }
}
