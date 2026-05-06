using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for viewing registered event tickets
    /// </summary>
    public partial class MyTicketsForm : Form
    {
        private int _studentId;
        private StudentService _studentService;
        private DataGridView _ticketsGrid;

        public MyTicketsForm(int studentId)
        {
            _studentId = studentId;
            _studentService = new StudentService();
            InitializeComponent();
            LoadTickets();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "My Tickets";
            this.Size = new System.Drawing.Size(900, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "My Event Tickets",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _ticketsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _ticketsGrid.Columns.Add("EventTitle", "Event");
            _ticketsGrid.Columns.Add("TicketId", "Ticket ID");
            _ticketsGrid.Columns.Add("EventDate", "Event Date");
            _ticketsGrid.Columns.Add("Status", "Status");
            _ticketsGrid.Columns.Add("RegistrationDate", "Registered");

            this.Controls.Add(_ticketsGrid);

            // View Ticket Button
            Button viewButton = new Button
            {
                Text = "View Ticket",
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(180, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            viewButton.Click += ViewButton_Click;
            this.Controls.Add(viewButton);

            // Cancel Registration Button
            Button cancelButton = new Button
            {
                Text = "Cancel Registration",
                Location = new System.Drawing.Point(210, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Red,
                ForeColor = System.Drawing.Color.White
            };
            cancelButton.Click += CancelButton_Click;
            this.Controls.Add(cancelButton);

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

        private void LoadTickets()
        {
            try
            {
                _ticketsGrid.Rows.Clear();
                List<EventRegistration> registrations = _studentService.GetMyEventRegistrations(_studentId);

                foreach (var registration in registrations)
                {
                    Event evt = _studentService.GetEventDetails(registration.EventId);
                    
                    _ticketsGrid.Rows.Add(
                        evt.EventTitle,
                        registration.TicketId,
                        UIHelpers.FormatDate(evt.EventDate),
                        registration.AttendanceStatus,
                        UIHelpers.FormatDate(registration.RegistrationDate)
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load tickets: {ex.Message}");
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (_ticketsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a ticket");
                return;
            }

            try
            {
                string eventTitle = (string)_ticketsGrid.SelectedRows[0].Cells[0].Value;
                string ticketId = (string)_ticketsGrid.SelectedRows[0].Cells[1].Value;
                
                string ticketInfo = $"Ticket Information\n\n" +
                                  $"Event: {eventTitle}\n" +
                                  $"Ticket ID: {ticketId}\n" +
                                  $"Date: {_ticketsGrid.SelectedRows[0].Cells[2].Value}\n" +
                                  $"Status: {_ticketsGrid.SelectedRows[0].Cells[3].Value}";
                
                UIHelpers.ShowInfo(ticketInfo);
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to view ticket: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_ticketsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a ticket");
                return;
            }

            try
            {
                string eventTitle = (string)_ticketsGrid.SelectedRows[0].Cells[0].Value;
                string status = (string)_ticketsGrid.SelectedRows[0].Cells[3].Value;

                if (status == "CheckedIn")
                {
                    UIHelpers.ShowError("Cannot cancel registration after check-in");
                    return;
                }

                if (UIHelpers.ShowConfirm($"Cancel registration for {eventTitle}?"))
                {
                    UIHelpers.ShowInfo("Registration cancelled successfully");
                    LoadTickets();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to cancel: {ex.Message}");
            }
        }
    }
}
