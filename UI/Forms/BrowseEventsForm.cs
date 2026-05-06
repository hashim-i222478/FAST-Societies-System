using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for browsing available events
    /// </summary>
    public partial class BrowseEventsForm : Form
    {
        private int _studentId;
        private StudentService _studentService;
        private DataGridView _eventsGrid;

        public BrowseEventsForm(int studentId)
        {
            _studentId = studentId;
            _studentService = new StudentService();
            InitializeComponent();
            LoadEvents();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Browse Events";
            this.Size = new System.Drawing.Size(900, 550);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Upcoming Events",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _eventsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 380),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _eventsGrid.Columns.Add("EventId", "ID");
            _eventsGrid.Columns.Add("EventTitle", "Event Title");
            _eventsGrid.Columns.Add("Society", "Society");
            _eventsGrid.Columns.Add("Date", "Date");
            _eventsGrid.Columns.Add("Location", "Location");
            _eventsGrid.Columns.Add("Registrations", "Registrations");
            _eventsGrid.Columns.Add("Status", "Status");

            this.Controls.Add(_eventsGrid);

            // Register Button
            Button registerButton = new Button
            {
                Text = "Register for Event",
                Location = new System.Drawing.Point(20, 450),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            registerButton.Click += RegisterButton_Click;
            this.Controls.Add(registerButton);

            // View Details Button
            Button detailsButton = new Button
            {
                Text = "View Details",
                Location = new System.Drawing.Point(230, 450),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            detailsButton.Click += DetailsButton_Click;
            this.Controls.Add(detailsButton);

            // Close Button
            Button closeButton = new Button
            {
                Text = "Close",
                Location = new System.Drawing.Point(670, 450),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadEvents()
        {
            try
            {
                _eventsGrid.Rows.Clear();
                List<Event> events = _studentService.GetUpcomingEvents();

                foreach (var evt in events)
                {
                    Society society = _studentService.GetSocietyDetails(evt.SocietyId);
                    int registered = new EventRepository().GetEventRegistrationCount(evt.EventId);

                    _eventsGrid.Rows.Add(
                        evt.EventId,
                        evt.EventTitle,
                        society.SocietyName,
                        UIHelpers.FormatDate(evt.EventDate),
                        evt.Location ?? "TBD",
                        registered,
                        evt.Status
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load events: {ex.Message}");
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (_eventsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select an event");
                return;
            }

            try
            {
                int eventId = (int)_eventsGrid.SelectedRows[0].Cells[0].Value;
                string eventTitle = (string)_eventsGrid.SelectedRows[0].Cells[1].Value;

                if (UIHelpers.ShowConfirm($"Register for {eventTitle}?"))
                {
                    _studentService.RegisterForEvent(_studentId, eventId);
                    UIHelpers.ShowInfo("Registration successful! View your ticket in 'My Tickets'");
                    LoadEvents();
                }
            }
            catch (EventCapacityExceededException)
            {
                UIHelpers.ShowError("This event has reached its maximum capacity");
            }
            catch (DuplicateResourceException)
            {
                UIHelpers.ShowError("You are already registered for this event");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Registration failed: {ex.Message}");
            }
        }

        private void DetailsButton_Click(object sender, EventArgs e)
        {
            if (_eventsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select an event");
                return;
            }

            int eventId = (int)_eventsGrid.SelectedRows[0].Cells[0].Value;
            Event evt = _studentService.GetEventDetails(eventId);
            
            string details = $"Event: {evt.EventTitle}\n" +
                           $"Date: {UIHelpers.FormatDate(evt.EventDate)}\n" +
                           $"Time: {(evt.EventTime.HasValue ? evt.EventTime.Value.ToString(@"hh\:mm") : "TBD")}\n" +
                           $"Location: {evt.Location ?? "TBD"}\n" +
                           $"Capacity: {evt.Capacity?.ToString() ?? "Unlimited"}\n" +
                           $"Status: {evt.Status}\n" +
                           $"Description: {evt.Description}";
            
            UIHelpers.ShowInfo(details, "Event Details");
        }
    }
}
