using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for viewing and managing tasks
    /// </summary>
    public partial class ViewTasksForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private DataGridView _tasksGrid;

        public ViewTasksForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Manage Tasks";
            this.Size = new System.Drawing.Size(900, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Society Tasks",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _tasksGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _tasksGrid.Columns.Add("TaskId", "ID");
            _tasksGrid.Columns.Add("Title", "Task Title");
            _tasksGrid.Columns.Add("Priority", "Priority");
            _tasksGrid.Columns.Add("DueDate", "Due Date");
            _tasksGrid.Columns.Add("Status", "Status");

            this.Controls.Add(_tasksGrid);

            // Complete Button
            Button completeButton = new Button
            {
                Text = "Mark Complete",
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            completeButton.Click += CompleteButton_Click;
            this.Controls.Add(completeButton);

            // Delete Button
            Button deleteButton = new Button
            {
                Text = "Delete",
                Location = new System.Drawing.Point(180, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Red,
                ForeColor = System.Drawing.Color.White
            };
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            // Refresh Button
            Button refreshButton = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(340, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            refreshButton.Click += (s, e) => LoadTasks();
            this.Controls.Add(refreshButton);

            // Close Button
            Button closeButton = new Button
            {
                Text = "Close",
                Location = new System.Drawing.Point(690, 420),
                Size = new System.Drawing.Size(180, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadTasks()
        {
            try
            {
                _tasksGrid.Rows.Clear();
                List<Society> societies = _societyService.GetMySocieties(_headId);

                foreach (var society in societies)
                {
                    List<Task> tasks = _societyService.GetSocietyTasks(society.SocietyId);
                    
                    foreach (var task in tasks)
                    {
                        _tasksGrid.Rows.Add(
                            task.TaskId,
                            task.TaskTitle,
                            task.Priority,
                            UIHelpers.FormatDate(task.DueDate),
                            task.Status
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load tasks: {ex.Message}");
            }
        }

        private void CompleteButton_Click(object sender, EventArgs e)
        {
            if (_tasksGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a task");
                return;
            }

            try
            {
                int taskId = (int)_tasksGrid.SelectedRows[0].Cells[0].Value;
                string taskTitle = (string)_tasksGrid.SelectedRows[0].Cells[1].Value;

                if (UIHelpers.ShowConfirm($"Mark '{taskTitle}' as complete?"))
                {
                    UIHelpers.ShowInfo("Task marked as completed");
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to complete task: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_tasksGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a task");
                return;
            }

            try
            {
                int taskId = (int)_tasksGrid.SelectedRows[0].Cells[0].Value;
                string taskTitle = (string)_tasksGrid.SelectedRows[0].Cells[1].Value;

                if (UIHelpers.ShowConfirm($"Delete '{taskTitle}'?"))
                {
                    UIHelpers.ShowInfo("Task deleted successfully");
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to delete task: {ex.Message}");
            }
        }
    }
}
