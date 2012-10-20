using System;
using System.Collections.Generic;
using System.Text;



namespace WLWSimpleAnchorManager
{
    class LinkBuilder : HtmlBuilderBase
    {
        //private static string wlwLinkToAnchor = "wlwLinkToAnchor";

        public LinkBuilder(AnchorData settings)
        {
            this.AnchorSettings = settings;
        }


        public override string getPublishHtml()
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + wlwAnchorTag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("name", wlwLinkToAnchor + ":" + this.AnchorSettings.AnchorName, '"'));


            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        public override string getPublishHtml(string selectedHtml, string selectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + wlwAnchorTag + ":" + this.AnchorSettings.AnchorName, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("name", wlwLinkToAnchor + ":" + this.AnchorSettings.AnchorName, '"'));
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

        public override string editPublishHtml(string selectedHtml, string selectedText)
        {
            throw new NotImplementedException();
        }
    }
}
