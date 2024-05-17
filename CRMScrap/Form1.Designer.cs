namespace CRMScrap
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            Username_TextBox = new TextBox();
            Password_TextBox = new TextBox();
            label2 = new Label();
            Login_BTN = new Button();
            SuspendLayout();
            // 
            // Username_TextBox
            // 
            Username_TextBox.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            Username_TextBox.Location = new Point(150, 36);
            Username_TextBox.Name = "Username_TextBox";
            Username_TextBox.Size = new Size(294, 34);
            Username_TextBox.TabIndex = 0;
            Username_TextBox.TextChanged += TxtBoxChanged;
            // 
            // Password_TextBox
            // 
            Password_TextBox.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            Password_TextBox.Location = new Point(150, 110);
            Password_TextBox.Name = "Password_TextBox";
            Password_TextBox.Size = new Size(294, 34);
            Password_TextBox.TabIndex = 1;
            Password_TextBox.UseSystemPasswordChar = true;
            Password_TextBox.TextChanged += TxtBoxChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ActiveCaption;
            label1.Font = new Font("Yu Gothic", 15F, FontStyle.Bold);
            label1.ForeColor = SystemColors.Desktop;
            label1.Location = new Point(12, 44);
            label1.Name = "label1";
            label1.Size = new Size(112, 26);
            label1.TabIndex = 3;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.ActiveCaption;
            label2.Font = new Font("Yu Gothic", 15F, FontStyle.Bold);
            label2.Location = new Point(12, 118);
            label2.Name = "label2";
            label2.Size = new Size(109, 26);
            label2.TabIndex = 4;
            label2.Text = "Password";
            // 
            // Login_BTN
            // 
            Login_BTN.Cursor = Cursors.No;
            Login_BTN.FlatStyle = FlatStyle.Popup;
            Login_BTN.Font = new Font("Segoe UI Semibold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Login_BTN.ForeColor = Color.Red;
            Login_BTN.Location = new Point(150, 190);
            Login_BTN.Name = "Login_BTN";
            Login_BTN.Size = new Size(153, 58);
            Login_BTN.TabIndex = 5;
            Login_BTN.Text = "Login";
            Login_BTN.UseVisualStyleBackColor = true;
            Login_BTN.Click += LoginButton;
            // 
            // Form1
            // 
            AcceptButton = Login_BTN;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 261);
            Controls.Add(Login_BTN);
            Controls.Add(Password_TextBox);
            Controls.Add(label2);
            Controls.Add(Username_TextBox);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "CRMScrap";
            FormClosed += CloseForm;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private TextBox Username_TextBox;
        private TextBox Password_TextBox;
        private Label label2;
        private Button Login_BTN;
    }
}
