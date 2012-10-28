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
        private string _selectedHtml;
        private string _selectedText;
        private string[] _anchorsList;
        IHTMLDocument2 _htmlDoc;

        private string _currentAnchorName = "";

        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            _editorHtml = WLWPostContentHelper.ExtractHtml(dialogOwner.Handle);
            _selectedText = WLWPostContentHelper.ExtractSelectedText(dialogOwner.Handle);
            _anchorsList = WLWPostContentHelper.getAnchorNames(_editorHtml);
            _htmlDoc = WLWPostContentHelper.getHtmlDocument(dialogOwner.Handle);

            try
            {
                IHTMLElement currentElement = this.getCurrentElement(_htmlDoc);
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


        IHTMLElement getCurrentElement(IHTMLDocument2 htmlDocument)
        {
            IHTMLSelectionObject selection = _htmlDoc.selection;

            // THis line will throw an exception if an Image or other non-Html
            // item is selected in teh editor. Allow the exception to propegate
            // up the call stack for handling at the UI level. 
            IHTMLTxtRange rng = selection.createRange() as IHTMLTxtRange;
            IHTMLElement elmt = null;

            try
            {
                elmt = this.getFirstValidSelectionElement(rng.parentElement());
            }
            catch (Exception)
            {
                return null;
            }

            rng.moveToElementText(elmt);
            rng.select();
            return elmt;
        }


        IHTMLElement getFirstValidSelectionElement(IHTMLElement intialElement)
        {
            
            if (Array.IndexOf(this.validSelectionElementClassNames(), intialElement.GetType().Name) >= 0)
            {
                // This current element is valid as selected HTML for the editor:
                return intialElement;
            }
            else
            {
                if (Array.IndexOf(this.CheckParentSelectionElementClassNames(), intialElement.GetType().Name) >= 0)
                {
                    IHTMLElement parent = intialElement.parentElement;
                    return this.getFirstValidSelectionElement(parent);
                }
                else
                {
                    return null;
                }
            }           
        }


        string[] validSelectionElementClassNames()
        {
            return new string[] { 
                "HTMLHeaderElementClass", "HTMLLIElementClass", "HTMLListElementClass", 
                "HTMLOListElementClass", "HTMLParaElementClass", 
                "HTMLSpanElementClass", "HTMLTableCaptionClass", "HTMLTableCellClass", 
                "HTMLTableRowClass" };
        }


        string[] CheckParentSelectionElementClassNames()
        {
            return new string[] {
                "HTMLAnchorElementClass",
                "HTMLBaseFontElementClass", "HTMLFontElementClass", "HTMLLinkElementClass", 
                "HTMLPhraseElementClass" };
        }


        string getSelectionOuterHtmlElement()
        {
            IHTMLSelectionObject selected = _htmlDoc.selection;
            IHTMLTxtRange rng = _htmlDoc.selection.createRange() as IHTMLTxtRange;

            try
            {
                IHTMLElement elmt = rng.parentElement();

                if (elmt.tagName != "DIV" && elmt.tagName != "HTML")
                {
                    rng.moveToElementText(elmt);
                    rng.select();

                    if (elmt.tagName == "A")
                    {
                        return elmt.parentElement.outerHTML;
                    }

                    return elmt.outerHTML;
                }
                else
                {
                    if (elmt.tagName == "A")
                    {
                        rng.moveToElementText(elmt);
                        rng.select();
                        return elmt.outerHTML;
                    }

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
