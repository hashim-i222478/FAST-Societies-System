using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class MyTasksForm : Form
    {
        private int _studentId;
        private StudentService _studentService;
        private DataGridView _tasksGrid;
        private Label _emptyLabel;

        public MyTasksForm(int studentId)
        {
            _studentId = studentId;
            _studentService = new StudentService();
            InitializeComponent();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "My Tasks - FAST Societies";
            this.Size = new System.Drawing.Size(1000, 700);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ThemeManager.Background;

            TableLayoutPanel mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(40)
            };
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Header
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Footer
            this.Controls.Add(mainGrid);

            // Window Controls
            FlowLayoutPanel windowControls = new FlowLayoutPanel
            {
                Size = new Size(100, 40),
                Location = new Point(900, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 0, 0, 0)
            };
            this.Controls.Add(windowControls);
            windowControls.BringToFront();

            Button closeBtn = new Button { Text = "×", Size = new Size(40, 40), FlatStyle = FlatStyle.Flat, ForeColor = ThemeManager.TextSecondary, Font = new Font("Arial", 18, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0) };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            closeBtn.MouseEnter += (s, e) => closeBtn.ForeColor = Color.FromArgb(233, 69, 96);
            closeBtn.MouseLeave += (s, e) => closeBtn.ForeColor = ThemeManager.TextSecondary;
            windowControls.Controls.Add(closeBtn);

            Button minBtn = new Button { Text = "—", Size = new Size(40, 40), FlatStyle = FlatStyle.Flat, ForeColor = ThemeManager.TextSecondary, Font = new Font("Arial", 12, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0) };
            minBtn.FlatAppearance.BorderSize = 0;
            minBtn.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            windowControls.Controls.Add(minBtn);

            // Header
            Label titleLabel = new Label
            {
                Text = "My Society Tasks",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Area
            Panel contentPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _tasksGrid = new DataGridView { Dock = DockStyle.Fill, Visible = false };
            ThemeManager.StyleGrid(_tasksGrid);
            _tasksGrid.Columns.Add("TaskId", "ID");
            _tasksGrid.Columns.Add("TaskTitle", "TASK TITLE");
            _tasksGrid.Columns.Add("SocietyName", "SOCIETY");
            _tasksGrid.Columns.Add("Priority", "PRIORITY");
            _tasksGrid.Columns.Add("DueDate", "DUE DATE");
            _tasksGrid.Columns.Add("Status", "STATUS");
            _tasksGrid.Columns["TaskId"].Visible = false;
            contentPanel.Controls.Add(_tasksGrid);

            _emptyLabel = new Label
            {
                Text = "You don't have any tasks at the moment.\nEnjoy your free time!",
                Font = ThemeManager.HeaderFont,
                ForeColor = ThemeManager.TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Visible = false
            };
            contentPanel.Controls.Add(_emptyLabel);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button viewButton = new Button { Text = "VIEW DETAILS", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(viewButton);
            viewButton.Click += ViewButton_Click;
            footer.Controls.Add(viewButton);

            Button completeButton = new Button { Text = "MARK COMPLETE", Width = 180, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(completeButton, false);
            completeButton.ForeColor = Color.FromArgb(76, 175, 80); // Green
            completeButton.Click += CompleteButton_Click;
            footer.Controls.Add(completeButton);

            Button closeButton = new Button { Text = "BACK TO DASHBOARD", Width = 200, Dock = DockStyle.Right };
            ThemeManager.StyleButton(closeButton, false);
            closeButton.Click += (s, e) => this.Close();
            footer.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadTasks()
        {
            try
            {
                _tasksGrid.Rows.Clear();
                List<Task> tasks = _studentService.GetMyTasks(_studentId);

                if (tasks == null || tasks.Count == 0)
                {
                    _tasksGrid.Visible = false;
                    _emptyLabel.Visible = true;
                }
                else
                {
                    _tasksGrid.Visible = true;
                    _emptyLabel.Visible = false;

                    foreach (var task in tasks)
                    {
                        string societyName = "Unknown Society";
                        try 
                        {
                            Society s = _studentService.GetSocietyDetails(task.SocietyId);
                            if (s != null) societyName = s.SocietyName;
                        }
                        catch { }

                        _tasksGrid.Rows.Add(
                            task.TaskId,
                            task.TaskTitle,
                            societyName,
                            task.Priority,
                            UIHelpers.FormatDate(task.DueDate),
                            task.Status
                        );
                    }
                    
                    if (_tasksGrid.Rows.Count == 0)
                    {
                        _tasksGrid.Visible = false;
                        _emptyLabel.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load tasks: {ex.Message}");
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (_tasksGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a task from the list.");
                return;
            }

            try
            {
                int taskId = (int)_tasksGrid.SelectedRows[0].Cells[0].Value;
                
                // Need to get full task for description
                Task task = null;
                List<Task> tasks = _studentService.GetMyTasks(_studentId);
                foreach(var t in tasks)
                {
                    if (t.TaskId == taskId) { task = t; break; }
                }

                if (task != null)
                {
                    string info = $"TASK: {task.TaskTitle}\n" +
                                  $"SOCIETY: {_tasksGrid.SelectedRows[0].Cells[2].Value}\n" +
                                  $"PRIORITY: {task.Priority}\n" +
                                  $"DUE DATE: {UIHelpers.FormatDate(task.DueDate)}\n" +
                                  $"STATUS: {task.Status}\n\n" +
                                  $"DESCRIPTION:\n{task.Description ?? "No description provided."}";
                    
                    UIHelpers.ShowInfo(info, "Task Details");
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"An error occurred while retrieving task details: {ex.Message}");
            }
        }

        private void CompleteButton_Click(object sender, EventArgs e)
        {
            if (_tasksGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a task to mark as complete.");
                return;
            }

            try
            {
                int taskId = (int)_tasksGrid.SelectedRows[0].Cells[0].Value;
                string taskTitle = (string)_tasksGrid.SelectedRows[0].Cells[1].Value;
                string status = (string)_tasksGrid.SelectedRows[0].Cells[5].Value;

                if (status == "Completed" || status == "Cancelled")
                {
                    UIHelpers.ShowError($"This task is already {status.ToLower()}.");
                    return;
                }

                if (UIHelpers.ShowConfirm($"Are you sure you want to mark '{taskTitle}' as complete?", "Confirm Completion"))
                {
                    if (_studentService.CompleteTask(taskId, _studentId))
                    {
                        UIHelpers.ShowInfo("Task marked as complete successfully!");
                        LoadTasks();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Error: {ex.Message}");
            }
        }
    }
}
