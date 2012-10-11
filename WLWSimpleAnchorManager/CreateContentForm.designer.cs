namespace WLWSimpleAnchorManager
{
    partial class CreateContentForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkShowAnchors = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboChooseFunction = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(3, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 350);
            this.panel1.TabIndex = 2;
            // 
            // chkShowAnchors
            // 
            this.chkShowAnchors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowAnchors.AutoSize = true;
            this.chkShowAnchors.Location = new System.Drawing.Point(3, 416);
            this.chkShowAnchors.Name = "chkShowAnchors";
            this.chkShowAnchors.Size = new System.Drawing.Size(136, 17);
            this.chkShowAnchors.TabIndex = 4;
            this.chkShowAnchors.Text = "Show Anchors in Editor";
            this.chkShowAnchors.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(47, 449);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(128, 449);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cboChooseFunction
            // 
            this.cboChooseFunction.AutoCompleteCustomSource.AddRange(new string[] {
            "Create a New Anchor",
            "Link to Existing Anchor"});
            this.cboChooseFunction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboChooseFunction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboChooseFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChooseFunction.FormattingEnabled = true;
            this.cboChooseFunction.Location = new System.Drawing.Point(3, 33);
            this.cboChooseFunction.Name = "cboChooseFunction";
            this.cboChooseFunction.Size = new System.Drawing.Size(200, 21);
            this.cboChooseFunction.TabIndex = 6;
            // 
            // CreateContentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 484);
            this.Controls.Add(this.cboChooseFunction);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkShowAnchors);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.panel1);
            this.Name = "CreateContentForm";
            this.Text = "CreateContentForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkShowAnchors;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboChooseFunction;
    }
}