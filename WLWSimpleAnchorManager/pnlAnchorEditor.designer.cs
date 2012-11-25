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
            this.chkShowAnchorText = new System.Windows.Forms.CheckBox();
            this.lblDiplayText = new System.Windows.Forms.Label();
            this.lblAnchorName = new System.Windows.Forms.Label();
            this.txtAnchorName = new WLWSimpleAnchorManager.ExtendedTextbox();
            this.txtDisplayText = new WLWSimpleAnchorManager.ExtendedTextbox();
            this.SuspendLayout();
            // 
            // chkShowAnchorText
            // 
            this.chkShowAnchorText.AutoSize = true;
            this.chkShowAnchorText.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FONT;
            this.chkShowAnchorText.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FORECOLOR;
            this.chkShowAnchorText.Location = new System.Drawing.Point(15, 106);
            this.chkShowAnchorText.Name = "chkShowAnchorText";
            this.chkShowAnchorText.Size = new System.Drawing.Size(148, 22);
            this.chkShowAnchorText.TabIndex = 13;
            this.chkShowAnchorText.Text = "Include Visible Text";
            this.chkShowAnchorText.UseVisualStyleBackColor = true;
            // 
            // lblDiplayText
            // 
            this.lblDiplayText.AutoSize = true;
            this.lblDiplayText.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FONT;
            this.lblDiplayText.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FORECOLOR;
            this.lblDiplayText.Location = new System.Drawing.Point(12, 60);
            this.lblDiplayText.Name = "lblDiplayText";
            this.lblDiplayText.Size = new System.Drawing.Size(82, 18);
            this.lblDiplayText.TabIndex = 12;
            this.lblDiplayText.Text = "Display Text";
            // 
            // lblAnchorName
            // 
            this.lblAnchorName.AutoSize = true;
            this.lblAnchorName.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FONT;
            this.lblAnchorName.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FORECOLOR;
            this.lblAnchorName.Location = new System.Drawing.Point(12, 8);
            this.lblAnchorName.Name = "lblAnchorName";
            this.lblAnchorName.Size = new System.Drawing.Size(92, 18);
            this.lblAnchorName.TabIndex = 10;
            this.lblAnchorName.Text = "Anchor Name";
            // 
            // txtAnchorName
            // 
            this.txtAnchorName.AutoSize = true;
            this.txtAnchorName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.txtAnchorName.BackColor = System.Drawing.Color.Silver;
            this.txtAnchorName.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.STANDARD_USER_FONT;
            this.txtAnchorName.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.STANDARD_TEXTBOX_FORECOLOR;
            this.txtAnchorName.Location = new System.Drawing.Point(15, 26);
            this.txtAnchorName.Margin = new System.Windows.Forms.Padding(1);
            this.txtAnchorName.Name = "txtAnchorName";
            this.txtAnchorName.Size = new System.Drawing.Size(400, 21);
            this.txtAnchorName.TabIndex = 20;
            this.txtAnchorName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtAnchorName.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.txtAnchorName.TextBoxBorderColor = System.Drawing.Color.Silver;
            // 
            // txtDisplayText
            // 
            this.txtDisplayText.AutoSize = true;
            this.txtDisplayText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.txtDisplayText.BackColor = System.Drawing.Color.Silver;
            this.txtDisplayText.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.STANDARD_USER_FONT;
            this.txtDisplayText.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.STANDARD_TEXTBOX_FORECOLOR;
            this.txtDisplayText.Location = new System.Drawing.Point(15, 78);
            this.txtDisplayText.Margin = new System.Windows.Forms.Padding(1);
            this.txtDisplayText.Name = "txtDisplayText";
            this.txtDisplayText.Size = new System.Drawing.Size(400, 21);
            this.txtDisplayText.TabIndex = 21;
            this.txtDisplayText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtDisplayText.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.txtDisplayText.TextBoxBorderColor = System.Drawing.Color.Silver;
            // 
            // pnlAnchorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDisplayText);
            this.Controls.Add(this.txtAnchorName);
            this.Controls.Add(this.chkShowAnchorText);
            this.Controls.Add(this.lblDiplayText);
            this.Controls.Add(this.lblAnchorName);
            this.Name = "pnlAnchorEditor";
            this.Size = new System.Drawing.Size(653, 259);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAnchorName;
        private System.Windows.Forms.Label lblDiplayText;
        private System.Windows.Forms.CheckBox chkShowAnchorText;
        private ExtendedTextbox txtAnchorName;
        private ExtendedTextbox txtDisplayText;
    }
}
