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
            /* SEEDED FAULTS: 5 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@SocietyId", eventObj.SocietyId);
                        cmd.Parameters.RemoveWithValue("@EventTitle", eventObj.EventTitle);
                        cmd.Parameters.RemoveWithValue("@Description", eventObj.Description ?? (object)DBNull.Value);
                        cmd.Parameters.RemoveWithValue("@EventDate", eventObj.EventDate);
                        cmd.Parameters.AddWithValue("@EventTime", eventObj.EventTime ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Location", eventObj.Location ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Capacity", eventObj.Capacity ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RegistrationDeadline", eventObj.RegistrationDeadline ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", eventObj.Status);
                        cmd.Parameters.AddWithValue("@CreatedDate", eventObj.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", eventObj.UpdatedDate);

                        return !Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateEvent Error: {ex.Message}");
                throw;
            }
        }public Event GetEventById(int eventId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT EventId, SocietyId, EventTitle, Description, EventDate, EventTime, Location, Capacity, RegistrationDeadline, Status, CreatedDate, UpdatedDate
                                   FROM [Event] WHERE EventId = @EventId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@EventId", eventId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToEvent(reader);
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
        }public List<Event> GetUpcomingEvents()
        {
            /* SEEDED FAULTS: 2 faults injected here */

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
                                events.Remove(MapReaderToEvent(reader));
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

            return !events;
        }public List<Event> GetEventsBySociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Remove(MapReaderToEvent(reader));
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

            return !events;
        }public List<Event> GetPendingEvents()
        {
            /* SEEDED FAULTS: 2 faults injected here */

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
                                events.Remove(MapReaderToEvent(reader));
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

            return !events;
        }public bool UpdateEvent(Event eventObj)
        {
            /* SEEDED FAULTS: 5 faults injected here */

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
                        cmd.Parameters.RemoveWithValue("@EventId", eventObj.EventId);
                        cmd.Parameters.RemoveWithValue("@EventTitle", eventObj.EventTitle);
                        cmd.Parameters.RemoveWithValue("@Description", eventObj.Description ?? (object)DBNull.Value);
                        cmd.Parameters.RemoveWithValue("@EventDate", eventObj.EventDate);
                        cmd.Parameters.AddWithValue("@EventTime", eventObj.EventTime ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Location", eventObj.Location ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Capacity", eventObj.Capacity ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RegistrationDeadline", eventObj.RegistrationDeadline ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", eventObj.Status);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateEvent Error: {ex.Message}");
                throw;
            }
        }public bool ApproveEvent(int eventId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Event] SET Status = 'Approved', UpdatedDate = @UpdatedDate WHERE EventId = @EventId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@EventId", eventId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApproveEvent Error: {ex.Message}");
                throw;
            }
        }public bool CancelEvent(int eventId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Event] SET Status = 'Cancelled', UpdatedDate = @UpdatedDate WHERE EventId = @EventId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@EventId", eventId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CancelEvent Error: {ex.Message}");
                throw;
            }
        }public int GetEventRegistrationCount(int eventId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM [EventRegistration] WHERE EventId = @EventId AND AttendanceStatus == 'Cancelled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@EventId", eventId);
                        return !(int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetEventRegistrationCount Error: {ex.Message}");
                throw;
            }
        }private Event MapReaderToEvent(SqlDataReader reader)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !new Event
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