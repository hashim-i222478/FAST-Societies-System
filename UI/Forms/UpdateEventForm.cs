using System;
using System.Drawing;
using System.Windows.Forms;
using FASTSocietiesSystem.BLL;
using FASTSocietiesSystem.Models;
using FASTSocietiesSystem.UI.Helpers;

namespace FASTSocietiesSystem.UI.Forms
{
    public partial class UpdateEventForm : Form
    {
        private int _eventId;
        private int _societyId;
        private SocietyService _societyService;
        private TextBox _titleTxt;
        private TextBox _locationTxt;
        private DateTimePicker _datePicker;
        private NumericUpDown _capacityNum;

        public UpdateEventForm(int eventId, int societyId)
        {
            _eventId = eventId;
            _societyId = societyId;
            _societyService = new SocietyService();
            InitializeComponent();
            LoadEventData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Update Event";
            this.Size = new Size(500, 500);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ThemeManager.Surface;

            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 8, Padding = new Padding(40) };
            this.Controls.Add(layout);

            layout.Controls.Add(new Label { Text = "UPDATE EVENT", Font = ThemeManager.HeaderFont, ForeColor = ThemeManager.Accent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
            
            layout.Controls.Add(new Label { Text = "TITLE", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Dock = DockStyle.Bottom }, 0, 1);
            _titleTxt = new TextBox { Dock = DockStyle.Top, BackColor = ThemeManager.Background, ForeColor = ThemeManager.TextPrimary, Font = ThemeManager.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            layout.Controls.Add(_titleTxt, 0, 2);

            layout.Controls.Add(new Label { Text = "LOCATION", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Dock = DockStyle.Bottom, Margin = new Padding(0, 15, 0, 0) }, 0, 3);
            _locationTxt = new TextBox { Dock = DockStyle.Top, BackColor = ThemeManager.Background, ForeColor = ThemeManager.TextPrimary, Font = ThemeManager.BodyFont, BorderStyle = BorderStyle.FixedSingle };
            layout.Controls.Add(_locationTxt, 0, 4);

            layout.Controls.Add(new Label { Text = "DATE & TIME", ForeColor = ThemeManager.TextSecondary, Font = ThemeManager.SmallFont, Dock = DockStyle.Bottom, Margin = new Padding(0, 15, 0, 0) }, 0, 5);
            _datePicker = new DateTimePicker { Dock = DockStyle.Top, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", BackColor = ThemeManager.Background, CalendarForeColor = ThemeManager.TextPrimary };
            layout.Controls.Add(_datePicker, 0, 6);

            FlowLayoutPanel buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, Margin = new Padding(0, 30, 0, 0) };
            layout.Controls.Add(buttons, 0, 7);

            Button cancelBtn = new Button { Text = "CANCEL", Width = 100 };
            ThemeManager.StyleButton(cancelBtn, false);
            cancelBtn.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            buttons.Controls.Add(cancelBtn);

            Button saveBtn = new Button { Text = "SAVE", Width = 120 };
            ThemeManager.StyleButton(saveBtn);
            saveBtn.Click += SaveBtn_Click;
            buttons.Controls.Add(saveBtn);

            this.ResumeLayout(false);
        }

        private void LoadEventData()
        {
            try
            {
                var evt = new DAL.EventRepository().GetEventById(_eventId);
                if (evt != null)
                {
                    _titleTxt.Text = evt.EventTitle;
                    _locationTxt.Text = evt.Location;
                    _datePicker.Value = evt.EventDate > DateTime.Now ? evt.EventDate : DateTime.Now.AddDays(1);
                }
            }
            catch (Exception ex) { UIHelpers.ShowError(ex.Message); }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_titleTxt.Text)) { UIHelpers.ShowError("Title is required."); return; }
                
                _societyService.UpdateEvent(_eventId, _societyId, _titleTxt.Text, "", _datePicker.Value, _locationTxt.Text, 100);
                UIHelpers.ShowInfo("Event updated successfully.");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex) { UIHelpers.ShowError(ex.Message); }
        }
    }
}
