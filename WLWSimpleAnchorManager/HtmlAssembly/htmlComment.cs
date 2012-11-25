using System;

namespace WLWSimpleAnchorManager
{
    class htmlComment
    {
        public const string COMMENT_OPEN = "<!-- ";
        public const string COMMENT_CLOSE = " -->";

        protected string _content = "";

        protected htmlComment() { }

        public htmlComment(String CommentText)
        {

        }


        public String CommentOpenMarkup
        {
            get
            {
                return COMMENT_OPEN;
            }
        }


        public String CommentCloseMarkup
        {
            get
            {
                return COMMENT_CLOSE;
            }
        }


        public String Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }
    }
}
