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

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Browse Societies";
            this.Size = new System.Drawing.Size(800, 500);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Title
            Label titleLabel = new Label
            {
                Text = "Available Societies",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(titleLabel);

            // Grid
            _societiesGrid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(750, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _societiesGrid.Columns.Add("SocietyId", "ID");
            _societiesGrid.Columns.Add("SocietyName", "Society Name");
            _societiesGrid.Columns.Add("Description", "Description");
            _societiesGrid.Columns.Add("MemberCount", "Members");

            this.Controls.Add(_societiesGrid);

            // Apply Button
            Button applyButton = new Button
            {
                Text = "Apply for Membership",
                Location = new System.Drawing.Point(20, 420),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            applyButton.Click += ApplyButton_Click;
            this.Controls.Add(applyButton);

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

        private void LoadSocieties()
        {
            try
            {
                _societiesGrid.Rows.Clear();
                List<Society> societies = _studentService.BrowseSocieties();

                foreach (var society in societies)
                {
                    int memberCount = new SocietyService().GetMemberCount(society.SocietyId);
                    _societiesGrid.Rows.Add(society.SocietyId, society.SocietyName, society.Description, memberCount);
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
                UIHelpers.ShowError("Please select a society");
                return;
            }

            try
            {
                int societyId = (int)_societiesGrid.SelectedRows[0].Cells[0].Value;
                string societyName = (string)_societiesGrid.SelectedRows[0].Cells[1].Value;

                if (UIHelpers.ShowConfirm($"Apply for membership in {societyName}?"))
                {
                    _studentService.ApplyForMembership(_studentId, societyId);
                    UIHelpers.ShowInfo("Membership application submitted successfully!");
                    LoadSocieties();
                }
            }
            catch (DuplicateResourceException)
            {
                UIHelpers.ShowError("You are already a member of this society");
            }
            catch (Exception ex)
            {
                UIHelpers.ShowError($"Failed to apply: {ex.Message}");
            }
        }
    }
}
