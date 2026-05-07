using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for viewing student's society memberships
    /// </summary>
    public partial class MyMembershipsForm : Form
    {
        private int _studentId;
        private StudentService _studentService;
        private DataGridView _membershipsGrid;

        public MyMembershipsForm(int studentId)
        {
            _studentId = studentId;
            _studentService = new StudentService();
            InitializeComponent();
            LoadMemberships();
        }

        private Label _emptyLabel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "My Memberships - FAST Societies";
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
                Text = "My Society Memberships",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Area
            Panel contentPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _membershipsGrid = new DataGridView { Dock = DockStyle.Fill, Visible = false };
            ThemeManager.StyleGrid(_membershipsGrid);
            _membershipsGrid.Columns.Add("SocietyName", "SOCIETY NAME");
            _membershipsGrid.Columns.Add("JoinDate", "JOIN DATE");
            _membershipsGrid.Columns.Add("Status", "STATUS");
            _membershipsGrid.Columns.Add("UpcomingEvents", "UPCOMING EVENTS");
            contentPanel.Controls.Add(_membershipsGrid);

            _emptyLabel = new Label
            {
                Text = "You are not a member of any societies yet.\nBrowse societies to join and get involved!",
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

            Button viewEventsButton = new Button { Text = "VIEW SOCIETY EVENTS", Width = 220, Dock = DockStyle.Left };
            ThemeManager.StyleButton(viewEventsButton);
            viewEventsButton.Click += ViewEventsButton_Click;
            footer.Controls.Add(viewEventsButton);

            Button leaveButton = new Button { Text = "LEAVE SOCIETY", Width = 180, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(leaveButton, false);
            leaveButton.ForeColor = Color.FromArgb(233, 69, 96);
            leaveButton.Click += LeaveButton_Click;
            footer.Controls.Add(leaveButton);

            Button closeButton = new Button { Text = "BACK TO DASHBOARD", Width = 200, Dock = DockStyle.Right };
            ThemeManager.StyleButton(closeButton, false);
            closeButton.Click += (s, e) => this.Close();
            footer.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadMemberships()
        {
            try
            {
                _membershipsGrid.Rows.Clear();
                List<Membership> memberships = _studentService.GetMyMemberships(_studentId);

                if (memberships == null || memberships.Count == 0)
                {
                    _membershipsGrid.Visible = false;
                    _emptyLabel.Visible = true;
                }
                else
                {
                    _membershipsGrid.Visible = true;
                    _emptyLabel.Visible = false;

                    foreach (var membership in memberships)
                    {
                        Society society = _studentService.GetSocietyDetails(membership.SocietyId);
                        var upcomingEvents = _studentService.GetUpcomingEventsBySociety(membership.SocietyId);
                        
                        _membershipsGrid.Rows.Add(
                            society.SocietyName,
                            UIHelpers.FormatDate(membership.JoinDate),
                            membership.Status,
                            upcomingEvents.Count
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load memberships: {ex.Message}");
            }
        }

        private void ViewEventsButton_Click(object sender, EventArgs e)
        {
            if (_membershipsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society from the list.");
                return;
            }

            BrowseEventsForm browseEventsForm = new BrowseEventsForm(_studentId);
            browseEventsForm.ShowDialog();
        }

        private void LeaveButton_Click(object sender, EventArgs e)
        {
            if (_membershipsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society to leave.");
                return;
            }

            try
            {
                string societyName = (string)_membershipsGrid.SelectedRows[0].Cells[0].Value;
                
                if (UIHelpers.ShowConfirm($"Are you absolutely sure you want to leave '{societyName}'?", "Confirm Leave"))
                {
                    // Logic to leave society would go here
                    UIHelpers.ShowInfo($"You are no longer a member of {societyName}.");
                    LoadMemberships();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"An error occurred while leaving the society: {ex.Message}");
            }
        }
    }
}
