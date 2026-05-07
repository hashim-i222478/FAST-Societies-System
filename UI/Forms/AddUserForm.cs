using System;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class AddUserForm : Form
    {
        private AuthenticationService _authService;
        private UserRepository _userRepository;

        public AddUserForm()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            _userRepository = new UserRepository();
            CenterToScreen();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Add New User - FAST Societies";
            this.Size = new System.Drawing.Size(550, 750);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeManager.Background;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(50, 40, 50, 40)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Subtitle
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Fields
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Buttons
            this.Controls.Add(mainLayout);

            // Window Controls
            FlowLayoutPanel windowControls = new FlowLayoutPanel
            {
                Size = new Size(100, 40),
                Location = new Point(450, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 0, 0, 0)
            };
            this.Controls.Add(windowControls);
            windowControls.BringToFront();

            Button closeBtn = new Button { Text = "×", Size = new Size(40, 40), FlatStyle = FlatStyle.Flat, ForeColor = ThemeManager.TextSecondary, Font = new Font("Arial", 18, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0) };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            closeBtn.MouseEnter += (s, e) => closeBtn.ForeColor = Color.FromArgb(233, 69, 96);
            closeBtn.MouseLeave += (s, e) => closeBtn.ForeColor = ThemeManager.TextSecondary;
            windowControls.Controls.Add(closeBtn);

            Label titleLabel = new Label { Text = "Create Account", Font = ThemeManager.HeaderFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            mainLayout.Controls.Add(titleLabel, 0, 0);

            Label subTitle = new Label { Text = "Administrator controlled user creation", Font = ThemeManager.BodyFont, ForeColor = ThemeManager.TextSecondary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft };
            mainLayout.Controls.Add(subTitle, 0, 1);

            // Container for form fields
            FlowLayoutPanel fieldsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 20, 10, 0)
            };
            mainLayout.Controls.Add(fieldsPanel, 0, 2);

            // Fields
            AddInputField(fieldsPanel, "FIRST NAME", "firstNameTextBox");
            AddInputField(fieldsPanel, "LAST NAME", "lastNameTextBox");
            AddInputField(fieldsPanel, "EMAIL ADDRESS", "emailTextBox");
            AddInputField(fieldsPanel, "PHONE NUMBER", "phoneTextBox");
            AddInputField(fieldsPanel, "PASSWORD", "passwordTextBox", true);

            // Role ComboBox
            Panel roleGroup = new Panel { Size = new Size(400, 75), Margin = new Padding(0, 0, 0, 15) };
            Label roleLbl = new Label { Text = "ASSIGN ROLE", Font = ThemeManager.SmallFont, ForeColor = ThemeManager.Accent, Location = new Point(0, 0), AutoSize = true };
            ComboBox roleCombo = new ComboBox { Name = "roleComboBox", Location = new Point(0, 25), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
            roleCombo.Items.AddRange(new string[] { "Student", "SocietyHead", "Admin" });
            roleCombo.SelectedIndex = 0;
            roleCombo.BackColor = ThemeManager.Surface;
            roleCombo.ForeColor = ThemeManager.TextPrimary;
            roleCombo.FlatStyle = FlatStyle.Flat;
            roleCombo.Font = ThemeManager.BodyFont;
            
            roleGroup.Controls.Add(roleLbl);
            roleGroup.Controls.Add(roleCombo);
            fieldsPanel.Controls.Add(roleGroup);

            // Buttons
            Panel btnPanel = new Panel { Dock = DockStyle.Fill };
            Button saveButton = new Button { Text = "CREATE USER", Width = 180, Dock = DockStyle.Left };
            ThemeManager.StyleButton(saveButton);
            saveButton.Click += SaveButton_Click;
            
            Button cancelButton = new Button { Text = "CANCEL", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(cancelButton, false);
            cancelButton.Click += (s, e) => this.Close();

            btnPanel.Controls.Add(saveButton);
            btnPanel.Controls.Add(cancelButton);
            mainLayout.Controls.Add(btnPanel, 0, 3);

            this.ResumeLayout(false);
        }

        private void AddInputField(FlowLayoutPanel panel, string label, string name, bool isPassword = false)
        {
            Panel group = new Panel { Size = new Size(400, 75), Margin = new Padding(0, 0, 0, 15) };
            Label lbl = new Label { Text = label, Font = ThemeManager.SmallFont, ForeColor = ThemeManager.Accent, Location = new Point(0, 0), AutoSize = true };
            TextBox tb = new TextBox { Name = name, Location = new Point(0, 25), Width = 400, UseSystemPasswordChar = isPassword };
            ThemeManager.StyleTextBox(tb);
            
            group.Controls.Add(lbl);
            group.Controls.Add(tb);
            panel.Controls.Add(group);
        }

        private Control FindControl(string name)
        {
            Control[] controls = this.Controls.Find(name, true);
            return controls.Length > 0 ? controls[0] : null;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string firstName = ((TextBox)FindControl("firstNameTextBox")).Text.Trim();
                string lastName = ((TextBox)FindControl("lastNameTextBox")).Text.Trim();
                string email = ((TextBox)FindControl("emailTextBox")).Text.Trim();
                string phone = ((TextBox)FindControl("phoneTextBox")).Text.Trim();
                string password = ((TextBox)FindControl("passwordTextBox")).Text;
                string role = ((ComboBox)FindControl("roleComboBox")).SelectedItem.ToString();

                // Validation
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    UIHelpers.ShowError("Required fields missing: First Name, Last Name, Email, and Password.");
                    return;
                }

                if (!UIHelpers.IsValidEmail(email))
                {
                    UIHelpers.ShowError("Please enter a valid university email address.");
                    return;
                }

                // Check if exists
                if (_userRepository.EmailExists(email))
                {
                    UIHelpers.ShowError("This email is already registered in the system.");
                    return;
                }

                // Create User
                string passwordHash = PasswordHasher.HashPassword(password);
                User newUser = new User
                {
                    Email = email,
                    PasswordHash = passwordHash,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phone,
                    Role = role,
                    Status = "Active"
                };

                _userRepository.CreateUser(newUser);
                UIHelpers.ShowInfo($"User '{firstName} {lastName}' created successfully as {role}.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to create user: {ex.Message}");
            }
        }
    }
}
