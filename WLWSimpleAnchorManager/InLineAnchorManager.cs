using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsLive.Writer.Api;
using mshtml;

namespace WLWStaticAnchorManager
{
    [WriterPlugin("07366A64-24A2-471B-ADB8-9256ADA500C6",
    "WLW Static Anchor Manager",
    PublisherUrl = "http://TypeCaseException.com",
    Description =
    "Insert inline html anchors and manage the links to" +
    "anchors within your post from a list",
    ImagePath = "Images.Clean-Anchor-Icon-PNG-1616.png",
    HasEditableOptions = false)]
    [InsertableContentSource("Inline Anchor")]
    public class InLineAnchorManager : ContentSource
    {
        private static string ANCHOR_ICON_KEY = Properties.Resources.ANCHOR_IMAGE_KEY;
        private static string LINK_ICON_KEY = Properties.Resources.LINK_IMAGE_KEY;

        private IHTMLElement _selectedElement;
        private IHTMLElement _selectedAnchor;

        private string _editorHtml;
        //private string _selectedHtml = "";
        private string _selectedText = "";
        //private string _selectedInnerHtml = "";
        //private string[] _anchorsList;

        //private string _currentAnchorName = "";
        //private string _LinkTargetName = "";

        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {

            EditorContent currentEditor = new EditorContent(dialogOwner.Handle);

            // Current editor contents:
            _editorHtml = currentEditor.EditorHtml;

            try
            {
                _selectedAnchor = currentEditor.TryGetAnchorFromSelection();

                if (_selectedAnchor == null)
                {
                    _selectedElement = currentEditor.TryGetCurrentElement();
                    _selectedText = _selectedElement.innerText;
                    _selectedElement.innerText = "";
                    _selectedElement.insertAdjacentHTML("afterBegin", "<a id=\"newAnchor\">New Anchor</a>");
                    _selectedAnchor = currentEditor.TryGetElementFromHtml("<a id=\"newAnchor\">New Anchor</a>");
                }
                
                //_selectedElement.outerHTML = "<a id=\"First\">First</a>";
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }


            if (_selectedAnchor == null) // No anchor exists
            {
                // does the current editor selection contain a WLWSAM Anchor or Link?

                return DialogResult.OK;
            }
            else // Otherwise, do nothing
            {
                return DialogResult.Cancel;
            }



        }
    }
}
