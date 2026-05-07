using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class UserManagementForm : Form
    {
        private AuthenticationService _authService;
        private UserRepository _userRepository;
        private DataGridView _usersGrid;

        public UserManagementForm()
        {
            _authService = new AuthenticationService();
            _userRepository = new UserRepository();
            InitializeComponent();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "User Management - FAST Societies";
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
            Label titleLabel = new Label
            {
                Text = "Platform User Management",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Grid
            _usersGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_usersGrid);

            _usersGrid.Columns.Add("UserId", "ID");
            _usersGrid.Columns.Add("FullName", "FULL NAME");
            _usersGrid.Columns.Add("Email", "EMAIL");
            _usersGrid.Columns.Add("Role", "ROLE");
            _usersGrid.Columns.Add("Status", "STATUS");
            _usersGrid.Columns.Add("CreatedDate", "CREATED");
            _usersGrid.Columns["UserId"].Visible = false;

            mainGrid.Controls.Add(_usersGrid, 0, 1);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button createButton = new Button { Text = "CREATE NEW USER", Width = 180, Dock = DockStyle.Left };
            ThemeManager.StyleButton(createButton, false);
            createButton.ForeColor = Color.FromArgb(0, 212, 255);
            createButton.Click += CreateButton_Click;
            footer.Controls.Add(createButton);

            Button activateButton = new Button { Text = "ACTIVATE", Width = 120, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(activateButton, false);
            activateButton.ForeColor = Color.FromArgb(0, 255, 159);
            activateButton.Click += ActivateButton_Click;
            footer.Controls.Add(activateButton);

            Button suspendButton = new Button { Text = "SUSPEND", Width = 120, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(suspendButton, false);
            suspendButton.ForeColor = Color.FromArgb(233, 69, 96);
            suspendButton.Click += SuspendButton_Click;
            footer.Controls.Add(suspendButton);

            Button viewButton = new Button { Text = "DETAILS", Width = 120, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(viewButton, false);
            viewButton.Click += ViewButton_Click;
            footer.Controls.Add(viewButton);

            Button backBtn = new Button { Text = "BACK", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(backBtn, false);
            backBtn.Click += (s, e) => this.Close();
            footer.Controls.Add(backBtn);

            this.ResumeLayout(false);
        }

        private void LoadUsers()
        {
            try
            {
                _usersGrid.Rows.Clear();
                List<User> users = _userRepository.GetAllUsers();
                
                foreach (var user in users)
                {
                    _usersGrid.Rows.Add(
                        user.UserId,
                        user.FullName,
                        user.Email,
                        user.Role,
                        user.Status,
                        UIHelpers.FormatDate(user.CreatedDate)
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load users: {ex.Message}");
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            using (AddUserForm form = new AddUserForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void SuspendButton_Click(object sender, EventArgs e)
        {
            if (_usersGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a user");
                return;
            }

            try
            {
                int userId = (int)_usersGrid.SelectedRows[0].Cells[0].Value;
                string userName = (string)_usersGrid.SelectedRows[0].Cells[1].Value;
                string status = (string)_usersGrid.SelectedRows[0].Cells[4].Value;

                if (status == "Suspended")
                {
                    UIHelpers.ShowError("User is already suspended");
                    return;
                }

                if (UIHelpers.ShowConfirm($"Suspend user {userName}?"))
                {
                    _userRepository.SuspendUser(userId);
                    UIHelpers.ShowInfo("User suspended successfully");
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to suspend user: {ex.Message}");
            }
        }

        private void ActivateButton_Click(object sender, EventArgs e)
        {
            if (_usersGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a user to activate.");
                return;
            }

            try
            {
                int userId = (int)_usersGrid.SelectedRows[0].Cells[0].Value;
                string userName = (string)_usersGrid.SelectedRows[0].Cells[1].Value;
                string status = (string)_usersGrid.SelectedRows[0].Cells[4].Value;

                if (status == "Active")
                {
                    UIHelpers.ShowError("User is already active.");
                    return;
                }

                if (UIHelpers.ShowConfirm($"Reactivate account for {userName}?", "Confirm Activation"))
                {
                    _userRepository.ActivateUser(userId);
                    UIHelpers.ShowInfo($"{userName}'s account has been restored.");
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to activate user: {ex.Message}");
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (_usersGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a user");
                return;
            }

            try
            {
                string name = (string)_usersGrid.SelectedRows[0].Cells[1].Value;
                string email = (string)_usersGrid.SelectedRows[0].Cells[2].Value;
                string role = (string)_usersGrid.SelectedRows[0].Cells[3].Value;
                string status = (string)_usersGrid.SelectedRows[0].Cells[4].Value;
                string created = (string)_usersGrid.SelectedRows[0].Cells[5].Value;

                string details = $"USER DETAILS\n\n" +
                               $"NAME: {name}\n" +
                               $"EMAIL: {email}\n" +
                               $"ROLE: {role}\n" +
                               $"STATUS: {status}\n" +
                               $"CREATED: {created}";

                UIHelpers.ShowInfo(details, "User Information");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to view details: {ex.Message}");
            }
        }
    }
}
