namespace schedulingApp
{
    partial class MainForm
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
            label1 = new Label();
            appointmentList = new ListBox();
            groupBox1 = new GroupBox();
            bttnCancelAppt = new Button();
            bttnNewAppt = new Button();
            groupBox2 = new GroupBox();
            bttnEditCustomer = new Button();
            bttnNewCustomer = new Button();
            calendarPanel = new TableLayoutPanel();
            bttnPreviosMonth = new Button();
            bttnNextMonth = new Button();
            labelMonth = new Label();
            bttnLogout = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Comic Sans MS", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(23, 27);
            label1.Name = "label1";
            label1.Size = new Size(337, 67);
            label1.TabIndex = 0;
            label1.Text = "Appointments";
            // 
            // appointmentList
            // 
            appointmentList.FormattingEnabled = true;
            appointmentList.ItemHeight = 15;
            appointmentList.Location = new Point(23, 141);
            appointmentList.Name = "appointmentList";
            appointmentList.Size = new Size(539, 349);
            appointmentList.TabIndex = 1;
            appointmentList.SelectedIndexChanged += appointmentList_SelectedIndexChanged;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Snow;
            groupBox1.Controls.Add(bttnCancelAppt);
            groupBox1.Controls.Add(bttnNewAppt);
            groupBox1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(23, 517);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(233, 162);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Appointments";
            // 
            // bttnCancelAppt
            // 
            bttnCancelAppt.BackColor = Color.Red;
            bttnCancelAppt.Cursor = Cursors.Hand;
            bttnCancelAppt.FlatStyle = FlatStyle.Flat;
            bttnCancelAppt.Font = new Font("Noto Sans", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnCancelAppt.ForeColor = Color.White;
            bttnCancelAppt.Location = new Point(45, 106);
            bttnCancelAppt.Name = "bttnCancelAppt";
            bttnCancelAppt.Size = new Size(140, 30);
            bttnCancelAppt.TabIndex = 1;
            bttnCancelAppt.Text = "Cancel Appt";
            bttnCancelAppt.UseVisualStyleBackColor = false;
            // 
            // bttnNewAppt
            // 
            bttnNewAppt.BackColor = Color.GreenYellow;
            bttnNewAppt.Cursor = Cursors.Hand;
            bttnNewAppt.FlatStyle = FlatStyle.Flat;
            bttnNewAppt.Font = new Font("Noto Sans", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnNewAppt.ForeColor = Color.Black;
            bttnNewAppt.Location = new Point(45, 48);
            bttnNewAppt.Name = "bttnNewAppt";
            bttnNewAppt.Size = new Size(140, 30);
            bttnNewAppt.TabIndex = 0;
            bttnNewAppt.Text = "New Appt";
            bttnNewAppt.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.Snow;
            groupBox2.Controls.Add(bttnEditCustomer);
            groupBox2.Controls.Add(bttnNewCustomer);
            groupBox2.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(289, 517);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(273, 162);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Customers";
            // 
            // bttnEditCustomer
            // 
            bttnEditCustomer.BackColor = Color.Yellow;
            bttnEditCustomer.Cursor = Cursors.Hand;
            bttnEditCustomer.FlatStyle = FlatStyle.Flat;
            bttnEditCustomer.Font = new Font("Noto Sans", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnEditCustomer.Location = new Point(66, 96);
            bttnEditCustomer.Name = "bttnEditCustomer";
            bttnEditCustomer.Size = new Size(140, 30);
            bttnEditCustomer.TabIndex = 3;
            bttnEditCustomer.Text = "Edit Customer";
            bttnEditCustomer.UseVisualStyleBackColor = false;
            // 
            // bttnNewCustomer
            // 
            bttnNewCustomer.BackColor = Color.GreenYellow;
            bttnNewCustomer.Cursor = Cursors.Hand;
            bttnNewCustomer.FlatStyle = FlatStyle.Flat;
            bttnNewCustomer.Font = new Font("Noto Sans", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnNewCustomer.Location = new Point(66, 32);
            bttnNewCustomer.Name = "bttnNewCustomer";
            bttnNewCustomer.Size = new Size(140, 30);
            bttnNewCustomer.TabIndex = 2;
            bttnNewCustomer.Text = "New Customer";
            bttnNewCustomer.UseVisualStyleBackColor = false;
            bttnNewCustomer.Click += bttnNewCustomer_Click;
            // 
            // calendarPanel
            // 
            calendarPanel.ColumnCount = 7;
            calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857113F));
            calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            calendarPanel.Location = new Point(636, 151);
            calendarPanel.Name = "calendarPanel";
            calendarPanel.RowCount = 6;
            calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            calendarPanel.Size = new Size(583, 475);
            calendarPanel.TabIndex = 4;
            // 
            // bttnPreviosMonth
            // 
            bttnPreviosMonth.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnPreviosMonth.Location = new Point(636, 116);
            bttnPreviosMonth.Name = "bttnPreviosMonth";
            bttnPreviosMonth.Size = new Size(75, 29);
            bttnPreviosMonth.TabIndex = 5;
            bttnPreviosMonth.Text = "Prev";
            bttnPreviosMonth.UseVisualStyleBackColor = true;
            bttnPreviosMonth.Click += bttnPreviosMonth_Click;
            // 
            // bttnNextMonth
            // 
            bttnNextMonth.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnNextMonth.Location = new Point(1144, 116);
            bttnNextMonth.Name = "bttnNextMonth";
            bttnNextMonth.Size = new Size(75, 29);
            bttnNextMonth.TabIndex = 6;
            bttnNextMonth.Text = "Next";
            bttnNextMonth.UseVisualStyleBackColor = true;
            bttnNextMonth.Click += bttnNextMonth_Click;
            // 
            // labelMonth
            // 
            labelMonth.AutoSize = true;
            labelMonth.BackColor = Color.Transparent;
            labelMonth.Font = new Font("Comic Sans MS", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelMonth.ForeColor = Color.WhiteSmoke;
            labelMonth.Location = new Point(857, 106);
            labelMonth.Name = "labelMonth";
            labelMonth.Size = new Size(98, 38);
            labelMonth.TabIndex = 7;
            labelMonth.Text = "Month";
            // 
            // bttnLogout
            // 
            bttnLogout.BackColor = Color.RoyalBlue;
            bttnLogout.Cursor = Cursors.Hand;
            bttnLogout.FlatStyle = FlatStyle.Flat;
            bttnLogout.Font = new Font("Noto Sans", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bttnLogout.ForeColor = Color.White;
            bttnLogout.Location = new Point(1096, 695);
            bttnLogout.Name = "bttnLogout";
            bttnLogout.Size = new Size(140, 30);
            bttnLogout.TabIndex = 5;
            bttnLogout.Text = "Log Out";
            bttnLogout.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackgroundImage = Properties.Resources.darkerBg;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1260, 750);
            Controls.Add(bttnLogout);
            Controls.Add(labelMonth);
            Controls.Add(bttnNextMonth);
            Controls.Add(bttnPreviosMonth);
            Controls.Add(calendarPanel);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(appointmentList);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainForm";
            Text = "MainForm";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox appointmentList;
        private GroupBox groupBox1;
        private Button bttnNewAppt;
        private GroupBox groupBox2;
        private Button bttnCancelAppt;
        private Button bttnNewCustomer;
        private Button bttnEditCustomer;
        private TableLayoutPanel calendarPanel;
        private Button bttnPreviosMonth;
        private Button bttnNextMonth;
        private Label labelMonth;
        private Button bttnLogout;
    }
}