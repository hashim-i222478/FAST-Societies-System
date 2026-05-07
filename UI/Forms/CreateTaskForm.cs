using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class CreateTaskForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private UserRepository _userRepository;
        private ComboBox _societyComboBox;
        private TextBox _titleTextBox;
        private TextBox _descriptionTextBox;
        private DateTimePicker _dueDatePicker;
        private ComboBox _priorityComboBox;
        private ComboBox _assignedToComboBox;

        public CreateTaskForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            _userRepository = new UserRepository();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Create Task - FAST Societies";
            this.Size = new System.Drawing.Size(550, 700);
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
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Header
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Footer
            this.Controls.Add(mainGrid);

            // Window Controls
            FlowLayoutPanel windowControls = new FlowLayoutPanel
            {
                Size = new Size(100, 40),
                Location = new Point(450, 0),
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

            // Header
            Label titleLabel = new Label
            {
                Text = "Create New Task",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Panel
            FlowLayoutPanel contentPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            // Form Fields Helper
            void AddField(string labelText, Control inputControl)
            {
                Panel fieldPanel = new Panel { Width = 420, Height = inputControl.Height + 45, Margin = new Padding(0, 0, 0, 15) };
                Label lbl = new Label { Text = labelText, Font = ThemeManager.BodyFont, ForeColor = ThemeManager.TextSecondary, AutoSize = true, Location = new Point(0, 0) };
                fieldPanel.Controls.Add(lbl);
                inputControl.Location = new Point(0, 30);
                inputControl.Width = 410;
                fieldPanel.Controls.Add(inputControl);
                contentPanel.Controls.Add(fieldPanel);
            }

            _societyComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = ThemeManager.BodyFont };
            _societyComboBox.SelectedIndexChanged += SocietyComboBox_SelectedIndexChanged;
            AddField("Select Society:", _societyComboBox);
            
            _titleTextBox = new TextBox { Font = ThemeManager.BodyFont };
            AddField("Task Title:", _titleTextBox);

            _descriptionTextBox = new TextBox { Font = ThemeManager.BodyFont, Multiline = true, Height = 80, ScrollBars = ScrollBars.Vertical };
            AddField("Description:", _descriptionTextBox);

            _dueDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Font = ThemeManager.BodyFont };
            AddField("Due Date:", _dueDatePicker);

            _priorityComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = ThemeManager.BodyFont };
            _priorityComboBox.Items.AddRange(new object[] { "Low", "Medium", "High", "Critical" });
            _priorityComboBox.SelectedIndex = 1;
            AddField("Priority:", _priorityComboBox);

            _assignedToComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = ThemeManager.BodyFont };
            AddField("Assign To (Optional):", _assignedToComboBox);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button createButton = new Button { Text = "CREATE TASK", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(createButton, false);
            createButton.ForeColor = Color.FromArgb(76, 175, 80); // Green
            createButton.Click += CreateButton_Click;
            footer.Controls.Add(createButton);

            Button cancelButton = new Button { Text = "CANCEL", Width = 150, Dock = DockStyle.Right };
            ThemeManager.StyleButton(cancelButton, false);
            cancelButton.Click += (s, e) => this.Close();
            footer.Controls.Add(cancelButton);

            this.ResumeLayout(false);
            
            PopulateSocieties();
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

        private void SocietyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_societyComboBox.SelectedIndex < 0) return;

            try
            {
                int societyId = ((ComboBoxItem)_societyComboBox.SelectedItem).Value;
                var members = _societyService.GetSocietyMembers(societyId);
                
                _assignedToComboBox.Items.Clear();
                _assignedToComboBox.Items.Add(new ComboBoxItem { Text = "-- Society-Wide Task (No Assignment) --", Value = 0 });
                
                foreach (var member in members)
                {
                    var student = _userRepository.GetUserById(member.StudentId);
                    if (student != null)
                    {
                        _assignedToComboBox.Items.Add(new ComboBoxItem { Text = student.FullName, Value = student.UserId });
                    }
                }
                _assignedToComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load members: {ex.Message}");
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
                string description = _descriptionTextBox.Text.Trim();
                
                int? assignedTo = null;
                if (_assignedToComboBox.SelectedIndex > 0)
                {
                    assignedTo = ((ComboBoxItem)_assignedToComboBox.SelectedItem).Value;
                }
                
                _societyService.CreateTask(societyId, title, description, dueDate, priority, assignedTo);
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
