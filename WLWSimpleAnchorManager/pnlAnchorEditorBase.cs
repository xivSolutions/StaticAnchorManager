using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WLWStaticAnchorManager
{
    public abstract partial  class pnlAnchorEditorBase : UserControl
    {
        public delegate void ValidAnchorContentHandler(object sender, EventArgs e);
        public event ValidAnchorContentHandler ValidContentDetected;
        public event ValidAnchorContentHandler InvalidContentDetected;

        // Default constructor required by derived classes:
        protected pnlAnchorEditorBase() { }

        public pnlAnchorEditorBase(AnchorData settings) : this()
        {
            InitializeComponent();
            this.AnchorSettings = settings;
        }


        public virtual AnchorClass AnchorType { get; set; }
        public virtual AnchorData AnchorSettings { get; set; }
        public virtual string DisplayText { get; set; }
        public virtual string AnchorName { get; set; }

        protected abstract bool CanSave();


        public virtual void PerformSave()
        {
            this.AnchorSettings.AnchorID = this.AnchorName;
            this.AnchorSettings.DisplayText = this.DisplayText;
            this.AnchorSettings.AnchorClass = this.AnchorType;
        }
    

        protected virtual void CheckContentValidation()
        {
            // Save conditions must be set on derived class:
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
            // Raises event to host form notifying that content
            // is valid and savable:
            if (this.ValidContentDetected != null)
            {
                this.ValidContentDetected(this, new EventArgs());
            }
        }

        protected virtual void OnInValidContent()
        {
            // Raises event to host form notifying that content
            // is invalid and not savable:
            if (this.InvalidContentDetected != null)
            {
                this.InvalidContentDetected(this, new EventArgs());
            }
        }

    }
}
