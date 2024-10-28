namespace schedulingApp
{
    partial class NewUserForm
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
            BgImg = new Panel();
            bttnExit = new Button();
            bttnRegister = new Button();
            titleLabel = new Label();
            PasswordConfirmation = new Label();
            CustomerPhoneInput = new TextBox();
            labelAddress = new Label();
            PasswordInput = new TextBox();
            labelUsername = new Label();
            userNameInput = new TextBox();
            BgImg.SuspendLayout();
            SuspendLayout();
            // 
            // BgImg
            // 
            BgImg.BackgroundImage = Properties.Resources.darkerBg;
            BgImg.BackgroundImageLayout = ImageLayout.Zoom;
            BgImg.Controls.Add(bttnExit);
            BgImg.Controls.Add(bttnRegister);
            BgImg.Controls.Add(titleLabel);
            BgImg.Controls.Add(PasswordConfirmation);
            BgImg.Controls.Add(CustomerPhoneInput);
            BgImg.Controls.Add(labelAddress);
            BgImg.Controls.Add(PasswordInput);
            BgImg.Controls.Add(labelUsername);
            BgImg.Controls.Add(userNameInput);
            BgImg.Dock = DockStyle.Fill;
            BgImg.Location = new Point(0, 0);
            BgImg.Margin = new Padding(3, 2, 3, 2);
            BgImg.Name = "BgImg";
            BgImg.Size = new Size(1424, 961);
            BgImg.TabIndex = 0;
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(1324, 884);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(75, 27);
            bttnExit.TabIndex = 22;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            bttnExit.Click += bttnExit_Click;
            // 
            // bttnRegister
            // 
            bttnRegister.BackColor = Color.LawnGreen;
            bttnRegister.Cursor = Cursors.Hand;
            bttnRegister.FlatStyle = FlatStyle.Flat;
            bttnRegister.Font = new Font("Rubik", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            bttnRegister.Location = new Point(240, 692);
            bttnRegister.Name = "bttnRegister";
            bttnRegister.Size = new Size(271, 35);
            bttnRegister.TabIndex = 21;
            bttnRegister.Text = "Register";
            bttnRegister.UseVisualStyleBackColor = false;
            bttnRegister.Click += bttnRegister_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(161, 170);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(189, 50);
            titleLabel.TabIndex = 20;
            titleLabel.Text = "New User";
            // 
            // PasswordConfirmation
            // 
            PasswordConfirmation.AutoSize = true;
            PasswordConfirmation.Location = new Point(161, 510);
            PasswordConfirmation.Name = "PasswordConfirmation";
            PasswordConfirmation.Size = new Size(104, 15);
            PasswordConfirmation.TabIndex = 19;
            PasswordConfirmation.Text = "Confirm Password";
            // 
            // CustomerPhoneInput
            // 
            CustomerPhoneInput.Location = new Point(161, 528);
            CustomerPhoneInput.Name = "CustomerPhoneInput";
            CustomerPhoneInput.PasswordChar = '*';
            CustomerPhoneInput.PlaceholderText = "Re-enter your password";
            CustomerPhoneInput.Size = new Size(423, 23);
            CustomerPhoneInput.TabIndex = 18;
            CustomerPhoneInput.UseSystemPasswordChar = true;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(161, 414);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(57, 15);
            labelAddress.TabIndex = 17;
            labelAddress.Text = "Password";
            // 
            // PasswordInput
            // 
            PasswordInput.Location = new Point(161, 432);
            PasswordInput.Name = "PasswordInput";
            PasswordInput.PasswordChar = '*';
            PasswordInput.PlaceholderText = "Enter your Password";
            PasswordInput.Size = new Size(423, 23);
            PasswordInput.TabIndex = 16;
            PasswordInput.UseSystemPasswordChar = true;
            // 
            // labelUsername
            // 
            labelUsername.AutoSize = true;
            labelUsername.Location = new Point(161, 320);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(63, 15);
            labelUsername.TabIndex = 15;
            labelUsername.Text = "User name";
            // 
            // userNameInput
            // 
            userNameInput.Location = new Point(161, 338);
            userNameInput.Name = "userNameInput";
            userNameInput.PlaceholderText = "Enter your username";
            userNameInput.Size = new Size(423, 23);
            userNameInput.TabIndex = 14;
            // 
            // NewUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1424, 961);
            Controls.Add(BgImg);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "NewUserForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NewUserForm";
            BgImg.ResumeLayout(false);
            BgImg.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel BgImg;
        private Button bttnExit;
        private Button bttnRegister;
        private Label titleLabel;
        private Label PasswordConfirmation;
        private TextBox CustomerPhoneInput;
        private Label labelAddress;
        private TextBox PasswordInput;
        private Label labelUsername;
        private TextBox userNameInput;
    }
}