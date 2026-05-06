using System;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Student Dashboard - FAST Societies Management System";
            this.Size = new System.Drawing.Size(900, 600);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Menu Strip
            MenuStrip menuStrip = new MenuStrip();
            
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("&File");
            fileMenu.DropDownItems.Add("&Logout", null, (s, e) => Logout());
            fileMenu.DropDownItems.Add("E&xit", null, (s, e) => this.Close());
            
            ToolStripMenuItem societyMenu = new ToolStripMenuItem("&Societies");
            societyMenu.DropDownItems.Add("&Browse Societies", null, (s, e) => OpenBrowseSocieties());
            societyMenu.DropDownItems.Add("&My Memberships", null, (s, e) => OpenMyMemberships());
            
            ToolStripMenuItem eventMenu = new ToolStripMenuItem("&Events");
            eventMenu.DropDownItems.Add("&Browse Events", null, (s, e) => OpenBrowseEvents());
            eventMenu.DropDownItems.Add("&My Tickets", null, (s, e) => OpenMyTickets());
            
            ToolStripMenuItem accountMenu = new ToolStripMenuItem("&Account");
            accountMenu.DropDownItems.Add("&Change Password", null, (s, e) => OpenChangePassword());
            accountMenu.DropDownItems.Add("&Profile", null, (s, e) => OpenProfile());

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(societyMenu);
            menuStrip.Items.Add(eventMenu);
            menuStrip.Items.Add(accountMenu);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;

            // Status Bar
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel userLabel = new ToolStripStatusLabel($"User: {_studentName}");
            statusStrip.Items.Add(userLabel);
            this.Controls.Add(statusStrip);

            // Welcome Panel
            Panel welcomePanel = new Panel
            {
                Location = new System.Drawing.Point(20, 40),
                Size = new System.Drawing.Size(850, 100),
                BackColor = System.Drawing.Color.LightBlue,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label welcomeLabel = new Label
            {
                Text = $"Welcome, {_studentName}!",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(400, 25),
                AutoSize = false
            };
            welcomePanel.Controls.Add(welcomeLabel);

            Label descriptionLabel = new Label
            {
                Text = "Browse societies, join memberships, register for events, and manage your student activities.",
                Font = new System.Drawing.Font("Segoe UI", 10),
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(600, 50),
                AutoSize = false
            };
            welcomePanel.Controls.Add(descriptionLabel);

            this.Controls.Add(welcomePanel);

            // Quick Action Buttons
            int btnX = 20, btnY = 160;
            
            Button browseSocietiesBtn = new Button
            {
                Text = "Browse\nSocieties",
                Location = new System.Drawing.Point(btnX, btnY),
                Size = new System.Drawing.Size(120, 60),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                Cursor = Cursors.Hand
            };
            browseSocietiesBtn.Click += (s, e) => OpenBrowseSocieties();
            this.Controls.Add(browseSocietiesBtn);

            Button myMembershipsBtn = new Button
            {
                Text = "My\nMemberships",
                Location = new System.Drawing.Point(btnX + 140, btnY),
                Size = new System.Drawing.Size(120, 60),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Cursor = Cursors.Hand
            };
            myMembershipsBtn.Click += (s, e) => OpenMyMemberships();
            this.Controls.Add(myMembershipsBtn);

            Button browseEventsBtn = new Button
            {
                Text = "Browse\nEvents",
                Location = new System.Drawing.Point(btnX + 280, btnY),
                Size = new System.Drawing.Size(120, 60),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White,
                Cursor = Cursors.Hand
            };
            browseEventsBtn.Click += (s, e) => OpenBrowseEvents();
            this.Controls.Add(browseEventsBtn);

            Button myTicketsBtn = new Button
            {
                Text = "My\nTickets",
                Location = new System.Drawing.Point(btnX + 420, btnY),
                Size = new System.Drawing.Size(120, 60),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Purple,
                ForeColor = System.Drawing.Color.White,
                Cursor = Cursors.Hand
            };
            myTicketsBtn.Click += (s, e) => OpenMyTickets();
            this.Controls.Add(myTicketsBtn);

            // Info Panel
            Panel infoPanel = new Panel
            {
                Location = new System.Drawing.Point(20, 250),
                Size = new System.Drawing.Size(850, 290),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };

            Label infoTitleLabel = new Label
            {
                Text = "Quick Info",
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(200, 25)
            };
            infoPanel.Controls.Add(infoTitleLabel);

            Label infoTextLabel = new Label
            {
                Text = "• View all active societies and explore opportunities\n• Apply for society memberships\n• Register for upcoming events\n• View your event tickets\n• Manage your profile and password",
                Font = new System.Drawing.Font("Segoe UI", 10),
                Location = new System.Drawing.Point(10, 45),
                Size = new System.Drawing.Size(800, 200),
                AutoSize = false
            };
            infoPanel.Controls.Add(infoTextLabel);

            this.Controls.Add(infoPanel);

            this.ResumeLayout(false);
            this.PerformLayout();
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
