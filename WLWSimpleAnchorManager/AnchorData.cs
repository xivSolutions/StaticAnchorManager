using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WLWSimpleAnchorManager
{
    public class AnchorData
    {
        private string rgxOnlyAlphaNumeric = "[^0-9a-zA-Z-]";
        private string _anchorName;


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
