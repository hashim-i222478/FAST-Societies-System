using System;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Main dashboard for Admin users
    /// </summary>
    public partial class AdminMainForm : Form
    {
        private int _adminId;
        private string _adminName;
        private ApprovalService _approvalService;

        public AdminMainForm()
        {
            _adminId = AuthenticationManager.Instance.CurrentUser.UserId;
            _adminName = AuthenticationManager.Instance.CurrentUser.FullName;
            _approvalService = new ApprovalService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Admin Control - FAST Societies";
            this.Size = new System.Drawing.Size(1200, 800);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeManager.Background;

            // --- Main Container ---
            TableLayoutPanel mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            this.Controls.Add(mainGrid);

            // --- Sidebar ---
            Panel sidebar = new Panel { Dock = DockStyle.Fill, BackColor = ThemeManager.Surface };
            mainGrid.Controls.Add(sidebar, 0, 0);

            TableLayoutPanel sidebarLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                Padding = new Padding(0, 20, 0, 20)
            };
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); // Title
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Overview
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Users
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Approvals
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Logs
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Reports
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Spacer
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Logout
            sidebar.Controls.Add(sidebarLayout);

            Label sidebarTitle = new Label { Text = "ADMIN\nPORTAL", Font = new Font("Trebuchet MS", 18, FontStyle.Bold), ForeColor = ThemeManager.Accent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            sidebarLayout.Controls.Add(sidebarTitle, 0, 0);

            AddSidebarButton(sidebarLayout, "Overview", (s, e) => { }, 1);
            AddSidebarButton(sidebarLayout, "User Management", (s, e) => OpenUserManagement(), 2);
            AddSidebarButton(sidebarLayout, "Societies", (s, e) => OpenSocietyManagement(), 3);
            AddSidebarButton(sidebarLayout, "System Logs", (s, e) => OpenActivityLog(), 4);
            AddSidebarButton(sidebarLayout, "Reports", (s, e) => OpenUniversityReport(), 5);

            Button logoutBtn = new Button { Text = "Logout", Dock = DockStyle.Fill };
            ThemeManager.StyleSidebarButton(logoutBtn);
            logoutBtn.ForeColor = Color.FromArgb(233, 69, 96);
            logoutBtn.Click += (s, e) => Logout();
            sidebarLayout.Controls.Add(logoutBtn, 0, 7);

            // --- Window Controls ---
            FlowLayoutPanel windowControls = new FlowLayoutPanel
            {
                Size = new Size(150, 40),
                Location = new Point(1050, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 0, 0, 0)
            };
            this.Controls.Add(windowControls);
            windowControls.BringToFront();

            AddWindowButton(windowControls, "×", (s, e) => Application.Exit(), Color.FromArgb(233, 69, 96));
            AddWindowButton(windowControls, "□", (s, e) => this.WindowState = this.WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized, ThemeManager.TextSecondary);
            AddWindowButton(windowControls, "—", (s, e) => this.WindowState = FormWindowState.Minimized, ThemeManager.TextSecondary);

            // --- Content Container (Table) ---
            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(50, 60, 50, 40)
            };
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); // Header
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));  // Spacer
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Grid
            mainGrid.Controls.Add(contentLayout, 1, 0);

            Panel header = new Panel { Dock = DockStyle.Fill };
            contentLayout.Controls.Add(header, 0, 0);

            Label welcome = new Label { Text = $"System Overview: {_adminName}", Font = ThemeManager.TitleFont, ForeColor = ThemeManager.TextPrimary, AutoSize = true, Location = new Point(0, 0) };
            header.Controls.Add(welcome);

            FlowLayoutPanel grid = new FlowLayoutPanel { Dock = DockStyle.Fill, Margin = new Padding(0), AutoScroll = true };
            contentLayout.Controls.Add(grid, 0, 2);

            AddDashboardCard(grid, "User Management", "Control access and permissions.", Color.FromArgb(0, 212, 255), (s, e) => OpenUserManagement());
            AddDashboardCard(grid, "Societies", "Create, approve, or suspend societies.", Color.FromArgb(233, 69, 96), (s, e) => OpenSocietyManagement());
            AddDashboardCard(grid, "Event Approvals", "Review and approve pending events.", Color.FromArgb(0, 255, 159), (s, e) => OpenEventApprovals());
            AddDashboardCard(grid, "Monitoring", "System health and activity logs.", Color.FromArgb(106, 76, 239), (s, e) => OpenActivityLog());
            AddDashboardCard(grid, "Reports", "Generate university-wide stats.", Color.FromArgb(255, 171, 64), (s, e) => OpenUniversityReport());

            this.ResumeLayout(false);
        }

        private void AddWindowButton(FlowLayoutPanel container, string text, EventHandler onClick, Color hoverColor)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(40, 40),
                FlatStyle = FlatStyle.Flat,
                ForeColor = ThemeManager.TextSecondary,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            btn.MouseEnter += (s, e) => btn.ForeColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.ForeColor = ThemeManager.TextSecondary;
            container.Controls.Add(btn);
        }

        private void AddSidebarButton(TableLayoutPanel layout, string text, EventHandler onClick, int row)
        {
            Button btn = new Button { Text = text, Dock = DockStyle.Fill };
            ThemeManager.StyleSidebarButton(btn);
            btn.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
            btn.Click += onClick;
            layout.Controls.Add(btn, 0, row);
        }

        private void AddDashboardCard(FlowLayoutPanel grid, string title, string desc, Color accent, EventHandler onClick)
        {
            Panel card = new Panel
            {
                Size = new Size(250, 180),
                Margin = new Padding(0, 0, 30, 30),
                Cursor = Cursors.Hand
            };
            ModernControls.ApplyCardStyle(card);
            card.Click += onClick;

            Label titleLbl = new Label
            {
                Text = title,
                Font = ThemeManager.HeaderFont,
                ForeColor = accent,
                Location = new Point(20, 20),
                Size = new Size(210, 30)
            };
            titleLbl.Click += onClick;
            card.Controls.Add(titleLbl);

            Label descLbl = new Label
            {
                Text = desc,
                Font = ThemeManager.SmallFont,
                ForeColor = ThemeManager.TextSecondary,
                Location = new Point(20, 60),
                Size = new Size(210, 80)
            };
            descLbl.Click += onClick;
            card.Controls.Add(descLbl);

            grid.Controls.Add(card);
        }

        private void OpenUserManagement()
        {
            UserManagementForm form = new UserManagementForm();
            form.ShowDialog();
        }

        private void OpenUserStatistics()
        {
            UIHelpers.ShowInfo("User Statistics - View user demographics and activity");
        }

        private void OpenSocietyManagement()
        {
            AdminSocietyManagementForm form = new AdminSocietyManagementForm();
            form.ShowDialog();
        }

        private void OpenSocietyApprovals()
        {
            SocietyApprovalForm form = new SocietyApprovalForm();
            form.ShowDialog();
        }

        private void OpenEventApprovals()
        {
            EventApprovalForm form = new EventApprovalForm();
            form.ShowDialog();
        }

        private void OpenActivityLog()
        {
            SystemLogsForm form = new SystemLogsForm();
            form.ShowDialog();
        }

        private void OpenSystemStatus()
        {
            UIHelpers.ShowInfo("System Status - Check database connectivity and system health");
        }

        private void OpenUniversityReport()
        {
            AdminReportsForm form = new AdminReportsForm();
            form.ShowDialog();
        }

        private void OpenMembershipReport()
        {
            UIHelpers.ShowInfo("Membership Report - View detailed membership data across all societies");
        }

        private void OpenActivityReport()
        {
            UIHelpers.ShowInfo("Activity Report - View event attendance and participation trends");
        }

        private void OpenChangePassword()
        {
            ChangePasswordForm form = new ChangePasswordForm(_adminId);
            form.ShowDialog();
        }

        private void OpenProfile()
        {
            User user = AuthenticationManager.Instance.CurrentUser;
            string profileInfo = $"Admin Profile\n\n" +
                               $"Name: {user.FullName}\n" +
                               $"Email: {user.Email}\n" +
                               $"Phone: {user.PhoneNumber}\n" +
                               $"Role: {user.Role}\n" +
                               $"Status: {user.Status}";
            UIHelpers.ShowInfo(profileInfo);
        }

        private void Logout()
        {
            if (UIHelpers.ShowConfirm("Are you sure you want to logout?"))
            {
                AuthenticationManager.Instance.Logout();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }
    }
}
