using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class MemberManagementForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private DataGridView _membersGrid;
        private ComboBox _societySelector;
        private List<Society> _mySocieties;

        public MemberManagementForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
            LoadSocieties();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Member Management - FAST Societies";
            this.Size = new System.Drawing.Size(1000, 700);
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
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); // Header
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Footer
            this.Controls.Add(mainGrid);

            // Window Controls
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
            windowControls.Controls.Add(closeBtn);

            // Header
            Panel header = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(header, 0, 0);

            Label titleLabel = new Label
            {
                Text = "Manage Society Members",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Location = new Point(0, 0),
                Size = new Size(400, 40)
            };
            header.Controls.Add(titleLabel);

            Label selectLbl = new Label { Text = "SELECT SOCIETY:", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Location = new Point(0, 50), AutoSize = true };
            header.Controls.Add(selectLbl);

            _societySelector = new ComboBox
            {
                Location = new Point(0, 70),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ThemeManager.Surface,
                ForeColor = ThemeManager.TextPrimary,
                Font = ThemeManager.BodyFont,
                FlatStyle = FlatStyle.Flat
            };
            _societySelector.SelectedIndexChanged += (s, e) => LoadMembers();
            header.Controls.Add(_societySelector);

            // Grid
            _membersGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_membersGrid);
            _membersGrid.Columns.Add("StudentName", "STUDENT NAME");
            _membersGrid.Columns.Add("Email", "EMAIL");
            _membersGrid.Columns.Add("JoinedDate", "JOINED DATE");
            _membersGrid.Columns.Add("Status", "STATUS");
            _membersGrid.Columns.Add("MembershipId", "ID");
            _membersGrid.Columns["MembershipId"].Visible = false;

            mainGrid.Controls.Add(_membersGrid, 0, 1);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button removeBtn = new Button { Text = "REMOVE MEMBER", Width = 180, Dock = DockStyle.Left };
            ThemeManager.StyleButton(removeBtn, false);
            removeBtn.ForeColor = Color.FromArgb(233, 69, 96);
            removeBtn.Click += RemoveMember_Click;
            footer.Controls.Add(removeBtn);

            Button refreshBtn = new Button { Text = "REFRESH", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(refreshBtn, false);
            refreshBtn.Click += (s, e) => LoadMembers();
            footer.Controls.Add(refreshBtn);

            this.ResumeLayout(false);
        }

        private void LoadSocieties()
        {
            try
            {
                _mySocieties = _societyService.GetMySocieties(_headId);
                _societySelector.Items.Clear();
                foreach (var s in _mySocieties)
                {
                    _societySelector.Items.Add(s.SocietyName);
                }
                if (_societySelector.Items.Count > 0) _societySelector.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load societies: {ex.Message}");
            }
        }

        private void LoadMembers()
        {
            if (_societySelector.SelectedIndex == -1) return;

            try
            {
                _membersGrid.Rows.Clear();
                int societyId = _mySocieties[_societySelector.SelectedIndex].SocietyId;
                var members = _societyService.GetSocietyMembers(societyId);
                var userRepo = new DAL.UserRepository();

                foreach (var m in members)
                {
                    var student = userRepo.GetUserById(m.StudentId);
                    _membersGrid.Rows.Add(
                        student?.FullName ?? "Unknown",
                        student?.Email ?? "N/A",
                        UIHelpers.FormatDate(m.JoinDate),
                        m.Status.ToUpper(),
                        m.MembershipId
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load members: {ex.Message}");
            }
        }

        private void RemoveMember_Click(object sender, EventArgs e)
        {
            if (_membersGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a member to remove.");
                return;
            }

            int membershipId = (int)_membersGrid.SelectedRows[0].Cells[4].Value;
            string name = (string)_membersGrid.SelectedRows[0].Cells[0].Value;
            int societyId = _mySocieties[_societySelector.SelectedIndex].SocietyId;

            if (UIHelpers.ShowConfirm($"Are you sure you want to remove '{name}' from the society?"))
            {
                try
                {
                    if (_societyService.RemoveMember(membershipId, societyId))
                    {
                        UIHelpers.ShowInfo($"Member '{name}' has been removed.");
                        LoadMembers();
                    }
                }
                catch (Exception ex)
                {
                    UIHelpers.ShowError($"Error: {ex.Message}");
                }
            }
        }
    }
}
