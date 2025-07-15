namespace Peak_deconvolution_for_OES_and_Actinometry
{
    partial class Loading
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
            this.LoadingBar = new System.Windows.Forms.ProgressBar();
            this.Message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadingBar
            // 
            this.LoadingBar.Location = new System.Drawing.Point(12, 41);
            this.LoadingBar.Name = "LoadingBar";
            this.LoadingBar.Size = new System.Drawing.Size(285, 30);
            this.LoadingBar.TabIndex = 0;
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Message.Location = new System.Drawing.Point(115, 9);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(74, 29);
            this.Message.TabIndex = 1;
            this.Message.Text = "label1";
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 84);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.LoadingBar);
            this.Name = "Loading";
            this.Text = "Loading";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar LoadingBar;
        private System.Windows.Forms.Label Message;
    }
}