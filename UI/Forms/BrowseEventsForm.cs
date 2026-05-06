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

        private Label _emptyLabel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Browse Events - FAST Societies";
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
                Text = "Upcoming Events",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Area
            Panel contentPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _eventsGrid = new DataGridView { Dock = DockStyle.Fill, Visible = false };
            ThemeManager.StyleGrid(_eventsGrid);
            _eventsGrid.Columns.Add("EventId", "ID");
            _eventsGrid.Columns.Add("EventTitle", "EVENT TITLE");
            _eventsGrid.Columns.Add("Society", "SOCIETY");
            _eventsGrid.Columns.Add("Date", "DATE");
            _eventsGrid.Columns.Add("Location", "LOCATION");
            _eventsGrid.Columns.Add("Registrations", "REGISTRATIONS");
            _eventsGrid.Columns.Add("Status", "STATUS");
            contentPanel.Controls.Add(_eventsGrid);

            _emptyLabel = new Label
            {
                Text = "There are no upcoming events at the moment.\nKeep an eye on the dashboard for updates!",
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

            Button registerButton = new Button { Text = "REGISTER NOW", Width = 200, Dock = DockStyle.Left };
            ThemeManager.StyleButton(registerButton);
            registerButton.Click += RegisterButton_Click;
            footer.Controls.Add(registerButton);

            Button detailsButton = new Button { Text = "VIEW DETAILS", Width = 150, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(detailsButton, false);
            detailsButton.Click += DetailsButton_Click;
            footer.Controls.Add(detailsButton);

            Button closeButton = new Button { Text = "BACK TO DASHBOARD", Width = 200, Dock = DockStyle.Right };
            ThemeManager.StyleButton(closeButton, false);
            closeButton.Click += (s, e) => this.Close();
            footer.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadEvents()
        {
            try
            {
                _eventsGrid.Rows.Clear();
                List<Event> events = _studentService.GetUpcomingEvents();

                if (events == null || events.Count == 0)
                {
                    _eventsGrid.Visible = false;
                    _emptyLabel.Visible = true;
                }
                else
                {
                    _eventsGrid.Visible = true;
                    _emptyLabel.Visible = false;

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
                UIHelpers.ShowError("Please select an event from the list.");
                return;
            }

            try
            {
                int eventId = (int)_eventsGrid.SelectedRows[0].Cells[0].Value;
                string eventTitle = (string)_eventsGrid.SelectedRows[0].Cells[1].Value;

                if (UIHelpers.ShowConfirm($"Would you like to register for '{eventTitle}'?"))
                {
                    _studentService.RegisterForEvent(_studentId, eventId);
                    UIHelpers.ShowInfo("Registration successful! Your ticket is available in 'My Tickets'.");
                    LoadEvents();
                }
            }
            catch (EventCapacityExceededException)
            {
                UIHelpers.ShowError("This event is fully booked.");
            }
            catch (DuplicateResourceException)
            {
                UIHelpers.ShowError("You have already registered for this event.");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to process registration: {ex.Message}");
            }
        }

        private void DetailsButton_Click(object sender, EventArgs e)
        {
            if (_eventsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select an event to view details.");
                return;
            }

            int eventId = (int)_eventsGrid.SelectedRows[0].Cells[0].Value;
            Event evt = _studentService.GetEventDetails(eventId);
            
            string details = $"EVENT: {evt.EventTitle}\n\n" +
                           $"DATE: {UIHelpers.FormatDate(evt.EventDate)}\n" +
                           $"TIME: {(evt.EventTime.HasValue ? evt.EventTime.Value.ToString(@"hh\:mm") : "TBD")}\n" +
                           $"LOCATION: {evt.Location ?? "TBD"}\n" +
                           $"CAPACITY: {evt.Capacity?.ToString() ?? "Unlimited"}\n" +
                           $"STATUS: {evt.Status}\n\n" +
                           $"DESCRIPTION:\n{evt.Description}";
            
            UIHelpers.ShowInfo(details, "Event Details");
        }
    }
}
