using System;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Main dashboard for Society Head users
    /// </summary>
    public partial class SocietyHeadMainForm : Form
    {
        private int _headId;
        private string _headName;
        private SocietyService _societyService;

        public SocietyHeadMainForm()
        {
            _headId = AuthenticationManager.Instance.CurrentUser.UserId;
            _headName = AuthenticationManager.Instance.CurrentUser.FullName;
            _societyService = new SocietyService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "FAST Societies - Society Head";
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

            // Societies Menu
            ToolStripMenuItem societiesMenu = new ToolStripMenuItem("&Societies");
            societiesMenu.DropDownItems.Add("&Manage Societies", null, (s, e) => OpenSocietyManagement());
            societiesMenu.DropDownItems.Add("&View Members", null, (s, e) => OpenMemberManagement());
            societiesMenu.DropDownItems.Add("&Membership Requests", null, (s, e) => OpenMembershipRequests());
            menuStrip.Items.Add(societiesMenu);

            // Events Menu
            ToolStripMenuItem eventsMenu = new ToolStripMenuItem("&Events");
            eventsMenu.DropDownItems.Add("&Create Event", null, (s, e) => OpenCreateEvent());
            eventsMenu.DropDownItems.Add("&Manage Events", null, (s, e) => OpenManageEvents());
            menuStrip.Items.Add(eventsMenu);

            // Tasks Menu
            ToolStripMenuItem tasksMenu = new ToolStripMenuItem("&Tasks");
            tasksMenu.DropDownItems.Add("&Create Task", null, (s, e) => OpenCreateTask());
            tasksMenu.DropDownItems.Add("&View Tasks", null, (s, e) => OpenViewTasks());
            menuStrip.Items.Add(tasksMenu);

            // Reports Menu
            ToolStripMenuItem reportsMenu = new ToolStripMenuItem("&Reports");
            reportsMenu.DropDownItems.Add("&Membership Report", null, (s, e) => OpenMembershipReport());
            reportsMenu.DropDownItems.Add("&Event Report", null, (s, e) => OpenEventReport());
            reportsMenu.DropDownItems.Add("&Task Report", null, (s, e) => OpenTaskReport());
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
                BackColor = System.Drawing.Color.LightBlue,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label welcomeLabel = new Label
            {
                Text = $"Welcome, {_headName}!",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(500, 35)
            };
            welcomePanel.Controls.Add(welcomeLabel);

            Label descLabel = new Label
            {
                Text = "Manage your society, members, events, and tasks from here.",
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

            CreateQuickButton("Manage Societies", System.Drawing.Color.SteelBlue, xPos, yPos, buttonWidth, buttonHeight, 
                () => OpenSocietyManagement());
            xPos += buttonWidth + 20;

            CreateQuickButton("Membership Requests", System.Drawing.Color.Green, xPos, yPos, buttonWidth, buttonHeight, 
                () => OpenMembershipRequests());
            xPos += buttonWidth + 20;

            CreateQuickButton("Create Event", System.Drawing.Color.Orange, xPos, yPos, buttonWidth, buttonHeight, 
                () => OpenCreateEvent());
            xPos += buttonWidth + 20;

            CreateQuickButton("Create Task", System.Drawing.Color.Purple, xPos, yPos, buttonWidth, buttonHeight, 
                () => OpenCreateTask());

            // Status Bar
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel
            {
                Text = $"User: {_headName} | Role: Society Head"
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

        private void OpenSocietyManagement()
        {
            SocietyManagementForm form = new SocietyManagementForm(_headId);
            form.ShowDialog();
        }

        private void OpenMemberManagement()
        {
            UIHelpers.ShowInfo("Member management functionality - View all members of your societies");
        }

        private void OpenMembershipRequests()
        {
            MembershipRequestsForm form = new MembershipRequestsForm(_headId);
            form.ShowDialog();
        }

        private void OpenCreateEvent()
        {
            CreateEventForm form = new CreateEventForm(_headId);
            form.ShowDialog();
        }

        private void OpenManageEvents()
        {
            UIHelpers.ShowInfo("Event management functionality - Edit or cancel your events");
        }

        private void OpenCreateTask()
        {
            CreateTaskForm form = new CreateTaskForm(_headId);
            form.ShowDialog();
        }

        private void OpenViewTasks()
        {
            ViewTasksForm form = new ViewTasksForm(_headId);
            form.ShowDialog();
        }

        private void OpenMembershipReport()
        {
            UIHelpers.ShowInfo("Membership Report - View detailed membership statistics");
        }

        private void OpenEventReport()
        {
            UIHelpers.ShowInfo("Event Report - View event attendance and registration data");
        }

        private void OpenTaskReport()
        {
            UIHelpers.ShowInfo("Task Report - View task completion statistics");
        }

        private void OpenChangePassword()
        {
            ChangePasswordForm form = new ChangePasswordForm(_headId);
            form.ShowDialog();
        }

        private void OpenProfile()
        {
            User user = AuthenticationManager.Instance.CurrentUser;
            string profileInfo = $"User Profile\n\n" +
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
