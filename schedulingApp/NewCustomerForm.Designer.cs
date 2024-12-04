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
            bttnRegister = new Button();
            labelCountry = new Label();
            CustomerCountryInput = new TextBox();
            labelZip = new Label();
            CustomerPostalCodeInput = new TextBox();
            labelCity = new Label();
            CustomerCityInput = new TextBox();
            labelAddress2 = new Label();
            CustomerAddress2Input = new TextBox();
            bttnExit = new Button();
            titleLabel = new Label();
            labelPhone = new Label();
            CustomerPhoneInput = new TextBox();
            labelAddress = new Label();
            CustomerAddressInput = new TextBox();
            labelName = new Label();
            CustomerNameInput = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AllowDrop = true;
            panel1.BackgroundImage = Properties.Resources.darkerBg;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(bttnRegister);
            panel1.Controls.Add(labelCountry);
            panel1.Controls.Add(CustomerCountryInput);
            panel1.Controls.Add(labelZip);
            panel1.Controls.Add(CustomerPostalCodeInput);
            panel1.Controls.Add(labelCity);
            panel1.Controls.Add(CustomerCityInput);
            panel1.Controls.Add(labelAddress2);
            panel1.Controls.Add(CustomerAddress2Input);
            panel1.Controls.Add(bttnExit);
            panel1.Controls.Add(titleLabel);
            panel1.Controls.Add(labelPhone);
            panel1.Controls.Add(CustomerPhoneInput);
            panel1.Controls.Add(labelAddress);
            panel1.Controls.Add(CustomerAddressInput);
            panel1.Controls.Add(labelName);
            panel1.Controls.Add(CustomerNameInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1276, 789);
            panel1.TabIndex = 0;
            // 
            // bttnRegister
            // 
            bttnRegister.BackColor = Color.LawnGreen;
            bttnRegister.Cursor = Cursors.Hand;
            bttnRegister.FlatStyle = FlatStyle.Flat;
            bttnRegister.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnRegister.Location = new Point(178, 551);
            bttnRegister.Name = "bttnRegister";
            bttnRegister.Size = new Size(271, 35);
            bttnRegister.TabIndex = 22;
            bttnRegister.Text = "Register";
            bttnRegister.UseVisualStyleBackColor = false;
            bttnRegister.Click += bttnRegister_Click;
            // 
            // labelCountry
            // 
            labelCountry.AutoSize = true;
            labelCountry.Location = new Point(341, 378);
            labelCountry.Name = "labelCountry";
            labelCountry.Size = new Size(50, 15);
            labelCountry.TabIndex = 21;
            labelCountry.Text = "Country";
            // 
            // CustomerCountryInput
            // 
            CustomerCountryInput.Location = new Point(341, 396);
            CustomerCountryInput.Name = "CustomerCountryInput";
            CustomerCountryInput.Size = new Size(184, 23);
            CustomerCountryInput.TabIndex = 20;
            // 
            // labelZip
            // 
            labelZip.AutoSize = true;
            labelZip.Location = new Point(101, 440);
            labelZip.Name = "labelZip";
            labelZip.Size = new Size(53, 15);
            labelZip.TabIndex = 19;
            labelZip.Text = "Zip code";
            // 
            // CustomerPostalCodeInput
            // 
            CustomerPostalCodeInput.Location = new Point(101, 458);
            CustomerPostalCodeInput.Name = "CustomerPostalCodeInput";
            CustomerPostalCodeInput.Size = new Size(161, 23);
            CustomerPostalCodeInput.TabIndex = 18;
            // 
            // labelCity
            // 
            labelCity.AutoSize = true;
            labelCity.Location = new Point(101, 378);
            labelCity.Name = "labelCity";
            labelCity.Size = new Size(28, 15);
            labelCity.TabIndex = 17;
            labelCity.Text = "City";
            // 
            // CustomerCityInput
            // 
            CustomerCityInput.Location = new Point(101, 396);
            CustomerCityInput.Name = "CustomerCityInput";
            CustomerCityInput.Size = new Size(214, 23);
            CustomerCityInput.TabIndex = 16;
            // 
            // labelAddress2
            // 
            labelAddress2.AutoSize = true;
            labelAddress2.Location = new Point(101, 326);
            labelAddress2.Name = "labelAddress2";
            labelAddress2.Size = new Size(58, 15);
            labelAddress2.TabIndex = 15;
            labelAddress2.Text = "Address 2";
            // 
            // CustomerAddress2Input
            // 
            CustomerAddress2Input.Location = new Point(101, 344);
            CustomerAddress2Input.Name = "CustomerAddress2Input";
            CustomerAddress2Input.Size = new Size(423, 23);
            CustomerAddress2Input.TabIndex = 14;
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(1129, 664);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(75, 27);
            bttnExit.TabIndex = 13;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            bttnExit.Click += bttnExit_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(101, 70);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(278, 50);
            titleLabel.TabIndex = 11;
            titleLabel.Text = "New Customer";
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(297, 440);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(86, 15);
            labelPhone.TabIndex = 9;
            labelPhone.Text = "Phone number";
            // 
            // CustomerPhoneInput
            // 
            CustomerPhoneInput.Location = new Point(297, 458);
            CustomerPhoneInput.Name = "CustomerPhoneInput";
            CustomerPhoneInput.Size = new Size(227, 23);
            CustomerPhoneInput.TabIndex = 8;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(101, 272);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(49, 15);
            labelAddress.TabIndex = 7;
            labelAddress.Text = "Address";
            // 
            // CustomerAddressInput
            // 
            CustomerAddressInput.Location = new Point(101, 290);
            CustomerAddressInput.Name = "CustomerAddressInput";
            CustomerAddressInput.Size = new Size(423, 23);
            CustomerAddressInput.TabIndex = 6;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(101, 220);
            labelName.Name = "labelName";
            labelName.Size = new Size(39, 15);
            labelName.TabIndex = 5;
            labelName.Text = "Name";
            // 
            // CustomerNameInput
            // 
            CustomerNameInput.Location = new Point(101, 238);
            CustomerNameInput.Name = "CustomerNameInput";
            CustomerNameInput.Size = new Size(423, 23);
            CustomerNameInput.TabIndex = 4;
            // 
            // NewCustomerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1276, 789);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            ImeMode = ImeMode.Disable;
            Margin = new Padding(3, 2, 3, 2);
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
        private Label labelCity;
        private TextBox CustomerCityInput;
        private Label labelAddress2;
        private TextBox CustomerAddress2Input;
        private Label labelZip;
        private TextBox CustomerPostalCodeInput;
        private Label labelCountry;
        private TextBox CustomerCountryInput;
        private Button bttnRegister;
    }
}