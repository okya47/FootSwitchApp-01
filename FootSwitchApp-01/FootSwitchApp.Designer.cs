namespace FootSwitchApp_01
{
    partial class FrmFootSwitchApp
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PicPicture1 = new System.Windows.Forms.PictureBox();
            this.LblCapture = new System.Windows.Forms.Label();
            this.LblMessage = new System.Windows.Forms.Label();
            this.BtnCapture = new System.Windows.Forms.Button();
            this.cmbCamera = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.PicPicture2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicPicture1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicPicture2)).BeginInit();
            this.SuspendLayout();
            // 
            // PicPicture1
            // 
            this.PicPicture1.ImageLocation = "";
            this.PicPicture1.Location = new System.Drawing.Point(11, 70);
            this.PicPicture1.Name = "PicPicture1";
            this.PicPicture1.Size = new System.Drawing.Size(450, 320);
            this.PicPicture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicPicture1.TabIndex = 0;
            this.PicPicture1.TabStop = false;
            // 
            // LblCapture
            // 
            this.LblCapture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblCapture.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LblCapture.Location = new System.Drawing.Point(12, 22);
            this.LblCapture.Name = "LblCapture";
            this.LblCapture.Size = new System.Drawing.Size(474, 35);
            this.LblCapture.TabIndex = 1;
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblMessage.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LblMessage.Location = new System.Drawing.Point(10, 659);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(476, 34);
            this.LblMessage.TabIndex = 2;
            // 
            // BtnCapture
            // 
            this.BtnCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCapture.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BtnCapture.Location = new System.Drawing.Point(969, 12);
            this.BtnCapture.Name = "BtnCapture";
            this.BtnCapture.Size = new System.Drawing.Size(102, 45);
            this.BtnCapture.TabIndex = 3;
            this.BtnCapture.Text = "撮影";
            this.BtnCapture.UseVisualStyleBackColor = true;
            this.BtnCapture.Click += new System.EventHandler(this.BtnCapture_Click);
            // 
            // cmbCamera
            // 
            this.cmbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCamera.FormattingEnabled = true;
            this.cmbCamera.Location = new System.Drawing.Point(941, 107);
            this.cmbCamera.Name = "cmbCamera";
            this.cmbCamera.Size = new System.Drawing.Size(121, 20);
            this.cmbCamera.TabIndex = 4;
            // 
            // PicPicture2
            // 
            this.PicPicture2.Location = new System.Drawing.Point(467, 70);
            this.PicPicture2.Name = "PicPicture2";
            this.PicPicture2.Size = new System.Drawing.Size(450, 320);
            this.PicPicture2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicPicture2.TabIndex = 7;
            this.PicPicture2.TabStop = false;
            // 
            // FrmFootSwitchApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 702);
            this.Controls.Add(this.PicPicture2);
            this.Controls.Add(this.cmbCamera);
            this.Controls.Add(this.BtnCapture);
            this.Controls.Add(this.LblMessage);
            this.Controls.Add(this.LblCapture);
            this.Controls.Add(this.PicPicture1);
            this.Name = "FrmFootSwitchApp";
            this.Text = "FootSwitchApp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmFootSwitchApp_FormClosing);
            this.Load += new System.EventHandler(this.FrmFootSwitchApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicPicture1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicPicture2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PicPicture1;
        private System.Windows.Forms.Label LblCapture;
        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.Button BtnCapture;
        private System.Windows.Forms.ComboBox cmbCamera;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox PicPicture2;
    }
}

