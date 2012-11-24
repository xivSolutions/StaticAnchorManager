using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WLWSimpleAnchorManager
{
    public partial class EditContentForm : Form
    {
        private static Color ACTIVE_TAB_FORECOLOR = Color.White;
        private static Color ACTIVE_TAB_BACKCOLOR = Color.DarkBlue;

        private static Color INACTIVE_TAB_FORECOLOR = Color.LightSteelBlue;
        private static Color INACTIVE_TAB_BACKCOLOR = Color.Transparent;

        private static Color MOUSE_OVER_TAB_FORECOLOR = Color.DarkBlue;
        private static Color MOUSE_OVER_TAB_BACKCOLOR = Color.Transparent;

        private List<Label> _TabLabelGroup;
        private Label _currentTabLabel;


        public EditContentForm()
        {
            InitializeComponent();


            this.tbInsertAnchor.Click += new EventHandler(TabLabel_Click);
            this.tbLinkToAnchor.Click += new EventHandler(TabLabel_Click);

            this.tbInsertAnchor.MouseEnter += new EventHandler(TabLabel_MouseEnter);
            this.tbLinkToAnchor.MouseEnter += new EventHandler(TabLabel_MouseEnter);

            this.tbInsertAnchor.MouseLeave += new EventHandler(TabLabel_MouseLEave);
            this.tbLinkToAnchor.MouseLeave += new EventHandler(TabLabel_MouseLEave);

            this._TabLabelGroup = new List<Label>();
            this._TabLabelGroup.Add(tbInsertAnchor);
            this._TabLabelGroup.Add(tbLinkToAnchor);
        }

        void TabLabel_MouseLEave(object sender, EventArgs e)
        {
            var selectedTabLabel = (Label)sender;
            this.OnTabLabelLeave(selectedTabLabel);
        }

        void TabLabel_MouseEnter(object sender, EventArgs e)
        {
            var selectedTabLabel = (Label)sender;
            this.OnTabLabelEnter(selectedTabLabel);
        }

        void TabLabel_Click(object sender, EventArgs e)
        {
            var selectedLabel = (Label)sender;
            this.ToggleTabLabelSelection(selectedLabel);
        }


        void ToggleTabLabelSelection(Label selectedTabLabel)
        {
            _currentTabLabel = selectedTabLabel;

            foreach (var tabLabel in _TabLabelGroup)
            {
                if (selectedTabLabel == tabLabel)
                {
                    tabLabel.ForeColor = ACTIVE_TAB_FORECOLOR;
                    tabLabel.BackColor = ACTIVE_TAB_BACKCOLOR;
                }
                else
                {
                    tabLabel.ForeColor = INACTIVE_TAB_FORECOLOR;
                    tabLabel.BackColor = INACTIVE_TAB_BACKCOLOR;
                }
            }
        }

        void OnTabLabelEnter(Label selectedTabLabel)
        {
            if (selectedTabLabel != _currentTabLabel)
            {
                selectedTabLabel.ForeColor = MOUSE_OVER_TAB_FORECOLOR;
                selectedTabLabel.BackColor = MOUSE_OVER_TAB_BACKCOLOR;
            }
        }

        void OnTabLabelLeave(Label selectedTabLabel)
        {
            if (selectedTabLabel != _currentTabLabel)
            {
                selectedTabLabel.ForeColor = INACTIVE_TAB_FORECOLOR;
                selectedTabLabel.BackColor = INACTIVE_TAB_BACKCOLOR;
            }
        }

        private void HorizontalDivider_Click(object sender, EventArgs e)
        {

        }




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
            this.ControlContainer = new System.Windows.Forms.Panel();
            this.tbInsertAnchor = new System.Windows.Forms.Label();
            this.tbLinkToAnchor = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.HorizontalDivider = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ControlContainer
            // 
            this.ControlContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ControlContainer.Location = new System.Drawing.Point(0, 68);
            this.ControlContainer.Name = "ControlContainer";
            this.ControlContainer.Size = new System.Drawing.Size(653, 295);
            this.ControlContainer.TabIndex = 3;
            // 
            // tbInsertAnchor
            // 
            this.tbInsertAnchor.BackColor = System.Drawing.Color.Transparent;
            this.tbInsertAnchor.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInsertAnchor.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.tbInsertAnchor.Location = new System.Drawing.Point(293, 30);
            this.tbInsertAnchor.Name = "tbInsertAnchor";
            this.tbInsertAnchor.Size = new System.Drawing.Size(156, 26);
            this.tbInsertAnchor.TabIndex = 4;
            this.tbInsertAnchor.Text = "Create a New Anchor";
            this.tbInsertAnchor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbLinkToAnchor
            // 
            this.tbLinkToAnchor.BackColor = System.Drawing.Color.Transparent;
            this.tbLinkToAnchor.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLinkToAnchor.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.tbLinkToAnchor.Location = new System.Drawing.Point(455, 31);
            this.tbLinkToAnchor.Name = "tbLinkToAnchor";
            this.tbLinkToAnchor.Size = new System.Drawing.Size(186, 24);
            this.tbLinkToAnchor.TabIndex = 5;
            this.tbLinkToAnchor.Text = "Link to an Existing Anchor";
            this.tbLinkToAnchor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "What do you want to do?";
            // 
            // HorizontalDivider
            // 
            this.HorizontalDivider.BackColor = System.Drawing.Color.DarkBlue;
            this.HorizontalDivider.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HorizontalDivider.ForeColor = System.Drawing.Color.DarkBlue;
            this.HorizontalDivider.Location = new System.Drawing.Point(-2, 55);
            this.HorizontalDivider.Name = "HorizontalDivider";
            this.HorizontalDivider.Size = new System.Drawing.Size(655, 10);
            this.HorizontalDivider.TabIndex = 7;
            this.HorizontalDivider.Text = "What do you want to do?";
            this.HorizontalDivider.Click += new System.EventHandler(this.HorizontalDivider_Click);
            // 
            // EditContentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 363);
            this.Controls.Add(this.HorizontalDivider);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbLinkToAnchor);
            this.Controls.Add(this.tbInsertAnchor);
            this.Controls.Add(this.ControlContainer);
            this.Name = "EditContentForm";
            this.Text = "EditContentForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ControlContainer;
        private System.Windows.Forms.Label tbInsertAnchor;
        private System.Windows.Forms.Label tbLinkToAnchor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label HorizontalDivider;


    }
}
