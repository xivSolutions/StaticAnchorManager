using System;
using System.Collections.Generic;
using System.Text;

namespace WLWStaticAnchorManager
{
    /// <summary>
    /// Class definition derived from List(Of T) which is strongly typed, and includes a method
    /// to return an html representaiton of the htmlElement objects contained in the current instance. 
    /// </summary>
    public class htmlElementList : List<htmlElement>
    {

        /// <summary>
        /// Overridden ToString method returns properly formatted html representing
        /// the html elements contained int he current list instance. 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            StringBuilder output = new StringBuilder("");

            // Iterate through the list items and append each element into a StringBuilder:
            foreach (htmlElement currentElement in this)
            {
                output.Append(currentElement.ToString());
            }
            return output.ToString();
        }
    }
}
