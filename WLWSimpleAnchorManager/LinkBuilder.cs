

namespace WLWStaticAnchorManager
{
    class LinkBuilder : AnchorBuilderBase
    {

        public LinkBuilder(WLWSAMAnchor settings)
        {
            this.AnchorSettings = settings;
        }


        public override string getPublishHtml()
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + this.AnchorSettings.WLWLinksToAnchorId(), '"'));
            newAnchor.Attributes.Add(new htmlAttribute("id", this.AnchorSettings.htmlElementID(), '"'));

            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        public override string getPublishHtml(string selectedHtml, string selectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + this.AnchorSettings.WLWLinksToAnchorId(), '"'));
            newAnchor.Attributes.Add(new htmlAttribute("id", this.AnchorSettings.htmlElementID(), '"'));
            newAnchor.Content = this.AnchorSettings.InnerText;

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
