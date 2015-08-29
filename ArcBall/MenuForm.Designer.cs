namespace ArcBall
{
    partial class MenuForm
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
            this.button1start = new System.Windows.Forms.Button();
            this.button3help = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1start
            // 
            this.button1start.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.button1start.Location = new System.Drawing.Point(117, 144);
            this.button1start.Name = "button1start";
            this.button1start.Size = new System.Drawing.Size(166, 67);
            this.button1start.TabIndex = 0;
            this.button1start.Text = "Старт";
            this.button1start.UseVisualStyleBackColor = true;
            this.button1start.Click += new System.EventHandler(this.button1start_Click);
            // 
            // button3help
            // 
            this.button3help.Location = new System.Drawing.Point(117, 233);
            this.button3help.Name = "button3help";
            this.button3help.Size = new System.Drawing.Size(166, 37);
            this.button3help.TabIndex = 2;
            this.button3help.Text = "Справка";
            this.button3help.UseVisualStyleBackColor = true;
            this.button3help.Click += new System.EventHandler(this.button3help_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Showcard Gothic", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(58, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 74);
            this.label1.TabIndex = 3;
            this.label1.Text = "ArcBall";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(401, 289);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3help);
            this.Controls.Add(this.button1start);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MenuForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ArcBall";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1start;
        private System.Windows.Forms.Button button3help;
        private System.Windows.Forms.Label label1;
    }
}