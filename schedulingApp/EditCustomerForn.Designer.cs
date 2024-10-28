namespace schedulingApp
{
    partial class EditCustomerForn
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
            label1 = new Label();
            dataGridView1 = new DataGridView();
            bttnExit = new Button();
            bttnModify = new Button();
            titleLabel = new Label();
            labelPhone = new Label();
            CustomerPhoneInput = new TextBox();
            labelAddress = new Label();
            CustomerAddressInput = new TextBox();
            labelName = new Label();
            CustomerNameInput = new TextBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.darkerBg;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Controls.Add(bttnDeleteCustomer);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(bttnExit);
            panel1.Controls.Add(bttnModify);
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
            panel1.Size = new Size(1424, 961);
            panel1.TabIndex = 1;
            // 
            // bttnDeleteCustomer
            // 
            bttnDeleteCustomer.BackColor = Color.IndianRed;
            bttnDeleteCustomer.Cursor = Cursors.Hand;
            bttnDeleteCustomer.FlatStyle = FlatStyle.Flat;
            bttnDeleteCustomer.Font = new Font("Rubik", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnDeleteCustomer.ForeColor = SystemColors.ButtonFace;
            bttnDeleteCustomer.Location = new Point(776, 780);
            bttnDeleteCustomer.Name = "bttnDeleteCustomer";
            bttnDeleteCustomer.Size = new Size(271, 35);
            bttnDeleteCustomer.TabIndex = 16;
            bttnDeleteCustomer.Text = "Delete Customer";
            bttnDeleteCustomer.UseVisualStyleBackColor = false;
            bttnDeleteCustomer.Click += bttnDeleteCustomer_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.SpringGreen;
            label1.Location = new Point(687, 169);
            label1.Name = "label1";
            label1.Size = new Size(229, 32);
            label1.TabIndex = 15;
            label1.Text = "Current Customers";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(687, 204);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(688, 543);
            dataGridView1.TabIndex = 14;
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(1300, 884);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(75, 27);
            bttnExit.TabIndex = 13;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            bttnExit.Click += bttnExit_Click;
            // 
            // bttnModify
            // 
            bttnModify.BackColor = Color.LawnGreen;
            bttnModify.Cursor = Cursors.Hand;
            bttnModify.FlatStyle = FlatStyle.Flat;
            bttnModify.Font = new Font("Rubik", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnModify.ForeColor = SystemColors.ActiveCaptionText;
            bttnModify.Location = new Point(424, 780);
            bttnModify.Name = "bttnModify";
            bttnModify.Size = new Size(271, 35);
            bttnModify.TabIndex = 12;
            bttnModify.Text = "Modify";
            bttnModify.UseVisualStyleBackColor = false;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(101, 70);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(267, 50);
            titleLabel.TabIndex = 11;
            titleLabel.Text = "Edit Customer";
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(101, 410);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(86, 15);
            labelPhone.TabIndex = 9;
            labelPhone.Text = "Phone number";
            // 
            // CustomerPhoneInput
            // 
            CustomerPhoneInput.Location = new Point(101, 428);
            CustomerPhoneInput.Name = "CustomerPhoneInput";
            CustomerPhoneInput.Size = new Size(423, 23);
            CustomerPhoneInput.TabIndex = 8;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(101, 314);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(49, 15);
            labelAddress.TabIndex = 7;
            labelAddress.Text = "Address";
            // 
            // CustomerAddressInput
            // 
            CustomerAddressInput.Location = new Point(101, 332);
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
            // EditCustomerForn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1424, 961);
            Controls.Add(panel1);
            Name = "EditCustomerForn";
            Text = "EditCustomerForn";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button bttnExit;
        private Button bttnModify;
        private Label titleLabel;
        private Label labelPhone;
        private TextBox CustomerPhoneInput;
        private Label labelAddress;
        private TextBox CustomerAddressInput;
        private Label labelName;
        private TextBox CustomerNameInput;
        private DataGridView dataGridView1;
        private Label label1;
        private Button bttnDeleteCustomer;
    }
}