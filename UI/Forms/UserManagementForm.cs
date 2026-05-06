using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for admin to manage users
    /// </summary>
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
            this.Size = new System.Drawing.Size(1100, 700);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ThemeManager.Background;

            Panel mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };
            this.Controls.Add(mainPanel);

            Label titleLabel = new Label
            {
                Text = "User Management",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Top,
                Height = 60
            };
            mainPanel.Controls.Add(titleLabel);

            // Grid
            _usersGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_usersGrid);

            _usersGrid.Columns.Add("UserId", "ID");
            _usersGrid.Columns.Add("FullName", "Full Name");
            _usersGrid.Columns.Add("Email", "Email");
            _usersGrid.Columns.Add("Role", "Role");
            _usersGrid.Columns.Add("Status", "Status");
            _usersGrid.Columns.Add("CreatedDate", "Created");

            mainPanel.Controls.Add(_usersGrid);

            // Bottom Actions
            Panel actionPanel = new Panel { Dock = DockStyle.Bottom, Height = 80, Padding = new Padding(0, 20, 0, 0) };
            mainPanel.Controls.Add(actionPanel);

            Button createButton = new Button { Text = "CREATE", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(createButton);
            createButton.Click += CreateButton_Click;
            actionPanel.Controls.Add(createButton);

            Button suspendButton = new Button { Text = "SUSPEND", Width = 130, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(suspendButton, false);
            suspendButton.Click += SuspendButton_Click;
            actionPanel.Controls.Add(suspendButton);

            Button activateButton = new Button { Text = "ACTIVATE", Width = 130, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(activateButton, true); // Active use cyan
            activateButton.Click += ActivateButton_Click;
            actionPanel.Controls.Add(activateButton);

            Button viewButton = new Button { Text = "VIEW", Width = 100, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(viewButton, false);
            viewButton.Click += ViewButton_Click;
            actionPanel.Controls.Add(viewButton);

            Button closeButton = new Button { Text = "CLOSE", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(closeButton, false);
            closeButton.Click += (s, e) => this.Close();
            actionPanel.Controls.Add(closeButton);

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

                if (users.Count == 0)
                {
                    UIHelpers.ShowInfo("No users found");
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

                string details = $"User Details\n\n" +
                               $"Name: {name}\n" +
                               $"Email: {email}\n" +
                               $"Role: {role}\n" +
                               $"Status: {status}\n" +
                               $"Created: {created}";

                UIHelpers.ShowInfo(details);
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to view details: {ex.Message}");
            }
        }
    }
}
