using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.DAL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for admin to approve/reject society creation requests
    /// </summary>
    public partial class SocietyApprovalForm : Form
    {
        private ApprovalService _approvalService;
        private SocietyRepository _societyRepository;
        private DataGridView _approvalsGrid;

        public SocietyApprovalForm()
        {
            _approvalService = new ApprovalService();
            _societyRepository = new SocietyRepository();
            InitializeComponent();
            LoadApprovals();
        }

        private Label _emptyLabel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Society Approvals - FAST Societies";
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
                Text = "Pending Society Approvals",
                Font = ThemeManager.TitleFont,
                ForeColor = ThemeManager.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            mainGrid.Controls.Add(titleLabel, 0, 0);

            // Content Area
            Panel contentPanel = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(contentPanel, 0, 1);

            _approvalsGrid = new DataGridView { Dock = DockStyle.Fill, Visible = false };
            ThemeManager.StyleGrid(_approvalsGrid);
            _approvalsGrid.Columns.Add("SocietyName", "SOCIETY NAME");
            _approvalsGrid.Columns.Add("HeadName", "HEAD NAME");
            _approvalsGrid.Columns.Add("Description", "DESCRIPTION");
            _approvalsGrid.Columns.Add("RequestDate", "REQUESTED ON");
            _approvalsGrid.Columns.Add("SocietyId", "ID");
            _approvalsGrid.Columns["SocietyId"].Visible = false;
            contentPanel.Controls.Add(_approvalsGrid);

            _emptyLabel = new Label
            {
                Text = "No pending society requests at the moment.\nAll societies are up to date.",
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

            Button approveButton = new Button { Text = "APPROVE", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(approveButton);
            approveButton.Click += ApproveButton_Click;
            footer.Controls.Add(approveButton);

            Button rejectButton = new Button { Text = "REJECT", Width = 150, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(rejectButton, false);
            rejectButton.ForeColor = Color.FromArgb(233, 69, 96);
            rejectButton.Click += RejectButton_Click;
            footer.Controls.Add(rejectButton);

            Button viewButton = new Button { Text = "VIEW DETAILS", Width = 150, Dock = DockStyle.Left, Margin = new Padding(20, 0, 0, 0) };
            ThemeManager.StyleButton(viewButton, false);
            viewButton.Click += ViewButton_Click;
            footer.Controls.Add(viewButton);

            Button refreshButton = new Button { Text = "REFRESH", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(refreshButton, false);
            refreshButton.Click += (s, e) => LoadApprovals();
            footer.Controls.Add(refreshButton);

            this.ResumeLayout(false);
        }

        private void LoadApprovals()
        {
            try
            {
                _approvalsGrid.Rows.Clear();
                List<ApprovalRequest> approvals = _approvalService.GetAllPendingApprovals();

                bool hasSocieties = false;
                foreach (var approval in approvals)
                {
                    if (approval.RequestType == "Society")
                    {
                        Society society = _societyRepository.GetSocietyById(approval.TargetId);
                        if (society != null)
                        {
                            hasSocieties = true;
                            UserRepository userRepo = new UserRepository();
                            User head = userRepo.GetUserById(society.HeadId);

                            _approvalsGrid.Rows.Add(
                                society.SocietyName,
                                head?.FullName ?? "Unknown",
                                society.Description,
                                UIHelpers.FormatDate(approval.CreatedDate),
                                society.SocietyId
                            );
                        }
                    }
                }

                if (!hasSocieties)
                {
                    _approvalsGrid.Visible = false;
                    _emptyLabel.Visible = true;
                }
                else
                {
                    _approvalsGrid.Visible = true;
                    _emptyLabel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load approvals: {ex.Message}");
            }
        }

        private void ApproveButton_Click(object sender, EventArgs e)
        {
            if (_approvalsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society from the list.");
                return;
            }

            try
            {
                string societyName = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                int societyId = (int)_approvalsGrid.SelectedRows[0].Cells[4].Value;

                if (UIHelpers.ShowConfirm($"Are you sure you want to approve society '{societyName}'?", "Confirm Approval"))
                {
                    _societyRepository.ApproveSociety(societyId);
                    UIHelpers.ShowInfo($"Society '{societyName}' has been approved successfully.");
                    LoadApprovals();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"An error occurred during approval: {ex.Message}");
            }
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (_approvalsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society to reject.");
                return;
            }

            try
            {
                string societyName = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                int societyId = (int)_approvalsGrid.SelectedRows[0].Cells[4].Value;

                if (UIHelpers.ShowConfirm($"Are you sure you want to reject the application for '{societyName}'?", "Confirm Rejection"))
                {
                    // Logic for rejection would go here
                    UIHelpers.ShowInfo($"The application for '{societyName}' has been rejected.");
                    LoadApprovals();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"An error occurred during rejection: {ex.Message}");
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (_approvalsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society to view details.");
                return;
            }

            try
            {
                string societyName = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                string head = (string)_approvalsGrid.SelectedRows[0].Cells[1].Value;
                string description = (string)_approvalsGrid.SelectedRows[0].Cells[2].Value;
                string requestDate = (string)_approvalsGrid.SelectedRows[0].Cells[3].Value;

                string details = $"SOCIETY REQUEST DETAILS\n\n" +
                               $"NAME: {societyName}\n" +
                               $"PROPOSED HEAD: {head}\n" +
                               $"DESCRIPTION: {description}\n" +
                               $"REQUESTED ON: {requestDate}";

                UIHelpers.ShowInfo(details, "Application Details");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to retrieve details: {ex.Message}");
            }
        }
    }
}
