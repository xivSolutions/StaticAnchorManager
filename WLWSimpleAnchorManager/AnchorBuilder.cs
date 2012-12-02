using System.Text.RegularExpressions;

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
            newAnchor.Attributes.Add(new htmlAttribute("id", this.AnchorSettings.currentInstanceID(), '"'));

            string anchorHtml = newAnchor.ToString();

            return anchorHtml;
        }


        public override string getPublishHtml(string selectedHtml, string selectedText)
        {
            htmlElement newAnchor = new htmlElement("a", false);
            newAnchor.Attributes.Add(new htmlAttribute("id", this.AnchorSettings.currentInstanceID(), '"'));
            newAnchor.Content = this.AnchorSettings.DisplayText;

            string anchorTag = newAnchor.ToString();

            if (string.IsNullOrEmpty(selectedText))
            {
                if (!string.IsNullOrEmpty(selectedHtml))
                {
                    htmlElement wrapper = this.EmptyHtmlWrapper(selectedHtml);
                    wrapper.InternalElements.Add(newAnchor);
                    anchorTag = wrapper.ToString();
                }

                return anchorTag;
            }
            else
            {
                return selectedHtml.Replace(selectedText, anchorTag);
            }
        }


        htmlElement EmptyHtmlWrapper(string emptyTagset)
        {
            string regexTagPair = @"(?<=<)\w+?(?=>)";
            Regex rgx = new Regex(regexTagPair);
            Match match = rgx.Match(emptyTagset);
            string tagName = match.Value;

            htmlElement output = new htmlElement(tagName, false);

            return output;
        }
    }
}
