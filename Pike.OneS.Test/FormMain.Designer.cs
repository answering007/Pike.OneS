namespace Pike.OneS.Test
{
    sealed partial class FormMain
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
            this.labelConnection = new System.Windows.Forms.Label();
            this.textBoxConnection = new System.Windows.Forms.TextBox();
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.textBoxMain = new System.Windows.Forms.TextBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelConnection
            // 
            this.labelConnection.AutoSize = true;
            this.labelConnection.Location = new System.Drawing.Point(12, 15);
            this.labelConnection.Name = "labelConnection";
            this.labelConnection.Size = new System.Drawing.Size(89, 13);
            this.labelConnection.TabIndex = 0;
            this.labelConnection.Text = "Connection string";
            // 
            // textBoxConnection
            // 
            this.textBoxConnection.Location = new System.Drawing.Point(107, 12);
            this.textBoxConnection.Name = "textBoxConnection";
            this.textBoxConnection.Size = new System.Drawing.Size(673, 20);
            this.textBoxConnection.TabIndex = 1;
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.textBoxMain);
            this.groupBoxMain.Location = new System.Drawing.Point(15, 38);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(765, 440);
            this.groupBoxMain.TabIndex = 2;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Messages";
            // 
            // textBoxMain
            // 
            this.textBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMain.Location = new System.Drawing.Point(3, 16);
            this.textBoxMain.Multiline = true;
            this.textBoxMain.Name = "textBoxMain";
            this.textBoxMain.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMain.Size = new System.Drawing.Size(759, 421);
            this.textBoxMain.TabIndex = 0;
            // 
            // buttonTest
            // 
            this.buttonTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTest.Location = new System.Drawing.Point(15, 484);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(765, 77);
            this.buttonTest.TabIndex = 3;
            this.buttonTest.Text = "Test provider";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.groupBoxMain);
            this.Controls.Add(this.textBoxConnection);
            this.Controls.Add(this.labelConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test DbProvider";
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelConnection;
        private System.Windows.Forms.TextBox textBoxConnection;
        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.TextBox textBoxMain;
        private System.Windows.Forms.Button buttonTest;
    }
}

