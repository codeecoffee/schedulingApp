﻿namespace schedulingApp
{
    partial class EditAppointments
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
            bttnSaveChanges = new Button();
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
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)AppointmentsDataGridView).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // labelCurrentAppointments
            // 
            labelCurrentAppointments.AutoSize = true;
            labelCurrentAppointments.Location = new Point(613, 130);
            labelCurrentAppointments.Name = "labelCurrentAppointments";
            labelCurrentAppointments.Size = new Size(126, 15);
            labelCurrentAppointments.TabIndex = 44;
            labelCurrentAppointments.Text = "Current Appointments";
            // 
            // CustomerComboBox
            // 
            CustomerComboBox.FormattingEnabled = true;
            CustomerComboBox.Location = new Point(60, 594);
            CustomerComboBox.Margin = new Padding(2);
            CustomerComboBox.Name = "CustomerComboBox";
            CustomerComboBox.Size = new Size(434, 23);
            CustomerComboBox.TabIndex = 43;
            // 
            // labelCustomer
            // 
            labelCustomer.AutoSize = true;
            labelCustomer.Location = new Point(60, 566);
            labelCustomer.Name = "labelCustomer";
            labelCustomer.Size = new Size(59, 15);
            labelCustomer.TabIndex = 42;
            labelCustomer.Text = "Customer";
            // 
            // EndDatePicker
            // 
            EndDatePicker.Location = new Point(286, 524);
            EndDatePicker.Margin = new Padding(2);
            EndDatePicker.MaxDate = new DateTime(2025, 1, 31, 0, 0, 0, 0);
            EndDatePicker.MinDate = new DateTime(2024, 12, 1, 0, 0, 0, 0);
            EndDatePicker.Name = "EndDatePicker";
            EndDatePicker.Size = new Size(204, 23);
            EndDatePicker.TabIndex = 40;
            EndDatePicker.Value = new DateTime(2024, 12, 1, 0, 0, 0, 0);
            // 
            // StartDatePicker
            // 
            StartDatePicker.Location = new Point(59, 524);
            StartDatePicker.Margin = new Padding(2);
            StartDatePicker.MaxDate = new DateTime(2025, 1, 31, 0, 0, 0, 0);
            StartDatePicker.MinDate = new DateTime(2024, 1, 12, 0, 0, 0, 0);
            StartDatePicker.Name = "StartDatePicker";
            StartDatePicker.Size = new Size(204, 23);
            StartDatePicker.TabIndex = 39;
            StartDatePicker.Value = new DateTime(2024, 11, 29, 0, 0, 0, 0);
            // 
            // DescriptionInput
            // 
            DescriptionInput.Location = new Point(59, 210);
            DescriptionInput.Margin = new Padding(2);
            DescriptionInput.Name = "DescriptionInput";
            DescriptionInput.Size = new Size(432, 92);
            DescriptionInput.TabIndex = 38;
            DescriptionInput.Text = "";
            // 
            // labelEndDate
            // 
            labelEndDate.AutoSize = true;
            labelEndDate.Location = new Point(293, 491);
            labelEndDate.Name = "labelEndDate";
            labelEndDate.Size = new Size(54, 15);
            labelEndDate.TabIndex = 37;
            labelEndDate.Text = "End Date";
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.Location = new Point(299, 374);
            labelType.Name = "labelType";
            labelType.Size = new Size(31, 15);
            labelType.TabIndex = 35;
            labelType.Text = "Type";
            // 
            // TypeInput
            // 
            TypeInput.Location = new Point(299, 392);
            TypeInput.Name = "TypeInput";
            TypeInput.Size = new Size(196, 23);
            TypeInput.TabIndex = 34;
            // 
            // labelUrl
            // 
            labelUrl.AutoSize = true;
            labelUrl.Location = new Point(59, 436);
            labelUrl.Name = "labelUrl";
            labelUrl.Size = new Size(28, 15);
            labelUrl.TabIndex = 33;
            labelUrl.Text = "URL";
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(1110, 628);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(75, 27);
            bttnExit.TabIndex = 17;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            // 
            // URLInput
            // 
            URLInput.Location = new Point(59, 454);
            URLInput.Name = "URLInput";
            URLInput.Size = new Size(435, 23);
            URLInput.TabIndex = 32;
            // 
            // bttnSaveChanges
            // 
            bttnSaveChanges.BackColor = Color.LawnGreen;
            bttnSaveChanges.Cursor = Cursors.Hand;
            bttnSaveChanges.FlatStyle = FlatStyle.Flat;
            bttnSaveChanges.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnSaveChanges.Location = new Point(51, 628);
            bttnSaveChanges.Name = "bttnSaveChanges";
            bttnSaveChanges.Size = new Size(442, 35);
            bttnSaveChanges.TabIndex = 16;
            bttnSaveChanges.Text = "Save Changes ";
            bttnSaveChanges.UseVisualStyleBackColor = false;
            // 
            // labelContact
            // 
            labelContact.AutoSize = true;
            labelContact.Location = new Point(59, 374);
            labelContact.Name = "labelContact";
            labelContact.Size = new Size(49, 15);
            labelContact.TabIndex = 31;
            labelContact.Text = "Contact";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(35, 41);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(332, 50);
            titleLabel.TabIndex = 12;
            titleLabel.Text = "Edit Appointment";
            // 
            // ContactInput
            // 
            ContactInput.Location = new Point(59, 392);
            ContactInput.Name = "ContactInput";
            ContactInput.Size = new Size(225, 23);
            ContactInput.TabIndex = 30;
            // 
            // AppointmentsDataGridView
            // 
            AppointmentsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AppointmentsDataGridView.Location = new Point(613, 145);
            AppointmentsDataGridView.Margin = new Padding(3, 2, 3, 2);
            AppointmentsDataGridView.Name = "AppointmentsDataGridView";
            AppointmentsDataGridView.RowHeadersWidth = 51;
            AppointmentsDataGridView.Size = new Size(572, 469);
            AppointmentsDataGridView.TabIndex = 0;
            // 
            // labelLocation
            // 
            labelLocation.AutoSize = true;
            labelLocation.Location = new Point(59, 322);
            labelLocation.Name = "labelLocation";
            labelLocation.Size = new Size(53, 15);
            labelLocation.TabIndex = 29;
            labelLocation.Text = "Location";
            // 
            // labelFieldTitle
            // 
            labelFieldTitle.AutoSize = true;
            labelFieldTitle.Location = new Point(59, 130);
            labelFieldTitle.Name = "labelFieldTitle";
            labelFieldTitle.Size = new Size(29, 15);
            labelFieldTitle.TabIndex = 23;
            labelFieldTitle.Text = "Title";
            // 
            // LocationInput
            // 
            LocationInput.Location = new Point(59, 340);
            LocationInput.Name = "LocationInput";
            LocationInput.Size = new Size(435, 23);
            LocationInput.TabIndex = 28;
            // 
            // TitleInput
            // 
            TitleInput.Location = new Point(59, 148);
            TitleInput.Name = "TitleInput";
            TitleInput.Size = new Size(435, 23);
            TitleInput.TabIndex = 22;
            // 
            // labelStartDate
            // 
            labelStartDate.AutoSize = true;
            labelStartDate.Location = new Point(59, 491);
            labelStartDate.Name = "labelStartDate";
            labelStartDate.Size = new Size(58, 15);
            labelStartDate.TabIndex = 27;
            labelStartDate.Text = "Start Date";
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Location = new Point(59, 182);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(67, 15);
            labelDescription.TabIndex = 25;
            labelDescription.Text = "Description";
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
            panel1.Controls.Add(bttnSaveChanges);
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
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1226, 788);
            panel1.TabIndex = 2;
            // 
            // EditAppointments
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1226, 788);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            Name = "EditAppointments";
            Text = "EditAppointments";
            ((System.ComponentModel.ISupportInitialize)AppointmentsDataGridView).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label labelCurrentAppointments;
        private ComboBox CustomerComboBox;
        private Label labelCustomer;
        private DateTimePicker EndDatePicker;
        private DateTimePicker StartDatePicker;
        private RichTextBox DescriptionInput;
        private Label labelEndDate;
        private Label labelType;
        private TextBox TypeInput;
        private Label labelUrl;
        private Button bttnExit;
        private TextBox URLInput;
        private Button bttnSaveChanges;
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
        private Panel panel1;
    }
}