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

        private TextBox emailTextBox;
        private TextBox passwordTextBox;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form Setup
            this.Text = "FAST Societies - Login";
            this.Size = new System.Drawing.Size(950, 600);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeManager.Background;

            // --- Main Container ---
            TableLayoutPanel mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400));
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            this.Controls.Add(mainGrid);

            // --- Left Branding (Table 1) ---
            Panel brandPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(brandPanel, 0, 0);
            ThemeManager.MakeGradientPanel(brandPanel);

            TableLayoutPanel brandLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent,
                Padding = new Padding(40)
            };
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50)); // Spacer
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180)); // Logo
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); // Title
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50)); // Tagline/Spacer
            brandPanel.Controls.Add(brandLayout);

            PictureBox logoBox = new PictureBox
            {
                Size = new Size(160, 160),
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Image.FromFile("UI\\Assets\\Logo.png")
            };
            brandLayout.Controls.Add(logoBox, 0, 1);

            Label brandTitle = new Label
            {
                Text = "FAST\nSOCIETIES",
                Font = new Font("Trebuchet MS", 32, FontStyle.Bold),
                ForeColor = ThemeManager.Accent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            brandLayout.Controls.Add(brandTitle, 0, 2);

            Label tagline = new Label
            {
                Text = "Empowering Student Communities",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = ThemeManager.TextSecondary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter
            };
            brandLayout.Controls.Add(tagline, 0, 3);

            // --- Right Login (Table 2) ---
            Panel loginPanel = new Panel { Dock = DockStyle.Fill, BackColor = ThemeManager.Background };
            mainGrid.Controls.Add(loginPanel, 1, 0);

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
            loginPanel.Controls.Add(windowControls);
            windowControls.BringToFront();

            Button closeBtn = new Button { Text = "×", Size = new Size(40, 40), FlatStyle = FlatStyle.Flat, ForeColor = ThemeManager.TextSecondary, Font = new Font("Arial", 18, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0) };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => Application.Exit();
            closeBtn.MouseEnter += (s, e) => closeBtn.ForeColor = Color.FromArgb(233, 69, 96);
            closeBtn.MouseLeave += (s, e) => closeBtn.ForeColor = ThemeManager.TextSecondary;
            windowControls.Controls.Add(closeBtn);

            Button minBtn = new Button { Text = "—", Size = new Size(40, 40), FlatStyle = FlatStyle.Flat, ForeColor = ThemeManager.TextSecondary, Font = new Font("Arial", 12, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0) };
            minBtn.FlatAppearance.BorderSize = 0;
            minBtn.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            windowControls.Controls.Add(minBtn);

            TableLayoutPanel loginLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7,
                Padding = new Padding(60, 80, 60, 60)
            };
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Header
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30)); // Subheader
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Spacer
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85)); // Email
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85)); // Password
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Spacer
            loginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Buttons
            loginPanel.Controls.Add(loginLayout);

            Label loginHeader = new Label { Text = "Welcome Back", Font = ThemeManager.TitleFont, ForeColor = ThemeManager.TextPrimary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            loginLayout.Controls.Add(loginHeader, 0, 0);

            Label subHeader = new Label { Text = "Please sign in to continue", Font = ThemeManager.BodyFont, ForeColor = ThemeManager.TextSecondary, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            loginLayout.Controls.Add(subHeader, 0, 1);

            // Email
            Panel emailGrp = new Panel { Dock = DockStyle.Fill };
            Label emailLbl = new Label { Text = "EMAIL ADDRESS", Font = ThemeManager.SmallFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Top, Height = 25 };
            emailTextBox = new TextBox { Dock = DockStyle.Bottom, Height = 40 };
            ThemeManager.StyleTextBox(emailTextBox);
            emailGrp.Controls.Add(emailLbl);
            emailGrp.Controls.Add(emailTextBox);
            loginLayout.Controls.Add(emailGrp, 0, 3);

            // Password
            Panel passGrp = new Panel { Dock = DockStyle.Fill };
            Label passLbl = new Label { Text = "PASSWORD", Font = ThemeManager.SmallFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Top, Height = 25 };
            passwordTextBox = new TextBox { Dock = DockStyle.Bottom, Height = 40, UseSystemPasswordChar = true };
            ThemeManager.StyleTextBox(passwordTextBox);
            passGrp.Controls.Add(passLbl);
            passGrp.Controls.Add(passwordTextBox);
            loginLayout.Controls.Add(passGrp, 0, 4);

            // Buttons
            Panel btnGrp = new Panel { Dock = DockStyle.Fill };
            Button loginBtn = new Button { Text = "SIGN IN", Width = 180, Dock = DockStyle.Left };
            ThemeManager.StyleButton(loginBtn);
            loginBtn.Click += LoginButton_Click;
            
            Button regBtn = new Button { Text = "CREATE ACCOUNT", Width = 180, Dock = DockStyle.Right };
            ThemeManager.StyleButton(regBtn, false);
            regBtn.Click += RegisterButton_Click;

            btnGrp.Controls.Add(loginBtn);
            btnGrp.Controls.Add(regBtn);
            loginLayout.Controls.Add(btnGrp, 0, 6);

            this.ResumeLayout(false);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
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
