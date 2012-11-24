using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WLWSimpleAnchorManager
{
    public partial class CreateContentForm : Form
    {

        private const string CREATE_ANCHOR_OPTION_TEXT = "Create a New Anchor";
        private const string CREATE_LINK_OPTION_TEXT = "Link to Existing Anchor";
        private const string COMBO_BOX_DEFAULT = "Select . . .";

        private ComboBoxConfigItem _CreateAnchorSelectionItem;
        private ComboBoxConfigItem _CreateLinkSelectionItem;
        private ComboBoxConfigItem _NoSelectionItem;

        private AnchorTypes _currentConfiguration = AnchorTypes.None;

        private AnchorData _currentAnchorSettings;
        private pnlAnchorEditorBase _currentEditorPanel;

        private string[] _anchorNames;


        public CreateContentForm(AnchorData settings, string[] anchorNames)
        {
            InitializeComponent();

            _anchorNames = anchorNames;
            _currentAnchorSettings = settings;

            this.setUpComboBox();
            this.btnOK.Enabled = false;

            this.btnOK.Click += new EventHandler(btnOK_Click);
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


        private class ComboBoxConfigItem
        {
            public ComboBoxConfigItem(string text, AnchorTypes option)
            {
                this.Text = text;
                this.Option = option;
            }

            public AnchorTypes Option { get; set;}

            public string Text { get; set; }
            public override string ToString()
            {
                return this.Text;
            }
        }


        private void setUpComboBox()
        {

            _CreateAnchorSelectionItem = new ComboBoxConfigItem(CREATE_ANCHOR_OPTION_TEXT, AnchorTypes.Anchor);
            _CreateLinkSelectionItem = new ComboBoxConfigItem(CREATE_LINK_OPTION_TEXT, AnchorTypes.Link);
            _NoSelectionItem = new ComboBoxConfigItem(COMBO_BOX_DEFAULT, AnchorTypes.None);

            // Remove the SelectedIndexChanged event handler while loading:
            this.cboChooseFunction.SelectedIndexChanged -=new EventHandler(cboChooseFunction_SelectedIndexChanged);
            this.cboChooseFunction.Items.Clear();
            this.cboChooseFunction.Items.Add(_NoSelectionItem);
            this.cboChooseFunction.Items.Add(_CreateAnchorSelectionItem);
            this.cboChooseFunction.Items.Add(_CreateLinkSelectionItem);


            // The default will exist as an option until the user makes an explicit selection
            // in the Combobox control. After that, is is removed:
            this.cboChooseFunction.SelectedItem = _NoSelectionItem;

            // Add the Selected Index Changed Event handler when loading is complete:
            this.cboChooseFunction.SelectedIndexChanged +=new EventHandler(cboChooseFunction_SelectedIndexChanged);
        }


        void  cboChooseFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboChooseFunction.SelectedItem != _NoSelectionItem)
            {
                this.cboChooseFunction.Items.Remove(_NoSelectionItem);
                var configItem = (ComboBoxConfigItem)this.cboChooseFunction.SelectedItem;
                _currentConfiguration = configItem.Option;

                this.ConfigurationSelectionChanged(_currentConfiguration);
            }
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



    }
}
