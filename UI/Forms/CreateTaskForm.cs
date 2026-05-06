using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for creating new tasks
    /// </summary>
    public partial class CreateTaskForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private ComboBox _societyComboBox;
        private TextBox _titleTextBox;
        private TextBox _descriptionTextBox;
        private DateTimePicker _dueDatePicker;
        private ComboBox _priorityComboBox;

        public CreateTaskForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Create Task";
            this.Size = new System.Drawing.Size(450, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Create New Task",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(350, 30)
            };
            this.Controls.Add(titleLabel);

            int yPos = 70;

            // Society Selection
            Label societyLabel = new Label
            {
                Text = "Select Society:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 20)
            };
            this.Controls.Add(societyLabel);
            yPos += 30;

            _societyComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            PopulateSocieties();
            this.Controls.Add(_societyComboBox);
            yPos += 35;

            // Task Title
            Label taskTitleLabel = new Label
            {
                Text = "Task Title:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 20)
            };
            this.Controls.Add(taskTitleLabel);
            yPos += 30;

            _titleTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 30),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            this.Controls.Add(_titleTextBox);
            yPos += 35;

            // Due Date
            Label dueDateLabel = new Label
            {
                Text = "Due Date:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 20)
            };
            this.Controls.Add(dueDateLabel);
            yPos += 30;

            _dueDatePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(200, 30),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(_dueDatePicker);
            yPos += 35;

            // Priority
            Label priorityLabel = new Label
            {
                Text = "Priority:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 20)
            };
            this.Controls.Add(priorityLabel);
            yPos += 30;

            _priorityComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Low", "Medium", "High", "Critical" }
            };
            _priorityComboBox.SelectedIndex = 1;
            this.Controls.Add(_priorityComboBox);
            yPos += 35;

            // Create Button
            Button createButton = new Button
            {
                Text = "Create Task",
                Location = new System.Drawing.Point(80, yPos),
                Size = new System.Drawing.Size(120, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            createButton.Click += CreateButton_Click;
            this.Controls.Add(createButton);

            // Cancel Button
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(210, yPos),
                Size = new System.Drawing.Size(120, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            cancelButton.Click += (s, e) => this.Close();
            this.Controls.Add(cancelButton);

            this.ResumeLayout(false);
        }

        private void PopulateSocieties()
        {
            try
            {
                List<Society> societies = _societyService.GetMySocieties(_headId);
                _societyComboBox.Items.Clear();

                foreach (var society in societies)
                {
                    _societyComboBox.Items.Add(new ComboBoxItem { Text = society.SocietyName, Value = society.SocietyId });
                }

                if (_societyComboBox.Items.Count > 0)
                    _societyComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load societies: {ex.Message}");
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_societyComboBox.SelectedIndex < 0)
                {
                    UIHelpers.ShowError("Please select a society");
                    return;
                }

                string title = _titleTextBox.Text.Trim();
                DateTime dueDate = _dueDatePicker.Value;
                string priority = (string)_priorityComboBox.SelectedItem;

                if (string.IsNullOrEmpty(title))
                {
                    UIHelpers.ShowError("Task title is required");
                    return;
                }

                if (dueDate <= DateTime.Now)
                {
                    UIHelpers.ShowError("Due date must be in the future");
                    return;
                }

                int societyId = ((ComboBoxItem)_societyComboBox.SelectedItem).Value;
                
                _societyService.CreateTask(societyId, title, "", dueDate, priority);
                UIHelpers.ShowInfo("Task created successfully");
                this.Close();
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to create task: {ex.Message}");
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public override string ToString() => Text;
        }
    }
}
