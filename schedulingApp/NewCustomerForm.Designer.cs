namespace schedulingApp
{
    partial class NewCustomerForm
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
            bttnExit = new Button();
            bttnRegister = new Button();
            titleLabel = new Label();
            labelPhone = new Label();
            CustomerPhoneInput = new TextBox();
            labelAddress = new Label();
            CustomerAddressInput = new TextBox();
            labelName = new Label();
            CustomerNameInput = new TextBox();
            labelAddress2 = new Label();
            CustomerAddress2Input = new TextBox();
            labelCity = new Label();
            CustomerCityInput = new TextBox();
            labelZip = new Label();
            CustomerPostalCodeInput = new TextBox();
            labelCountry = new Label();
            CustomerCountryInput = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.darkerBg;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Controls.Add(labelCountry);
            panel1.Controls.Add(CustomerCountryInput);
            panel1.Controls.Add(labelZip);
            panel1.Controls.Add(CustomerPostalCodeInput);
            panel1.Controls.Add(labelCity);
            panel1.Controls.Add(CustomerCityInput);
            panel1.Controls.Add(labelAddress2);
            panel1.Controls.Add(CustomerAddress2Input);
            panel1.Controls.Add(bttnExit);
            panel1.Controls.Add(bttnRegister);
            panel1.Controls.Add(titleLabel);
            panel1.Controls.Add(labelPhone);
            panel1.Controls.Add(CustomerPhoneInput);
            panel1.Controls.Add(labelAddress);
            panel1.Controls.Add(CustomerAddressInput);
            panel1.Controls.Add(labelName);
            panel1.Controls.Add(CustomerNameInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1955, 1144);
            panel1.TabIndex = 0;
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(1774, 1062);
            bttnExit.Margin = new Padding(4, 5, 4, 5);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(118, 43);
            bttnExit.TabIndex = 13;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            bttnExit.Click += bttnExit_Click;
            // 
            // bttnRegister
            // 
            bttnRegister.BackColor = Color.LawnGreen;
            bttnRegister.Cursor = Cursors.Hand;
            bttnRegister.FlatStyle = FlatStyle.Flat;
            bttnRegister.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnRegister.Location = new Point(397, 896);
            bttnRegister.Margin = new Padding(4, 5, 4, 5);
            bttnRegister.Name = "bttnRegister";
            bttnRegister.Size = new Size(426, 56);
            bttnRegister.TabIndex = 12;
            bttnRegister.Text = "Register";
            bttnRegister.UseVisualStyleBackColor = false;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(158, 112);
            titleLabel.Margin = new Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(420, 78);
            titleLabel.TabIndex = 11;
            titleLabel.Text = "New Customer";
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(467, 704);
            labelPhone.Margin = new Padding(4, 0, 4, 0);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(123, 24);
            labelPhone.TabIndex = 9;
            labelPhone.Text = "Phone number";
            // 
            // CustomerPhoneInput
            // 
            CustomerPhoneInput.Location = new Point(467, 733);
            CustomerPhoneInput.Margin = new Padding(4, 5, 4, 5);
            CustomerPhoneInput.Name = "CustomerPhoneInput";
            CustomerPhoneInput.Size = new Size(354, 31);
            CustomerPhoneInput.TabIndex = 8;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(158, 435);
            labelAddress.Margin = new Padding(4, 0, 4, 0);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(82, 24);
            labelAddress.TabIndex = 7;
            labelAddress.Text = "Address";
            // 
            // CustomerAddressInput
            // 
            CustomerAddressInput.Location = new Point(158, 464);
            CustomerAddressInput.Margin = new Padding(4, 5, 4, 5);
            CustomerAddressInput.Name = "CustomerAddressInput";
            CustomerAddressInput.Size = new Size(663, 31);
            CustomerAddressInput.TabIndex = 6;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(158, 352);
            labelName.Margin = new Padding(4, 0, 4, 0);
            labelName.Name = "labelName";
            labelName.Size = new Size(57, 24);
            labelName.TabIndex = 5;
            labelName.Text = "Name";
            // 
            // CustomerNameInput
            // 
            CustomerNameInput.Location = new Point(158, 380);
            CustomerNameInput.Margin = new Padding(4, 5, 4, 5);
            CustomerNameInput.Name = "CustomerNameInput";
            CustomerNameInput.Size = new Size(663, 31);
            CustomerNameInput.TabIndex = 4;
            // 
            // labelAddress2
            // 
            labelAddress2.AutoSize = true;
            labelAddress2.Location = new Point(158, 521);
            labelAddress2.Margin = new Padding(4, 0, 4, 0);
            labelAddress2.Name = "labelAddress2";
            labelAddress2.Size = new Size(98, 24);
            labelAddress2.TabIndex = 15;
            labelAddress2.Text = "Address 2";
            // 
            // CustomerAddress2Input
            // 
            CustomerAddress2Input.Location = new Point(158, 550);
            CustomerAddress2Input.Margin = new Padding(4, 5, 4, 5);
            CustomerAddress2Input.Name = "CustomerAddress2Input";
            CustomerAddress2Input.Size = new Size(663, 31);
            CustomerAddress2Input.TabIndex = 14;
            // 
            // labelCity
            // 
            labelCity.AutoSize = true;
            labelCity.Location = new Point(158, 605);
            labelCity.Margin = new Padding(4, 0, 4, 0);
            labelCity.Name = "labelCity";
            labelCity.Size = new Size(42, 24);
            labelCity.TabIndex = 17;
            labelCity.Text = "City";
            // 
            // CustomerCityInput
            // 
            CustomerCityInput.Location = new Point(158, 634);
            CustomerCityInput.Margin = new Padding(4, 5, 4, 5);
            CustomerCityInput.Name = "CustomerCityInput";
            CustomerCityInput.Size = new Size(334, 31);
            CustomerCityInput.TabIndex = 16;
            // 
            // labelZip
            // 
            labelZip.AutoSize = true;
            labelZip.Location = new Point(158, 704);
            labelZip.Margin = new Padding(4, 0, 4, 0);
            labelZip.Name = "labelZip";
            labelZip.Size = new Size(81, 24);
            labelZip.TabIndex = 19;
            labelZip.Text = "Zip code";
            // 
            // CustomerPostalCodeInput
            // 
            CustomerPostalCodeInput.Location = new Point(158, 733);
            CustomerPostalCodeInput.Margin = new Padding(4, 5, 4, 5);
            CustomerPostalCodeInput.Name = "CustomerPostalCodeInput";
            CustomerPostalCodeInput.Size = new Size(250, 31);
            CustomerPostalCodeInput.TabIndex = 18;
            // 
            // labelCountry
            // 
            labelCountry.AutoSize = true;
            labelCountry.Location = new Point(536, 605);
            labelCountry.Margin = new Padding(4, 0, 4, 0);
            labelCountry.Name = "labelCountry";
            labelCountry.Size = new Size(74, 24);
            labelCountry.TabIndex = 21;
            labelCountry.Text = "Country";
            // 
            // CustomerCountryInput
            // 
            CustomerCountryInput.Location = new Point(536, 634);
            CustomerCountryInput.Margin = new Padding(4, 5, 4, 5);
            CustomerCountryInput.Name = "CustomerCountryInput";
            CustomerCountryInput.Size = new Size(287, 31);
            CustomerCountryInput.TabIndex = 20;
            // 
            // NewCustomerForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1955, 1144);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            ImeMode = ImeMode.Disable;
            Margin = new Padding(4, 4, 4, 4);
            Name = "NewCustomerForm";
            Text = "NewCustomerForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label labelPhone;
        private TextBox CustomerPhoneInput;
        private Label labelAddress;
        private TextBox CustomerAddressInput;
        private Label labelName;
        private TextBox CustomerNameInput;
        private Label titleLabel;
        private Button bttnExit;
        private Button bttnRegister;
        private Label labelCity;
        private TextBox CustomerCityInput;
        private Label labelAddress2;
        private TextBox CustomerAddress2Input;
        private Label labelZip;
        private TextBox CustomerPostalCodeInput;
        private Label labelCountry;
        private TextBox CustomerCountryInput;
    }
}