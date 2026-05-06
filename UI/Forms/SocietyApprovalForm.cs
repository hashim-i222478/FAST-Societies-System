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

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Society Approvals";
            this.Size = new System.Drawing.Size(900, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Pending Society Approvals",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _approvalsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _approvalsGrid.Columns.Add("SocietyName", "Society Name");
            _approvalsGrid.Columns.Add("HeadName", "Head Name");
            _approvalsGrid.Columns.Add("Description", "Description");
            _approvalsGrid.Columns.Add("RequestDate", "Requested");
            _approvalsGrid.Columns.Add("SocietyId", "SocietyId");

            this.Controls.Add(_approvalsGrid);

            // Approve Button
            Button approveButton = new Button
            {
                Text = "Approve",
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            approveButton.Click += ApproveButton_Click;
            this.Controls.Add(approveButton);

            // Reject Button
            Button rejectButton = new Button
            {
                Text = "Reject",
                Location = new System.Drawing.Point(180, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Red,
                ForeColor = System.Drawing.Color.White
            };
            rejectButton.Click += RejectButton_Click;
            this.Controls.Add(rejectButton);

            // View Details Button
            Button viewButton = new Button
            {
                Text = "View Details",
                Location = new System.Drawing.Point(340, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            viewButton.Click += ViewButton_Click;
            this.Controls.Add(viewButton);

            // Refresh Button
            Button refreshButton = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(500, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.CornflowerBlue,
                ForeColor = System.Drawing.Color.White
            };
            refreshButton.Click += (s, e) => LoadApprovals();
            this.Controls.Add(refreshButton);

            // Close Button
            Button closeButton = new Button
            {
                Text = "Close",
                Location = new System.Drawing.Point(690, 420),
                Size = new System.Drawing.Size(180, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadApprovals()
        {
            try
            {
                _approvalsGrid.Rows.Clear();
                List<ApprovalRequest> approvals = _approvalService.GetAllPendingApprovals();

                foreach (var approval in approvals)
                {
                    if (approval.RequestType == "Society")
                    {
                        Society society = _societyRepository.GetSocietyById(approval.TargetId);
                        if (society != null)
                        {
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

                if (_approvalsGrid.Rows.Count == 0)
                {
                    UIHelpers.ShowInfo("No pending society approvals");
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
                UIHelpers.ShowError("Please select a society");
                return;
            }

            try
            {
                string societyName = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                int societyId = (int)_approvalsGrid.SelectedRows[0].Cells[4].Value;

                if (UIHelpers.ShowConfirm($"Approve society '{societyName}'?"))
                {
                    _societyRepository.ApproveSociety(societyId);
                    UIHelpers.ShowInfo("Society approved successfully");
                    LoadApprovals();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to approve: {ex.Message}");
            }
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (_approvalsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society");
                return;
            }

            try
            {
                string societyName = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                int societyId = (int)_approvalsGrid.SelectedRows[0].Cells[4].Value;

                if (UIHelpers.ShowConfirm($"Reject society '{societyName}'?"))
                {
                    UIHelpers.ShowInfo("Society rejected");
                    LoadApprovals();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to reject: {ex.Message}");
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (_approvalsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society");
                return;
            }

            try
            {
                string societyName = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                string head = (string)_approvalsGrid.SelectedRows[0].Cells[1].Value;
                string description = (string)_approvalsGrid.SelectedRows[0].Cells[2].Value;
                string requestDate = (string)_approvalsGrid.SelectedRows[0].Cells[3].Value;

                string details = $"Society Details\n\n" +
                               $"Name: {societyName}\n" +
                               $"Head: {head}\n" +
                               $"Description: {description}\n" +
                               $"Requested: {requestDate}";

                UIHelpers.ShowInfo(details);
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to view details: {ex.Message}");
            }
        }
    }
}
