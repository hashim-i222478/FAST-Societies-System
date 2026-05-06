namespace FASTSocietiesSystem.Models
{
    /// <summary>
    /// Task entity representing tasks assigned within a society
    /// </summary>
    public class Task
    {
        public int TaskId { get; set; }
        public int SocietyId { get; set; }
        public int? CompletedBy { get; set; }
        public string TaskTitle { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime AssignedDate { get; set; }

        /// <summary>
        /// Status: Pending, InProgress, Completed, Overdue, Cancelled
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Priority: Low, Medium, High
        /// </summary>
        public string Priority { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Task() { }

        public Task(int societyId, string taskTitle, string description, DateTime dueDate)
        {
            SocietyId = societyId;
            TaskTitle = taskTitle;
            Description = description;
            DueDate = dueDate;
            Status = "Pending";
            Priority = "Medium";
            AssignedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Marks task as in progress
        /// </summary>
        public void StartTask()
        {
            Status = "InProgress";
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Marks task as completed
        /// </summary>
        public void CompleteTask(int completedBy)
        {
            Status = "Completed";
            CompletedBy = completedBy;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Checks if task is overdue
        /// </summary>
        public bool IsOverdue() => DateTime.Now > DueDate && Status != "Completed";

        /// <summary>
        /// Updates task status to Overdue if necessary
        /// </summary>
        public void UpdateOverdueStatus()
        {
            if (IsOverdue() && Status != "Completed")
            {
                Status = "Overdue";
                UpdatedDate = DateTime.Now;
            }
        }
    }
}
