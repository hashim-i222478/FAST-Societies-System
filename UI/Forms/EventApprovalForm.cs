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
    /// Form for admin to approve/reject event requests
    /// </summary>
    public partial class EventApprovalForm : Form
    {
        private ApprovalService _approvalService;
        private EventRepository _eventRepository;
        private DataGridView _approvalsGrid;

        public EventApprovalForm()
        {
            _approvalService = new ApprovalService();
            _eventRepository = new EventRepository();
            InitializeComponent();
            LoadApprovals();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Event Approvals";
            this.Size = new System.Drawing.Size(900, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Pending Event Approvals",
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

            _approvalsGrid.Columns.Add("EventTitle", "Event Title");
            _approvalsGrid.Columns.Add("EventDate", "Event Date");
            _approvalsGrid.Columns.Add("Location", "Location");
            _approvalsGrid.Columns.Add("Capacity", "Capacity");
            _approvalsGrid.Columns.Add("EventId", "EventId");

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
                    if (approval.RequestType == "Event")
                    {
                        Event evt = _eventRepository.GetEventById(approval.TargetId);
                        if (evt != null)
                        {
                            _approvalsGrid.Rows.Add(
                                evt.EventTitle,
                                UIHelpers.FormatDate(evt.EventDate),
                                evt.Location,
                                evt.Capacity,
                                evt.EventId
                            );
                        }
                    }
                }

                if (_approvalsGrid.Rows.Count == 0)
                {
                    UIHelpers.ShowInfo("No pending event approvals");
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
                UIHelpers.ShowError("Please select an event");
                return;
            }

            try
            {
                string eventTitle = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                int eventId = (int)_approvalsGrid.SelectedRows[0].Cells[4].Value;

                if (UIHelpers.ShowConfirm($"Approve event '{eventTitle}'?"))
                {
                    _eventRepository.ApproveEvent(eventId);
                    UIHelpers.ShowInfo("Event approved successfully");
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
                UIHelpers.ShowError("Please select an event");
                return;
            }

            try
            {
                string eventTitle = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                int eventId = (int)_approvalsGrid.SelectedRows[0].Cells[4].Value;

                if (UIHelpers.ShowConfirm($"Reject event '{eventTitle}'?"))
                {
                    _eventRepository.CancelEvent(eventId);
                    UIHelpers.ShowInfo("Event rejected");
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
                UIHelpers.ShowError("Please select an event");
                return;
            }

            try
            {
                string eventTitle = (string)_approvalsGrid.SelectedRows[0].Cells[0].Value;
                string eventDate = (string)_approvalsGrid.SelectedRows[0].Cells[1].Value;
                string location = (string)_approvalsGrid.SelectedRows[0].Cells[2].Value;
                int capacity = (int)_approvalsGrid.SelectedRows[0].Cells[3].Value;

                string details = $"Event Details\n\n" +
                               $"Title: {eventTitle}\n" +
                               $"Date: {eventDate}\n" +
                               $"Location: {location}\n" +
                               $"Capacity: {capacity}";

                UIHelpers.ShowInfo(details);
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to view details: {ex.Message}");
            }
        }
    }
}
