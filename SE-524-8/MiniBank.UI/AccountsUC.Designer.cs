namespace MiniBank.UI
{
    partial class AccountsUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            accountListBox = new ListBox();
            deleteAccountBtn = new Button();
            SuspendLayout();
            // 
            // accountListBox
            // 
            accountListBox.BackColor = Color.LightGray;
            accountListBox.BorderStyle = BorderStyle.None;
            accountListBox.Dock = DockStyle.Top;
            accountListBox.FormattingEnabled = true;
            accountListBox.Location = new Point(0, 0);
            accountListBox.Name = "accountListBox";
            accountListBox.Size = new Size(586, 150);
            accountListBox.TabIndex = 0;
            // 
            // deleteAccountBtn
            // 
            deleteAccountBtn.BackColor = Color.Red;
            deleteAccountBtn.ForeColor = SystemColors.ButtonHighlight;
            deleteAccountBtn.Location = new Point(408, 639);
            deleteAccountBtn.Name = "deleteAccountBtn";
            deleteAccountBtn.Size = new Size(175, 34);
            deleteAccountBtn.TabIndex = 1;
            deleteAccountBtn.Text = "Delete Account";
            deleteAccountBtn.UseVisualStyleBackColor = false;
            // 
            // AccountsUC
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            Controls.Add(deleteAccountBtn);
            Controls.Add(accountListBox);
            Name = "AccountsUC";
            Size = new Size(586, 685);
            Load += AccountsUC_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListBox accountListBox;
        private Button deleteAccountBtn;
    }
}
