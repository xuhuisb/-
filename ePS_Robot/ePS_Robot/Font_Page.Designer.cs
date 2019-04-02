namespace ePS_Robot
{
    partial class Font_Page
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_prescription = new System.Windows.Forms.Button();
            this.btn_medicine = new System.Windows.Forms.Button();
            this.btn_manual = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 29.5F);
            this.label1.Location = new System.Drawing.Point(267, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(547, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = " 您好！请选择服务内容";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_prescription
            // 
            this.btn_prescription.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_prescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_prescription.Font = new System.Drawing.Font("宋体", 24F);
            this.btn_prescription.Location = new System.Drawing.Point(380, 249);
            this.btn_prescription.Name = "btn_prescription";
            this.btn_prescription.Size = new System.Drawing.Size(320, 70);
            this.btn_prescription.TabIndex = 1;
            this.btn_prescription.Text = "处方咨询";
            this.btn_prescription.UseVisualStyleBackColor = false;
            this.btn_prescription.Click += new System.EventHandler(this.btn_prescription_Click);
            // 
            // btn_medicine
            // 
            this.btn_medicine.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_medicine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_medicine.Font = new System.Drawing.Font("宋体", 24F);
            this.btn_medicine.Location = new System.Drawing.Point(380, 356);
            this.btn_medicine.Name = "btn_medicine";
            this.btn_medicine.Size = new System.Drawing.Size(320, 70);
            this.btn_medicine.TabIndex = 2;
            this.btn_medicine.Text = "药物咨询";
            this.btn_medicine.UseVisualStyleBackColor = false;
            this.btn_medicine.Click += new System.EventHandler(this.btn_medicine_Click);
            // 
            // btn_manual
            // 
            this.btn_manual.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_manual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_manual.Font = new System.Drawing.Font("宋体", 24F);
            this.btn_manual.Location = new System.Drawing.Point(380, 464);
            this.btn_manual.Name = "btn_manual";
            this.btn_manual.Size = new System.Drawing.Size(320, 70);
            this.btn_manual.TabIndex = 3;
            this.btn_manual.Text = "人工咨询";
            this.btn_manual.UseVisualStyleBackColor = false;
            this.btn_manual.Click += new System.EventHandler(this.btn_manual_Click);
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("微软雅黑", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Title.Location = new System.Drawing.Point(326, 43);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(444, 80);
            this.Title.TabIndex = 4;
            this.Title.Text = "ePS语音机器人";
            // 
            // Font_Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(219)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(1062, 753);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.btn_manual);
            this.Controls.Add(this.btn_medicine);
            this.Controls.Add(this.btn_prescription);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Font_Page";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Font_Page_Load);
            this.Text = "ePS语音机器人";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_prescription;
        private System.Windows.Forms.Button btn_medicine;
        private System.Windows.Forms.Button btn_manual;
        private System.Windows.Forms.Label Title;
    }
}