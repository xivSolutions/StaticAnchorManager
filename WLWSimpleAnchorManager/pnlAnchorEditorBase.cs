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


        public pnlAnchorEditorBase(WLWSAMAnchor settings) : this()
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
        public WLWSAMAnchor AnchorSettings { get; set; }


        public virtual void PerformSave()
        {
            this.AnchorSettings.DescriptiveName = this.AnchorName;
            this.AnchorSettings.InnerText = this.DisplayText;
            this.AnchorSettings.AnchorType = this.AnchorType;
            this.AnchorSettings.LinkTargetAnchorId = this.AnchorName;
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
