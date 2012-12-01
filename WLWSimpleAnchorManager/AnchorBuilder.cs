
namespace WLWSimpleAnchorManager
{
    class AnchorBuilder : AnchorBuilderBase
    {
        public AnchorBuilder(AnchorData settings)
        {
            this.AnchorSettings = settings;
        }


        public override string getPublishHtml()
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("id", this.AnchorSettings.FullAnchorName(), '"'));

            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        public override string getPublishHtml(string selectedHtml, string selectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("id", this.AnchorSettings.FullAnchorName(), '"'));
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
