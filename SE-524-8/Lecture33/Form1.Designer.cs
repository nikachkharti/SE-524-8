namespace Lecture33
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
            btn1 = new Button();
            btn2 = new Button();
            testTxtLabel = new Label();
            SuspendLayout();
            // 
            // btn1
            // 
            btn1.Location = new Point(130, 349);
            btn1.Name = "btn1";
            btn1.Size = new Size(112, 34);
            btn1.TabIndex = 0;
            btn1.Text = "button1";
            btn1.UseVisualStyleBackColor = true;
            btn1.Click += btn1_Click;
            // 
            // btn2
            // 
            btn2.Location = new Point(546, 349);
            btn2.Name = "btn2";
            btn2.Size = new Size(112, 34);
            btn2.TabIndex = 1;
            btn2.Text = "button2";
            btn2.UseVisualStyleBackColor = true;
            btn2.Click += btn2_Click;
            // 
            // testTxtLabel
            // 
            testTxtLabel.AutoSize = true;
            testTxtLabel.Location = new Point(346, 170);
            testTxtLabel.Name = "testTxtLabel";
            testTxtLabel.Size = new Size(0, 25);
            testTxtLabel.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(testTxtLabel);
            Controls.Add(btn2);
            Controls.Add(btn1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn1;
        private Button btn2;
        private Label testTxtLabel;
    }
}
