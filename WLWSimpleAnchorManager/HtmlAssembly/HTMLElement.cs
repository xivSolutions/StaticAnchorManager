using System;
using System.Text;

namespace WLWSimpleAnchorManager
{
    /// <summary>
    /// Represents an HTML Element
    /// </summary>
    public class htmlElement
    {

        public const string TAG_OPEN = "<";
        public const string TAG_CLOSE = ">";
        public const string TAG_END = "/";
        public const string COMMENT_OPEN = "<!-- ";
        public const string COMMENT_CLOSE = " -->";
        public const string XHTML_TAG_VOID_CLOSE = "/>";
        public const string HTML_INDENT = "  ";

        protected string _name;
        protected htmlAttributeList _attributes;
        protected htmlElementList _internalElements;
        protected string _content;
        protected bool _isVoidElement;
        protected bool _useXhtml;

        /// <summary>
        /// Protected Constructor ensures initialization with required properties, but allows inheritance.
        /// </summary>
        protected htmlElement()
        {
            //Initialize the List-derived objects so that they will not return null references:

            // A custom derivation of List<T> to hold html Attributes for the current instance:
            _attributes = new htmlAttributeList();

            // A custom implementation of List<T> to hold html elements nested within the current instance:
            _internalElements = new htmlElementList();
        }

        /// <summary>
        /// Constructor reuires at least an element name, and a value indicating whether this is
        /// a void element or not. 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsVoidElement"></param>
        public htmlElement(String Name, Boolean IsVoidElement) : this(Name, IsVoidElement, "", false)
        {

        }

        /// <summary>
        /// Constructor requires at least a valid html element name, a value indicating whether this
        /// is a void element or not, and provides an argument to specify the use of XHTML for output.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsVoidElement"></param>
        /// <param name="UseXHTML"></param>
        public htmlElement(String Name, Boolean IsVoidElement, Boolean UseXHTML): this(Name, IsVoidElement, "", UseXHTML)
        {
        }

        /// <summary>
        /// Constructor requires at lease a valid html element name. Since content is being provided, 
        /// the current instance cannot be a void element. 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Content"></param>
        public htmlElement(String Name, String Content) : this(Name, false, Content, false)
        {
        }

        /// <summary>
        /// Constructor requires at least a valid html element name, and provides arguments to specify whether the
        /// current instance is a void element, provide content, and to specify the use of XHTML as output. If content
        /// is provided as and argument when the element is defined as a void element, an InvalidHTMLException
        /// will be thrown. 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsVoidElement"></param>
        /// <param name="Content"></param>
        /// <param name="UseXHTML"></param>
        public htmlElement(String Name, Boolean IsVoidElement, String Content, Boolean UseXHTML) : this()
        {
            _name = Name;

            // Use the Property setter to initialize the member variable _isVoidElement so that
            // validation can occur:
            this.IsVoidElement = IsVoidElement;

            _content = Content;
            _useXhtml = UseXHTML;
        }


        /// <summary>
        /// Read-only: returns the name of the html element.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }


        /// <summary>
        /// Read Only: returns an htmlAttributeList representing the attributes
        /// for the current instance. Use the ToString method defined on htmlAttributesList
        /// to return html text for the attributes. 
        /// </summary>
        public htmlAttributeList Attributes
        {
            get { return _attributes; }
        }


        /// <summary>
        /// Read Only" Returns an htmlElementsList representing any nested elements contained by
        /// the current element. Use the ToString method defined on htmlElementList
        /// to return html text for the contined elements.
        /// </summary>
        public htmlElementList InternalElements
        {
            get { return _internalElements; }
        }


        /// <summary>
        /// Gets or sets the text content contained by th current instance. Attempting to set a value for content 
        /// other than an empty string will cause an InvalidHTMLException to be thrown if the IsVoidTag property
        /// is set to True.
        /// </summary>
        public String Content
        {
            get { return _content; }
            set 
            {
                //Before allowing the property to be set, validate the element status as "non-void"
                // (Void elements cannot contain content). 
                if (_isVoidElement)
                {
                    if (value.Length > 0)
                    {
                        throw new InvalidhtmlException("You have tried to add content to an HTML Element that is void or empty.");
                    }
                }
                _content = value; 
            }
        }


        /// <summary>
        /// Gets or Sets a boolean value indicating whether the current instance represents a void
        /// html element (Void html elements cannot contain content).
        /// </summary>
        public bool IsVoidElement
        {
            get { return _isVoidElement; }
            set { _isVoidElement = value; }
        }


        /// <summary>
        /// Gets or Sets a boolean property indicating whether or not to use XHTML
        /// when rendering the output for this element.
        /// </summary>
        public bool UseXHTML
        {
            get { return _useXhtml; }
            set { _useXhtml = value; }
        }


        /// <summary>
        /// Returns a string representation of the opening html tag for the current element instance. 
        /// </summary>
        /// <returns></returns>
        public String TagOpen()
        {
            StringBuilder output = new StringBuilder();

            // Add the opening bracket and the tag name:
            output.Append(TAG_OPEN + _name);

            // Use the overriden ToString method of htmlAttributeList to add the string
            // of attributes within the opening tag:
            output.Append(_attributes.ToString());

            // The default symbol for closing the opening tag of the element:
            string closeTag = TAG_CLOSE;

            // We may need to handle clsing the tag differently if this is a void tag
            // and XHTML output is required:
            if (this.IsVoidElement && this.UseXHTML)
            {
                closeTag = XHTML_TAG_VOID_CLOSE;
            }

            output.Append(closeTag);

            return output.ToString();
        }


        /// <summary>
        /// Returns a string representation of the proper closing tag for the curernt element instance. If the
        /// current instance represents a void html element, the closing tag will be an empty string. 
        /// </summary>
        /// <returns></returns>
        public String TagClose()
        {
            // The output defaults to an empty string:
            string output = "";

            // If the element is NOT a void element, it will need a closing tag:
            if (!this.IsVoidElement)
            {
                output = TAG_OPEN + TAG_END + _name + TAG_CLOSE;
            }

            return output;
        }


        /// <summary>
        /// Overriden ToString method returns a string representation of the current element instance
        /// as properly formatted HTML or XHTML. The string will include the current element and any
        /// nested elements.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // The opening tag includes any attributes:
            sb.Append(this.TagOpen());

            // The content represents any text content to be displayed within the current element instance. 
            // Content for nested elements is displayed within the nested element tags. 
            sb.Append(this.Content);


            sb.Append(this.InternalElements.ToString());
            sb.Append(this.TagClose());

            return sb.ToString();

        }

    }
}
