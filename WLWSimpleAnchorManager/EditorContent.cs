using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using mshtml;
using WLWPluginBase.Win32;

namespace WLWStaticAnchorManager
{
    public class EditorContent
    {
        private const string WNDCLSNAME_IE_SERVER = "Internet Explorer_Server";
        private const char ANCHOR_LIST_DELIMITER = '|';

        IntPtr owner;

        private IHTMLDocument2 _htmlDocument;


        public EditorContent(IntPtr wlwEditorHandle)
        {
            owner = wlwEditorHandle;
            _htmlDocument = EditorContent.getHtmlDocument2(wlwEditorHandle);
        }


        private static IHTMLDocument2 getHtmlDocument2(IntPtr owner)
        {
            IHTMLDocument2 output = null;

            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(owner, WNDCLSNAME_IE_SERVER);
            if (item != null)
            {
                output = Win32IEHelper.GetIEDocumentFromWindowHandle(item.Handle);
            }
            return output;
        }

        
        public IHTMLElementCollection getAnchorCollection()
        {
            return _htmlDocument.anchors;
        }


        public IHTMLElement TryGetAnchorFromSelection()
        {
            IHTMLElement elmt = null;
            IHTMLSelectionObject selection = _htmlDocument.selection;

            if (selection.type == "Control")
            {
                IHTMLControlRange ctlRng = (IHTMLControlRange)selection.createRange();
                if (ctlRng.length == 1)
                {
                    elmt = ctlRng.item(0);
                    elmt = this.getAnchorFromSelection(elmt.parentElement);
                }
            }
            else
            {
                // This line will throw an exception if an Image or other non-Html
                // item is selected in the html editor. Allow the exception to propegate
                // up the call stack for handling at the UI level. 
                IHTMLTxtRange rng = selection.createRange() as IHTMLTxtRange;
                if (rng != null)
                {
                    if (rng.text == null)
                    {
                        rng.text = "";
                    }
                    else
                    {
                        rng.findText(rng.text);
                    }


                    try
                    {
                        elmt = this.getAnchorFromSelection(rng.parentElement());

                        if (elmt != null)
                        {
                            rng.moveToElementText(elmt);
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }

            return elmt;
        }


        private IHTMLElement getAnchorFromSelection(IHTMLElement initialElement)
        {
            if (initialElement.GetType().Name == "HTMLAnchorElementClass")
            {
                // This current element is valid as selected HTML for the editor:
                return initialElement;
            }
            else
            {
                if (Array.IndexOf(this.CheckParentSelectionElementClassNames(), initialElement.GetType().Name) >= 0)
                {
                    IHTMLElement parent = initialElement.parentElement;

                    if (parent.GetType().Name == "HTMLAnchorElementClass"
                    || Array.IndexOf(this.CheckParentSelectionElementClassNames(), parent.GetType().Name) >= 0)
                    {
                        return this.getAnchorFromSelection(parent);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

        }


        public IHTMLElement TryGetCurrentElement()
        {
            
            IHTMLSelectionObject selection = _htmlDocument.selection;

            // This line will throw an exception if an Image or other non-Html
            // item is selected in the html editor. Allow the exception to propegate
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

            if (elmt != null)
            {
                rng.moveToElementText(elmt);
                rng.select();
            }

            return elmt;
        }


        private IHTMLElement getFirstValidSelectionElement(IHTMLElement intialElement)
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

                    if (Array.IndexOf(this.validSelectionElementClassNames(), parent.GetType().Name) >= 0
                    || Array.IndexOf(this.CheckParentSelectionElementClassNames(), parent.GetType().Name) >= 0)
                    {
                        return this.getFirstValidSelectionElement(parent);
                    }
                    else
                    {
                        return intialElement;
                    }
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
                "HTMLAnchorElementClass", "HTMLBaseFontElementClass", "HTMLFontElementClass", 
                "HTMLLinkElementClass", "HTMLPhraseElementClass" };
        }


        public IHTMLElement InsertNewAnchor()
        {
            IHTMLElement newAnchor = (HTMLAnchorElementClass)_htmlDocument.createElement("a");
            return newAnchor;
        }


        public IHTMLElement InsertNewContainerElement()
        {
            IHTMLSelectionObject selection = _htmlDocument.selection;

            // This line will throw an exception if an Image or other non-Html
            // item is selected in the html editor. Allow the exception to propegate
            // up the call stack for handling at the UI level. 
            IHTMLTxtRange rng = selection.createRange() as IHTMLTxtRange;

            IHTMLElement elmt = this.InsertNewAnchor();
            IHTMLDOMNode DOMelmt = (IHTMLDOMNode)elmt;

            IHTMLDOMNode parent = (IHTMLDOMNode)rng.parentElement();
            parent.appendChild(DOMelmt);

            elmt.innerText = "newAnchor";
            rng.moveToElementText(elmt);
            rng.select();

            return elmt;
        }


        public void MoveSelectionToElementText(IHTMLElement element)
        {
            IHTMLSelectionObject selection = _htmlDocument.selection;

            // This line will throw an exception if an Image or other non-Html
            // item is selected in the html editor. Allow the exception to propegate
            // up the call stack for handling at the UI level. 
            IHTMLTxtRange rng = selection.createRange() as IHTMLTxtRange;
            rng.moveToElementText(element);

            if (element.innerText != null)
            {
                rng.findText(element.innerText);
            }
            else
            {
                element.innerText = "";
            }

            rng.select();
        }
    }
}
