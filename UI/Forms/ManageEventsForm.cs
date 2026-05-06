using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class ManageEventsForm : Form
    {
        private int _headId;
        private SocietyService _societyService;
        private DataGridView _eventsGrid;
        private ComboBox _societySelector;
        private List<Society> _mySocieties;

        public ManageEventsForm(int headId)
        {
            _headId = headId;
            _societyService = new SocietyService();
            InitializeComponent();
            LoadSocieties();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Manage Events - FAST Societies";
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
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
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

            Label titleLabel = new Label { Text = "Manage Your Events", Font = ThemeManager.TitleFont, ForeColor = ThemeManager.TextPrimary, Location = new Point(0, 0), Size = new Size(400, 40) };
            header.Controls.Add(titleLabel);

            Label selectLbl = new Label { Text = "SOCIETY:", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Location = new Point(0, 50), AutoSize = true };
            header.Controls.Add(selectLbl);

            _societySelector = new ComboBox { Location = new Point(0, 70), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = ThemeManager.Surface, ForeColor = ThemeManager.TextPrimary, Font = ThemeManager.BodyFont, FlatStyle = FlatStyle.Flat };
            _societySelector.SelectedIndexChanged += (s, e) => LoadEvents();
            header.Controls.Add(_societySelector);

            // Grid
            _eventsGrid = new DataGridView { Dock = DockStyle.Fill };
            ThemeManager.StyleGrid(_eventsGrid);
            _eventsGrid.Columns.Add("Title", "EVENT TITLE");
            _eventsGrid.Columns.Add("Date", "DATE");
            _eventsGrid.Columns.Add("Location", "LOCATION");
            _eventsGrid.Columns.Add("Status", "STATUS");
            _eventsGrid.Columns.Add("EventId", "ID");
            _eventsGrid.Columns["EventId"].Visible = false;

            mainGrid.Controls.Add(_eventsGrid, 0, 1);

            // Footer
            Panel footer = new Panel { Dock = DockStyle.Fill };
            mainGrid.Controls.Add(footer, 0, 2);

            Button createBtn = new Button { Text = "CREATE NEW", Width = 150, Dock = DockStyle.Left };
            ThemeManager.StyleButton(createBtn);
            createBtn.Click += (s, e) => {
                CreateEventForm form = new CreateEventForm(_headId);
                form.ShowDialog();
                LoadEvents();
            };
            footer.Controls.Add(createBtn);

            Button updateBtn = new Button { Text = "UPDATE DETAILS", Width = 180, Dock = DockStyle.Left, Margin = new Padding(15, 0, 0, 0) };
            ThemeManager.StyleButton(updateBtn, false);
            updateBtn.Click += UpdateEvent_Click;
            footer.Controls.Add(updateBtn);

            Button cancelBtn = new Button { Text = "CANCEL EVENT", Width = 180, Dock = DockStyle.Left, Margin = new Padding(15, 0, 0, 0) };
            ThemeManager.StyleButton(cancelBtn, false);
            cancelBtn.ForeColor = Color.FromArgb(233, 69, 96);
            cancelBtn.Click += CancelEvent_Click;
            footer.Controls.Add(cancelBtn);

            Button refreshBtn = new Button { Text = "REFRESH", Width = 120, Dock = DockStyle.Right };
            ThemeManager.StyleButton(refreshBtn, false);
            refreshBtn.Click += (s, e) => LoadEvents();
            footer.Controls.Add(refreshBtn);

            this.ResumeLayout(false);
        }

        private void LoadSocieties()
        {
            try
            {
                _mySocieties = _societyService.GetMySocieties(_headId);
                _societySelector.Items.Clear();
                foreach (var s in _mySocieties) _societySelector.Items.Add(s.SocietyName);
                if (_societySelector.Items.Count > 0) _societySelector.SelectedIndex = 0;
            }
            catch (Exception ex) { UIHelpers.ShowError($"Error: {ex.Message}"); }
        }

        private void LoadEvents()
        {
            if (_societySelector.SelectedIndex == -1) return;
            try
            {
                _eventsGrid.Rows.Clear();
                int societyId = _mySocieties[_societySelector.SelectedIndex].SocietyId;
                var events = _societyService.GetSocietyEvents(societyId);
                foreach (var e in events)
                {
                    _eventsGrid.Rows.Add(e.EventTitle, e.EventDate.ToString("dd MMM yyyy HH:mm"), e.Location, e.Status.ToUpper(), e.EventId);
                }
            }
            catch (Exception ex) { UIHelpers.ShowError($"Error: {ex.Message}"); }
        }

        private void UpdateEvent_Click(object sender, EventArgs e)
        {
            if (_eventsGrid.SelectedRows.Count == 0) { UIHelpers.ShowError("Select an event."); return; }
            int eventId = (int)_eventsGrid.SelectedRows[0].Cells[4].Value;
            int societyId = _mySocieties[_societySelector.SelectedIndex].SocietyId;
            
            // For simplicity, we could open a variation of CreateEventForm or a dedicated update dialog.
            // I'll use simple input boxes for core fields to keep it fast, or I can build a dedicated form.
            // Let's create a small UpdateEventForm.
            UpdateEventForm form = new UpdateEventForm(eventId, societyId);
            if (form.ShowDialog() == DialogResult.OK) LoadEvents();
        }

        private void CancelEvent_Click(object sender, EventArgs e)
        {
            if (_eventsGrid.SelectedRows.Count == 0) { UIHelpers.ShowError("Select an event."); return; }
            int eventId = (int)_eventsGrid.SelectedRows[0].Cells[4].Value;
            string title = (string)_eventsGrid.SelectedRows[0].Cells[0].Value;
            int societyId = _mySocieties[_societySelector.SelectedIndex].SocietyId;

            if (UIHelpers.ShowConfirm($"Cancel '{title}'? This will notify all registered students."))
            {
                try
                {
                    if (_societyService.CancelEvent(eventId, societyId))
                    {
                        UIHelpers.ShowInfo("Event cancelled.");
                        LoadEvents();
                    }
                }
                catch (Exception ex) { UIHelpers.ShowError($"Error: {ex.Message}"); }
            }
        }
    }
}
