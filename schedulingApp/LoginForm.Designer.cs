namespace schedulingApp
{
    partial class LoginForm
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
            bttnLogin = new Button();
            usernameInput = new TextBox();
            pictureBox1 = new PictureBox();
            labelUsername = new Label();
            bttnRegister = new Button();
            bttnExit = new Button();
            labelPassword = new Label();
            passwordInput = new TextBox();
            titleLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // bttnLogin
            // 
            bttnLogin.BackColor = Color.LawnGreen;
            bttnLogin.Cursor = Cursors.Hand;
            bttnLogin.FlatStyle = FlatStyle.Flat;
            bttnLogin.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnLogin.Location = new Point(108, 478);
            bttnLogin.Name = "bttnLogin";
            bttnLogin.Size = new Size(75, 27);
            bttnLogin.TabIndex = 0;
            bttnLogin.Text = "Login";
            bttnLogin.UseVisualStyleBackColor = false;
            bttnLogin.Click += bttnLogin_Click;
            // 
            // usernameInput
            // 
            usernameInput.Location = new Point(108, 349);
            usernameInput.Name = "usernameInput";
            usernameInput.Size = new Size(334, 23);
            usernameInput.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.ocean__1_;
            pictureBox1.Location = new Point(189, 50);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(172, 166);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // labelUsername
            // 
            labelUsername.AutoSize = true;
            labelUsername.Location = new Point(108, 331);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(60, 15);
            labelUsername.TabIndex = 3;
            labelUsername.Text = "Username";
            // 
            // bttnRegister
            // 
            bttnRegister.BackColor = Color.Yellow;
            bttnRegister.Cursor = Cursors.Hand;
            bttnRegister.FlatStyle = FlatStyle.Flat;
            bttnRegister.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnRegister.Location = new Point(367, 478);
            bttnRegister.Name = "bttnRegister";
            bttnRegister.Size = new Size(75, 27);
            bttnRegister.TabIndex = 4;
            bttnRegister.Text = "Register";
            bttnRegister.UseVisualStyleBackColor = false;
            bttnRegister.Click += bttnRegister_Click;
            // 
            // bttnExit
            // 
            bttnExit.BackColor = Color.Red;
            bttnExit.Cursor = Cursors.Hand;
            bttnExit.FlatStyle = FlatStyle.Flat;
            bttnExit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnExit.Location = new Point(452, 628);
            bttnExit.Name = "bttnExit";
            bttnExit.Size = new Size(75, 27);
            bttnExit.TabIndex = 5;
            bttnExit.Text = "Exit";
            bttnExit.UseVisualStyleBackColor = false;
            bttnExit.Click += bttnExit_Click;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(108, 392);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(57, 15);
            labelPassword.TabIndex = 7;
            labelPassword.Text = "Password";
            // 
            // passwordInput
            // 
            passwordInput.Location = new Point(108, 410);
            passwordInput.Name = "passwordInput";
            passwordInput.Size = new Size(334, 23);
            passwordInput.TabIndex = 6;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.Location = new Point(215, 245);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(120, 50);
            titleLabel.TabIndex = 8;
            titleLabel.Text = "Login";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.darkerBg;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(551, 681);
            Controls.Add(titleLabel);
            Controls.Add(labelPassword);
            Controls.Add(passwordInput);
            Controls.Add(bttnExit);
            Controls.Add(bttnRegister);
            Controls.Add(labelUsername);
            Controls.Add(pictureBox1);
            Controls.Add(usernameInput);
            Controls.Add(bttnLogin);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            Text = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bttnLogin;
        private TextBox usernameInput;
        private PictureBox pictureBox1;
        private Label labelUsername;
        private Button bttnRegister;
        private Button bttnExit;
        private Label labelPassword;
        private TextBox passwordInput;
        private Label titleLabel;
    }
}