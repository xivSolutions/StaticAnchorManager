using System.Collections.Generic;
using System.Text;

namespace WLWStaticAnchorManager
{

    /// <summary>
    /// Class definition derived from List(Of T) which is strongly typed, and includes a method
    /// to return an html representation of the htmlAttribute objects contained in the current instance. 
    /// </summary>
    public class htmlAttributeList : List<htmlAttribute>
    {
        public override string ToString()
        {
            StringBuilder output = new StringBuilder("");

            //Iterate through the items contained in the current instance, and append
            // each item with a preceding space:
            foreach (htmlAttribute currentAttribute in this)
            {
                output.Append(" " + currentAttribute.ToString());
            }

            return output.ToString();
        }
    }
}
