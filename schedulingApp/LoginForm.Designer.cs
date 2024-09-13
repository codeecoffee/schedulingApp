namespace schedulingApp
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            LoginContainer = new FlowLayoutPanel();
            LoginLabel = new Label();
            UserNameInput = new TextBox();
            PasswordInput = new TextBox();
            LoginContainer.SuspendLayout();
            SuspendLayout();
            // 
            // LoginContainer
            // 
            resources.ApplyResources(LoginContainer, "LoginContainer");
            LoginContainer.Controls.Add(LoginLabel);
            LoginContainer.Controls.Add(UserNameInput);
            LoginContainer.Controls.Add(PasswordInput);
            LoginContainer.Name = "LoginContainer";
            // 
            // LoginLabel
            // 
            resources.ApplyResources(LoginLabel, "LoginLabel");
            LoginLabel.ForeColor = Color.MediumSlateBlue;
            LoginLabel.Name = "LoginLabel";
            // 
            // UserNameInput
            // 
            resources.ApplyResources(UserNameInput, "UserNameInput");
            UserNameInput.AutoCompleteMode = AutoCompleteMode.Suggest;
            UserNameInput.BackColor = Color.Gainsboro;
            UserNameInput.Name = "UserNameInput";
            // 
            // PasswordInput
            // 
            resources.ApplyResources(PasswordInput, "PasswordInput");
            PasswordInput.AutoCompleteMode = AutoCompleteMode.Suggest;
            PasswordInput.BackColor = Color.Gainsboro;
            PasswordInput.Name = "PasswordInput";
            // 
            // LoginForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(LoginContainer);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            SizeGripStyle = SizeGripStyle.Hide;
            LoginContainer.ResumeLayout(false);
            LoginContainer.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel LoginContainer;
        private Label LoginLabel;
        private TextBox UserNameInput;
        private TextBox PasswordInput;
    }
}
