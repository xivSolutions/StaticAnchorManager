using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace WLWSimpleAnchorManager
{
    class LinkBuilder : HtmlBuilderBase
    {

        public LinkBuilder(AnchorData settings)
        {
            this.AnchorSettings = settings;
        }


        public override string getPublishHtml()
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + HtmlBuilderBase.wlwAnchorTag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("name", HtmlBuilderBase.wlwLinkToAnchor + ":" + this.AnchorSettings.AnchorName, '"'));

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


        public override string editPublishHtml(string selectedHtml, string selectedText)
        {
            string freshHtml = this.stripLinkHtml(selectedHtml);

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
                return freshHtml.Replace(selectedText, anchorTag);
            }
        }


        private string stripLinkHtml(string selectedHtml)
        {
            Regex rgx = new Regex(HtmlBuilderBase.LinkTagRegexPattern);
            string output = rgx.Replace(selectedHtml, "");
            output = output.Replace("</A>", "");
            return output;
        }
    }
}
