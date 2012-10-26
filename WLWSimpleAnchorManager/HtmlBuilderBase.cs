using System;
using System.Collections.Generic;
using System.Text;

namespace WLWSimpleAnchorManager
{
    public abstract class HtmlBuilderBase
    {
        protected const string PLACEHOLDER_TEXT_START = "[";
        protected const string PLACEHOLDER_TEXT_CLOSE = "]";
        protected const string PLACEHOLDER_TEXT_COLOR = "#ff0000";
        protected const string PLACEHOLDER_TEXT_SIZE = "1";

        public static string wlwAnchorTag = "wlwSmartAnchorName";
        public static string wlwLinkToAnchor = "wlwLinkToAnchor";

        public static string LinkTagRegexPattern = "<A\\s.*name=" + HtmlBuilderBase.wlwLinkToAnchor + ":.*?(>|\\s+>)";
        public static string AnchorTagRegexPattern = "<A\\sname=" + HtmlBuilderBase.wlwAnchorTag + ":.*?(>|\\s+>)";

        // <A\s.*name=wlwLinkToAnchor:.*?(>|\s+>)



        public AnchorData AnchorSettings { get; set; }
        public string ExistingHtml { get; set; }

        public abstract string getPublishHtml();
        public abstract string getPublishHtml(string selectedHtml, string selectedText);
        public abstract string editPublishHtml(string selectedHtml, string selectedText);
    }
}
