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

            this.Text = "FAST Societies - Administrator";
            this.Size = new System.Drawing.Size(1000, 700);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Menu Strip
            MenuStrip menuStrip = new MenuStrip();

            // File Menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("&File");
            fileMenu.DropDownItems.Add("&Logout", null, (s, e) => Logout());
            fileMenu.DropDownItems.Add("E&xit", null, (s, e) => Application.Exit());
            menuStrip.Items.Add(fileMenu);

            // Users Menu
            ToolStripMenuItem usersMenu = new ToolStripMenuItem("&Users");
            usersMenu.DropDownItems.Add("&User Management", null, (s, e) => OpenUserManagement());
            usersMenu.DropDownItems.Add("&View Statistics", null, (s, e) => OpenUserStatistics());
            menuStrip.Items.Add(usersMenu);

            // Approvals Menu
            ToolStripMenuItem approvalsMenu = new ToolStripMenuItem("&Approvals");
            approvalsMenu.DropDownItems.Add("&Society Approvals", null, (s, e) => OpenSocietyApprovals());
            approvalsMenu.DropDownItems.Add("&Event Approvals", null, (s, e) => OpenEventApprovals());
            menuStrip.Items.Add(approvalsMenu);

            // Monitoring Menu
            ToolStripMenuItem monitoringMenu = new ToolStripMenuItem("&Monitoring");
            monitoringMenu.DropDownItems.Add("&Activity Log", null, (s, e) => OpenActivityLog());
            monitoringMenu.DropDownItems.Add("&System Status", null, (s, e) => OpenSystemStatus());
            menuStrip.Items.Add(monitoringMenu);

            // Reports Menu
            ToolStripMenuItem reportsMenu = new ToolStripMenuItem("&Reports");
            reportsMenu.DropDownItems.Add("&University Report", null, (s, e) => OpenUniversityReport());
            reportsMenu.DropDownItems.Add("&Membership Report", null, (s, e) => OpenMembershipReport());
            reportsMenu.DropDownItems.Add("&Activity Report", null, (s, e) => OpenActivityReport());
            menuStrip.Items.Add(reportsMenu);

            // Account Menu
            ToolStripMenuItem accountMenu = new ToolStripMenuItem("&Account");
            accountMenu.DropDownItems.Add("&Change Password", null, (s, e) => OpenChangePassword());
            accountMenu.DropDownItems.Add("&Profile", null, (s, e) => OpenProfile());
            menuStrip.Items.Add(accountMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Welcome Panel
            Panel welcomePanel = new Panel
            {
                Location = new System.Drawing.Point(20, 40),
                Size = new System.Drawing.Size(950, 120),
                BackColor = System.Drawing.Color.LightCyan,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label welcomeLabel = new Label
            {
                Text = $"Welcome, {_adminName}!",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(500, 35)
            };
            welcomePanel.Controls.Add(welcomeLabel);

            Label descLabel = new Label
            {
                Text = "Manage users, approve activities, and monitor the entire system.",
                Font = new System.Drawing.Font("Segoe UI", 11),
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(500, 40)
            };
            welcomePanel.Controls.Add(descLabel);

            this.Controls.Add(welcomePanel);

            // Quick Action Buttons
            int xPos = 40;
            int yPos = 180;
            int buttonWidth = 200;
            int buttonHeight = 80;

            CreateQuickButton("User Management", System.Drawing.Color.SteelBlue, xPos, yPos, buttonWidth, buttonHeight,
                () => OpenUserManagement());
            xPos += buttonWidth + 20;

            CreateQuickButton("Pending Approvals", System.Drawing.Color.Orange, xPos, yPos, buttonWidth, buttonHeight,
                () => OpenSocietyApprovals());
            xPos += buttonWidth + 20;

            CreateQuickButton("Monitoring", System.Drawing.Color.Red, xPos, yPos, buttonWidth, buttonHeight,
                () => OpenActivityLog());
            xPos += buttonWidth + 20;

            CreateQuickButton("Reports", System.Drawing.Color.Green, xPos, yPos, buttonWidth, buttonHeight,
                () => OpenUniversityReport());

            // Status Bar
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel
            {
                Text = $"User: {_adminName} | Role: Administrator"
            };
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CreateQuickButton(string text, System.Drawing.Color color, int x, int y, int width, int height, Action action)
        {
            Button button = new Button
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(width, height),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                BackColor = color,
                ForeColor = System.Drawing.Color.White,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            button.Click += (s, e) => action?.Invoke();
            this.Controls.Add(button);
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
            UIHelpers.ShowInfo("Activity Log - View recent system activities and transactions");
        }

        private void OpenSystemStatus()
        {
            UIHelpers.ShowInfo("System Status - Check database connectivity and system health");
        }

        private void OpenUniversityReport()
        {
            UIHelpers.ShowInfo("University Report - View institution-wide statistics");
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
