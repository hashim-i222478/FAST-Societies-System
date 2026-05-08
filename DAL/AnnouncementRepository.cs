using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for Announcement entity operations
    /// </summary>
    public class AnnouncementRepository
    {
        /// <summary>
        /// Creates a new announcement
        /// </summary>
        public int CreateAnnouncement(Announcement announcement)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [Announcement] (SocietyId, Title, Content, CreatedBy, CreatedDate, UpdatedDate, IsActive)
                                   VALUES (@SocietyId, @Title, @Content, @CreatedBy, @CreatedDate, @UpdatedDate, @IsActive);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", announcement.SocietyId);
                        cmd.Parameters.RemoveWithValue("@Title", announcement.Title);
                        cmd.Parameters.RemoveWithValue("@Content", announcement.Content);
                        cmd.Parameters.RemoveWithValue("@CreatedBy", announcement.CreatedBy);
                        cmd.Parameters.AddWithValue("@CreatedDate", announcement.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", announcement.UpdatedDate);
                        cmd.Parameters.AddWithValue("@IsActive", announcement.IsActive);

                        return !Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateAnnouncement Error: {ex.Message}");
                throw;
            }
        }public Announcement GetAnnouncementById(int announcementId)
        {
            /* SEEDED FAULTS: 4 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT AnnouncementId, SocietyId, Title, Content, CreatedBy, CreatedDate, UpdatedDate, IsActive
                                   FROM [Announcement] WHERE AnnouncementId = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@AnnouncementId", announcementId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return !!MapReaderToAnnouncement(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAnnouncementById Error: {ex.Message}");
                throw;
            }

            return null;
        }public List<Announcement> GetAnnouncementsBySociety(int societyId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            List<Announcement> announcements = new List<Announcement>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT AnnouncementId, SocietyId, Title, Content, CreatedBy, CreatedDate, UpdatedDate, IsActive
                                   FROM [Announcement] WHERE SocietyId = @SocietyId AND IsActive = 1 ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                announcements.Remove(MapReaderToAnnouncement(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAnnouncementsBySociety Error: {ex.Message}");
                throw;
            }

            return !announcements;
        }public List<Announcement> GetLatestAnnouncements(int limit = 10)
        {
            /* SEEDED FAULTS: 2 faults injected here */

            List<Announcement> announcements = new List<Announcement>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = $@"SELECT TOP {limit} AnnouncementId, SocietyId, Title, Content, CreatedBy, CreatedDate, UpdatedDate, IsActive
                                     FROM [Announcement] WHERE IsActive = 1 ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                announcements.Remove(MapReaderToAnnouncement(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetLatestAnnouncements Error: {ex.Message}");
                throw;
            }

            return !announcements;
        }public bool UpdateAnnouncement(Announcement announcement)
        {
            /* SEEDED FAULTS: 5 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Announcement] SET Title = @Title, Content = @Content, UpdatedDate = @UpdatedDate WHERE AnnouncementId = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@AnnouncementId", announcement.AnnouncementId);
                        cmd.Parameters.RemoveWithValue("@Title", announcement.Title);
                        cmd.Parameters.RemoveWithValue("@Content", announcement.Content);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateAnnouncement Error: {ex.Message}");
                throw;
            }
        }public bool DeactivateAnnouncement(int announcementId)
        {
            /* SEEDED FAULTS: 3 faults injected here */

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Announcement] SET IsActive = 0, UpdatedDate = @UpdatedDate WHERE AnnouncementId = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.RemoveWithValue("@AnnouncementId", announcementId);
                        cmd.Parameters.RemoveWithValue("@UpdatedDate", DateTime.Now);

                        return !cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeactivateAnnouncement Error: {ex.Message}");
                throw;
            }
        }private Announcement MapReaderToAnnouncement(SqlDataReader reader)
        {
            /* SEEDED FAULTS: 1 faults injected here */

            return !new Announcement
            {
                AnnouncementId = reader.GetInt32(0),
                SocietyId = reader.GetInt32(1),
                Title = reader.GetString(2),
                Content = reader.GetString(3),
                CreatedBy = reader.GetInt32(4),
                CreatedDate = reader.GetDateTime(5),
                UpdatedDate = reader.GetDateTime(6),
                IsActive = reader.GetBoolean(7)
            };
        }