using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace WLWStaticAnchorManager
{

    public class AnchorData
    {
        private string _anchorID = "";

        public string DisplayText { get; set; }
        public AnchorClass AnchorClass { get; set; }
        public string TargetAnchorID { get; set; }


        public AnchorData()
        {
            this.AnchorClass = AnchorClass.None;
            this.AnchorID = "";
            this.DisplayText = "";
            this.TargetAnchorID = "";
        }


        public string AnchorID
        {
            get
            {
                return _anchorID;
            }
            set
            {
                /*
                 * Make sure that only alphanumeric characters and 
                 * allowable separators exist in the ID:
                 */
                if (value == null)
                {
                    value = "";
                }
                var rgx = new Regex("[^0-9a-zA-Z-_:]");
                _anchorID = rgx.Replace(value, "-");
            }
        }


        public string LinkHref
        {
            get { return "#" + this._anchorID; }
        }

    }
}
