using System;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Login form for user authentication
    /// Entry point of the application
    /// </summary>
    public partial class LoginForm : Form
    {
        private AuthenticationService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            CenterToScreen();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "FAST Societies Management System - Login";
            this.Size = new System.Drawing.Size(600, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            // Title label
            Label titleLabel = new Label
            {
                Text = "Login",
                Font = new System.Drawing.Font("Segoe UI", 28, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(50, 40),
                Size = new System.Drawing.Size(500, 60),
                ForeColor = System.Drawing.Color.DarkBlue,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(titleLabel);

            // Email Label
            Label emailLabel = new Label
            {
                Text = "Email:",
                Location = new System.Drawing.Point(80, 130),
                Size = new System.Drawing.Size(150, 30),
                Font = new System.Drawing.Font("Segoe UI", 12)
            };
            this.Controls.Add(emailLabel);

            // Email TextBox
            TextBox emailTextBox = new TextBox
            {
                Name = "emailTextBox",
                Location = new System.Drawing.Point(80, 165),
                Size = new System.Drawing.Size(440, 40),
                Font = new System.Drawing.Font("Segoe UI", 12),
                Padding = new Padding(5)
            };
            this.Controls.Add(emailTextBox);

            // Password Label
            Label passwordLabel = new Label
            {
                Text = "Password:",
                Location = new System.Drawing.Point(80, 220),
                Size = new System.Drawing.Size(150, 30),
                Font = new System.Drawing.Font("Segoe UI", 12)
            };
            this.Controls.Add(passwordLabel);

            // Password TextBox
            TextBox passwordTextBox = new TextBox
            {
                Name = "passwordTextBox",
                Location = new System.Drawing.Point(80, 255),
                Size = new System.Drawing.Size(440, 40),
                Font = new System.Drawing.Font("Segoe UI", 12),
                UseSystemPasswordChar = true,
                Padding = new Padding(5)
            };
            this.Controls.Add(passwordTextBox);

            // Login Button
            Button loginButton = new Button
            {
                Text = "Login",
                Location = new System.Drawing.Point(80, 330),
                Size = new System.Drawing.Size(200, 50),
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.DodgerBlue,
                ForeColor = System.Drawing.Color.White,
                Cursor = Cursors.Hand
            };
            loginButton.Click += LoginButton_Click;
            this.Controls.Add(loginButton);

            // Register Button
            Button registerButton = new Button
            {
                Text = "Register",
                Location = new System.Drawing.Point(320, 330),
                Size = new System.Drawing.Size(200, 50),
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Cursor = Cursors.Hand
            };
            registerButton.Click += RegisterButton_Click;
            this.Controls.Add(registerButton);

            this.ResumeLayout(false);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            TextBox emailTextBox = (TextBox)this.Controls["emailTextBox"];
            TextBox passwordTextBox = (TextBox)this.Controls["passwordTextBox"];

            string email = emailTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                UIHelpers.ShowError("Email and password are required");
                return;
            }

            try
            {
                User user = _authService.Login(email, password);
                
                // Set current user in authentication manager
                AuthenticationManager.Instance.Login(user);

                UIHelpers.ShowInfo($"Welcome, {user.FullName}!");

                // Navigate based on role
                NavigateToDashboard(user.Role);

                // Close login form
                this.Hide();
            }
            catch (InvalidCredentialsException ex)
            {
                UIHelpers.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Login failed: {ex.Message}");
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // Show registration form
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void NavigateToDashboard(string role)
        {
            switch (role)
            {
                case "Student":
                    StudentMainForm studentForm = new StudentMainForm();
                    studentForm.Show();
                    break;

                case "SocietyHead":
                    SocietyHeadMainForm headForm = new SocietyHeadMainForm();
                    headForm.Show();
                    break;

                case "Admin":
                    AdminMainForm adminForm = new AdminMainForm();
                    adminForm.Show();
                    break;

                default:
                    UIHelpers.ShowError("Unknown user role");
                    break;
            }
        }
    }
}
