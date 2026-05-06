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

            this.Text = "Create New Society";
            this.Size = new System.Drawing.Size(500, 600);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ThemeManager.Surface;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                Padding = new Padding(40)
            };
            this.Controls.Add(layout);

            Label title = new Label { Text = "NEW SOCIETY", Font = ThemeManager.HeaderFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            layout.Controls.Add(title, 0, 0);

            layout.Controls.Add(new Label { Text = "SOCIETY NAME", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Dock = DockStyle.Bottom }, 0, 1);
            _nameTxt = new TextBox { Dock = DockStyle.Top, BackColor = ThemeManager.Background, ForeColor = ThemeManager.TextPrimary, Font = ThemeManager.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            layout.Controls.Add(_nameTxt, 0, 2);

            layout.Controls.Add(new Label { Text = "DESCRIPTION", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Dock = DockStyle.Bottom, Margin = new Padding(0, 20, 0, 0) }, 0, 3);
            _descTxt = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 100, BackColor = ThemeManager.Background, ForeColor = ThemeManager.TextPrimary, Font = ThemeManager.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            layout.Controls.Add(_descTxt, 0, 4);

            layout.Controls.Add(new Label { Text = "ASSIGN HEAD", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Dock = DockStyle.Bottom, Margin = new Padding(0, 20, 0, 0) }, 0, 5);
            _headCombo = new ComboBox { Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = ThemeManager.Background, ForeColor = ThemeManager.TextPrimary, Font = ThemeManager.BodyFont, FlatStyle = FlatStyle.Flat };
            layout.Controls.Add(_headCombo, 0, 6);

            FlowLayoutPanel buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, Margin = new Padding(0, 30, 0, 0) };
            layout.Controls.Add(buttons, 0, 7);

            Button cancelBtn = new Button { Text = "CANCEL", Width = 100 };
            ThemeManager.StyleButton(cancelBtn, false);
            cancelBtn.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            buttons.Controls.Add(cancelBtn);

            Button saveBtn = new Button { Text = "CREATE", Width = 120 };
            ThemeManager.StyleButton(saveBtn);
            saveBtn.Click += SaveBtn_Click;
            buttons.Controls.Add(saveBtn);

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
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load heads: {ex.Message}");
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_nameTxt.Text))
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
                _societyService.CreateSociety(_nameTxt.Text, _descTxt.Text, headId, "Active");
                UIHelpers.ShowInfo("Society created and activated successfully.");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to create society: {ex.Message}");
            }
        }
    }
}
