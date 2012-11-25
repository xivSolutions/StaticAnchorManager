
namespace WLWSimpleAnchorManager
{
    public enum AnchorTypes
    {
        None = 0,
        Anchor = 1,
        Link = 2
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
                case "Anchor":
                    return AnchorTypes.Anchor;
                case "Link":
                    return AnchorTypes.Link;
                default:
                    return AnchorTypes.None;
            }
        }
    }
}
