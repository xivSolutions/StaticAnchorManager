
namespace WLWStaticAnchorManager
{
    public enum AnchorClass
    {
        None = 0,
        wlwStaticAnchor = 1,
        wlwStaticLink = 2
    }


    public class AnchorClassSelector
    {
        public static AnchorClass selectByName(string TypeName)
        {
            switch(TypeName)
            {
                case "None":
                    return AnchorClass.None;
                    //break;
                case "wlwStaticAnchor":
                    return AnchorClass.wlwStaticAnchor;
                case "wlwStaticLink":
                    return AnchorClass.wlwStaticLink;
                default:
                    return AnchorClass.None;
            }
        }
    }
}
