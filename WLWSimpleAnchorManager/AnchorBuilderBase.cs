using System;
using System.Collections.Generic;
using System.Text;

namespace WLWSimpleAnchorManager
{
    public abstract class AnchorBuilderBase
    {
        protected const string PLACEHOLDER_TEXT_START = "[";
        protected const string PLACEHOLDER_TEXT_CLOSE = "]";
        protected const string PLACEHOLDER_TEXT_COLOR = "#ff0000";
        protected const string PLACEHOLDER_TEXT_SIZE = "1";

        public static string wlwAnchorTag = "wlwSmartAnchorName";
        public static string wlwLinkToAnchor = "wlwLinkToAnchor";

        public AnchorData AnchorSettings { get; set; }
        public string ExistingHtml { get; set; }

        public abstract string getPublishHtml();
        public abstract string getPublishHtml(string selectedHtml, string selectedText);
    }
}
