using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using Microsoft.Data.SqlClient;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class AdminReportsForm : Form
    {
        private DataGridView _statsGrid;
        private ComboBox _reportTypeSelector;
        private Label _statSummary;

        public AdminReportsForm()
        {
            InitializeComponent();
            _reportTypeSelector.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "University Analytics - FAST Societies";
            this.Size = new System.Drawing.Size(1100, 800);
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
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); // Header
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Footer
            this.Controls.Add(mainGrid);

            // Window Controls
            FlowLayoutPanel windowControls = new FlowLayoutPanel
            {
                Size = new Size(100, 40),
                Location = new Point(1000, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent
            };
            this.Controls.Add(windowControls);
            windowControls.BringToFront();

            Button closeBtn = new Button { Text = "×", Size = new Size(40, 40), FlatStyle = FlatStyle.Flat, ForeColor = ThemeManager.TextSecondary, Font = new Font("Arial", 18, FontStyle.Bold), Cursor = Cursors.Hand };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            windowControls.Controls.Add(closeBtn);

            // Header Section
            Panel headerPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(headerPanel, 0, 0);

            Label titleLabel = new Label
            {
                Text = "University Analytics Hub",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            headerPanel.Controls.Add(titleLabel);

            _reportTypeSelector = new ComboBox
            {
                Width = 280,
                Location = new Point(0, 50),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ThemeManager.Surface,
                ForeColor = ThemeManager.TextPrimary,
                FlatStyle = FlatStyle.Flat,
                Font = ThemeManager.BodyFont
            };
            _reportTypeSelector.Items.AddRange(new string[] { "User Distribution", "Society Performance", "Event Participation", "System Logs Summary" });
            _reportTypeSelector.SelectedIndexChanged += (s, e) => LoadReportData();
            headerPanel.Controls.Add(_reportTypeSelector);

            // Content Section (Using TableLayoutPanel to prevent overlap)
            TableLayoutPanel contentPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(0, 10, 0, 0)
            };
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _statSummary = new Label
            {
                Text = "Initializing statistics...",
                Font = ThemeManager.HeaderFont,
                ForeColor = ThemeManager.Accent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            contentPanel.Controls.Add(_statSummary, 0, 0);

            _statsGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_statsGrid);
            _statsGrid.ColumnHeadersVisible = true; // Explicitly force visibility
            contentPanel.Controls.Add(_statsGrid, 0, 1);

            // Footer Section
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button exportBtn = new Button { Text = "EXPORT UNIVERSITY DATA", Width = 260, Dock = DockStyle.Left };
            ThemeManager.StyleButton(exportBtn, false);
            exportBtn.ForeColor = Color.FromArgb(0, 255, 159);
            exportBtn.Click += ExportBtn_Click;
            footer.Controls.Add(exportBtn);

            Button refreshBtn = new Button { Text = "REFRESH DATA", Width = 160, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(refreshBtn, false);
            refreshBtn.Click += (s, e) => LoadReportData();
            footer.Controls.Add(refreshBtn);

            Button backBtn = new Button { Text = "BACK", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(backBtn, false);
            backBtn.Click += (s, e) => this.Close();
            footer.Controls.Add(backBtn);

            this.ResumeLayout(false);
        }

        private void LoadReportData()
        {
            _statsGrid.Columns.Clear();
            _statsGrid.Rows.Clear();

            string selected = _reportTypeSelector.SelectedItem.ToString();

            try
            {
                if (selected == "User Distribution") LoadUserDistribution();
                else if (selected == "Society Performance") LoadSocietyPerformance();
                else if (selected == "Event Participation") LoadEventParticipation();
                else if (selected == "System Logs Summary") LoadLogsSummary();
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load data: {ex.Message}");
            }
        }

        private void LoadUserDistribution()
        {
            _statsGrid.Columns.Add("Role", "USER ROLE");
            _statsGrid.Columns.Add("Count", "TOTAL USERS");
            _statsGrid.Columns.Add("Active", "ACTIVE");
            _statsGrid.Columns.Add("Suspended", "SUSPENDED");

            var userRepo = new UserRepository();
            var users = userRepo.GetAllUsers();

            int studentsActive = 0, studentsSuspended = 0;
            int headsActive = 0, headsSuspended = 0;
            int adminsActive = 0, adminsSuspended = 0;

            foreach (var u in users)
            {
                if (u.Role == "Student")
                {
                    if (u.Status == "Active") studentsActive++;
                    else studentsSuspended++;
                }
                else if (u.Role == "SocietyHead")
                {
                    if (u.Status == "Active") headsActive++;
                    else headsSuspended++;
                }
                else if (u.Role == "Admin")
                {
                    if (u.Status == "Active") adminsActive++;
                    else adminsSuspended++;
                }
            }

            _statsGrid.Rows.Add("Students", studentsActive + studentsSuspended, studentsActive, studentsSuspended);
            _statsGrid.Rows.Add("Society Heads", headsActive + headsSuspended, headsActive, headsSuspended);
            _statsGrid.Rows.Add("Administrators", adminsActive + adminsSuspended, adminsActive, adminsSuspended);
            
            int totalActive = studentsActive + headsActive + adminsActive;
            int totalSuspended = studentsSuspended + headsSuspended + adminsSuspended;
            _statSummary.Text = $"Total System Users: {users.Count} | Active: {totalActive} | Suspended: {totalSuspended}";
        }

        private void LoadSocietyPerformance()
        {
            _statsGrid.Columns.Add("Society", "SOCIETY NAME");
            _statsGrid.Columns.Add("Status", "STATUS");
            _statsGrid.Columns.Add("Members", "MEMBERS");
            _statsGrid.Columns.Add("Events", "EVENTS");

            var socService = new SocietyService();
            var societies = socService.GetAllSocieties();

            foreach (var s in societies)
            {
                int memberCount = socService.GetMemberCount(s.SocietyId);
                int eventCount = socService.GetSocietyEvents(s.SocietyId).Count;
                _statsGrid.Rows.Add(s.SocietyName, s.Status.ToUpper(), memberCount, eventCount);
            }

            _statSummary.Text = $"Total Registered Societies: {societies.Count}";
        }

        private void LoadEventParticipation()
        {
            _statsGrid.Columns.Add("Event", "EVENT NAME");
            _statsGrid.Columns.Add("Society", "SOCIETY");
            _statsGrid.Columns.Add("Registrations", "REGISTRATIONS");
            _statsGrid.Columns.Add("Date", "DATE");

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT e.EventTitle, s.SocietyName, 
                               (SELECT COUNT(*) FROM [EventRegistration] WHERE EventId = e.EventId) as Regs,
                               e.EventDate
                               FROM [Event] e
                               JOIN [Society] s ON e.SocietyId = s.SocietyId
                               ORDER BY e.EventDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        int totalRegs = 0;
                        while (r.Read())
                        {
                            int regs = r.GetInt32(2);
                            totalRegs += regs;
                            _statsGrid.Rows.Add(r.GetString(0), r.GetString(1), regs, r.GetDateTime(3).ToShortDateString());
                        }
                        _statSummary.Text = $"University-Wide Event Participation: {totalRegs} Total Registrations";
                    }
                }
            }
        }

        private void LoadLogsSummary()
        {
            _statsGrid.Columns.Add("Action", "ACTION TYPE");
            _statsGrid.Columns.Add("Count", "OCCURRENCES");

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT Action, COUNT(*) FROM [SystemLog] GROUP BY Action ORDER BY COUNT(*) DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            _statsGrid.Rows.Add(r.GetString(0), r.GetInt32(1));
                        }
                    }
                }
            }
            _statSummary.Text = "System Activity Frequency Analysis";
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder csv = new StringBuilder();
                foreach (DataGridViewColumn col in _statsGrid.Columns)
                {
                    csv.Append(col.HeaderText + ",");
                }
                csv.AppendLine();

                foreach (DataGridViewRow row in _statsGrid.Rows)
                {
                    if (row.IsNewRow) continue;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        csv.Append(cell.Value?.ToString().Replace(",", ";") + ",");
                    }
                    csv.AppendLine();
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"University_Report_{_reportTypeSelector.SelectedItem.ToString().Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string filePath = Path.Combine(desktopPath, fileName);

                File.WriteAllText(filePath, csv.ToString());
                UIHelpers.ShowInfo($"Report exported successfully to Desktop:\n{fileName}");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Export failed: {ex.Message}");
            }
        }
    }
}
