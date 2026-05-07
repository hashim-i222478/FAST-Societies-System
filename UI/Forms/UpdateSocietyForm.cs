using System;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class UpdateSocietyForm : Form
    {
        private int _societyId;
        private int _headId;
        private SocietyService _societyService;

        private TextBox _nameTextBox;
        private TextBox _descTextBox;

        public UpdateSocietyForm(int societyId, int headId)
        {
            _societyId = societyId;
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
            LoadSocietyData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Update Society - FAST Societies";
            this.Size = new Size(500, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            ThemeManager.ApplyTheme(this);

            TableLayoutPanel mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(30)
            };
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Title
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Name
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Description
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Buttons
            this.Controls.Add(mainGrid);

            // Title
            Label titleLabel = new Label
            {
                Text = "Update Society Details",
                Font = ThemeManager.HeaderFont,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Name Field
            Panel namePanel = new Panel { Dock = DockStyle.Fill };
            Label nameLabel = new Label { Text = "Society Name", Location = new Point(0, 0), AutoSize = true, Font = ThemeManager.SmallFont, ForeColor = ThemeManager.TextSecondary };
            _nameTextBox = new TextBox { Location = new Point(0, 25), Width = 400 };
            ThemeManager.StyleTextBox(_nameTextBox);
            namePanel.Controls.Add(nameLabel);
            namePanel.Controls.Add(_nameTextBox);
            mainGrid.Controls.Add(namePanel, 0, 1);

            // Description Field
            Panel descPanel = new Panel { Dock = DockStyle.Fill };
            Label descLabel = new Label { Text = "Description", Location = new Point(0, 0), AutoSize = true, Font = ThemeManager.SmallFont, ForeColor = ThemeManager.TextSecondary };
            _descTextBox = new TextBox { Location = new Point(0, 25), Width = 400, Height = 100, Multiline = true };
            ThemeManager.StyleTextBox(_descTextBox);
            descPanel.Controls.Add(descLabel);
            descPanel.Controls.Add(_descTextBox);
            mainGrid.Controls.Add(descPanel, 0, 2);

            // Buttons
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 10, 0, 0)
            };

            Button saveBtn = new Button { Text = "SAVE CHANGES", Width = 150 };
            ThemeManager.StyleButton(saveBtn);
            saveBtn.Click += SaveButton_Click;

            Button cancelBtn = new Button { Text = "CANCEL", Width = 120, Margin = new Padding(0, 0, 10, 0) };
            ThemeManager.StyleButton(cancelBtn, false);
            cancelBtn.Click += (s, e) => this.Close();

            buttonPanel.Controls.Add(saveBtn);
            buttonPanel.Controls.Add(cancelBtn);
            mainGrid.Controls.Add(buttonPanel, 0, 3);

            this.ResumeLayout(false);
        }

        private void LoadSocietyData()
        {
            try
            {
                Society society = _societyService.GetSocietyProfile(_societyId);
                if (society != null)
                {
                    _nameTextBox.Text = society.SocietyName;
                    _descTextBox.Text = society.Description;
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load society data: {ex.Message}");
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string name = _nameTextBox.Text.Trim();
            string desc = _descTextBox.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                UIHelpers.ShowError("Society name is required");
                return;
            }

            try
            {
                _societyService.UpdateSocietyProfile(_societyId, _headId, name, desc);
                UIHelpers.ShowInfo("Society updated successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to update society: {ex.Message}");
            }
        }
    }
}
