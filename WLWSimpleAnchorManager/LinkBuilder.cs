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
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + AnchorBuilderBase.wlwAnchorFlag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("name", AnchorBuilderBase.wlwLinkToAnchorFlag + ":" + this.AnchorSettings.AnchorName, '"'));

            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        public override string getPublishHtml(string selectedHtml, string selectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + wlwAnchorFlag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("name", wlwLinkToAnchorFlag + ":" + this.AnchorSettings.AnchorName, '"'));
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


        private static string stripLinkHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                Regex rgx = new Regex(WLWPostContentHelper.LinkTagRegexPattern);
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }
    }
}
