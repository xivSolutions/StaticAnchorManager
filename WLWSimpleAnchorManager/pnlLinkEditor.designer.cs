﻿namespace WLWSimpleAnchorManager
{
    partial class pnlLinkEditor
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
            this.lblDiplayText = new System.Windows.Forms.Label();
            this.lblLvSelectedAnchor = new System.Windows.Forms.Label();
            this.lvSelectedAnchor = new System.Windows.Forms.ListView();
            this.txtDisplayText = new WLWSimpleAnchorManager.ExtendedTextbox();
            this.txtAnchorName = new WLWSimpleAnchorManager.ExtendedTextbox();
            this.SuspendLayout();
            // 
            // lblAnchorName
            // 
            this.lblAnchorName.AutoSize = true;
            this.lblAnchorName.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FONT;
            this.lblAnchorName.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FORECOLOR;
            this.lblAnchorName.Location = new System.Drawing.Point(12, 60);
            this.lblAnchorName.Name = "lblAnchorName";
            this.lblAnchorName.Size = new System.Drawing.Size(88, 17);
            this.lblAnchorName.TabIndex = 10;
            this.lblAnchorName.Text = "Anchor Name";
            // 
            // lblDiplayText
            // 
            this.lblDiplayText.AutoSize = true;
            this.lblDiplayText.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FONT;
            this.lblDiplayText.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FORECOLOR;
            this.lblDiplayText.Location = new System.Drawing.Point(12, 8);
            this.lblDiplayText.Name = "lblDiplayText";
            this.lblDiplayText.Size = new System.Drawing.Size(78, 17);
            this.lblDiplayText.TabIndex = 12;
            this.lblDiplayText.Text = "Display Text";
            // 
            // lblLvSelectedAnchor
            // 
            this.lblLvSelectedAnchor.AutoSize = true;
            this.lblLvSelectedAnchor.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FONT;
            this.lblLvSelectedAnchor.ForeColor = global::WLWSimpleAnchorManager.Properties.Settings.Default.FORM_LABEL_FORECOLOR;
            this.lblLvSelectedAnchor.Location = new System.Drawing.Point(12, 113);
            this.lblLvSelectedAnchor.Name = "lblLvSelectedAnchor";
            this.lblLvSelectedAnchor.Size = new System.Drawing.Size(87, 17);
            this.lblLvSelectedAnchor.TabIndex = 14;
            this.lblLvSelectedAnchor.Text = "Select Anchor";
            // 
            // lvSelectedAnchor
            // 
            this.lvSelectedAnchor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSelectedAnchor.Location = new System.Drawing.Point(15, 133);
            this.lvSelectedAnchor.Name = "lvSelectedAnchor";
            this.lvSelectedAnchor.Size = new System.Drawing.Size(444, 169);
            this.lvSelectedAnchor.TabIndex = 2;
            this.lvSelectedAnchor.UseCompatibleStateImageBehavior = false;
            // 
            // txtDisplayText
            // 
            this.txtDisplayText.BackColor = System.Drawing.Color.Silver;
            this.txtDisplayText.Location = new System.Drawing.Point(15, 26);
            this.txtDisplayText.Margin = new System.Windows.Forms.Padding(1);
            this.txtDisplayText.Name = "txtDisplayText";
            this.txtDisplayText.Size = new System.Drawing.Size(444, 24);
            this.txtDisplayText.TabIndex = 15;
            this.txtDisplayText.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.txtDisplayText.TextBoxBorderColor = System.Drawing.Color.Silver;
            // 
            // txtAnchorName
            // 
            this.txtAnchorName.BackColor = System.Drawing.Color.Silver;
            this.txtAnchorName.Location = new System.Drawing.Point(15, 78);
            this.txtAnchorName.Margin = new System.Windows.Forms.Padding(1);
            this.txtAnchorName.Name = "txtAnchorName";
            this.txtAnchorName.Size = new System.Drawing.Size(444, 24);
            this.txtAnchorName.TabIndex = 16;
            this.txtAnchorName.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.txtAnchorName.TextBoxBorderColor = System.Drawing.Color.Silver;
            // 
            // pnlLinkEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtAnchorName);
            this.Controls.Add(this.txtDisplayText);
            this.Controls.Add(this.lblLvSelectedAnchor);
            this.Controls.Add(this.lvSelectedAnchor);
            this.Controls.Add(this.lblDiplayText);
            this.Controls.Add(this.lblAnchorName);
            this.Name = "pnlLinkEditor";
            this.Size = new System.Drawing.Size(653, 315);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAnchorName;
        private System.Windows.Forms.Label lblDiplayText;
        private System.Windows.Forms.Label lblLvSelectedAnchor;
        private System.Windows.Forms.ListView lvSelectedAnchor;
        private ExtendedTextbox txtDisplayText;
        private ExtendedTextbox txtAnchorName;
    }
}
