using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WindowsLive.Writer.Api;
using System.Windows.Forms;
using System.Drawing;
using WLWPluginBase.Win32;
using mshtml;

namespace WLWSimpleAnchorManager
{
    public class EditorContent
    {
        private const string WNDCLSNAME_IE_SERVER = "Internet Explorer_Server";
        private const char ANCHOR_LIST_DELIMITER = '|';

        IntPtr owner;

        private string _editorHtml;
        private IHTMLDocument2 _htmlDocument;


        public EditorContent(IntPtr wlwEditorHandle)
        {
            owner = wlwEditorHandle;

            _editorHtml = EditorContent.getHtml(wlwEditorHandle);
            _htmlDocument = EditorContent.getHtmlDocument(wlwEditorHandle);
        }


        public string EditorHtml
        {
            get { return _editorHtml; }
        }


        private static IHTMLDocument2 getHtmlDocument(IntPtr owner)
        {
            IHTMLDocument2 output = null;

            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(owner, WNDCLSNAME_IE_SERVER);
            // Determine if it is visible (i.e. active at the time of the request).
            if (item != null)
            {
                output = Win32IEHelper.GetIEDocumentFromWindowHandle(item.Handle);
            }
            return output;
        }


        private static string getHtml(IntPtr owner)
        {
            string selectedText = "";

            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(owner, WNDCLSNAME_IE_SERVER);
            // Determine if it is visible (i.e. active at the time of the request).
            if (item != null)
            {
                selectedText = Win32IEHelper.GetHtml(item.Handle);
            }
            return selectedText;
        }


        public IHTMLElement TryGetCurrentElement()
        {
            IHTMLSelectionObject selection = _htmlDocument.selection;

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
    }
}
