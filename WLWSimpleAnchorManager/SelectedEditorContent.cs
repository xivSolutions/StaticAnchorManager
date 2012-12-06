using System;
using System.Collections.Generic;
using System.Text;

namespace WLWStaticAnchorManager
{
    public class SelectedEditorContent
    {
        private string _selectedHtml = "";
        private string _innerHtml = "";
        private string _selectedText = "";

        public SelectedEditorContent(string selectedHtml, string innerHtml, string selectedText)
        {
            _selectedHtml = selectedHtml;
            _innerHtml = InnerHtml;
            _selectedText = selectedText;
        }

        public string SelectedHtml { get { return _selectedHtml; } }
        public string InnerHtml { get { return _innerHtml; } }
        public string SelectedText { get { return _selectedText; } }
    }
}
