using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WLWStaticAnchorManager
{
    public partial class pnlAnchorEditorBase : UserControl
    {
        public delegate void ValidAnchorContentHandler(object sender, EventArgs e);
        public event ValidAnchorContentHandler ValidContentDetected;
        public event ValidAnchorContentHandler InvalidContentDetected;


        protected pnlAnchorEditorBase()
        {
        
        }


        public pnlAnchorEditorBase(AnchorData settings) : this()
        {
            InitializeComponent();
            this.AnchorSettings = settings;
        }


        // Abstract methods/properties to be implemented on derived classes:
        protected virtual bool CanSave()
        {
            return false;
        }

        public virtual AnchorTypes AnchorType{get; set; }
        public AnchorData AnchorSettings { get; set; }


        public virtual void PerformSave()
        {
            this.AnchorSettings.AnchorID = this.AnchorName;
            this.AnchorSettings.DisplayText = this.DisplayText;
            this.AnchorSettings.AnchorClass = this.AnchorType;
        }


        public virtual string DisplayText { get; set; }
        public virtual string AnchorName { get; set; }


        protected virtual void CheckContentValidation()
        {
            if (this.CanSave())
            {
                this.OnValidContent();
            }
            else
            {
                this.OnInValidContent();
            }
        }


        protected virtual void OnValidContent()
        {
            if (this.ValidContentDetected != null)
            {
                this.ValidContentDetected(this, new EventArgs());
            }
        }


        protected virtual void OnInValidContent()
        {
            if (this.InvalidContentDetected != null)
            {
                this.InvalidContentDetected(this, new EventArgs());
            }
        }



    }
}
