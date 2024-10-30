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
            labelPhone = new Label();
            CustomerPhoneInput = new TextBox();
            labelAddress = new Label();
            CustomerAddressInput = new TextBox();
            CustomerGridLabel = new Label();
            bttnExit = new Button();
            bttnModify = new Button();
            labelName = new Label();
            CustomerNameInput = new TextBox();
            titleLabel = new Label();
            customersDataGridView = new DataGridView();
            bttnDeleteCustomer = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)customersDataGridView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.darkerBg;
            panel1.Controls.Add(bttnDeleteCustomer);
            panel1.Controls.Add(labelPhone);
            panel1.Controls.Add(CustomerPhoneInput);
            panel1.Controls.Add(labelAddress);
            panel1.Controls.Add(CustomerAddressInput);
            panel1.Controls.Add(CustomerGridLabel);
            panel1.Controls.Add(bttnExit);
            panel1.Controls.Add(bttnModify);
            panel1.Controls.Add(labelName);
            panel1.Controls.Add(CustomerNameInput);
            panel1.Controls.Add(titleLabel);
            panel1.Controls.Add(customersDataGridView);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1404, 906);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(54, 565);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(105, 20);
            labelPhone.TabIndex = 22;
            labelPhone.Text = "Phone number";
            // 
            // CustomerPhoneInput
            // 
            CustomerPhoneInput.Location = new Point(54, 589);
            CustomerPhoneInput.Margin = new Padding(3, 4, 3, 4);
            CustomerPhoneInput.Name = "CustomerPhoneInput";
            CustomerPhoneInput.Size = new Size(483, 27);
            CustomerPhoneInput.TabIndex = 21;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(54, 437);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(62, 20);
            labelAddress.TabIndex = 20;
            labelAddress.Text = "Address";
            // 
            // CustomerAddressInput
            // 
            CustomerAddressInput.Location = new Point(54, 461);
            CustomerAddressInput.Margin = new Padding(3, 4, 3, 4);
            CustomerAddressInput.Name = "CustomerAddressInput";
            CustomerAddressInput.Size = new Size(483, 27);
            CustomerAddressInput.TabIndex = 19;
            // 
            // CustomerGridLabel
            // 
            CustomerGridLabel.AutoSize = true;
            CustomerGridLabel.Location = new Point(701, 170);
            CustomerGridLabel.Name = "CustomerGridLabel";
            CustomerGridLabel.Size = new Size(130, 20);
            CustomerGridLabel.TabIndex = 18;
            CustomerGridLabel.Text = "Current Customers";
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(1269, 837);
            bttnExit.Margin = new Padding(3, 4, 3, 4);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(86, 36);
            bttnExit.TabIndex = 17;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            // 
            // bttnModify
            // 
            bttnModify.BackColor = Color.LawnGreen;
            bttnModify.Cursor = Cursors.Hand;
            bttnModify.FlatStyle = FlatStyle.Flat;
            bttnModify.Font = new Font("Rubik", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnModify.Location = new Point(22, 771);
            bttnModify.Margin = new Padding(3, 4, 3, 4);
            bttnModify.Name = "bttnModify";
            bttnModify.Size = new Size(310, 47);
            bttnModify.TabIndex = 16;
            bttnModify.Text = "Modify";
            bttnModify.UseVisualStyleBackColor = false;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(54, 313);
            labelName.Name = "labelName";
            labelName.Size = new Size(49, 20);
            labelName.TabIndex = 15;
            labelName.Text = "Name";
            // 
            // CustomerNameInput
            // 
            CustomerNameInput.Location = new Point(54, 337);
            CustomerNameInput.Margin = new Padding(3, 4, 3, 4);
            CustomerNameInput.Name = "CustomerNameInput";
            CustomerNameInput.Size = new Size(483, 27);
            CustomerNameInput.TabIndex = 14;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(40, 55);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(337, 62);
            titleLabel.TabIndex = 12;
            titleLabel.Text = "Edit Customer";
            // 
            // customersDataGridView
            // 
            customersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            customersDataGridView.Location = new Point(701, 193);
            customersDataGridView.Name = "customersDataGridView";
            customersDataGridView.RowHeadersWidth = 51;
            customersDataGridView.Size = new Size(654, 625);
            customersDataGridView.TabIndex = 0;
            // 
            // bttnDeleteCustomer
            // 
            bttnDeleteCustomer.BackColor = Color.Red;
            bttnDeleteCustomer.Cursor = Cursors.Hand;
            bttnDeleteCustomer.FlatStyle = FlatStyle.Flat;
            bttnDeleteCustomer.Font = new Font("Rubik", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnDeleteCustomer.ForeColor = SystemColors.ButtonHighlight;
            bttnDeleteCustomer.Location = new Point(362, 771);
            bttnDeleteCustomer.Margin = new Padding(3, 4, 3, 4);
            bttnDeleteCustomer.Name = "bttnDeleteCustomer";
            bttnDeleteCustomer.Size = new Size(310, 47);
            bttnDeleteCustomer.TabIndex = 23;
            bttnDeleteCustomer.Text = "Delete";
            bttnDeleteCustomer.UseVisualStyleBackColor = false;
            // 
            // EditCustomerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1404, 906);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
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
        private Label labelName;
        private TextBox CustomerNameInput;
        private Label labelPhone;
        private TextBox CustomerPhoneInput;
        private Label labelAddress;
        private TextBox CustomerAddressInput;
        private Button bttnDeleteCustomer;
    }
}