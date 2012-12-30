

namespace WLWStaticAnchorManager
{
    class LinkBuilder : AnchorBuilderBase
    {

        public LinkBuilder(AnchorData settings)
        {
            this.AnchorSettings = settings;
        }


        public override string getPublishHtml(string SelectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("href", "#" + this.AnchorSettings.AnchorID, '"'));
            newAnchor.Attributes.Add(new htmlAttribute("class", this.AnchorSettings.AnchorClass.ToString(), '"'));

            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        //public override string getPublishHtml(string selectedHtml, string selectedText)
        //{
        //    htmlElement newAnchor = new htmlElement("a", false);
        //    newAnchor.Attributes.Add(new htmlAttribute("href", "#" + this.AnchorSettings.WLWLinksToAnchorId(), '"'));
        //    newAnchor.Attributes.Add(new htmlAttribute("id", this.AnchorSettings.htmlElementID(), '"'));
        //    newAnchor.Content = this.AnchorSettings.InnerText;

        //    string anchorTag = newAnchor.ToString();

        //    if (string.IsNullOrEmpty(selectedText))
        //    {
        //        return anchorTag;
        //    }
        //    else
        //    {
        //        return selectedHtml.Replace(selectedText, anchorTag);
        //    }
        //}
    }
}
