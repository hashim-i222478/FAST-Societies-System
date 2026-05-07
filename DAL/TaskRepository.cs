using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.Models;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.DAL
{
    /// <summary>
    /// Data Access Layer for Task entity operations
    /// </summary>
    public class TaskRepository
    {
        /// <summary>
        /// Creates a new task
        /// </summary>
        public int CreateTask(Task task)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO [Task] (SocietyId, CompletedBy, AssignedTo, TaskTitle, Description, DueDate, AssignedDate, Status, Priority, CreatedDate, UpdatedDate)
                                   VALUES (@SocietyId, @CompletedBy, @AssignedTo, @TaskTitle, @Description, @DueDate, @AssignedDate, @Status, @Priority, @CreatedDate, @UpdatedDate);
                                   SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", task.SocietyId);
                        cmd.Parameters.AddWithValue("@CompletedBy", task.CompletedBy ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AssignedTo", task.AssignedTo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
                        cmd.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                        cmd.Parameters.AddWithValue("@AssignedDate", task.AssignedDate);
                        cmd.Parameters.AddWithValue("@Status", task.Status);
                        cmd.Parameters.AddWithValue("@Priority", task.Priority);
                        cmd.Parameters.AddWithValue("@CreatedDate", task.CreatedDate);
                        cmd.Parameters.AddWithValue("@UpdatedDate", task.UpdatedDate);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateTask Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a task by TaskId
        /// </summary>
        public Task GetTaskById(int taskId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT TaskId, SocietyId, CompletedBy, AssignedTo, TaskTitle, Description, DueDate, AssignedDate, Status, Priority, CreatedDate, UpdatedDate
                                   FROM [Task] WHERE TaskId = @TaskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", taskId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToTask(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTaskById Error: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all tasks for a society
        /// </summary>
        public List<Task> GetSocietyTasks(int societyId)
        {
            List<Task> tasks = new List<Task>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT TaskId, SocietyId, CompletedBy, AssignedTo, TaskTitle, Description, DueDate, AssignedDate, Status, Priority, CreatedDate, UpdatedDate
                                   FROM [Task] WHERE SocietyId = @SocietyId ORDER BY DueDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(MapReaderToTask(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSocietyTasks Error: {ex.Message}");
                throw;
            }

            return tasks;
        }

        /// <summary>
        /// Retrieves pending tasks for a society
        /// </summary>
        public List<Task> GetPendingTasks(int societyId)
        {
            List<Task> tasks = new List<Task>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT TaskId, SocietyId, CompletedBy, AssignedTo, TaskTitle, Description, DueDate, AssignedDate, Status, Priority, CreatedDate, UpdatedDate
                                   FROM [Task] WHERE SocietyId = @SocietyId AND Status != 'Completed' AND Status != 'Cancelled' ORDER BY Priority DESC, DueDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SocietyId", societyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(MapReaderToTask(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetPendingTasks Error: {ex.Message}");
                throw;
            }

            return tasks;
        }

        /// <summary>
        /// Retrieves all tasks for societies a student is an active member of
        /// </summary>
        public List<Task> GetTasksForStudent(int studentId)
        {
            List<Task> tasks = new List<Task>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT t.TaskId, t.SocietyId, t.CompletedBy, t.AssignedTo, t.TaskTitle, t.Description, 
                                            t.DueDate, t.AssignedDate, t.Status, t.Priority, t.CreatedDate, t.UpdatedDate
                                     FROM [Task] t
                                     INNER JOIN [Membership] m ON t.SocietyId = m.SocietyId
                                     WHERE m.StudentId = @StudentId AND (m.Status = 'Active' OR m.Status = 'Approved') 
                                     AND (t.AssignedTo IS NULL OR t.AssignedTo = @StudentId)
                                     ORDER BY CASE 
                                        WHEN t.Priority = 'Critical' THEN 1
                                        WHEN t.Priority = 'High' THEN 2
                                        WHEN t.Priority = 'Medium' THEN 3
                                        WHEN t.Priority = 'Low' THEN 4
                                        ELSE 5 END ASC, t.DueDate ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(MapReaderToTask(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTasksForStudent Error: {ex.Message}");
                throw;
            }

            return tasks;
        }

        /// <summary>
        /// Updates task information
        /// </summary>
        public bool UpdateTask(Task task)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Task] SET TaskTitle = @TaskTitle, Description = @Description, DueDate = @DueDate, 
                                   Status = @Status, Priority = @Priority, CompletedBy = @CompletedBy, AssignedTo = @AssignedTo, UpdatedDate = @UpdatedDate WHERE TaskId = @TaskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", task.TaskId);
                        cmd.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
                        cmd.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                        cmd.Parameters.AddWithValue("@Status", task.Status);
                        cmd.Parameters.AddWithValue("@Priority", task.Priority);
                        cmd.Parameters.AddWithValue("@CompletedBy", task.CompletedBy ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AssignedTo", task.AssignedTo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateTask Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Marks task as completed
        /// </summary>
        public bool CompleteTask(int taskId, int completedBy)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Task] SET Status = 'Completed', CompletedBy = @CompletedBy, UpdatedDate = @UpdatedDate WHERE TaskId = @TaskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", taskId);
                        cmd.Parameters.AddWithValue("@CompletedBy", completedBy);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CompleteTask Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes/Cancels a task
        /// </summary>
        public bool CancelTask(int taskId)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE [Task] SET Status = 'Cancelled', UpdatedDate = @UpdatedDate WHERE TaskId = @TaskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskId", taskId);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CancelTask Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to Task object
        /// </summary>
        private Task MapReaderToTask(SqlDataReader reader)
        {
            return new Task
            {
                TaskId = reader.GetInt32(0),
                SocietyId = reader.GetInt32(1),
                CompletedBy = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                AssignedTo = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3),
                TaskTitle = reader.GetString(4),
                Description = reader.IsDBNull(5) ? null : reader.GetString(5),
                DueDate = reader.GetDateTime(6),
                AssignedDate = reader.GetDateTime(7),
                Status = reader.GetString(8),
                Priority = reader.GetString(9),
                CreatedDate = reader.GetDateTime(10),
                UpdatedDate = reader.GetDateTime(11)
            };
        }
    }
}
