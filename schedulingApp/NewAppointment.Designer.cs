namespace schedulingApp
{
    partial class NewAppointment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            labelCurrentAppointments = new Label();
            CustomerComboBox = new ComboBox();
            labelCustomer = new Label();
            EndDatePicker = new DateTimePicker();
            StartDatePicker = new DateTimePicker();
            DescriptionInput = new RichTextBox();
            labelEndDate = new Label();
            labelType = new Label();
            TypeInput = new TextBox();
            labelUrl = new Label();
            bttnExit = new Button();
            URLInput = new TextBox();
            bttnCreate = new Button();
            labelContact = new Label();
            titleLabel = new Label();
            ContactInput = new TextBox();
            AppointmentsDataGridView = new DataGridView();
            labelLocation = new Label();
            labelFieldTitle = new Label();
            LocationInput = new TextBox();
            TitleInput = new TextBox();
            labelStartDate = new Label();
            labelDescription = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AppointmentsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.darkerBg;
            panel1.Controls.Add(labelCurrentAppointments);
            panel1.Controls.Add(CustomerComboBox);
            panel1.Controls.Add(labelCustomer);
            panel1.Controls.Add(EndDatePicker);
            panel1.Controls.Add(StartDatePicker);
            panel1.Controls.Add(DescriptionInput);
            panel1.Controls.Add(labelEndDate);
            panel1.Controls.Add(labelType);
            panel1.Controls.Add(TypeInput);
            panel1.Controls.Add(labelUrl);
            panel1.Controls.Add(bttnExit);
            panel1.Controls.Add(URLInput);
            panel1.Controls.Add(bttnCreate);
            panel1.Controls.Add(labelContact);
            panel1.Controls.Add(titleLabel);
            panel1.Controls.Add(ContactInput);
            panel1.Controls.Add(AppointmentsDataGridView);
            panel1.Controls.Add(labelLocation);
            panel1.Controls.Add(labelFieldTitle);
            panel1.Controls.Add(LocationInput);
            panel1.Controls.Add(TitleInput);
            panel1.Controls.Add(labelStartDate);
            panel1.Controls.Add(labelDescription);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1958, 1144);
            panel1.TabIndex = 1;
            // 
            // labelCurrentAppointments
            // 
            labelCurrentAppointments.AutoSize = true;
            labelCurrentAppointments.Location = new Point(964, 208);
            labelCurrentAppointments.Margin = new Padding(4, 0, 4, 0);
            labelCurrentAppointments.Name = "labelCurrentAppointments";
            labelCurrentAppointments.Size = new Size(193, 24);
            labelCurrentAppointments.TabIndex = 44;
            labelCurrentAppointments.Text = "Current Appointments";
            // 
            // CustomerComboBox
            // 
            CustomerComboBox.FormattingEnabled = true;
            CustomerComboBox.Location = new Point(94, 950);
            CustomerComboBox.Name = "CustomerComboBox";
            CustomerComboBox.Size = new Size(679, 32);
            CustomerComboBox.TabIndex = 43;
            // 
            // labelCustomer
            // 
            labelCustomer.AutoSize = true;
            labelCustomer.Location = new Point(94, 905);
            labelCustomer.Margin = new Padding(4, 0, 4, 0);
            labelCustomer.Name = "labelCustomer";
            labelCustomer.Size = new Size(89, 24);
            labelCustomer.TabIndex = 42;
            labelCustomer.Text = "Customer";
            // 
            // EndDatePicker
            // 
            EndDatePicker.Location = new Point(450, 839);
            EndDatePicker.MaxDate = new DateTime(2029, 12, 31, 0, 0, 0, 0);
            EndDatePicker.MinDate = new DateTime(2024, 11, 4, 0, 0, 0, 0);
            EndDatePicker.Name = "EndDatePicker";
            EndDatePicker.Size = new Size(318, 31);
            EndDatePicker.TabIndex = 40;
            // 
            // StartDatePicker
            // 
            StartDatePicker.Location = new Point(92, 839);
            StartDatePicker.Name = "StartDatePicker";
            StartDatePicker.Size = new Size(318, 31);
            StartDatePicker.TabIndex = 39;
            // 
            // DescriptionInput
            // 
            DescriptionInput.Location = new Point(92, 336);
            DescriptionInput.Name = "DescriptionInput";
            DescriptionInput.Size = new Size(676, 144);
            DescriptionInput.TabIndex = 38;
            DescriptionInput.Text = "";
            // 
            // labelEndDate
            // 
            labelEndDate.AutoSize = true;
            labelEndDate.Location = new Point(461, 785);
            labelEndDate.Margin = new Padding(4, 0, 4, 0);
            labelEndDate.Name = "labelEndDate";
            labelEndDate.Size = new Size(86, 24);
            labelEndDate.TabIndex = 37;
            labelEndDate.Text = "End Date";
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.Location = new Point(470, 599);
            labelType.Margin = new Padding(4, 0, 4, 0);
            labelType.Name = "labelType";
            labelType.Size = new Size(51, 24);
            labelType.TabIndex = 35;
            labelType.Text = "Type";
            // 
            // TypeInput
            // 
            TypeInput.Location = new Point(470, 628);
            TypeInput.Margin = new Padding(4, 5, 4, 5);
            TypeInput.Name = "TypeInput";
            TypeInput.Size = new Size(305, 31);
            TypeInput.TabIndex = 34;
            // 
            // labelUrl
            // 
            labelUrl.AutoSize = true;
            labelUrl.Location = new Point(92, 698);
            labelUrl.Margin = new Padding(4, 0, 4, 0);
            labelUrl.Name = "labelUrl";
            labelUrl.Size = new Size(45, 24);
            labelUrl.TabIndex = 33;
            labelUrl.Text = "URL";
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(1745, 1004);
            bttnExit.Margin = new Padding(4, 5, 4, 5);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(118, 43);
            bttnExit.TabIndex = 17;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            // 
            // URLInput
            // 
            URLInput.Location = new Point(92, 727);
            URLInput.Margin = new Padding(4, 5, 4, 5);
            URLInput.Name = "URLInput";
            URLInput.Size = new Size(681, 31);
            URLInput.TabIndex = 32;
            // 
            // bttnCreate
            // 
            bttnCreate.BackColor = Color.LawnGreen;
            bttnCreate.Cursor = Cursors.Hand;
            bttnCreate.FlatStyle = FlatStyle.Flat;
            bttnCreate.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnCreate.Location = new Point(80, 1004);
            bttnCreate.Margin = new Padding(4, 5, 4, 5);
            bttnCreate.Name = "bttnCreate";
            bttnCreate.Size = new Size(695, 56);
            bttnCreate.TabIndex = 16;
            bttnCreate.Text = "Create";
            bttnCreate.UseVisualStyleBackColor = false;
            // 
            // labelContact
            // 
            labelContact.AutoSize = true;
            labelContact.Location = new Point(92, 599);
            labelContact.Margin = new Padding(4, 0, 4, 0);
            labelContact.Name = "labelContact";
            labelContact.Size = new Size(73, 24);
            labelContact.TabIndex = 31;
            labelContact.Text = "Contact";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(55, 66);
            titleLabel.Margin = new Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(503, 78);
            titleLabel.TabIndex = 12;
            titleLabel.Text = "New Appointment";
            // 
            // ContactInput
            // 
            ContactInput.Location = new Point(92, 628);
            ContactInput.Margin = new Padding(4, 5, 4, 5);
            ContactInput.Name = "ContactInput";
            ContactInput.Size = new Size(352, 31);
            ContactInput.TabIndex = 30;
            // 
            // AppointmentsDataGridView
            // 
            AppointmentsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AppointmentsDataGridView.Location = new Point(964, 232);
            AppointmentsDataGridView.Margin = new Padding(4);
            AppointmentsDataGridView.Name = "AppointmentsDataGridView";
            AppointmentsDataGridView.RowHeadersWidth = 51;
            AppointmentsDataGridView.Size = new Size(899, 750);
            AppointmentsDataGridView.TabIndex = 0;
            // 
            // labelLocation
            // 
            labelLocation.AutoSize = true;
            labelLocation.Location = new Point(92, 515);
            labelLocation.Margin = new Padding(4, 0, 4, 0);
            labelLocation.Name = "labelLocation";
            labelLocation.Size = new Size(77, 24);
            labelLocation.TabIndex = 29;
            labelLocation.Text = "Location";
            // 
            // labelFieldTitle
            // 
            labelFieldTitle.AutoSize = true;
            labelFieldTitle.Location = new Point(92, 208);
            labelFieldTitle.Margin = new Padding(4, 0, 4, 0);
            labelFieldTitle.Name = "labelFieldTitle";
            labelFieldTitle.Size = new Size(49, 24);
            labelFieldTitle.TabIndex = 23;
            labelFieldTitle.Text = "Title";
            // 
            // LocationInput
            // 
            LocationInput.Location = new Point(92, 544);
            LocationInput.Margin = new Padding(4, 5, 4, 5);
            LocationInput.Name = "LocationInput";
            LocationInput.Size = new Size(681, 31);
            LocationInput.TabIndex = 28;
            // 
            // TitleInput
            // 
            TitleInput.Location = new Point(92, 236);
            TitleInput.Margin = new Padding(4, 5, 4, 5);
            TitleInput.Name = "TitleInput";
            TitleInput.Size = new Size(681, 31);
            TitleInput.TabIndex = 22;
            // 
            // labelStartDate
            // 
            labelStartDate.AutoSize = true;
            labelStartDate.Location = new Point(92, 785);
            labelStartDate.Margin = new Padding(4, 0, 4, 0);
            labelStartDate.Name = "labelStartDate";
            labelStartDate.Size = new Size(101, 24);
            labelStartDate.TabIndex = 27;
            labelStartDate.Text = "Start Date";
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Location = new Point(92, 291);
            labelDescription.Margin = new Padding(4, 0, 4, 0);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(104, 24);
            labelDescription.TabIndex = 25;
            labelDescription.Text = "Description";
            // 
            // NewAppointment
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1958, 1144);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "NewAppointment";
            Text = "NewAppointment";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AppointmentsDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label labelType;
        private Button bttnDeleteCustomer;
        private TextBox TypeInput;
        private Label CustomerGridLabel;
        private Label labelUrl;
        private Button bttnExit;
        private TextBox URLInput;
        private Button bttnCreate;
        private Label labelContact;
        private Label titleLabel;
        private TextBox ContactInput;
        private DataGridView AppointmentsDataGridView;
        private Label labelLocation;
        private Label labelFieldTitle;
        private TextBox LocationInput;
        private TextBox TitleInput;
        private Label labelStartDate;
        private Label labelDescription;
        private Label labelEndDate;
        private RichTextBox DescriptionInput;
        private DateTimePicker EndDatePicker;
        private DateTimePicker StartDatePicker;
        private Label labelCustomer;
        private ComboBox CustomerComboBox;
        private Label labelCurrentAppointments;
    }
}