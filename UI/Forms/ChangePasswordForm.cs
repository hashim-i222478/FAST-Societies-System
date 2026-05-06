using System;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for changing user password
    /// </summary>
    public partial class ChangePasswordForm : Form
    {
        private int _userId;
        private AuthenticationService _authService;
        private TextBox _currentPasswordTextBox;
        private TextBox _newPasswordTextBox;
        private TextBox _confirmPasswordTextBox;

        public ChangePasswordForm(int userId = -1)
        {
            _userId = userId > 0 ? userId : AuthenticationManager.Instance.CurrentUser.UserId;
            _authService = new AuthenticationService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Change Password";
            this.Size = new System.Drawing.Size(400, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Change Your Password",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(350, 30)
            };
            this.Controls.Add(titleLabel);

            int yPos = 70;

            // Current Password Label
            Label currentLabel = new Label
            {
                Text = "Current Password:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(350, 20)
            };
            this.Controls.Add(currentLabel);
            yPos += 30;

            // Current Password TextBox
            _currentPasswordTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(350, 30),
                UseSystemPasswordChar = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            this.Controls.Add(_currentPasswordTextBox);
            yPos += 40;

            // New Password Label
            Label newLabel = new Label
            {
                Text = "New Password:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(350, 20)
            };
            this.Controls.Add(newLabel);
            yPos += 30;

            // New Password TextBox
            _newPasswordTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(350, 30),
                UseSystemPasswordChar = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            this.Controls.Add(_newPasswordTextBox);
            yPos += 40;

            // Confirm Password Label
            Label confirmLabel = new Label
            {
                Text = "Confirm Password:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(350, 20)
            };
            this.Controls.Add(confirmLabel);
            yPos += 30;

            // Confirm Password TextBox
            _confirmPasswordTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(350, 30),
                UseSystemPasswordChar = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            this.Controls.Add(_confirmPasswordTextBox);
            yPos += 50;

            // Change Button
            Button changeButton = new Button
            {
                Text = "Change Password",
                Location = new System.Drawing.Point(100, yPos),
                Size = new System.Drawing.Size(120, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            changeButton.Click += ChangeButton_Click;
            this.Controls.Add(changeButton);

            // Cancel Button
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(230, yPos),
                Size = new System.Drawing.Size(120, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            cancelButton.Click += (s, e) => this.Close();
            this.Controls.Add(cancelButton);

            this.ResumeLayout(false);
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            try
            {
                string currentPassword = _currentPasswordTextBox.Text.Trim();
                string newPassword = _newPasswordTextBox.Text.Trim();
                string confirmPassword = _confirmPasswordTextBox.Text.Trim();

                // Validation
                if (string.IsNullOrEmpty(currentPassword))
                {
                    UIHelpers.ShowError("Current password is required");
                    return;
                }

                if (string.IsNullOrEmpty(newPassword))
                {
                    UIHelpers.ShowError("New password is required");
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    UIHelpers.ShowError("New password and confirm password do not match");
                    return;
                }

                if (currentPassword == newPassword)
                {
                    UIHelpers.ShowError("New password must be different from current password");
                    return;
                }

                // Call service
                _authService.ChangePassword(_userId, currentPassword, newPassword);
                UIHelpers.ShowInfo("Password changed successfully");
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                UIHelpers.ShowError($"Password change failed: {ex.Message}");
            }
            catch (ValidationException ex)
            {
                UIHelpers.ShowError($"Invalid password: {ex.Message}");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Error: {ex.Message}");
            }
        }
    }
}
