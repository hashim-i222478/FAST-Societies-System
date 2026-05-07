using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class AdminSocietyManagementForm : Form
    {
        private SocietyService _societyService;
        private DataGridView _societiesGrid;
        private Button _approveBtn;
        private Button _suspendBtn;
        private Button _activateBtn;
        private Button _deleteBtn;

        public AdminSocietyManagementForm()
        {
            _societyService = new SocietyService();
            InitializeComponent();
            LoadSocieties();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Society Management - FAST Societies";
            this.Size = new System.Drawing.Size(1100, 750);
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
                Location = new Point(1000, 0),
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

            // Header
            Label titleLabel = new Label
            {
                Text = "Manage All Societies",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Grid
            _societiesGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_societiesGrid);
            _societiesGrid.Columns.Add("SocietyName", "SOCIETY NAME");
            _societiesGrid.Columns.Add("Status", "STATUS");
            _societiesGrid.Columns.Add("Head", "SOCIETY HEAD");
            _societiesGrid.Columns.Add("Members", "MEMBERS");
            _societiesGrid.Columns.Add("SocietyId", "ID");
            _societiesGrid.Columns["SocietyId"].Visible = false;

            _societiesGrid.SelectionChanged += SocietiesGrid_SelectionChanged;
            mainGrid.Controls.Add(_societiesGrid, 0, 1);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button createBtn = new Button { Text = "CREATE", Width = 130, Dock = DockStyle.Left };
            ThemeManager.StyleButton(createBtn);
            createBtn.Click += (s, e) => OpenAddSociety();
            footer.Controls.Add(createBtn);

            _approveBtn = new Button { Text = "APPROVE", Width = 130, Dock = DockStyle.Left, Margin = new Padding(15, 0, 0, 0), Visible = false };
            ThemeManager.StyleButton(_approveBtn, false);
            _approveBtn.Click += (s, e) => ChangeStatus("Approve");
            footer.Controls.Add(_approveBtn);

            _suspendBtn = new Button { Text = "SUSPEND", Width = 130, Dock = DockStyle.Left, Margin = new Padding(15, 0, 0, 0), Visible = false };
            ThemeManager.StyleButton(_suspendBtn, false);
            _suspendBtn.ForeColor = Color.FromArgb(255, 171, 64);
            _suspendBtn.Click += (s, e) => ChangeStatus("Suspend");
            footer.Controls.Add(_suspendBtn);

            _activateBtn = new Button { Text = "ACTIVATE", Width = 130, Dock = DockStyle.Left, Margin = new Padding(15, 0, 0, 0), Visible = false };
            ThemeManager.StyleButton(_activateBtn, false);
            _activateBtn.Click += (s, e) => ChangeStatus("Activate");
            footer.Controls.Add(_activateBtn);

            _deleteBtn = new Button { Text = "DELETE", Width = 130, Dock = DockStyle.Left, Margin = new Padding(15, 0, 0, 0), Visible = false };
            ThemeManager.StyleButton(_deleteBtn, false);
            _deleteBtn.ForeColor = Color.FromArgb(233, 69, 96);
            _deleteBtn.Click += (s, e) => DeleteSociety();
            footer.Controls.Add(_deleteBtn);

            this.ResumeLayout(false);
        }

        private void LoadSocieties()
        {
            try
            {
                _societiesGrid.Rows.Clear();
                var societies = _societyService.GetAllSocieties();
                var userRepo = new DAL.UserRepository();

                foreach (var society in societies)
                {
                    var head = userRepo.GetUserById(society.HeadId);
                    int memberCount = _societyService.GetMemberCount(society.SocietyId);
                    
                    _societiesGrid.Rows.Add(
                        society.SocietyName,
                        society.Status.ToUpper(),
                        head?.FullName ?? "Unknown",
                        memberCount,
                        society.SocietyId
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load societies: {ex.Message}");
            }
        }

        private void OpenAddSociety()
        {
            AddSocietyForm form = new AddSocietyForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSocieties();
            }
        }

        private void SocietiesGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_societiesGrid.SelectedRows.Count == 0)
            {
                _approveBtn.Visible = false;
                _suspendBtn.Visible = false;
                _activateBtn.Visible = false;
                _deleteBtn.Visible = false;
                return;
            }

            string status = _societiesGrid.SelectedRows[0].Cells[1].Value.ToString().ToUpper();
            
            _deleteBtn.Visible = true; // Delete always visible for selected rows

            if (status == "PENDING")
            {
                _approveBtn.Visible = true;
                _suspendBtn.Visible = false;
                _activateBtn.Visible = false;
            }
            else if (status == "ACTIVE" || status == "APPROVED")
            {
                _approveBtn.Visible = false;
                _suspendBtn.Visible = true;
                _activateBtn.Visible = false;
            }
            else if (status == "SUSPENDED")
            {
                _approveBtn.Visible = false;
                _suspendBtn.Visible = false;
                _activateBtn.Visible = true;
            }
        }

        private void ChangeStatus(string action)
        {
            if (_societiesGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society.");
                return;
            }

            int societyId = (int)_societiesGrid.SelectedRows[0].Cells[4].Value;
            string name = (string)_societiesGrid.SelectedRows[0].Cells[0].Value;

            try
            {
                bool success = false;
                if (action == "Approve") success = _societyService.ApproveSociety(societyId);
                else if (action == "Suspend") success = _societyService.SuspendSociety(societyId);
                else if (action == "Activate") success = _societyService.ActivateSociety(societyId);

                if (success)
                {
                    UIHelpers.ShowInfo($"Society '{name}' {action}d successfully.");
                    LoadSocieties();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Error: {ex.Message}");
            }
        }

        private void DeleteSociety()
        {
            if (_societiesGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society to delete.");
                return;
            }

            int societyId = (int)_societiesGrid.SelectedRows[0].Cells[4].Value;
            string name = (string)_societiesGrid.SelectedRows[0].Cells[0].Value;

            if (UIHelpers.ShowConfirm($"Are you sure you want to PERMANENTLY DELETE '{name}'? This cannot be undone.", "Confirm Delete"))
            {
                try
                {
                    if (_societyService.DeleteSociety(societyId))
                    {
                        UIHelpers.ShowInfo($"Society '{name}' has been deleted.");
                        LoadSocieties();
                    }
                }
                catch (Exception ex)
                {
                    UIHelpers.ShowError($"Could not delete society: {ex.Message}");
                }
            }
        }
    }
}
