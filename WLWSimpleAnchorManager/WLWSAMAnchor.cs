using System;
using System.Text.RegularExpressions;

namespace WLWStaticAnchorManager
{
    public class WLWSAMAnchor
    {
        public static string wlwAnchorFlag = AnchorTypes.wlwStaticAnchor.ToString();
        public static string wlwLinkToAnchorFlag = AnchorTypes.wlwStaticLink.ToString();

        private static string rgxOnlyAlphaNumeric = "[^0-9a-zA-Z-_:]";
        private string _anchorName = "";


        public WLWSAMAnchor(string anchorName, string displayText, AnchorTypes type)
        {
            this.DescriptiveName = anchorName;
            this.InnerText = displayText;
            this.AnchorType = type;
        }

        public AnchorTypes AnchorType { get; set; }
        public string InnerText { get; set; }
        public string LinkTargetAnchorId { get; set; }

        public string DescriptiveName 
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


        public string htmlElementID()
        {
            string output = this.DescriptiveName;
            switch(this.AnchorType)
            {
                case AnchorTypes.wlwStaticAnchor:
                    output = this.WLWAnchorElementId();
                    break;
                case AnchorTypes.wlwStaticLink:
                    output = this.WLWLinkElementID();
                    break;
                default:
                    output = this.DescriptiveName;
                    break;
            }
            return output;
        }


        public string WLWAnchorElementId()
        {
            return WLWSAMAnchor.wlwAnchorFlag + ":" + this.DescriptiveName;
        }


        public string WLWLinkElementID()
        {
            return WLWSAMAnchor.wlwLinkToAnchorFlag + ":" + this.DescriptiveName;
        }


        public string WLWLinksToAnchorId()
        {
            return WLWSAMAnchor.wlwAnchorFlag + ":" + this.LinkTargetAnchorId;
        }


        public static string getAnchorNameFromHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                String regExMatchPattern = "(?<=id=" + WLWSAMAnchor.wlwAnchorFlag + ":|" + WLWSAMAnchor.wlwLinkToAnchorFlag + ":).*?(?=\\s|>|\")";
                Match anchorMatch = Regex.Match(selectedHtml, regExMatchPattern);
                if (anchorMatch.Success)
                {
                    output = anchorMatch.Value;
                }
            }

            return output;
        }


        public static string getFriendlyLinkTargetIdFromHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                String regExMatchPattern = "(?<=href=\"#" + WLWSAMAnchor.wlwAnchorFlag + ":).*?(?=\\s|>|\")";
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
                String regExMatchPattern = "(?<=id=)(" + WLWSAMAnchor.wlwAnchorFlag + "|" + WLWSAMAnchor.wlwLinkToAnchorFlag + ").*?(?=:)(?=.*?(\\s+|\"|>))";
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
                string AnchorTagRegexPattern = "<A\\sid=" + WLWSAMAnchor.wlwAnchorFlag + ":.*?(>|\\s+>)";

                Regex rgx = new Regex(AnchorTagRegexPattern);
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }


        public static string stripLinkHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                string LinkTagRegexPattern = "<A\\s.*id=" + WLWSAMAnchor.wlwLinkToAnchorFlag + ":.*?(>|\\s+>)";

                Regex rgx = new Regex(LinkTagRegexPattern);
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }
    }
}
