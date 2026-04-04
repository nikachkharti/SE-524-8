namespace MiniBank.UI
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
            listBox1 = new ListBox();
            nameLabel = new Label();
            nameValue = new TextBox();
            idLabel = new Label();
            idValue = new TextBox();
            phoneNumberLabel = new Label();
            phoneNumberValue = new TextBox();
            emailLabel = new Label();
            emailValue = new TextBox();
            customerTypeLabel = new Label();
            customerTypeCombo = new ComboBox();
            newCustomerBtn = new Button();
            deleteCustomerBtn = new Button();
            updateCustomerBtn = new Button();
            clearFormBtn = new Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Left;
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(320, 746);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(326, 9);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(59, 25);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Name";
            // 
            // nameValue
            // 
            nameValue.Location = new Point(326, 37);
            nameValue.Name = "nameValue";
            nameValue.Size = new Size(262, 31);
            nameValue.TabIndex = 2;
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.Location = new Point(326, 71);
            idLabel.Name = "idLabel";
            idLabel.Size = new Size(142, 25);
            idLabel.TabIndex = 1;
            idLabel.Text = "Identity Number";
            // 
            // idValue
            // 
            idValue.Location = new Point(326, 99);
            idValue.Name = "idValue";
            idValue.Size = new Size(262, 31);
            idValue.TabIndex = 2;
            // 
            // phoneNumberLabel
            // 
            phoneNumberLabel.AutoSize = true;
            phoneNumberLabel.Location = new Point(326, 140);
            phoneNumberLabel.Name = "phoneNumberLabel";
            phoneNumberLabel.Size = new Size(62, 25);
            phoneNumberLabel.TabIndex = 1;
            phoneNumberLabel.Text = "Phone";
            // 
            // phoneNumberValue
            // 
            phoneNumberValue.Location = new Point(326, 168);
            phoneNumberValue.Name = "phoneNumberValue";
            phoneNumberValue.Size = new Size(262, 31);
            phoneNumberValue.TabIndex = 2;
            // 
            // emailLabel
            // 
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(326, 202);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(54, 25);
            emailLabel.TabIndex = 1;
            emailLabel.Text = "Email";
            // 
            // emailValue
            // 
            emailValue.Location = new Point(326, 230);
            emailValue.Name = "emailValue";
            emailValue.Size = new Size(262, 31);
            emailValue.TabIndex = 2;
            // 
            // customerTypeLabel
            // 
            customerTypeLabel.AutoSize = true;
            customerTypeLabel.Location = new Point(326, 264);
            customerTypeLabel.Name = "customerTypeLabel";
            customerTypeLabel.Size = new Size(49, 25);
            customerTypeLabel.TabIndex = 1;
            customerTypeLabel.Text = "Type";
            // 
            // customerTypeCombo
            // 
            customerTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            customerTypeCombo.FormattingEnabled = true;
            customerTypeCombo.Location = new Point(326, 292);
            customerTypeCombo.Name = "customerTypeCombo";
            customerTypeCombo.Size = new Size(262, 33);
            customerTypeCombo.TabIndex = 3;
            // 
            // newCustomerBtn
            // 
            newCustomerBtn.BackColor = Color.Lime;
            newCustomerBtn.Location = new Point(326, 371);
            newCustomerBtn.Name = "newCustomerBtn";
            newCustomerBtn.Size = new Size(139, 34);
            newCustomerBtn.TabIndex = 4;
            newCustomerBtn.Text = "New Customer";
            newCustomerBtn.UseVisualStyleBackColor = false;
            newCustomerBtn.Click += newCustomerBtn_Click;
            // 
            // deleteCustomerBtn
            // 
            deleteCustomerBtn.BackColor = Color.Red;
            deleteCustomerBtn.ForeColor = SystemColors.Control;
            deleteCustomerBtn.Location = new Point(326, 411);
            deleteCustomerBtn.Name = "deleteCustomerBtn";
            deleteCustomerBtn.Size = new Size(163, 34);
            deleteCustomerBtn.TabIndex = 4;
            deleteCustomerBtn.Text = "Delete Customer";
            deleteCustomerBtn.UseVisualStyleBackColor = false;
            deleteCustomerBtn.Click += deleteCustomerBtn_Click_1;
            // 
            // updateCustomerBtn
            // 
            updateCustomerBtn.BackColor = Color.FromArgb(255, 255, 128);
            updateCustomerBtn.Location = new Point(326, 451);
            updateCustomerBtn.Name = "updateCustomerBtn";
            updateCustomerBtn.Size = new Size(181, 34);
            updateCustomerBtn.TabIndex = 4;
            updateCustomerBtn.Text = "Update Customer";
            updateCustomerBtn.UseVisualStyleBackColor = false;
            updateCustomerBtn.Click += updateCustomerBtn_Click_1;
            // 
            // clearFormBtn
            // 
            clearFormBtn.BackColor = Color.FromArgb(0, 192, 192);
            clearFormBtn.ForeColor = Color.Black;
            clearFormBtn.Location = new Point(512, 4);
            clearFormBtn.Name = "clearFormBtn";
            clearFormBtn.Size = new Size(76, 34);
            clearFormBtn.TabIndex = 4;
            clearFormBtn.Text = "Clear";
            clearFormBtn.UseVisualStyleBackColor = false;
            clearFormBtn.Click += clearFormBtn_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1192, 746);
            Controls.Add(clearFormBtn);
            Controls.Add(deleteCustomerBtn);
            Controls.Add(updateCustomerBtn);
            Controls.Add(newCustomerBtn);
            Controls.Add(customerTypeCombo);
            Controls.Add(customerTypeLabel);
            Controls.Add(emailValue);
            Controls.Add(emailLabel);
            Controls.Add(phoneNumberValue);
            Controls.Add(phoneNumberLabel);
            Controls.Add(idValue);
            Controls.Add(idLabel);
            Controls.Add(nameValue);
            Controls.Add(nameLabel);
            Controls.Add(listBox1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Label nameLabel;
        private TextBox nameValue;
        private Label idLabel;
        private TextBox idValue;
        private Label phoneNumberLabel;
        private TextBox phoneNumberValue;
        private Label emailLabel;
        private TextBox emailValue;
        private Label customerTypeLabel;
        private ComboBox customerTypeCombo;
        private Button newCustomerBtn;
        private Button deleteCustomerBtn;
        private Button updateCustomerBtn;
        private Button clearFormBtn;
    }
}
