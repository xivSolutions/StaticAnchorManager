using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace WLWSimpleAnchorManager
{
    class LinkBuilder : AnchorBuilderBase
    {

        public LinkBuilder(AnchorData settings)
        {
            this.AnchorSettings = settings;
        }


        public override string getPublishHtml()
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + AnchorBuilderBase.wlwAnchorTag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("name", AnchorBuilderBase.wlwLinkToAnchor + ":" + this.AnchorSettings.AnchorName, '"'));

            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        public override string getPublishHtml(string selectedHtml, string selectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + wlwAnchorTag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("name", wlwLinkToAnchor + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Content = this.AnchorSettings.DisplayText;

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
