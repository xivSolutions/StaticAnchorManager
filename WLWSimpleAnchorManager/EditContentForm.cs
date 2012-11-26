using System;
using System.Collections.Generic;
using System.Drawing;
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


        private AnchorData _currentAnchorSettings;
        private pnlAnchorEditorBase _currentEditorPanel;

        private string[] _anchorNames;


        protected EditContentForm()
        {
            InitializeComponent();

            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.tbInsertAnchor.Click += new EventHandler(TabLabel_Click);
            this.tbLinkToAnchor.Click += new EventHandler(TabLabel_Click);

            this.tbInsertAnchor.MouseEnter += new EventHandler(TabLabel_MouseEnter);
            this.tbLinkToAnchor.MouseEnter += new EventHandler(TabLabel_MouseEnter);

            this.tbInsertAnchor.MouseLeave += new EventHandler(TabLabel_MouseLEave);
            this.tbLinkToAnchor.MouseLeave += new EventHandler(TabLabel_MouseLEave);

            this.tbInsertAnchor.Tag = AnchorTypes.Anchor;
            this.tbLinkToAnchor.Tag = AnchorTypes.Link;

            this._TabLabelGroup = new List<Label>();
            this._TabLabelGroup.Add(tbInsertAnchor);
            this._TabLabelGroup.Add(tbLinkToAnchor);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }


        public EditContentForm(AnchorData settings, string[] anchorNames) : this()
        {
            _anchorNames = anchorNames;
            _currentAnchorSettings = settings;
            this.ConfigureForm(_currentAnchorSettings);

            this.btnOK.Enabled = false;
        }


        private void ConfigureForm(AnchorData settings)
        {
            if (settings.AnchorType == AnchorTypes.Anchor)
            {
                this.FormSetupEditAnchorConfig();
            }

            if (settings.AnchorType == AnchorTypes.Link)
            {
                this.FormSetupEditLinkConfig();
            }
        }



        private void FormSetupEditAnchorConfig()
        {
            this.tbLinkToAnchor.Visible = false;
            this.lblChooseAction.Visible = false;
            this.tbInsertAnchor.Left = 0;
            this.tbInsertAnchor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));

            this.SetTabLabelText(tbInsertAnchor, "Edit Existing Anchor");

            this.SelectTabLabel(tbInsertAnchor);
        }


        private void FormSetupEditLinkConfig()
        {
            this.tbInsertAnchor.Visible = false;
            this.lblChooseAction.Visible = false;
            this.tbLinkToAnchor.Left = 0;
            this.tbLinkToAnchor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));


            this.SetTabLabelText(tbLinkToAnchor, "Edit Link to Anchor");

            this.SelectTabLabel(tbLinkToAnchor);
        }


        private void SetTabLabelText(Label tabLabel, string newText)
        {
            int origHeight = tabLabel.Height;
            tabLabel.AutoSize = true;
            tabLabel.Text = newText;
            tabLabel.AutoSize = false;
            tabLabel.Height = origHeight;
        }


        private void SelectTabLabel(Label selectedTabLabel)
        {
            this.TabLabel_Click(selectedTabLabel, new EventArgs());
        }



        void btnOK_Click(object sender, EventArgs e)
        {
            this.OnSaveAnchor();
        }


        private void OnSaveAnchor()
        {
            _currentEditorPanel.PerformSave();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }


        void ConfigurationSelectionChanged(AnchorTypes selectedConfiguration)
        {
            if (_currentEditorPanel != null)
            {
                this.ControlContainer.Controls.Clear();
            }

            switch (selectedConfiguration)
            {
                case AnchorTypes.Anchor:
                    _currentEditorPanel = new pnlAnchorEditor(_currentAnchorSettings);
                    break;
                case AnchorTypes.Link:
                    _currentEditorPanel = new pnlLinkEditor(_anchorNames, _currentAnchorSettings);
                    break;
                case AnchorTypes.None:
                    _currentEditorPanel = null;
                    break;
                default:
                    _currentEditorPanel = null;
                    break;
            }

            _currentEditorPanel.ValidContentDetected += new pnlAnchorEditorBase.ValidAnchorContentHandler(CurrentEditorPanel_ValidContentDetected);
            _currentEditorPanel.InvalidContentDetected += new pnlAnchorEditorBase.ValidAnchorContentHandler(CurrentEditorPanel_InvalidContentDetected);

            _currentEditorPanel.Dock = DockStyle.Fill;
            this.ControlContainer.Controls.Add(_currentEditorPanel);
            _currentEditorPanel.Show();

        }


        void CurrentEditorPanel_ValidContentDetected(object sender, EventArgs e)
        {
            this.btnOK.Enabled = true;
        }


        void CurrentEditorPanel_InvalidContentDetected(object sender, EventArgs e)
        {
            this.btnOK.Enabled = false;
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

            AnchorTypes newAnchorType = (AnchorTypes)_currentTabLabel.Tag;

            this.ConfigurationSelectionChanged(newAnchorType);
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
            this.HorizontalDivider = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblChooseAction = new System.Windows.Forms.Label();
            this.tbLinkToAnchor = new System.Windows.Forms.Label();
            this.tbInsertAnchor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ControlContainer
            // 
            this.ControlContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlContainer.Location = new System.Drawing.Point(0, 64);
            this.ControlContainer.Name = "ControlContainer";
            this.ControlContainer.Size = new System.Drawing.Size(620, 315);
            this.ControlContainer.TabIndex = 3;
            // 
            // HorizontalDivider
            // 
            this.HorizontalDivider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HorizontalDivider.BackColor = System.Drawing.Color.DarkBlue;
            this.HorizontalDivider.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HorizontalDivider.ForeColor = System.Drawing.Color.DarkBlue;
            this.HorizontalDivider.Location = new System.Drawing.Point(-2, 51);
            this.HorizontalDivider.Name = "HorizontalDivider";
            this.HorizontalDivider.Size = new System.Drawing.Size(622, 10);
            this.HorizontalDivider.TabIndex = 7;
            this.HorizontalDivider.Text = "What do you want to do?";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(533, 385);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(452, 385);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblChooseAction
            // 
            this.lblChooseAction.BackColor = System.Drawing.Color.Transparent;
            this.lblChooseAction.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.STANDARD_UI_FONT;
            this.lblChooseAction.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblChooseAction.Location = new System.Drawing.Point(2, 27);
            this.lblChooseAction.Name = "lblChooseAction";
            this.lblChooseAction.Size = new System.Drawing.Size(193, 28);
            this.lblChooseAction.TabIndex = 6;
            this.lblChooseAction.Text = "What do you want to do?";
            // 
            // tbLinkToAnchor
            // 
            this.tbLinkToAnchor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLinkToAnchor.BackColor = System.Drawing.Color.Transparent;
            this.tbLinkToAnchor.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.STANDARD_UI_FONT;
            this.tbLinkToAnchor.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.tbLinkToAnchor.Location = new System.Drawing.Point(436, 23);
            this.tbLinkToAnchor.Name = "tbLinkToAnchor";
            this.tbLinkToAnchor.Size = new System.Drawing.Size(184, 28);
            this.tbLinkToAnchor.TabIndex = 5;
            this.tbLinkToAnchor.Text = "Link to Existing Anchor";
            this.tbLinkToAnchor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbInsertAnchor
            // 
            this.tbInsertAnchor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInsertAnchor.BackColor = System.Drawing.Color.Transparent;
            this.tbInsertAnchor.Font = global::WLWSimpleAnchorManager.Properties.Settings.Default.STANDARD_UI_FONT;
            this.tbInsertAnchor.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.tbInsertAnchor.Location = new System.Drawing.Point(263, 23);
            this.tbInsertAnchor.Name = "tbInsertAnchor";
            this.tbInsertAnchor.Size = new System.Drawing.Size(167, 28);
            this.tbInsertAnchor.TabIndex = 4;
            this.tbInsertAnchor.Text = "Create a New Anchor";
            this.tbInsertAnchor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EditContentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 420);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.HorizontalDivider);
            this.Controls.Add(this.lblChooseAction);
            this.Controls.Add(this.tbLinkToAnchor);
            this.Controls.Add(this.tbInsertAnchor);
            this.Controls.Add(this.ControlContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditContentForm";
            this.Text = "EditContentForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ControlContainer;
        private System.Windows.Forms.Label tbInsertAnchor;
        private System.Windows.Forms.Label tbLinkToAnchor;
        private System.Windows.Forms.Label lblChooseAction;
        private System.Windows.Forms.Label HorizontalDivider;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;

    }
}
