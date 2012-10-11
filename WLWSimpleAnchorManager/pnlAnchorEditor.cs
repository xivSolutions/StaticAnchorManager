using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WLWSimpleAnchorManager
{
    public partial class pnlAnchorEditor : pnlAnchorEditorBase
    {
        private static string ANCHOR_IMAGE_KEY = Properties.Resources.ANCHOR_IMAGE_KEY;
        private Image ANCHOR_IMAGE = Properties.Resources.Anchor1616;


        public pnlAnchorEditor(AnchorData settings) : base(settings)
        {
            InitializeComponent();
            this.chkShowAnchorText.CheckedChanged += new EventHandler(chkShowAnchorText_CheckedChanged);
            this.txtAnchorName.TextChanged += new EventHandler(txtAnchorName_TextChanged);
            this.txtDisplayText.TextChanged += new EventHandler(txtDisplayText_TextChanged);
            this.Load += new EventHandler(pnlAnchorEditor_Load);
        }


        void pnlAnchorEditor_Load(object sender, EventArgs e)
        {
            this.DisplayText = this.AnchorSettings.DisplayText;
            this.AnchorName = this.AnchorSettings.AnchorName;
            
            this.chkShowAnchorText.Checked = false;
            if (this.txtDisplayText.Text != "")
            {
                this.chkShowAnchorText.Checked = true;
            }
            else
            {
                this.chkShowAnchorText_CheckedChanged(chkShowAnchorText, new EventArgs());
            }
        }


        void txtDisplayText_TextChanged(object sender, EventArgs e)
        {
            this.CheckContentValidation();
            
            this.chkShowAnchorText.Enabled = this.CanKillDisplayText();
        }


        void txtAnchorName_TextChanged(object sender, EventArgs e)
        {
            this.CheckContentValidation();
        }


        void chkShowAnchorText_CheckedChanged(object sender, EventArgs e)
        {
            this.txtDisplayText.Enabled = this.chkShowAnchorText.Checked;
        }


        private bool CanKillDisplayText()
        {
            if (this.DisplayText == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected override bool CanSave()
        {
            if (this.txtAnchorName.Text.Trim() != String.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override AnchorTypes AnchorType
        {
            get
            {
                return AnchorTypes.Anchor;
            }
            set
            {
                throw new NotImplementedException("The AnchorType is determined by default on this control");
            }

        }



        public override string DisplayText
        {
            get
            {
                return this.txtDisplayText.Text;
            }
            set
            {
                this.txtDisplayText.Text = value;
            }
        }

        public override string AnchorName
        {
            get
            {
                return this.txtAnchorName.Text;
            }
            set
            {
                this.txtAnchorName.Text = value;
            }
        }
    }
}
