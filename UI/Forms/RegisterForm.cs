using System;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Registration form for new student users
    /// </summary>
    public partial class RegisterForm : Form
    {
        private AuthenticationService _authService;

        public RegisterForm()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            CenterToScreen();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Register - FAST Societies Management System";
            this.Size = new System.Drawing.Size(700, 900);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            int yPos = 30;
            
            // Title
            Label titleLabel = new Label
            {
                Text = "Student Registration",
                Font = new System.Drawing.Font("Segoe UI", 22, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, yPos),
                Size = new System.Drawing.Size(640, 50),
                ForeColor = System.Drawing.Color.DarkGreen,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(titleLabel);
            yPos += 70;

            // First Name
            Label firstNameLabel = new Label { Text = "First Name:", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(firstNameLabel);
            yPos += 30;
            TextBox firstNameTextBox = new TextBox { Name = "firstNameTextBox", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(640, 35), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(firstNameTextBox);
            yPos += 50;

            // Last Name
            Label lastNameLabel = new Label { Text = "Last Name:", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(lastNameLabel);
            yPos += 30;
            TextBox lastNameTextBox = new TextBox { Name = "lastNameTextBox", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(640, 35), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(lastNameTextBox);
            yPos += 50;

            // Email
            Label emailLabel = new Label { Text = "Email:", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(emailLabel);
            yPos += 30;
            TextBox emailTextBox = new TextBox { Name = "emailTextBox", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(640, 35), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(emailTextBox);
            yPos += 50;

            // Phone Number
            Label phoneLabel = new Label { Text = "Phone (Optional):", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(phoneLabel);
            yPos += 30;
            TextBox phoneTextBox = new TextBox { Name = "phoneTextBox", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(640, 35), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(phoneTextBox);
            yPos += 50;

            // Password
            Label passwordLabel = new Label { Text = "Password:", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(passwordLabel);
            yPos += 30;
            TextBox passwordTextBox = new TextBox { Name = "passwordTextBox", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(640, 35), UseSystemPasswordChar = true, Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(passwordTextBox);
            yPos += 50;

            // Confirm Password
            Label confirmPasswordLabel = new Label { Text = "Confirm Password:", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(confirmPasswordLabel);
            yPos += 30;
            TextBox confirmPasswordTextBox = new TextBox { Name = "confirmPasswordTextBox", Location = new System.Drawing.Point(30, yPos), Size = new System.Drawing.Size(640, 35), UseSystemPasswordChar = true, Font = new System.Drawing.Font("Segoe UI", 11) };
            this.Controls.Add(confirmPasswordTextBox);
            yPos += 60;

            // Register Button
            Button registerButton = new Button
            {
                Text = "Register",
                Location = new System.Drawing.Point(130, yPos),
                Size = new System.Drawing.Size(200, 45),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            };
            registerButton.Click += RegisterButton_Click;
            this.Controls.Add(registerButton);

            // Cancel Button
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(370, yPos),
                Size = new System.Drawing.Size(200, 45),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            };
            cancelButton.Click += (s, e) => this.Close();
            this.Controls.Add(cancelButton);

            this.ResumeLayout(false);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                string firstName = ((TextBox)this.Controls["firstNameTextBox"]).Text.Trim();
                string lastName = ((TextBox)this.Controls["lastNameTextBox"]).Text.Trim();
                string email = ((TextBox)this.Controls["emailTextBox"]).Text.Trim();
                string phone = ((TextBox)this.Controls["phoneTextBox"]).Text.Trim();
                string password = ((TextBox)this.Controls["passwordTextBox"]).Text;
                string confirmPassword = ((TextBox)this.Controls["confirmPasswordTextBox"]).Text;

                // Validation
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    UIHelpers.ShowError("First name, last name, email, and password are required");
                    return;
                }

                if (!UIHelpers.IsValidEmail(email))
                {
                    UIHelpers.ShowError("Invalid email format");
                    return;
                }

                if (password != confirmPassword)
                {
                    UIHelpers.ShowError("Passwords do not match");
                    return;
                }

                // Register
                var student = _authService.RegisterStudent(email, firstName, lastName, password, phone);
                UIHelpers.ShowInfo($"Registration successful! Welcome, {student.FullName}");
                this.Close();
            }
            catch (DuplicateResourceException ex)
            {
                UIHelpers.ShowError(ex.Message);
            }
            catch (ValidationException ex)
            {
                UIHelpers.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Registration failed: {ex.Message}");
            }
        }
    }
}
