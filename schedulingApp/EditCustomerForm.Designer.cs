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
            bttnDeleteCustomer = new Button();
            CustomerGridLabel = new Label();
            bttnExit = new Button();
            bttnModify = new Button();
            titleLabel = new Label();
            customersDataGridView = new DataGridView();
            labelCountry = new Label();
            CustomerCountryInput = new TextBox();
            labelZip = new Label();
            CustomerPostalCodeInput = new TextBox();
            labelCity = new Label();
            CustomerCityInput = new TextBox();
            labelAddress2 = new Label();
            CustomerAddress2Input = new TextBox();
            labelPhone = new Label();
            CustomerPhoneInput = new TextBox();
            labelAddress = new Label();
            CustomerAddressInput = new TextBox();
            labelName = new Label();
            CustomerNameInput = new TextBox();
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
            panel1.Margin = new Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1930, 1087);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // bttnDeleteCustomer
            // 
            bttnDeleteCustomer.BackColor = Color.Red;
            bttnDeleteCustomer.Cursor = Cursors.Hand;
            bttnDeleteCustomer.FlatStyle = FlatStyle.Flat;
            bttnDeleteCustomer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnDeleteCustomer.ForeColor = SystemColors.ButtonHighlight;
            bttnDeleteCustomer.Location = new Point(498, 925);
            bttnDeleteCustomer.Margin = new Padding(4, 5, 4, 5);
            bttnDeleteCustomer.Name = "bttnDeleteCustomer";
            bttnDeleteCustomer.Size = new Size(426, 56);
            bttnDeleteCustomer.TabIndex = 23;
            bttnDeleteCustomer.Text = "Delete";
            bttnDeleteCustomer.UseVisualStyleBackColor = false;
            // 
            // CustomerGridLabel
            // 
            CustomerGridLabel.AutoSize = true;
            CustomerGridLabel.Location = new Point(964, 204);
            CustomerGridLabel.Margin = new Padding(4, 0, 4, 0);
            CustomerGridLabel.Name = "CustomerGridLabel";
            CustomerGridLabel.Size = new Size(168, 24);
            CustomerGridLabel.TabIndex = 18;
            CustomerGridLabel.Text = "Current Customers";
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
            // bttnModify
            // 
            bttnModify.BackColor = Color.LawnGreen;
            bttnModify.Cursor = Cursors.Hand;
            bttnModify.FlatStyle = FlatStyle.Flat;
            bttnModify.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnModify.Location = new Point(30, 925);
            bttnModify.Margin = new Padding(4, 5, 4, 5);
            bttnModify.Name = "bttnModify";
            bttnModify.Size = new Size(426, 56);
            bttnModify.TabIndex = 16;
            bttnModify.Text = "Modify";
            bttnModify.UseVisualStyleBackColor = false;
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
            titleLabel.Size = new Size(415, 78);
            titleLabel.TabIndex = 12;
            titleLabel.Text = "Edit Customer";
            // 
            // customersDataGridView
            // 
            customersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            customersDataGridView.Location = new Point(964, 232);
            customersDataGridView.Margin = new Padding(4, 4, 4, 4);
            customersDataGridView.Name = "customersDataGridView";
            customersDataGridView.RowHeadersWidth = 51;
            customersDataGridView.Size = new Size(899, 750);
            customersDataGridView.TabIndex = 0;
            // 
            // labelCountry
            // 
            labelCountry.AutoSize = true;
            labelCountry.Location = new Point(460, 573);
            labelCountry.Margin = new Padding(4, 0, 4, 0);
            labelCountry.Name = "labelCountry";
            labelCountry.Size = new Size(74, 24);
            labelCountry.TabIndex = 35;
            labelCountry.Text = "Country";
            // 
            // CustomerCountryInput
            // 
            CustomerCountryInput.Location = new Point(460, 602);
            CustomerCountryInput.Margin = new Padding(4, 5, 4, 5);
            CustomerCountryInput.Name = "CustomerCountryInput";
            CustomerCountryInput.Size = new Size(287, 31);
            CustomerCountryInput.TabIndex = 34;
            // 
            // labelZip
            // 
            labelZip.AutoSize = true;
            labelZip.Location = new Point(82, 672);
            labelZip.Margin = new Padding(4, 0, 4, 0);
            labelZip.Name = "labelZip";
            labelZip.Size = new Size(81, 24);
            labelZip.TabIndex = 33;
            labelZip.Text = "Zip code";
            // 
            // CustomerPostalCodeInput
            // 
            CustomerPostalCodeInput.Location = new Point(82, 701);
            CustomerPostalCodeInput.Margin = new Padding(4, 5, 4, 5);
            CustomerPostalCodeInput.Name = "CustomerPostalCodeInput";
            CustomerPostalCodeInput.Size = new Size(250, 31);
            CustomerPostalCodeInput.TabIndex = 32;
            // 
            // labelCity
            // 
            labelCity.AutoSize = true;
            labelCity.Location = new Point(82, 573);
            labelCity.Margin = new Padding(4, 0, 4, 0);
            labelCity.Name = "labelCity";
            labelCity.Size = new Size(42, 24);
            labelCity.TabIndex = 31;
            labelCity.Text = "City";
            // 
            // CustomerCityInput
            // 
            CustomerCityInput.Location = new Point(82, 602);
            CustomerCityInput.Margin = new Padding(4, 5, 4, 5);
            CustomerCityInput.Name = "CustomerCityInput";
            CustomerCityInput.Size = new Size(334, 31);
            CustomerCityInput.TabIndex = 30;
            // 
            // labelAddress2
            // 
            labelAddress2.AutoSize = true;
            labelAddress2.Location = new Point(82, 489);
            labelAddress2.Margin = new Padding(4, 0, 4, 0);
            labelAddress2.Name = "labelAddress2";
            labelAddress2.Size = new Size(98, 24);
            labelAddress2.TabIndex = 29;
            labelAddress2.Text = "Address 2";
            // 
            // CustomerAddress2Input
            // 
            CustomerAddress2Input.Location = new Point(82, 518);
            CustomerAddress2Input.Margin = new Padding(4, 5, 4, 5);
            CustomerAddress2Input.Name = "CustomerAddress2Input";
            CustomerAddress2Input.Size = new Size(663, 31);
            CustomerAddress2Input.TabIndex = 28;
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(391, 672);
            labelPhone.Margin = new Padding(4, 0, 4, 0);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(123, 24);
            labelPhone.TabIndex = 27;
            labelPhone.Text = "Phone number";
            // 
            // CustomerPhoneInput
            // 
            CustomerPhoneInput.Location = new Point(391, 701);
            CustomerPhoneInput.Margin = new Padding(4, 5, 4, 5);
            CustomerPhoneInput.Name = "CustomerPhoneInput";
            CustomerPhoneInput.Size = new Size(354, 31);
            CustomerPhoneInput.TabIndex = 26;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(82, 403);
            labelAddress.Margin = new Padding(4, 0, 4, 0);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(82, 24);
            labelAddress.TabIndex = 25;
            labelAddress.Text = "Address";
            // 
            // CustomerAddressInput
            // 
            CustomerAddressInput.Location = new Point(82, 432);
            CustomerAddressInput.Margin = new Padding(4, 5, 4, 5);
            CustomerAddressInput.Name = "CustomerAddressInput";
            CustomerAddressInput.Size = new Size(663, 31);
            CustomerAddressInput.TabIndex = 24;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(82, 320);
            labelName.Margin = new Padding(4, 0, 4, 0);
            labelName.Name = "labelName";
            labelName.Size = new Size(57, 24);
            labelName.TabIndex = 23;
            labelName.Text = "Name";
            // 
            // CustomerNameInput
            // 
            CustomerNameInput.Location = new Point(82, 348);
            CustomerNameInput.Margin = new Padding(4, 5, 4, 5);
            CustomerNameInput.Name = "CustomerNameInput";
            CustomerNameInput.Size = new Size(663, 31);
            CustomerNameInput.TabIndex = 22;
            // 
            // EditCustomerForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1930, 1087);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 4, 4, 4);
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