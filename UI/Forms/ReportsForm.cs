using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;
using Task = FASTSocietiesSystem.Models.Task;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class ReportsForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private UserRepository _userRepo;
        private EventRepository _eventRepo;
        private EventRegistrationRepository _regRepo;
        private TaskRepository _taskRepo;
        
        private ComboBox _reportTypeCombo;
        private ComboBox _societyCombo;
        private DataGridView _reportGrid;
        private Label _statsLabel;

        public ReportsForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            _userRepo = new UserRepository();
            _eventRepo = new EventRepository();
            _regRepo = new EventRegistrationRepository();
            _taskRepo = new TaskRepository();
            
            InitializeComponent();
            PopulateSocieties();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Society Reports - FAST Societies";
            this.Size = new Size(1100, 750);
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

            // Header
            Panel header = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(header, 0, 0);

            Label title = new Label { Text = "Society Analytics & Reports", Font = ThemeManager.TitleFont, ForeColor = ThemeManager.TextPrimary, AutoSize = true, Location = new Point(0, 0) };
            header.Controls.Add(title);

            _statsLabel = new Label { Text = "Select a report type to view statistics", Font = ThemeManager.BodyFont, ForeColor = ThemeManager.Accent, AutoSize = true, Location = new Point(5, 55) };
            header.Controls.Add(_statsLabel);

            // Content - Filters & Grid
            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Margin = new Padding(0, 20, 0, 0)
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainGrid.Controls.Add(contentLayout, 0, 1);

            _societyCombo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = ThemeManager.BodyFont, Margin = new Padding(0, 0, 10, 0) };
            _societyCombo.SelectedIndexChanged += (s, e) => GenerateReport();
            contentLayout.Controls.Add(_societyCombo, 0, 0);

            _reportTypeCombo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = ThemeManager.BodyFont, Margin = new Padding(10, 0, 0, 0) };
            _reportTypeCombo.Items.AddRange(new object[] { "Members Report", "Events Report", "Tasks Analytics" });
            _reportTypeCombo.SelectedIndex = 0;
            _reportTypeCombo.SelectedIndexChanged += (s, e) => GenerateReport();
            contentLayout.Controls.Add(_reportTypeCombo, 1, 0);

            _reportGrid = new DataGridView { Dock = DockStyle.Fill, Margin = new Padding(0, 20, 0, 0) };
            ThemeManager.StyleGrid(_reportGrid);
            contentLayout.Controls.Add(_reportGrid, 0, 1);
            contentLayout.SetColumnSpan(_reportGrid, 2);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button exportBtn = new Button { Text = "EXPORT TO CSV", Width = 180, Dock = DockStyle.Left };
            ThemeManager.StyleButton(exportBtn, false);
            exportBtn.ForeColor = Color.FromArgb(0, 212, 255);
            exportBtn.Click += ExportBtn_Click;
            footer.Controls.Add(exportBtn);

            Button backBtn = new Button { Text = "BACK", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(backBtn, false);
            backBtn.Click += (s, e) => this.Close();
            footer.Controls.Add(backBtn);

            this.ResumeLayout(false);
        }

        private void PopulateSocieties()
        {
            try
            {
                var societies = _societyService.GetMySocieties(_headId);
                _societyCombo.Items.Clear();
                foreach (var s in societies)
                {
                    _societyCombo.Items.Add(new { Text = s.SocietyName, Value = s.SocietyId });
                }
                _societyCombo.DisplayMember = "Text";
                _societyCombo.ValueMember = "Value";
                if (_societyCombo.Items.Count > 0) _societyCombo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Error loading societies: {ex.Message}");
            }
        }

        private void GenerateReport()
        {
            if (_societyCombo.SelectedItem == null) return;
            
            dynamic selectedSoc = _societyCombo.SelectedItem;
            int societyId = selectedSoc.Value;
            string reportType = _reportTypeCombo.SelectedItem.ToString();

            _reportGrid.Columns.Clear();
            _reportGrid.Rows.Clear();

            try
            {
                if (reportType == "Members Report")
                {
                    _reportGrid.Columns.Add("Name", "STUDENT NAME");
                    _reportGrid.Columns.Add("Email", "EMAIL");
                    _reportGrid.Columns.Add("Joined", "JOIN DATE");
                    _reportGrid.Columns.Add("Status", "MEMBERSHIP STATUS");

                    var members = _societyService.GetSocietyMembers(societyId);
                    foreach (var m in members)
                    {
                        var user = _userRepo.GetUserById(m.StudentId);
                        _reportGrid.Rows.Add(user?.FullName ?? "N/A", user?.Email ?? "N/A", UIHelpers.FormatDate(m.JoinDate), m.Status);
                    }
                    _statsLabel.Text = $"Total Members: {members.Count} | Active: {members.FindAll(x => x.Status == "Active").Count}";
                }
                else if (reportType == "Events Report")
                {
                    _reportGrid.Columns.Add("Title", "EVENT TITLE");
                    _reportGrid.Columns.Add("Date", "EVENT DATE");
                    _reportGrid.Columns.Add("Registrations", "REGISTRATIONS");
                    _reportGrid.Columns.Add("Status", "STATUS");

                    var events = _eventRepo.GetEventsBySociety(societyId);
                    int totalRegs = 0;
                    foreach (var ev in events)
                    {
                        // Logic to get registration count
                        int count = GetRegistrationCount(ev.EventId);
                        totalRegs += count;
                        _reportGrid.Rows.Add(ev.EventTitle, UIHelpers.FormatDate(ev.EventDate), count, ev.Status);
                    }
                    _statsLabel.Text = $"Total Events: {events.Count} | Total Registrations: {totalRegs}";
                }
                else if (reportType == "Tasks Analytics")
                {
                    _reportGrid.Columns.Add("Title", "TASK TITLE");
                    _reportGrid.Columns.Add("AssignedTo", "ASSIGNED TO");
                    _reportGrid.Columns.Add("Status", "STATUS");
                    _reportGrid.Columns.Add("Due", "DUE DATE");

                    var tasks = _taskRepo.GetSocietyTasks(societyId);
                    int completed = 0;
                    foreach (var t in tasks)
                    {
                        string assigned = "Society-Wide";
                        if (t.AssignedTo.HasValue) {
                            var u = _userRepo.GetUserById(t.AssignedTo.Value);
                            assigned = u?.FullName ?? "N/A";
                        }
                        if (t.Status == "Completed") completed++;
                        _reportGrid.Rows.Add(t.TaskTitle, assigned, t.Status, UIHelpers.FormatDate(t.DueDate));
                    }
                    float rate = tasks.Count > 0 ? (float)completed / tasks.Count * 100 : 0;
                    _statsLabel.Text = $"Total Tasks: {tasks.Count} | Completed: {completed} | Efficiency: {rate:F1}%";
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to generate report: {ex.Message}");
            }
        }

        private int GetRegistrationCount(int eventId)
        {
            // Simple helper to count registrations from DAL
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT COUNT(*) FROM [EventRegistration] WHERE EventId = @id", conn);
                cmd.Parameters.AddWithValue("@id", eventId);
                return (int)cmd.ExecuteScalar();
            }
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            if (_reportGrid.Rows.Count == 0)
            {
                UIHelpers.ShowError("No data to export.");
                return;
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                
                // Headers
                for (int i = 0; i < _reportGrid.Columns.Count; i++)
                {
                    sb.Append(_reportGrid.Columns[i].HeaderText + (i == _reportGrid.Columns.Count - 1 ? "" : ","));
                }
                sb.AppendLine();

                // Data
                foreach (DataGridViewRow row in _reportGrid.Rows)
                {
                    if (row.IsNewRow) continue;
                    for (int i = 0; i < _reportGrid.Columns.Count; i++)
                    {
                        string val = row.Cells[i].Value?.ToString() ?? "";
                        if (val.Contains(",")) val = "\"" + val + "\"";
                        sb.Append(val + (i == _reportGrid.Columns.Count - 1 ? "" : ","));
                    }
                    sb.AppendLine();
                }

                string reportName = _reportTypeCombo.SelectedItem.ToString().Replace(" ", "_");
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                
                File.WriteAllText(path, sb.ToString());
                UIHelpers.ShowInfo($"Report successfully exported to Desktop:\n{Path.GetFileName(path)}", "Export Complete");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Export failed: {ex.Message}");
            }
        }
    }
}
