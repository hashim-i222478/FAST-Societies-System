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
    /// Form for managing pending membership requests
    /// </summary>
    public partial class MembershipRequestsForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private DataGridView _requestsGrid;

        public MembershipRequestsForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
            LoadRequests();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Membership Requests";
            this.Size = new System.Drawing.Size(900, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Pending Membership Requests",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _requestsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _requestsGrid.Columns.Add("StudentName", "Student Name");
            _requestsGrid.Columns.Add("Email", "Email");
            _requestsGrid.Columns.Add("JoinDate", "Applied Date");
            _requestsGrid.Columns.Add("Status", "Status");
            _requestsGrid.Columns.Add("MembershipId", "MembershipId");
            _requestsGrid.Columns.Add("SocietyId", "SocietyId");
            
            _requestsGrid.Columns["MembershipId"].Visible = false;
            _requestsGrid.Columns["SocietyId"].Visible = false;

            this.Controls.Add(_requestsGrid);

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

            // Refresh Button
            Button refreshButton = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(340, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            refreshButton.Click += (s, e) => LoadRequests();
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

        private void LoadRequests()
        {
            try
            {
                _requestsGrid.Rows.Clear();
                List<Society> societies = _societyService.GetMySocieties(_headId);

                foreach (var society in societies)
                {
                    List<Membership> pendingRequests = _societyService.GetPendingMembershipRequests(society.SocietyId);
                    
                    foreach (var request in pendingRequests)
                    {
                        // Get student details
                        UserRepository userRepo = new UserRepository();
                        User student = userRepo.GetUserById(request.StudentId);
                        
                        _requestsGrid.Rows.Add(
                            student.FullName,
                            student.Email,
                            UIHelpers.FormatDate(request.JoinDate),
                            request.Status,
                            request.MembershipId,
                            request.SocietyId
                        );
                    }
                }

                if (_requestsGrid.Rows.Count == 0)
                {
                    UIHelpers.ShowInfo("No pending requests");
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load requests: {ex.Message}");
            }
        }

        private void ApproveButton_Click(object sender, EventArgs e)
        {
            if (_requestsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a request");
                return;
            }

            try
            {
                int membershipId = (int)_requestsGrid.SelectedRows[0].Cells[4].Value;
                int societyId = (int)_requestsGrid.SelectedRows[0].Cells[5].Value;
                string studentName = (string)_requestsGrid.SelectedRows[0].Cells[0].Value;

                if (UIHelpers.ShowConfirm($"Approve membership for {studentName}?"))
                {
                    _societyService.ApproveMembership(membershipId, societyId);
                    UIHelpers.ShowInfo("Membership approved successfully");
                    LoadRequests();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to approve: {ex.Message}");
            }
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (_requestsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a request");
                return;
            }

            try
            {
                int membershipId = (int)_requestsGrid.SelectedRows[0].Cells[4].Value;
                int societyId = (int)_requestsGrid.SelectedRows[0].Cells[5].Value;
                string studentName = (string)_requestsGrid.SelectedRows[0].Cells[0].Value;

                if (UIHelpers.ShowConfirm($"Reject membership for {studentName}?"))
                {
                    _societyService.RejectMembership(membershipId, societyId);
                    UIHelpers.ShowInfo("Membership rejected");
                    LoadRequests();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to reject: {ex.Message}");
            }
        }
    }
}
