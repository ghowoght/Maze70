namespace HeartMaze
{
    partial class HeartMaze
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.startBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlBoard
            // 
            this.pnlBoard.Location = new System.Drawing.Point(12, 12);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(938, 723);
            this.pnlBoard.TabIndex = 0;
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(981, 12);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(111, 70);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // HeartMaze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 743);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.pnlBoard);
            this.Name = "HeartMaze";
            this.Text = "Maze";
            this.Load += new System.EventHandler(this.HeartMaze_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBoard;
        private System.Windows.Forms.Button startBtn;
    }
}

