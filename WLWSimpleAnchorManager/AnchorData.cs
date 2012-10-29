using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WLWSimpleAnchorManager
{
    public class AnchorData
    {
        private static string rgxOnlyAlphaNumeric = "[^0-9a-zA-Z-]";
        private string _anchorName = "";


        public AnchorData(string anchorName, string displayText, AnchorTypes type)
        {
            this.AnchorName = anchorName;
            this.DisplayText = displayText;
            this.AnchorType = type;
        }


        public string DisplayText { get; set; }

        public string AnchorName 
        { 
            get 
            { 
                return _anchorName; 
            } 
            set
            {
                var rgx = new Regex(rgxOnlyAlphaNumeric);
                _anchorName = rgx.Replace(value, "-");
            }
        
        }

        public AnchorTypes AnchorType { get; set; }

    }
}
