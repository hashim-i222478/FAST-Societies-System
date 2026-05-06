using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for creating new events
    /// </summary>
    public partial class CreateEventForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private ComboBox _societyComboBox;
        private TextBox _titleTextBox;
        private TextBox _descriptionTextBox;
        private DateTimePicker _dateTimePicker;
        private TextBox _locationTextBox;
        private NumericUpDown _capacityUpDown;
        private DateTimePicker _deadlinePicker;

        public CreateEventForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Create Event";
            this.Size = new System.Drawing.Size(450, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Create New Event",
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

            // Event Title
            Label eventTitleLabel = new Label
            {
                Text = "Event Title:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 20)
            };
            this.Controls.Add(eventTitleLabel);
            yPos += 30;

            _titleTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 30),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            this.Controls.Add(_titleTextBox);
            yPos += 35;

            // Event Date
            Label dateLabel = new Label
            {
                Text = "Event Date & Time:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 20)
            };
            this.Controls.Add(dateLabel);
            yPos += 30;

            _dateTimePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 30),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm"
            };
            this.Controls.Add(_dateTimePicker);
            yPos += 35;

            // Location
            Label locationLabel = new Label
            {
                Text = "Location:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 20)
            };
            this.Controls.Add(locationLabel);
            yPos += 30;

            _locationTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(400, 30),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            this.Controls.Add(_locationTextBox);
            yPos += 35;

            // Capacity
            Label capacityLabel = new Label
            {
                Text = "Capacity:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(200, 20)
            };
            this.Controls.Add(capacityLabel);

            _capacityUpDown = new NumericUpDown
            {
                Location = new System.Drawing.Point(20, yPos + 30),
                Size = new System.Drawing.Size(100, 30),
                Minimum = 1,
                Maximum = 10000,
                Value = 50
            };
            this.Controls.Add(_capacityUpDown);
            yPos += 65;

            // Create Button
            Button createButton = new Button
            {
                Text = "Create Event",
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
                DateTime eventDate = _dateTimePicker.Value;
                string location = _locationTextBox.Text.Trim();
                int capacity = (int)_capacityUpDown.Value;

                if (string.IsNullOrEmpty(title))
                {
                    UIHelpers.ShowError("Event title is required");
                    return;
                }

                if (string.IsNullOrEmpty(location))
                {
                    UIHelpers.ShowError("Location is required");
                    return;
                }

                if (eventDate <= DateTime.Now)
                {
                    UIHelpers.ShowError("Event date must be in the future");
                    return;
                }

                int societyId = ((ComboBoxItem)_societyComboBox.SelectedItem).Value;
                
                _societyService.CreateEvent(societyId, title, "", eventDate, location, capacity);
                UIHelpers.ShowInfo("Event created successfully! Pending admin approval.");
                this.Close();
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to create event: {ex.Message}");
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
