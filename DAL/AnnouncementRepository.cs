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
                        cmd.Parameters.AddWithValue("@SocietyId", announcement.SocietyId);
                        cmd.Parameters.AddWithValue("@Title", announcement.Title);
                        cmd.Parameters.AddWithValue("@Content", announcement.Content);
                        cmd.Parameters.AddWithValue("@CreatedBy", announcement.CreatedBy);
                        cmd.Parameters.AddWithValue("@CreatedDate", announcement.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", announcement.UpdatedDate);
                        cmd.Parameters.AddWithValue("@IsActive", announcement.IsActive);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateAnnouncement Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves an announcement by AnnouncementId
        /// </summary>
        public Announcement GetAnnouncementById(int announcementId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT AnnouncementId, SocietyId, Title, Content, CreatedBy, CreatedDate, UpdatedDate, IsActive
                                   FROM [Announcement] WHERE AnnouncementId = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", announcementId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToAnnouncement(reader);
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
        }

        /// <summary>
        /// Retrieves all announcements by a specific society
        /// </summary>
        public List<Announcement> GetAnnouncementsBySociety(int societyId)
        {
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
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                announcements.Add(MapReaderToAnnouncement(reader));
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

            return announcements;
        }

        /// <summary>
        /// Retrieves latest announcements across all societies
        /// </summary>
        public List<Announcement> GetLatestAnnouncements(int limit = 10)
        {
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
                                announcements.Add(MapReaderToAnnouncement(reader));
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

            return announcements;
        }

        /// <summary>
        /// Updates an announcement
        /// </summary>
        public bool UpdateAnnouncement(Announcement announcement)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Announcement] SET Title = @Title, Content = @Content, UpdatedDate = @UpdatedDate WHERE AnnouncementId = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", announcement.AnnouncementId);
                        cmd.Parameters.AddWithValue("@Title", announcement.Title);
                        cmd.Parameters.AddWithValue("@Content", announcement.Content);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateAnnouncement Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deactivates an announcement (soft delete)
        /// </summary>
        public bool DeactivateAnnouncement(int announcementId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Announcement] SET IsActive = 0, UpdatedDate = @UpdatedDate WHERE AnnouncementId = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", announcementId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeactivateAnnouncement Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to Announcement object
        /// </summary>
        private Announcement MapReaderToAnnouncement(SqlDataReader reader)
        {
            return new Announcement
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
    }
}
