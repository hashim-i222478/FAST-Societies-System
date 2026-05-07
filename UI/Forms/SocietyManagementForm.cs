using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for society head to manage their society profile
    /// </summary>
    public partial class SocietyManagementForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private DataGridView _societiesGrid;

        public SocietyManagementForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
            LoadSocieties();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Society Management - FAST Societies";
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
                Text = "Manage Your Societies",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Area
            Panel contentPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _societiesGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_societiesGrid);
            _societiesGrid.Columns.Add("SocietyId", "ID");
            _societiesGrid.Columns.Add("SocietyName", "SOCIETY NAME");
            _societiesGrid.Columns.Add("Status", "STATUS");
            _societiesGrid.Columns.Add("Members", "MEMBERS");
            _societiesGrid.Columns.Add("CreatedDate", "CREATED");
            contentPanel.Controls.Add(_societiesGrid);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button editButton = new Button { Text = "EDIT DETAILS", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(editButton);
            editButton.Click += EditButton_Click;
            footer.Controls.Add(editButton);

            Button viewMembersButton = new Button { Text = "VIEW MEMBERS", Width = 180, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(viewMembersButton, false);
            viewMembersButton.Click += ViewMembersButton_Click;
            footer.Controls.Add(viewMembersButton);

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
                List<Society> societies = _societyService.GetMySocieties(_headId);

                foreach (var society in societies)
                {
                    int memberCount = _societyService.GetMemberCount(society.SocietyId);
                    
                    _societiesGrid.Rows.Add(
                        society.SocietyId,
                        society.SocietyName,
                        society.Status,
                        memberCount,
                        UIHelpers.FormatDate(society.CreatedDate)
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load societies: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (_societiesGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society");
                return;
            }

            try
            {
                int societyId = (int)_societiesGrid.SelectedRows[0].Cells[0].Value;
                string societyName = (string)_societiesGrid.SelectedRows[0].Cells[1].Value;

                UpdateSocietyForm updateForm = new UpdateSocietyForm(societyId, _headId);
                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    LoadSocieties();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to update: {ex.Message}");
            }
        }

        private void ViewMembersButton_Click(object sender, EventArgs e)
        {
            if (_societiesGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society");
                return;
            }

            try
            {
                int societyId = (int)_societiesGrid.SelectedRows[0].Cells[0].Value;
                string societyName = (string)_societiesGrid.SelectedRows[0].Cells[1].Value;

                List<Membership> members = _societyService.GetSocietyMembers(societyId);
                
                string membersList = $"Members of {societyName}\n\n";
                if (members.Count > 0)
                {
                    foreach (var member in members)
                    {
                        membersList += $"• Student ID: {member.StudentId} - Status: {member.Status}\n";
                    }
                }
                else
                {
                    membersList += "No members yet";
                }

                UIHelpers.ShowInfo(membersList);
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load members: {ex.Message}");
            }
        }
    }
}
