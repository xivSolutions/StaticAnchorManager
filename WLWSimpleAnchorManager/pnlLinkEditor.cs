using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WLWStaticAnchorManager
{
    public partial class pnlLinkEditor : pnlAnchorEditorBase
    {
        private static string ANCHOR_IMAGE_KEY = Properties.Resources.ANCHOR_IMAGE_KEY;
        private Image ANCHOR_IMAGE = Properties.Resources.Anchor1616;

        private ListViewItem _selectedListItem;
        private string[] _anchorNames;

        protected pnlLinkEditor() : base()
        {

        }


        public pnlLinkEditor(string[] anchorNames, AnchorData settings) : base(settings)
        {
            InitializeComponent();

            _anchorNames = anchorNames;

            this.txtDisplayText.TextChanged += new EventHandler(txtDisplayText_TextChanged);
            this.lvSelectedAnchor.ItemSelectionChanged +=new ListViewItemSelectionChangedEventHandler(lvSelectedAnchor_ItemSelect);
            this.lvSelectedAnchor.MouseDown +=new MouseEventHandler(lvSelectedAnchor_MouseDown);
            this.lvSelectedAnchor.MouseDoubleClick +=new MouseEventHandler(lvSelectedAnchor_MouseDoubleClick);
            this.Load += new EventHandler(pnlLinkEditor_Load);
            this.txtAnchorName.Enabled = false;
        }


        void pnlLinkEditor_Load(object sender, EventArgs e)
        {
            this.FillAnchorList(_anchorNames);
            this.DisplayText = this.AnchorSettings.DisplayText;
            this.SetSelectedAnchor(this.AnchorSettings.TargetAnchorID);
        }


        public override string DisplayText
        {
            get { return this.txtDisplayText.Text; }
            set { this.txtDisplayText.Text = value; }
        }


        public override string AnchorName
        {
            get { return this.txtAnchorName.Text; }
            set { this.txtAnchorName.Text = value; }
        }


        public override AnchorClass AnchorType
        {
            get { return AnchorClass.wlwStaticLink; }
            set { throw new NotImplementedException("The AnchorType is determined by default on this control"); }
        }

     
        private void SetUpAnchorList()
        {
            ImageList AnchorListImages = new ImageList();
            AnchorListImages.Images.Add(ANCHOR_IMAGE_KEY, ANCHOR_IMAGE);

            ListView lv = lvSelectedAnchor;
            lv.SmallImageList = AnchorListImages;
            lv.FullRowSelect = true;
            lv.View = View.List;
            lv.HideSelection = false;
        }


        private void FillAnchorList(String[] Anchors)
        {
            this.SetUpAnchorList();
            this.lvSelectedAnchor.Items.Clear();

            if (Anchors != null)
            {
                foreach (string anchor in Anchors)
                {
                    ListViewItem item = this.lvSelectedAnchor.Items.Add(anchor, ANCHOR_IMAGE_KEY);
                    item.Name = anchor;
                }
            }
        }


        private void SetSelectedAnchor(String AnchorName)
        {
            char[] delim = {':'};
            string[] arr = AnchorName.Split(delim);

            string findName = arr[0];
            ListView lv = this.lvSelectedAnchor;
            ListViewItem selected = lv.Items[AnchorName];
            if (selected != null)
            {
                selected.Selected = true;
            }
            else
            {
                lv.SelectedItems.Clear();
            }
            
            this.CheckContentValidation();
        }


        private void lvSelectedAnchor_ItemSelect(object Sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.txtAnchorName.Text = "";
            _selectedListItem = null;

            if (e.Item.Selected)
            {
                _selectedListItem = e.Item;

                if (_selectedListItem != null)
                {
                    this.txtAnchorName.Text = e.Item.Text;
                }

            }

            this.CheckContentValidation();
        }


        void lvSelectedAnchor_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _selectedListItem = this.lvSelectedAnchor.HitTest(e.X, e.Y).Item;
            this.CheckContentValidation();
        }


        void lvSelectedAnchor_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.CheckContentValidation();
        }


        void txtDisplayText_TextChanged(object sender, EventArgs e)
        {
            this.CheckContentValidation();
        }


        protected override bool CanSave()
        {
            {
                if (this.txtDisplayText.Text.Trim() != String.Empty && _selectedListItem != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
