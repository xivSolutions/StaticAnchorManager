using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace WLWStaticAnchorManager
{

    public class AnchorData
    {
        private static string rgxOnlyAlphaNumeric = "[^0-9a-zA-Z-_:]";
        private string _anchorID = "";


        public string DisplayText { get; set; }
        public AnchorTypes AnchorClass { get; set; }


        public AnchorData()
        {
            this.AnchorClass = AnchorTypes.None;
            this.AnchorID = "";
            this.DisplayText = "";
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
                var rgx = new Regex(rgxOnlyAlphaNumeric);
                _anchorID = rgx.Replace(value, "-");
            }
        }


        public string LinkReference
        {
            get { return "#" + this._anchorID; }
        }




    }
}
