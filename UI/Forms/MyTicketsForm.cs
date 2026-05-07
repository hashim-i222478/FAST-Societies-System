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

        private Label _emptyLabel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "My Tickets - FAST Societies";
            this.Size = new System.Drawing.Size(1000, 700);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ThemeManager.Background;

            // --- Main Container ---
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

            // --- Window Controls ---
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
                Text = "My Event Tickets",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Area
            Panel contentPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _ticketsGrid = new DataGridView { Dock = DockStyle.Fill, Visible = false };
            ThemeManager.StyleGrid(_ticketsGrid);
            _ticketsGrid.Columns.Add("EventTitle", "EVENT TITLE");
            _ticketsGrid.Columns.Add("TicketId", "TICKET ID");
            _ticketsGrid.Columns.Add("EventDate", "EVENT DATE");
            _ticketsGrid.Columns.Add("Status", "STATUS");
            _ticketsGrid.Columns.Add("RegistrationDate", "REGISTERED ON");
            _ticketsGrid.Columns.Add("RegistrationId", "ID");
            _ticketsGrid.Columns["RegistrationId"].Visible = false;
            contentPanel.Controls.Add(_ticketsGrid);

            _emptyLabel = new Label
            {
                Text = "You don't have any active tickets.\nBrowse upcoming events to register!",
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

            Button viewButton = new Button { Text = "VIEW TICKET DETAILS", Width = 200, Dock = DockStyle.Left };
            ThemeManager.StyleButton(viewButton);
            viewButton.Click += ViewButton_Click;
            footer.Controls.Add(viewButton);

            Button cancelButton = new Button { Text = "CANCEL REGISTRATION", Width = 200, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(cancelButton, false);
            cancelButton.ForeColor = Color.FromArgb(233, 69, 96);
            cancelButton.Click += CancelButton_Click;
            footer.Controls.Add(cancelButton);

            Button closeButton = new Button { Text = "BACK TO DASHBOARD", Width = 200, Dock = DockStyle.Right };
            ThemeManager.StyleButton(closeButton, false);
            closeButton.Click += (s, e) => this.Close();
            footer.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadTickets()
        {
            try
            {
                _ticketsGrid.Rows.Clear();
                List<EventRegistration> registrations = _studentService.GetMyEventRegistrations(_studentId);

                if (registrations == null || registrations.Count == 0)
                {
                    _ticketsGrid.Visible = false;
                    _emptyLabel.Visible = true;
                }
                else
                {
                    _ticketsGrid.Visible = true;
                    _emptyLabel.Visible = false;

                    foreach (var registration in registrations)
                    {
                        if (registration.AttendanceStatus == "Cancelled") continue;

                        Event evt = _studentService.GetEventDetails(registration.EventId);
                        
                        string displayStatus = registration.AttendanceStatus;
                        if (evt.Status == "Cancelled")
                        {
                            displayStatus = "INVALID (EVENT CANCELLED)";
                        }
                        
                        _ticketsGrid.Rows.Add(
                            evt.EventTitle,
                            registration.TicketId,
                            UIHelpers.FormatDate(evt.EventDate),
                            displayStatus,
                            UIHelpers.FormatDate(registration.RegistrationDate),
                            registration.RegistrationId
                        );
                    }
                    
                    if (_ticketsGrid.Rows.Count == 0)
                    {
                        _ticketsGrid.Visible = false;
                        _emptyLabel.Visible = true;
                    }
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
                UIHelpers.ShowError("Please select a ticket from the list.");
                return;
            }

            try
            {
                string status = (string)_ticketsGrid.SelectedRows[0].Cells[3].Value;
                if (status.Contains("CANCELLED"))
                {
                    UIHelpers.ShowError("This event has been cancelled. The ticket is no longer valid.");
                    return;
                }

                string eventTitle = (string)_ticketsGrid.SelectedRows[0].Cells[0].Value;
                string ticketId = (string)_ticketsGrid.SelectedRows[0].Cells[1].Value;
                
                string ticketInfo = $"TICKET CONFIRMATION\n\n" +
                                  $"EVENT: {eventTitle}\n" +
                                  $"TICKET ID: {ticketId}\n" +
                                  $"DATE: {_ticketsGrid.SelectedRows[0].Cells[2].Value}\n" +
                                  $"STATUS: {_ticketsGrid.SelectedRows[0].Cells[3].Value}\n\n" +
                                  $"Please present this ID at the entrance.";
                
                UIHelpers.ShowInfo(ticketInfo, "E-Ticket Details");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"An error occurred while retrieving ticket details: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_ticketsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a ticket to cancel.");
                return;
            }

            try
            {
                string eventTitle = (string)_ticketsGrid.SelectedRows[0].Cells[0].Value;
                string status = (string)_ticketsGrid.SelectedRows[0].Cells[3].Value;

                if (status.Contains("CANCELLED"))
                {
                    UIHelpers.ShowError("This event is already cancelled.");
                    return;
                }

                if (status == "CheckedIn")
                {
                    UIHelpers.ShowError("You cannot cancel a registration after check-in.");
                    return;
                }

                if (UIHelpers.ShowConfirm($"Are you sure you want to cancel your registration for '{eventTitle}'?", "Confirm Cancellation"))
                {
                    int registrationId = (int)_ticketsGrid.SelectedRows[0].Cells[5].Value;
                    _studentService.CancelEventRegistration(registrationId, _studentId);
                    
                    UIHelpers.ShowInfo("Your registration has been cancelled successfully.");
                    LoadTickets();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"An error occurred during cancellation: {ex.Message}");
            }
        }
    }
}
