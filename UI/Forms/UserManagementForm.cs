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

            this.Text = "User Management";
            this.Size = new System.Drawing.Size(900, 550);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Manage Users",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _usersGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _usersGrid.Columns.Add("UserId", "ID");
            _usersGrid.Columns.Add("FullName", "Full Name");
            _usersGrid.Columns.Add("Email", "Email");
            _usersGrid.Columns.Add("Role", "Role");
            _usersGrid.Columns.Add("Status", "Status");
            _usersGrid.Columns.Add("CreatedDate", "Created");

            this.Controls.Add(_usersGrid);

            // Create User Button
            Button createButton = new Button
            {
                Text = "Create User",
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            createButton.Click += CreateButton_Click;
            this.Controls.Add(createButton);

            // Suspend User Button
            Button suspendButton = new Button
            {
                Text = "Suspend User",
                Location = new System.Drawing.Point(180, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White
            };
            suspendButton.Click += SuspendButton_Click;
            this.Controls.Add(suspendButton);

            // View Details Button
            Button viewButton = new Button
            {
                Text = "View Details",
                Location = new System.Drawing.Point(340, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            viewButton.Click += ViewButton_Click;
            this.Controls.Add(viewButton);

            // Refresh Button
            Button refreshButton = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(500, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.CornflowerBlue,
                ForeColor = System.Drawing.Color.White
            };
            refreshButton.Click += (s, e) => LoadUsers();
            this.Controls.Add(refreshButton);

            // Close Button
            Button closeButton = new Button
            {
                Text = "Close",
                Location = new System.Drawing.Point(690, 420),
                Size = new System.Drawing.Size(180, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);

            // Status Label
            Label statusLabel = new Label
            {
                Text = "Total Users: 0",
                Location = new System.Drawing.Point(20, 470),
                Size = new System.Drawing.Size(850, 25),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            this.Controls.Add(statusLabel);

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
            string email = Microsoft.VisualBasic.Interaction.InputBox("Enter email:", "Create User");
            if (string.IsNullOrEmpty(email)) return;

            try
            {
                UIHelpers.ShowInfo("User creation form - would open separate form");
                LoadUsers();
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to create user: {ex.Message}");
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
