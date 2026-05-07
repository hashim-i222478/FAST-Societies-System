using System;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Main dashboard form for Student users
    /// </summary>
    public partial class StudentMainForm : Form
    {
        private int _studentId;
        private string _studentName;

        public StudentMainForm()
        {
            InitializeComponent();
            _studentId = (int)AuthenticationManager.Instance.CurrentUserId;
            _studentName = AuthenticationManager.Instance.GetCurrentUserName();
            CenterToScreen();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CheckForCancelledEvents();
        }

        private void CheckForCancelledEvents()
        {
            try
            {
                StudentService studentService = new StudentService();
                var registrations = studentService.GetMyEventRegistrations(_studentId);
                if (registrations != null)
                {
                    bool hasCancelled = false;
                    foreach (var reg in registrations)
                    {
                        if (reg.AttendanceStatus == "Cancelled") continue; // Skip if student cancelled it
                        
                        Event evt = studentService.GetEventDetails(reg.EventId);
                        if (evt != null && evt.Status == "Cancelled")
                        {
                            hasCancelled = true;
                            break;
                        }
                    }

                    if (hasCancelled)
                    {
                        UIHelpers.ShowInfo("One or more events you registered for have been cancelled by the organizers. Please check 'My Tickets' for details.", "Important Update");
                    }
                }
            }
            catch (Exception)
            {
                // Suppress background errors on load
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Student Dashboard - FAST Societies";
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
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260)); // Slightly wider
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
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Dashboard
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Societies
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Memberships
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Events
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Tickets
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Spacer
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Logout
            sidebar.Controls.Add(sidebarLayout);

            Label sidebarTitle = new Label { Text = "FAST\nSOCIETIES", Font = new Font("Trebuchet MS", 18, FontStyle.Bold), ForeColor = ThemeManager.Accent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            sidebarLayout.Controls.Add(sidebarTitle, 0, 0);

            AddSidebarButton(sidebarLayout, "Dashboard", (s, e) => { }, 1);
            AddSidebarButton(sidebarLayout, "Browse Societies", (s, e) => OpenBrowseSocieties(), 2);
            AddSidebarButton(sidebarLayout, "My Memberships", (s, e) => OpenMyMemberships(), 3);
            AddSidebarButton(sidebarLayout, "Browse Events", (s, e) => OpenBrowseEvents(), 4);
            AddSidebarButton(sidebarLayout, "My Tickets", (s, e) => OpenMyTickets(), 5);
            AddSidebarButton(sidebarLayout, "My Tasks", (s, e) => OpenMyTasks(), 6);

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
                Padding = new Padding(50, 60, 50, 40) // Increased top padding
            };
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); // Increased Header height
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));  // Smaller spacer
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Grid
            mainGrid.Controls.Add(contentLayout, 1, 0);

            Panel header = new Panel { Dock = DockStyle.Fill };
            contentLayout.Controls.Add(header, 0, 0);

            Label welcome = new Label { Text = $"Welcome back, {_studentName}", Font = ThemeManager.TitleFont, ForeColor = ThemeManager.TextPrimary, AutoSize = true, Location = new Point(0, 0) };
            header.Controls.Add(welcome);

            Label dateLabel = new Label { Text = DateTime.Now.ToString("dddd, MMMM dd"), Font = ThemeManager.BodyFont, ForeColor = ThemeManager.TextSecondary, AutoSize = true, Location = new Point(5, 55) }; // Pushed down
            header.Controls.Add(dateLabel);

            FlowLayoutPanel grid = new FlowLayoutPanel { Dock = DockStyle.Fill, Margin = new Padding(0), AutoScroll = true };
            contentLayout.Controls.Add(grid, 0, 2);

            AddDashboardCard(grid, "Societies", "Explore and join university societies.", Color.FromArgb(0, 212, 255), (s, e) => OpenBrowseSocieties());
            AddDashboardCard(grid, "Events", "Register for upcoming workshops and talks.", Color.FromArgb(233, 69, 96), (s, e) => OpenBrowseEvents());
            AddDashboardCard(grid, "Tickets", "View your active event passes.", Color.FromArgb(106, 76, 239), (s, e) => OpenMyTickets());
            AddDashboardCard(grid, "Tasks", "View and complete your assigned society tasks.", Color.FromArgb(76, 175, 80), (s, e) => OpenMyTasks());
            AddDashboardCard(grid, "Profile", "Manage your account and password.", Color.FromArgb(255, 171, 64), (s, e) => OpenProfile());

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
            btn.Font = new Font("Trebuchet MS", 11, FontStyle.Bold); // Slightly smaller to fit
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
            titleLbl.Click += onClick; // Propagate click
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

        private void OpenBrowseSocieties()
        {
            BrowseSocietiesForm form = new BrowseSocietiesForm(_studentId);
            form.ShowDialog();
        }

        private void OpenMyMemberships()
        {
            MyMembershipsForm form = new MyMembershipsForm(_studentId);
            form.ShowDialog();
        }

        private void OpenBrowseEvents()
        {
            BrowseEventsForm form = new BrowseEventsForm(_studentId);
            form.ShowDialog();
        }

        private void OpenMyTickets()
        {
            MyTicketsForm form = new MyTicketsForm(_studentId);
            form.ShowDialog();
        }

        private void OpenMyTasks()
        {
            MyTasksForm form = new MyTasksForm(_studentId);
            form.ShowDialog();
        }

        private void OpenChangePassword()
        {
            ChangePasswordForm form = new ChangePasswordForm();
            form.ShowDialog();
        }

        private void OpenProfile()
        {
            UIHelpers.ShowInfo($"Profile Information:\n\nUser ID: {_studentId}\nName: {_studentName}\nRole: Student");
        }

        private void Logout()
        {
            if (UIHelpers.ShowConfirm("Are you sure you want to logout?", "Confirm Logout"))
            {
                AuthenticationManager.Instance.Logout();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }
    }
}
