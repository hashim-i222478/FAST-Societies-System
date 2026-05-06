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

            this.Text = "Society Management";
            this.Size = new System.Drawing.Size(900, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Manage Your Societies",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _societiesGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _societiesGrid.Columns.Add("SocietyId", "ID");
            _societiesGrid.Columns.Add("SocietyName", "Society Name");
            _societiesGrid.Columns.Add("Status", "Status");
            _societiesGrid.Columns.Add("Members", "Members");
            _societiesGrid.Columns.Add("CreatedDate", "Created");

            this.Controls.Add(_societiesGrid);

            // Edit Button
            Button editButton = new Button
            {
                Text = "Edit Details",
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            editButton.Click += EditButton_Click;
            this.Controls.Add(editButton);

            // View Members Button
            Button viewMembersButton = new Button
            {
                Text = "View Members",
                Location = new System.Drawing.Point(180, 420),
                Size = new System.Drawing.Size(150, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            viewMembersButton.Click += ViewMembersButton_Click;
            this.Controls.Add(viewMembersButton);

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

                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new society name:", "Edit Society", societyName);
                
                if (!string.IsNullOrEmpty(newName) && newName != societyName)
                {
                    Society society = _societyService.GetSocietyProfile(societyId);
                    if (society != null)
                    {
                        society.SocietyName = newName;
                        _societyService.UpdateSocietyProfile(societyId, _headId, newName, society.Description);
                        UIHelpers.ShowInfo("Society name updated successfully");
                        LoadSocieties();
                    }
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
