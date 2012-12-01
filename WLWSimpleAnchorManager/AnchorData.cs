﻿using System;
using System.Text.RegularExpressions;

namespace WLWSimpleAnchorManager
{
    public class AnchorData
    {
        public static string wlwAnchorFlag = "wlwAnchor";
        public static string wlwLinkToAnchorFlag = "wlwLink";

        private static string rgxOnlyAlphaNumeric = "[^0-9a-zA-Z-_:]";
        private string _anchorName = "";


        public AnchorData(string anchorName, string displayText, AnchorTypes type)
        {
            this.AnchorName = anchorName;
            this.DisplayText = displayText;
            this.AnchorType = type;
        }

        public AnchorTypes AnchorType { get; set; }
        public string DisplayText { get; set; }
        public string LinksToAnchorName { get; set; }

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


        public string currentInstanceID()
        {
            string output = this.AnchorName;
            switch(this.AnchorType)
            {
                case AnchorTypes.Anchor:
                    output = this.WLWTagAnchorId();
                    break;
                case AnchorTypes.Link:
                    output = this.WLWTagLinkID();
                    break;
                default:
                    output = this.AnchorName;
                    break;
            }
            return output;
        }


        public string WLWTagAnchorId()
        {
            return AnchorData.wlwAnchorFlag + ":" + this.AnchorName;
        }


        public string WLWTagLinkID()
        {
            return AnchorData.wlwLinkToAnchorFlag + ":" + this.AnchorName;
        }


        public string WLWLinksToAnchorId()
        {
            return AnchorData.wlwAnchorFlag + ":" + this.LinksToAnchorName;
        }


        public static string getAnchorNameFromHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                String regExMatchPattern = "(?<=id=" + AnchorData.wlwAnchorFlag + ":).*?(?=\\s|>|\")";
                Match anchorMatch = Regex.Match(selectedHtml, regExMatchPattern);
                if (anchorMatch.Success)
                {
                    output = anchorMatch.Value;
                }
            }

            return output;
        }


        public static AnchorTypes getAnchorTypeFromHtml(string selectedHtml)
        {
            AnchorTypes output = AnchorTypes.None;

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                String regExMatchPattern = "(?<=id=)(" + AnchorData.wlwAnchorFlag + "|" + AnchorData.wlwLinkToAnchorFlag + ").*?(?=:)(?=.*?(\\s+|\"|>))";
                Match anchorTypeMatch = Regex.Match(selectedHtml, regExMatchPattern);
                if (anchorTypeMatch.Success)
                {
                    output = AnchorTypeHelper.getAnchorTypeFromString(anchorTypeMatch.Value.Replace("wlw", ""));
                }
            }

            return output;
        }


        public static string stripAnchorHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                string AnchorTagRegexPattern = "<A\\sid=" + AnchorData.wlwAnchorFlag + ":.*?(>|\\s+>)";

                Regex rgx = new Regex(AnchorTagRegexPattern);
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }


        string LinkTagRegexPattern = "<A\\s.*id=" + AnchorData.wlwLinkToAnchorFlag + ":.*?(>|\\s+>)";
        public static string stripLinkHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                string LinkTagRegexPattern = "<A\\s.*id=" + AnchorData.wlwLinkToAnchorFlag + ":.*?(>|\\s+>)";

                Regex rgx = new Regex(LinkTagRegexPattern);
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }
    }
}
