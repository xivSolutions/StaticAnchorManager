
namespace WLWStaticAnchorManager
{
    public enum AnchorTypes
    {
        None = 0,
        wlwStaticAnchor = 1,
        wlwStaticLink = 2
    }


    public class AnchorTypeHelper
    {
        public static AnchorTypes getAnchorTypeFromString(string TypeName)
        {
            switch(TypeName)
            {
                case "None":
                    return AnchorTypes.None;
                    //break;
                case "wlwStaticAnchor":
                    return AnchorTypes.wlwStaticAnchor;
                case "wlwStaticLink":
                    return AnchorTypes.wlwStaticLink;
                default:
                    return AnchorTypes.None;
            }
        }
    }
}
