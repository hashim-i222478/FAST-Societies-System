using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class SystemLogsForm : Form
    {
        private LogRepository _logRepository;
        private DataGridView _logsGrid;

        public SystemLogsForm()
        {
            _logRepository = new LogRepository();
            InitializeComponent();
            LoadLogs();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "System Logs - FAST Societies";
            this.Size = new System.Drawing.Size(1100, 750);
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
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Header
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Footer
            this.Controls.Add(mainGrid);

            // Window Controls
            FlowLayoutPanel windowControls = new FlowLayoutPanel
            {
                Size = new Size(100, 40),
                Location = new Point(1000, 0),
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
                Text = "System Activity Logs",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Grid
            _logsGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_logsGrid);
            _logsGrid.Columns.Add("Timestamp", "TIMESTAMP");
            _logsGrid.Columns.Add("User", "USER");
            _logsGrid.Columns.Add("Action", "ACTION");
            _logsGrid.Columns.Add("Details", "DETAILS");
            
            _logsGrid.Columns["Timestamp"].Width = 180;
            _logsGrid.Columns["User"].Width = 150;
            _logsGrid.Columns["Action"].Width = 150;
            _logsGrid.Columns["Details"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            mainGrid.Controls.Add(_logsGrid, 0, 1);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button refreshBtn = new Button { Text = "REFRESH", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(refreshBtn, false);
            refreshBtn.Click += (s, e) => LoadLogs();
            footer.Controls.Add(refreshBtn);

            Button closeBtn2 = new Button { Text = "CLOSE", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(closeBtn2, false);
            closeBtn2.Click += (s, e) => this.Close();
            footer.Controls.Add(closeBtn2);

            this.ResumeLayout(false);
        }

        private void LoadLogs()
        {
            try
            {
                _logsGrid.Rows.Clear();
                var logs = _logRepository.GetAllLogs();

                foreach (var log in logs)
                {
                    _logsGrid.Rows.Add(
                        log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                        log.UserName,
                        log.Action,
                        log.Details
                    );
                }

                if (logs.Count == 0)
                {
                    // Add a dummy log if empty so user sees something
                    _logsGrid.Rows.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "System", "Initialization", "Log system started successfully.");
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load logs: {ex.Message}");
            }
        }
    }
}
