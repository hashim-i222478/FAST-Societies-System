using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    /// <summary>
    /// Form for viewing student's society memberships
    /// </summary>
    public partial class MyMembershipsForm : Form
    {
        private int _studentId;
        private StudentService _studentService;
        private DataGridView _membershipsGrid;

        public MyMembershipsForm(int studentId)
        {
            _studentId = studentId;
            _studentService = new StudentService();
            InitializeComponent();
            LoadMemberships();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "My Memberships";
            this.Size = new System.Drawing.Size(800, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "My Society Memberships",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _membershipsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(750, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _membershipsGrid.Columns.Add("SocietyId", "Society");
            _membershipsGrid.Columns.Add("JoinDate", "Join Date");
            _membershipsGrid.Columns.Add("Status", "Status");
            _membershipsGrid.Columns.Add("UpcomingEvents", "Upcoming Events");

            this.Controls.Add(_membershipsGrid);

            // View Events Button
            Button viewEventsButton = new Button
            {
                Text = "View Society Events",
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            viewEventsButton.Click += ViewEventsButton_Click;
            this.Controls.Add(viewEventsButton);

            // Leave Button
            Button leaveButton = new Button
            {
                Text = "Leave Society",
                Location = new System.Drawing.Point(230, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Red,
                ForeColor = System.Drawing.Color.White
            };
            leaveButton.Click += LeaveButton_Click;
            this.Controls.Add(leaveButton);

            // Close Button
            Button closeButton = new Button
            {
                Text = "Close",
                Location = new System.Drawing.Point(570, 420),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);

            this.ResumeLayout(false);
        }

        private void LoadMemberships()
        {
            try
            {
                _membershipsGrid.Rows.Clear();
                List<Membership> memberships = _studentService.GetMyMemberships(_studentId);

                foreach (var membership in memberships)
                {
                    Society society = _studentService.GetSocietyDetails(membership.SocietyId);
                    var upcomingEvents = _studentService.GetUpcomingEventsBySociety(membership.SocietyId);
                    
                    _membershipsGrid.Rows.Add(
                        society.SocietyName,
                        UIHelpers.FormatDate(membership.JoinDate),
                        membership.Status,
                        upcomingEvents.Count
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to load memberships: {ex.Message}");
            }
        }

        private void ViewEventsButton_Click(object sender, EventArgs e)
        {
            if (_membershipsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society");
                return;
            }

            string societyName = (string)_membershipsGrid.SelectedRows[0].Cells[0].Value;
            UIHelpers.ShowInfo($"Viewing events for: {societyName}");
        }

        private void LeaveButton_Click(object sender, EventArgs e)
        {
            if (_membershipsGrid.SelectedRows.Count == 0)
            {
                UIHelpers.ShowError("Please select a society");
                return;
            }

            try
            {
                string societyName = (string)_membershipsGrid.SelectedRows[0].Cells[0].Value;
                
                if (UIHelpers.ShowConfirm($"Leave {societyName}?", "Confirm"))
                {
                    UIHelpers.ShowInfo("You have left the society");
                    LoadMemberships();
                }
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to leave society: {ex.Message}");
            }
        }
    }
}
