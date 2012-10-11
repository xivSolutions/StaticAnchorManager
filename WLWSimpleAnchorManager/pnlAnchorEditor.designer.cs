namespace WLWSimpleAnchorManager
{
    partial class pnlAnchorEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAnchorName = new System.Windows.Forms.Label();
            this.txtAnchorName = new System.Windows.Forms.TextBox();
            this.lblDiplayText = new System.Windows.Forms.Label();
            this.txtDisplayText = new System.Windows.Forms.TextBox();
            this.chkShowAnchorText = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblAnchorName
            // 
            this.lblAnchorName.AutoSize = true;
            this.lblAnchorName.Location = new System.Drawing.Point(3, 19);
            this.lblAnchorName.Name = "lblAnchorName";
            this.lblAnchorName.Size = new System.Drawing.Size(72, 13);
            this.lblAnchorName.TabIndex = 10;
            this.lblAnchorName.Text = "Anchor Name";
            // 
            // txtAnchorName
            // 
            this.txtAnchorName.Location = new System.Drawing.Point(3, 35);
            this.txtAnchorName.Name = "txtAnchorName";
            this.txtAnchorName.Size = new System.Drawing.Size(190, 20);
            this.txtAnchorName.TabIndex = 1;
            // 
            // lblDiplayText
            // 
            this.lblDiplayText.AutoSize = true;
            this.lblDiplayText.Location = new System.Drawing.Point(3, 58);
            this.lblDiplayText.Name = "lblDiplayText";
            this.lblDiplayText.Size = new System.Drawing.Size(65, 13);
            this.lblDiplayText.TabIndex = 12;
            this.lblDiplayText.Text = "Display Text";
            // 
            // txtDisplayText
            // 
            this.txtDisplayText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayText.Location = new System.Drawing.Point(3, 74);
            this.txtDisplayText.Multiline = true;
            this.txtDisplayText.Name = "txtDisplayText";
            this.txtDisplayText.Size = new System.Drawing.Size(190, 20);
            this.txtDisplayText.TabIndex = 0;
            // 
            // chkShowAnchorText
            // 
            this.chkShowAnchorText.AutoSize = true;
            this.chkShowAnchorText.Location = new System.Drawing.Point(3, 101);
            this.chkShowAnchorText.Name = "chkShowAnchorText";
            this.chkShowAnchorText.Size = new System.Drawing.Size(102, 17);
            this.chkShowAnchorText.TabIndex = 13;
            this.chkShowAnchorText.Text = "Use Visible Text";
            this.chkShowAnchorText.UseVisualStyleBackColor = true;
            // 
            // pnlAnchorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkShowAnchorText);
            this.Controls.Add(this.lblDiplayText);
            this.Controls.Add(this.txtDisplayText);
            this.Controls.Add(this.lblAnchorName);
            this.Controls.Add(this.txtAnchorName);
            this.Name = "pnlAnchorEditor";
            this.Size = new System.Drawing.Size(200, 350);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAnchorName;
        private System.Windows.Forms.TextBox txtAnchorName;
        private System.Windows.Forms.Label lblDiplayText;
        private System.Windows.Forms.TextBox txtDisplayText;
        private System.Windows.Forms.CheckBox chkShowAnchorText;
    }
}
