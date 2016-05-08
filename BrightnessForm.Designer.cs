namespace DHHTA
{
    partial class BrightnessForm
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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.image = new System.Windows.Forms.GroupBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.filterPreview1 = new DHHTA.FilterPreview();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.image.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(30, 80);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Minimum = -1000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(295, 45);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // image
            // 
            this.image.Controls.Add(this.filterPreview1);
            this.image.Location = new System.Drawing.Point(350, 15);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(180, 180);
            this.image.TabIndex = 1;
            this.image.TabStop = false;
            this.image.Text = "Image";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(80, 160);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 25);
            this.ok.TabIndex = 2;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(200, 160);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 25);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Brightness";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "0.0";
            // 
            // filterPreview1
            // 
            this.filterPreview1.Image = null;
            this.filterPreview1.Location = new System.Drawing.Point(6, 19);
            this.filterPreview1.Name = "filterPreview1";
            this.filterPreview1.Size = new System.Drawing.Size(168, 149);
            this.filterPreview1.TabIndex = 0;
            this.filterPreview1.Text = "filterPreview1";
            // 
            // BrightnessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 241);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.image);
            this.Controls.Add(this.trackBar1);
            this.Name = "BrightnessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BrightnessForm";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.image.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.GroupBox image;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        // private DHHTA.FilterPreview filterPreview;
        private FilterPreview filterPreview1;
    }
}