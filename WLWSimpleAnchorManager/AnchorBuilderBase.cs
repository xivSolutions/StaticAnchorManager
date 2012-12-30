
namespace WLWStaticAnchorManager
{
    public abstract class AnchorBuilderBase
    {
        protected const string PLACEHOLDER_TEXT_START = "[";
        protected const string PLACEHOLDER_TEXT_CLOSE = "]";
        protected const string PLACEHOLDER_TEXT_COLOR = "#ff0000";
        protected const string PLACEHOLDER_TEXT_SIZE = "1";

        //public static string wlwAnchorFlag = "wlwAnchor";
        //public static string wlwLinkToAnchorFlag = "wlwLink";

        public AnchorData AnchorSettings { get; set; }
        public string ExistingHtml { get; set; }

        public abstract string getPublishHtml(string selectedText);
        //public abstract string getPublishHtml(string selectedHtml, string selectedText);
    }
}
