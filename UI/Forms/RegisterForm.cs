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

            this.Text = "Register - FAST Societies";
            this.Size = new System.Drawing.Size(600, 800);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeManager.Background;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(60, 40, 60, 40)
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
                Location = new Point(500, 0),
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

            Button minBtn = new Button { Text = "—", Size = new Size(40, 40), FlatStyle = FlatStyle.Flat, ForeColor = ThemeManager.TextSecondary, Font = new Font("Arial", 12, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0) };
            minBtn.FlatAppearance.BorderSize = 0;
            minBtn.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            windowControls.Controls.Add(minBtn);

            Label titleLabel = new Label { Text = "Join the Community", Font = ThemeManager.HeaderFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            mainLayout.Controls.Add(titleLabel, 0, 0);

            Label subTitle = new Label { Text = "Create your student account", Font = ThemeManager.BodyFont, ForeColor = ThemeManager.TextSecondary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft };
            mainLayout.Controls.Add(subTitle, 0, 1);

            // Container for form fields
            FlowLayoutPanel fieldsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 20, 20, 0)
            };
            mainLayout.Controls.Add(fieldsPanel, 0, 2);

            // Fields in desired order
            string[] labels = { "FIRST NAME", "LAST NAME", "EMAIL ADDRESS", "PHONE NUMBER", "PASSWORD", "CONFIRM PASSWORD" };
            string[] names = { "firstNameTextBox", "lastNameTextBox", "emailTextBox", "phoneTextBox", "passwordTextBox", "confirmPasswordTextBox" };
            bool[] isPassword = { false, false, false, false, true, true };

            for (int i = 0; i < labels.Length; i++)
            {
                Panel group = new Panel { Size = new Size(450, 80), Margin = new Padding(0, 0, 0, 10) };
                Label lbl = new Label { Text = labels[i], Font = ThemeManager.SmallFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Top, Height = 25 };
                TextBox tb = new TextBox { Name = names[i], Dock = DockStyle.Bottom, Height = 40, UseSystemPasswordChar = isPassword[i] };
                ThemeManager.StyleTextBox(tb);
                
                group.Controls.Add(lbl);
                group.Controls.Add(tb);
                fieldsPanel.Controls.Add(group);
            }

            // Buttons
            Panel btnPanel = new Panel { Dock = DockStyle.Fill };
            Button registerButton = new Button { Text = "REGISTER", Width = 200, Dock = DockStyle.Left };
            ThemeManager.StyleButton(registerButton);
            registerButton.Click += RegisterButton_Click;
            
            Button cancelButton = new Button { Text = "CANCEL", Width = 150, Dock = DockStyle.Right };
            ThemeManager.StyleButton(cancelButton, false);
            cancelButton.Click += (s, e) => this.Close();

            btnPanel.Controls.Add(registerButton);
            btnPanel.Controls.Add(cancelButton);
            mainLayout.Controls.Add(btnPanel, 0, 3);

            this.ResumeLayout(false);
        }

        private Control FindControl(string name)
        {
            Control[] controls = this.Controls.Find(name, true);
            return controls.Length > 0 ? controls[0] : null;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                string firstName = ((TextBox)FindControl("firstNameTextBox")).Text.Trim();
                string lastName = ((TextBox)FindControl("lastNameTextBox")).Text.Trim();
                string email = ((TextBox)FindControl("emailTextBox")).Text.Trim();
                string phone = ((TextBox)FindControl("phoneTextBox")).Text.Trim();
                string password = ((TextBox)FindControl("passwordTextBox")).Text;
                string confirmPassword = ((TextBox)FindControl("confirmPasswordTextBox")).Text;

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
