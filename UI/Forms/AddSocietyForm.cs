using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class AddSocietyForm : Form
    {
        private TextBox _nameTxt;
        private TextBox _descTxt;
        private ComboBox _headCombo;
        private SocietyService _societyService;
        private List<User> _heads;

        public AddSocietyForm()
        {
            _societyService = new SocietyService();
            InitializeComponent();
            LoadHeads();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Create Society - FAST Societies";
            this.Size = new System.Drawing.Size(550, 650);
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
                Location = new Point(450, 0),
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
                Text = "Register New Society",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Panel
            FlowLayoutPanel contentPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 20, 0, 0)
            };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            // Field Helper
            void AddField(string labelText, Control inputControl, int height = 35)
            {
                Panel group = new Panel { Width = 450, Height = inputControl.Height + 40, Margin = new Padding(0, 0, 0, 20) };
                Label lbl = new Label { Text = labelText, Font = ThemeManager.SmallFont, ForeColor = ThemeManager.Accent, Location = new Point(0, 0), AutoSize = true };
                inputControl.Location = new Point(0, 25);
                inputControl.Width = 430;
                group.Height = inputControl.Height + 35;
                group.Controls.Add(lbl);
                group.Controls.Add(inputControl);
                contentPanel.Controls.Add(group);
            }

            _nameTxt = new TextBox { Font = ThemeManager.BodyFont };
            ThemeManager.StyleTextBox(_nameTxt);
            AddField("SOCIETY NAME", _nameTxt);

            _descTxt = new TextBox { Font = ThemeManager.BodyFont, Multiline = true, Height = 100 };
            ThemeManager.StyleTextBox(_descTxt);
            AddField("DESCRIPTION", _descTxt);

            _headCombo = new ComboBox { Font = ThemeManager.BodyFont, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            _headCombo.BackColor = ThemeManager.Surface;
            _headCombo.ForeColor = ThemeManager.TextPrimary;
            AddField("ASSIGN SOCIETY HEAD", _headCombo);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button createButton = new Button { Text = "CREATE SOCIETY", Width = 180, Dock = DockStyle.Left };
            ThemeManager.StyleButton(createButton, false);
            createButton.ForeColor = Color.FromArgb(0, 255, 159);
            createButton.Click += SaveBtn_Click;
            footer.Controls.Add(createButton);

            Button cancelButton = new Button { Text = "CANCEL", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(cancelButton, false);
            cancelButton.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            footer.Controls.Add(cancelButton);

            this.ResumeLayout(false);
        }

        private void LoadHeads()
        {
            try
            {
                var userRepo = new DAL.UserRepository();
                _heads = userRepo.GetUsersByRole("SocietyHead");
                
                foreach (var head in _heads)
                {
                    _headCombo.Items.Add($"{head.FullName} ({head.Email})");
                }
                if (_headCombo.Items.Count > 0) _headCombo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load heads: {ex.Message}");
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_nameTxt.Text.Trim()))
            {
                UIHelpers.ShowError("Society Name is required.");
                return;
            }

            if (_headCombo.SelectedIndex == -1)
            {
                UIHelpers.ShowError("Please assign a Society Head.");
                return;
            }

            try
            {
                int headId = _heads[_headCombo.SelectedIndex].UserId;
                _societyService.CreateSociety(_nameTxt.Text.Trim(), _descTxt.Text.Trim(), headId, "Active");
                UIHelpers.ShowInfo("Society created and activated successfully.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to create society: {ex.Message}");
            }
        }
    }
}
