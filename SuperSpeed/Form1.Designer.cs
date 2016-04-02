namespace SuperSpeed
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.fbdFilePath = new System.Windows.Forms.FolderBrowserDialog();
            this.tbFilePath = new System.Windows.Forms.TextBox();
            this.btnCrock = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbFilePath
            // 
            this.tbFilePath.Enabled = false;
            this.tbFilePath.Location = new System.Drawing.Point(24, 28);
            this.tbFilePath.Name = "tbFilePath";
            this.tbFilePath.Size = new System.Drawing.Size(434, 21);
            this.tbFilePath.TabIndex = 1;
            // 
            // btnCrock
            // 
            this.btnCrock.Location = new System.Drawing.Point(174, 77);
            this.btnCrock.Name = "btnCrock";
            this.btnCrock.Size = new System.Drawing.Size(93, 23);
            this.btnCrock.TabIndex = 2;
            this.btnCrock.Text = "破解高速通道";
            this.btnCrock.UseVisualStyleBackColor = true;
            this.btnCrock.Click += new System.EventHandler(this.btnCrock_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 143);
            this.Controls.Add(this.btnCrock);
            this.Controls.Add(this.tbFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "迅雷高速通道破解";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog fbdFilePath;
        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.Button btnCrock;
    }
}

