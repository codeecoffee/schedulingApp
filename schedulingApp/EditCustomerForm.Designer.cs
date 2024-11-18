namespace schedulingApp
{
    partial class EditCustomerForm
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
            labelCountry = new Label();
            bttnDeleteCustomer = new Button();
            CustomerCountryInput = new TextBox();
            CustomerGridLabel = new Label();
            labelZip = new Label();
            bttnExit = new Button();
            CustomerPostalCodeInput = new TextBox();
            bttnModify = new Button();
            labelCity = new Label();
            titleLabel = new Label();
            CustomerCityInput = new TextBox();
            customersDataGridView = new DataGridView();
            labelAddress2 = new Label();
            labelName = new Label();
            CustomerAddress2Input = new TextBox();
            CustomerNameInput = new TextBox();
            labelPhone = new Label();
            CustomerAddressInput = new TextBox();
            CustomerPhoneInput = new TextBox();
            labelAddress = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)customersDataGridView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.darkerBg;
            panel1.Controls.Add(labelCountry);
            panel1.Controls.Add(bttnDeleteCustomer);
            panel1.Controls.Add(CustomerCountryInput);
            panel1.Controls.Add(CustomerGridLabel);
            panel1.Controls.Add(labelZip);
            panel1.Controls.Add(bttnExit);
            panel1.Controls.Add(CustomerPostalCodeInput);
            panel1.Controls.Add(bttnModify);
            panel1.Controls.Add(labelCity);
            panel1.Controls.Add(titleLabel);
            panel1.Controls.Add(CustomerCityInput);
            panel1.Controls.Add(customersDataGridView);
            panel1.Controls.Add(labelAddress2);
            panel1.Controls.Add(labelName);
            panel1.Controls.Add(CustomerAddress2Input);
            panel1.Controls.Add(CustomerNameInput);
            panel1.Controls.Add(labelPhone);
            panel1.Controls.Add(CustomerAddressInput);
            panel1.Controls.Add(CustomerPhoneInput);
            panel1.Controls.Add(labelAddress);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1276, 789);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // labelCountry
            // 
            labelCountry.AutoSize = true;
            labelCountry.Location = new Point(293, 358);
            labelCountry.Name = "labelCountry";
            labelCountry.Size = new Size(50, 15);
            labelCountry.TabIndex = 35;
            labelCountry.Text = "Country";
            // 
            // bttnDeleteCustomer
            // 
            bttnDeleteCustomer.BackColor = Color.Red;
            bttnDeleteCustomer.Cursor = Cursors.Hand;
            bttnDeleteCustomer.FlatStyle = FlatStyle.Flat;
            bttnDeleteCustomer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnDeleteCustomer.ForeColor = SystemColors.ButtonHighlight;
            bttnDeleteCustomer.Location = new Point(317, 578);
            bttnDeleteCustomer.Name = "bttnDeleteCustomer";
            bttnDeleteCustomer.Size = new Size(271, 35);
            bttnDeleteCustomer.TabIndex = 23;
            bttnDeleteCustomer.Text = "Delete";
            bttnDeleteCustomer.UseVisualStyleBackColor = false;
            // 
            // CustomerCountryInput
            // 
            CustomerCountryInput.Location = new Point(293, 376);
            CustomerCountryInput.Name = "CustomerCountryInput";
            CustomerCountryInput.Size = new Size(184, 23);
            CustomerCountryInput.TabIndex = 34;
            // 
            // CustomerGridLabel
            // 
            CustomerGridLabel.AutoSize = true;
            CustomerGridLabel.Location = new Point(613, 128);
            CustomerGridLabel.Name = "CustomerGridLabel";
            CustomerGridLabel.Size = new Size(107, 15);
            CustomerGridLabel.TabIndex = 18;
            CustomerGridLabel.Text = "Current Customers";
            // 
            // labelZip
            // 
            labelZip.AutoSize = true;
            labelZip.Location = new Point(52, 420);
            labelZip.Name = "labelZip";
            labelZip.Size = new Size(53, 15);
            labelZip.TabIndex = 33;
            labelZip.Text = "Zip code";
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
            // CustomerPostalCodeInput
            // 
            CustomerPostalCodeInput.Location = new Point(52, 438);
            CustomerPostalCodeInput.Name = "CustomerPostalCodeInput";
            CustomerPostalCodeInput.Size = new Size(161, 23);
            CustomerPostalCodeInput.TabIndex = 32;
            // 
            // bttnModify
            // 
            bttnModify.BackColor = Color.LawnGreen;
            bttnModify.Cursor = Cursors.Hand;
            bttnModify.FlatStyle = FlatStyle.Flat;
            bttnModify.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnModify.Location = new Point(19, 578);
            bttnModify.Name = "bttnModify";
            bttnModify.Size = new Size(271, 35);
            bttnModify.TabIndex = 16;
            bttnModify.Text = "Modify";
            bttnModify.UseVisualStyleBackColor = false;
            // 
            // labelCity
            // 
            labelCity.AutoSize = true;
            labelCity.Location = new Point(52, 358);
            labelCity.Name = "labelCity";
            labelCity.Size = new Size(28, 15);
            labelCity.TabIndex = 31;
            labelCity.Text = "City";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(35, 41);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(267, 50);
            titleLabel.TabIndex = 12;
            titleLabel.Text = "Edit Customer";
            // 
            // CustomerCityInput
            // 
            CustomerCityInput.Location = new Point(52, 376);
            CustomerCityInput.Name = "CustomerCityInput";
            CustomerCityInput.Size = new Size(214, 23);
            CustomerCityInput.TabIndex = 30;
            // 
            // customersDataGridView
            // 
            customersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            customersDataGridView.Location = new Point(613, 145);
            customersDataGridView.Margin = new Padding(3, 2, 3, 2);
            customersDataGridView.Name = "customersDataGridView";
            customersDataGridView.RowHeadersWidth = 51;
            customersDataGridView.Size = new Size(572, 469);
            customersDataGridView.TabIndex = 0;
            // 
            // labelAddress2
            // 
            labelAddress2.AutoSize = true;
            labelAddress2.Location = new Point(52, 306);
            labelAddress2.Name = "labelAddress2";
            labelAddress2.Size = new Size(58, 15);
            labelAddress2.TabIndex = 29;
            labelAddress2.Text = "Address 2";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(52, 200);
            labelName.Name = "labelName";
            labelName.Size = new Size(39, 15);
            labelName.TabIndex = 23;
            labelName.Text = "Name";
            // 
            // CustomerAddress2Input
            // 
            CustomerAddress2Input.Location = new Point(52, 324);
            CustomerAddress2Input.Name = "CustomerAddress2Input";
            CustomerAddress2Input.Size = new Size(423, 23);
            CustomerAddress2Input.TabIndex = 28;
            // 
            // CustomerNameInput
            // 
            CustomerNameInput.Location = new Point(52, 218);
            CustomerNameInput.Name = "CustomerNameInput";
            CustomerNameInput.Size = new Size(423, 23);
            CustomerNameInput.TabIndex = 22;
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(249, 420);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(86, 15);
            labelPhone.TabIndex = 27;
            labelPhone.Text = "Phone number";
            // 
            // CustomerAddressInput
            // 
            CustomerAddressInput.Location = new Point(52, 270);
            CustomerAddressInput.Name = "CustomerAddressInput";
            CustomerAddressInput.Size = new Size(423, 23);
            CustomerAddressInput.TabIndex = 24;
            // 
            // CustomerPhoneInput
            // 
            CustomerPhoneInput.Location = new Point(249, 438);
            CustomerPhoneInput.Name = "CustomerPhoneInput";
            CustomerPhoneInput.Size = new Size(227, 23);
            CustomerPhoneInput.TabIndex = 26;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(52, 252);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(49, 15);
            labelAddress.TabIndex = 25;
            labelAddress.Text = "Address";
            // 
            // EditCustomerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1276, 789);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "EditCustomerForm";
            Text = "EditCustomerForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)customersDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView customersDataGridView;
        private Label titleLabel;
        private Label CustomerGridLabel;
        private Button bttnExit;
        private Button bttnModify;
        private Button bttnDeleteCustomer;
        private Label labelCountry;
        private TextBox CustomerCountryInput;
        private Label labelZip;
        private TextBox CustomerPostalCodeInput;
        private Label labelCity;
        private TextBox CustomerCityInput;
        private Label labelAddress2;
        private Label labelName;
        private TextBox CustomerAddress2Input;
        private TextBox CustomerNameInput;
        private Label labelPhone;
        private TextBox CustomerAddressInput;
        private TextBox CustomerPhoneInput;
        private Label labelAddress;
    }
}