using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WindowsLive.Writer.Api;
using System.Windows.Forms;
using System.Drawing;
using mshtml;
//using WLWPluginBase.Win32;

using System.Runtime.InteropServices;


namespace WLWSimpleAnchorManager
{
    [WriterPlugin("5439640A-F022-4A52-8BB6-3F9983D96153",
    "WLW Anchor Manager",
    PublisherUrl = "http://TypeCaseException.com",
    Description =
    "Insert inline html anchors and manage the links to" +
    "anchors within your post from a list",
    ImagePath = "writer.png",
    HasEditableOptions = false)]
    [InsertableContentSource("Inline Anchor")]
    public class InLineAnchorManager : ContentSource
    {
        private static string ANCHOR_ICON_KEY = Properties.Resources.ANCHOR_IMAGE_KEY;
        private static string LINK_ICON_KEY = Properties.Resources.LINK_IMAGE_KEY;

        private string _editorHtml;
        private string _selectedHtml = "";
        private string _selectedText = "";
        private string[] _anchorsList;

        private string _currentAnchorName = "";

        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            EditorContent currentEditor = new EditorContent(dialogOwner.Handle);
            _editorHtml = currentEditor.EditorHtml;
            _anchorsList = WLWPostContentHelper.getAnchorNames(_editorHtml);

            try
            {
                IHTMLElement currentElement = currentEditor.TryGetCurrentElement();
                _selectedHtml = currentElement.outerHTML;
                _selectedText = currentElement.innerText;
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }

            _currentAnchorName = WLWPostContentHelper.getAnchorNameFromHtml(_selectedHtml);
            string anchorType = WLWPostContentHelper.getAnchorTypeFromHtml(_selectedHtml);

            _selectedHtml = WLWPostContentHelper.stripAnchorHtml(_selectedHtml);
            _selectedHtml = WLWPostContentHelper.stripLinkHtml(_selectedHtml);

            var anchor = new AnchorData(_currentAnchorName, _selectedText, AnchorTypes.None);
            AnchorBuilderBase builder;

            using (var frm = new CreateContentForm(anchor, _anchorsList))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    switch(anchor.AnchorType)
                    {
                        case AnchorTypes.Anchor:
                            builder = new AnchorBuilder(anchor);
                            break;

                        case AnchorTypes.Link:
                            builder = new LinkBuilder(anchor);
                            break;
                        default:
                            builder = null; 
                            content = "";
                            break;
                    }

                    // If no text is selected in the editor, a named anchor will be inserted at the 
                    // the cursor location, but will not be bound to a specific HTML text element:
                    if (builder != null)
                    {
                        if (string.IsNullOrEmpty(_selectedHtml))
                        {
                            content = builder.getPublishHtml() + _selectedHtml;
                        }
                        else
                        {
                            content = builder.getPublishHtml(_selectedHtml, _selectedText);
                        }

                        // TODO: List, optional find and replace all instances of edited anchors!
                        return DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Anchor Settings");
                        return DialogResult.Cancel;
                    }

                }
                else
                {
                    return DialogResult.Cancel;
                }
            }
        }
    }
}
