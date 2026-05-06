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

            this.Text = "Change Password - FAST Societies";
            this.Size = new System.Drawing.Size(450, 500);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ThemeManager.Background;

            Panel mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40) };
            this.Controls.Add(mainPanel);

            Label titleLabel = new Label
            {
                Text = "Security Settings",
                Font = ThemeManager.HeaderFont,
                ForeColor = ThemeManager.Accent,
                Dock = DockStyle.Top,
                Height = 60
            };
            mainPanel.Controls.Add(titleLabel);

            // Container for fields
            Panel fieldsPanel = new Panel { Dock = DockStyle.Top, Height = 280, Padding = new Padding(0, 20, 0, 0) };
            mainPanel.Controls.Add(fieldsPanel);

            // Fields (reversed order for DockStyle.Top)
            string[] labels = { "CONFIRM PASSWORD", "NEW PASSWORD", "CURRENT PASSWORD" };
            TextBox[] tbs = new TextBox[3];

            for (int i = 0; i < labels.Length; i++)
            {
                Panel group = new Panel { Dock = DockStyle.Top, Height = 80 };
                TextBox tb = new TextBox { Dock = DockStyle.Bottom, Height = 35, UseSystemPasswordChar = true };
                ThemeManager.StyleTextBox(tb);
                Label lbl = new Label { Text = labels[i], Font = ThemeManager.SmallFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Top, Height = 25 };
                
                group.Controls.Add(tb);
                group.Controls.Add(lbl);
                fieldsPanel.Controls.Add(group);
                tbs[i] = tb;
            }

            _confirmPasswordTextBox = tbs[0];
            _newPasswordTextBox = tbs[1];
            _currentPasswordTextBox = tbs[2];

            // Buttons
            Panel btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            mainPanel.Controls.Add(btnPanel);

            Button changeButton = new Button { Text = "UPDATE", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(changeButton);
            changeButton.Click += ChangeButton_Click;
            btnPanel.Controls.Add(changeButton);

            Button cancelButton = new Button { Text = "CANCEL", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(cancelButton, false);
            cancelButton.Click += (s, e) => this.Close();
            btnPanel.Controls.Add(cancelButton);

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
