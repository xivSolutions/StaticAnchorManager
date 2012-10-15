using System;
using System.Collections.Generic;
using System.Text;



namespace WLWSimpleAnchorManager
{
    class AnchorBuilder : HtmlBuilderBase
    {
        private static string wlwAnchorTag = "wlwSmartAnchorName";

        public AnchorBuilder(AnchorData settings)
        {
            this.AnchorSettings = settings;
        }


        public override string getPublishHtml()
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("name", wlwAnchorTag + ":" + this.AnchorSettings.AnchorName, '"'));

            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        public override string getPublishHtml(string selectedHtml, string selectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("name", wlwAnchorTag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Content = selectedText;

            string anchorTag = newAnchor.ToString();

            if (string.IsNullOrEmpty(selectedText))
            {
                return anchorTag;
            }
            else
            {
                return selectedHtml.Replace(selectedText, anchorTag);
            }
        }
    }
}
