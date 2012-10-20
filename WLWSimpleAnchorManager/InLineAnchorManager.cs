using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WindowsLive.Writer.Api;
using System.Windows.Forms;
using System.Drawing;
using mshtml;
using WLWPluginBase.Win32;

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
        private string _selectedHtml;
        private string _selectedText;
        private string[] _anchorsList;
        IHTMLDocument2 _htmlDoc;

        private string _currentAnchorName = "";

        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            // String representation of the HTML currently in the WLW Editor window:
            _editorHtml = WLWPostContentHelper.ExtractHtml(dialogOwner.Handle);

            // String representation of the HTML enclosing the current selected text in the editor:
            _selectedHtml = WLWPostContentHelper.ExtractSelectedHtml(dialogOwner.Handle);

            // String representation of the Text currently selected in the editor:
            _selectedText = WLWPostContentHelper.ExtractSelectedText(dialogOwner.Handle);
            _anchorsList = WLWPostContentHelper.getAnchorNames(_editorHtml);
            _htmlDoc = WLWPostContentHelper.getHtmlDocument(dialogOwner.Handle);

            _currentAnchorName = WLWPostContentHelper.getAnchorNameFromHtml(_selectedHtml);

            var anchor = new AnchorData(_currentAnchorName, _selectedText, AnchorTypes.None);

            using (var frm = new CreateContentForm(anchor, _anchorsList))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    HtmlBuilderBase builder;
                    switch(anchor.AnchorType)
                    {
                        case AnchorTypes.Anchor:
                            builder = new AnchorBuilder(anchor);

                            // If no text is selected in the editor, a named anchor will be inserted at the 
                            // the cursor location, but will not be bound to a specific HTML text element:
                            if (string.IsNullOrEmpty(_selectedHtml) || _selectedHtml == "")
                            {
                                try
                                {
                                    _selectedHtml = this.getSelectionOuterHtmlElement();
                                    content = builder.getPublishHtml() + _selectedHtml;
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("You cannot attach an anchor to this type of object");
                                    return DialogResult.Cancel;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(_currentAnchorName))
                                {
                                    content = builder.getPublishHtml(_selectedHtml, _selectedText);
                                }
                                else
                                {
                                    content = builder.editPublishHtml(_selectedHtml, _selectedText);
                                }
                            }
                            break;

                        case AnchorTypes.Link:
                            builder = new LinkBuilder(anchor);

                            // If no text is selected in the editor, a named anchor will be inserted at the 
                            // the cursor location, but will not be bound to a specific HTML text element:
                            if (string.IsNullOrEmpty(_selectedHtml) || _selectedHtml == "")
                            {
                                try
                                {
                                    _selectedHtml = this.getSelectionOuterHtmlElement();
                                    content = builder.getPublishHtml() + _selectedHtml;
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("You cannot attach a Link to this type of object");
                                    return DialogResult.Cancel;
                                }
                            }
                            else
                            {
                                content = builder.getPublishHtml(_selectedHtml, _selectedText);
                            }
                            break;
                        case AnchorTypes.None:
                            content = "";
                            break;
                        default:
                            content = "";
                            break;
                    }
                    return DialogResult.OK;
                }
                else
                {
                    return DialogResult.Cancel;
                }
            }
        }


        string getSelectionOuterHtmlElement()
        {
            IHTMLSelectionObject selected = _htmlDoc.selection;
            IHTMLTxtRange rng = _htmlDoc.selection.createRange() as IHTMLTxtRange;

            try
            {
                rng.expand("Word");
                IHTMLElement elmt = rng.parentElement();

                if (elmt.tagName != "DIV" && elmt.tagName != "HTML")
                {
                    rng.moveToElementText(elmt);
                    rng.select();
                    return elmt.outerHTML;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
