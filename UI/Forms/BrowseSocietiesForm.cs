using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for browsing available societies
    /// </summary>
    public partial class BrowseSocietiesForm : Form
    {
        private int _studentId;
        private StudentService _studentService;
        private DataGridView _societiesGrid;

        public BrowseSocietiesForm(int studentId)
        {
            _studentId = studentId;
            _studentService = new StudentService();
            InitializeComponent();
            LoadSocieties();
        }

        private Label _emptyLabel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Browse Societies - FAST Societies";
            this.Size = new System.Drawing.Size(1000, 700);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ThemeManager.Background;

            // --- Main Container ---
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

            // --- Window Controls ---
            FlowLayoutPanel windowControls = new FlowLayoutPanel
            {
                Size = new Size(100, 40),
                Location = new Point(900, 0),
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

            // Header
            Label titleLabel = new Label
            {
                Text = "Available Societies",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Area (Grid + Empty State)
            Panel contentPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _societiesGrid = new DataGridView { Dock = DockStyle.Fill, Visible = false };
            ThemeManager.StyleGrid(_societiesGrid);
            _societiesGrid.Columns.Add("SocietyId", "ID");
            _societiesGrid.Columns.Add("SocietyName", "SOCIETY NAME");
            _societiesGrid.Columns.Add("Description", "DESCRIPTION");
            _societiesGrid.Columns.Add("MemberCount", "MEMBERS");
            contentPanel.Controls.Add(_societiesGrid);

            _emptyLabel = new Label
            {
                Text = "No societies are currently available to join.\nCheck back later!",
                Font = ThemeManager.HeaderFont,
                ForeColor = ThemeManager.TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Visible = false
            };
            contentPanel.Controls.Add(_emptyLabel);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button applyButton = new Button { Text = "APPLY FOR MEMBERSHIP", Width = 250, Dock = DockStyle.Left };
            ThemeManager.StyleButton(applyButton);
            applyButton.Click += ApplyButton_Click;
            footer.Controls.Add(applyButton);

            Button closeButton = new Button { Text = "BACK TO DASHBOARD", Width = 200, Dock = DockStyle.Right };
            ThemeManager.StyleButton(closeButton, false);
            closeButton.Click += (s, e) => this.Close();
            footer.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadSocieties()
        {
            try
            {
                _societiesGrid.Rows.Clear();
                List<Society> societies = _studentService.BrowseSocieties();

                if (societies == null || societies.Count == 0)
                {
                    _societiesGrid.Visible = false;
                    _emptyLabel.Visible = true;
                }
                else
                {
                    _societiesGrid.Visible = true;
                    _emptyLabel.Visible = false;

                    foreach (var society in societies)
                    {
                        int memberCount = new SocietyService().GetMemberCount(society.SocietyId);
                        _societiesGrid.Rows.Add(society.SocietyId, society.SocietyName, society.Description, memberCount);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load societies: {ex.Message}");
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (_societiesGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society from the list first.");
                return;
            }

            try
            {
                int societyId = (int)_societiesGrid.SelectedRows[0].Cells[0].Value;
                string societyName = (string)_societiesGrid.SelectedRows[0].Cells[1].Value;

                if (UIHelpers.ShowConfirm($"Would you like to apply for membership in '{societyName}'?"))
                {
                    _studentService.ApplyForMembership(_studentId, societyId);
                    UIHelpers.ShowInfo("Your membership application has been submitted successfully!");
                    LoadSocieties();
                }
            }
            catch (DuplicateResourceException)
            {
                UIHelpers.ShowError("You have already submitted an application or are already a member of this society.");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to process application: {ex.Message}");
            }
        }
    }
}
