using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using mshtml;
using WLWPluginBase.Win32;

namespace WLWSimpleAnchorManager
{
    public class EditorContent
    {
        private const string WNDCLSNAME_IE_SERVER = "Internet Explorer_Server";
        private const char ANCHOR_LIST_DELIMITER = '|';

        IntPtr owner;

        private IHTMLDocument2 _htmlDocument;

        private string _editorHtml;
        private string _selectedHtml = "";
        private string _selectionInnerHtml = "";
        private string _selectedText = "";



        public EditorContent(IntPtr wlwEditorHandle)
        {
            owner = wlwEditorHandle;

            _editorHtml = EditorContent.getHtml(wlwEditorHandle);
            _htmlDocument = EditorContent.getHtmlDocument2(wlwEditorHandle);
        }


        public string EditorHtml
        {
            get { return _editorHtml; }
        }


        public SelectedEditorContent SelectedContent()
        {
            /* 
             * the call to currentEditor.TryGetCurrentElement will throw an exception
             * if the current editor selection is not a valid html item. This is related to the 
             * underlying COM basis of the operation, hence, the TRY block here:
             */
            mshtml.IHTMLElement currentElement = this.TryGetCurrentElement();

            /* 
             * The current html selected in the editor, or within which the cursor 
             * is currently located. This includes any relevant formatting tags surrounding the
             * visible text, and any pre-existing WLW plug-in anchor or link-to-anchor markup:
             * */
            string selectedHtml = currentElement.outerHTML;

            /*
             * Visible text contained within the current element which has additional formatting
             * tags (such as <em> or <pre>) will mess with our eventual insertion of a 
             * well-formed anchor or link element. We need to be able to test for this later, 
             * using the currentSelection.selectedInnerHtml variable:
             */
           string innerHtml = currentElement.innerHTML;

            /* The visible text contained within the html element */
            string selectedText = currentElement.innerText;

            return new SelectedEditorContent(selectedHtml, innerHtml, selectedText);


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


        private static string getHtml(IntPtr owner)
        {
            string selectedText = "";

            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(owner, WNDCLSNAME_IE_SERVER);
            if (item != null)
            {
                selectedText = Win32IEHelper.GetHtml(item.Handle);
            }
            return selectedText;
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
                    return intialElement;
                }
            }
        }


        public static string[] getAnchorNames(string editorHtml)
        {
            string delimitedList = EditorContent.ExtractDelimitedAnchorsList(editorHtml);
            return delimitedList.Split(ANCHOR_LIST_DELIMITER);
        }


        private static string ExtractDelimitedAnchorsList(string PostContent)
        {
            String regExMatchPattern = "(?<=id=" + AnchorData.wlwAnchorFlag + ":).*?(?=\\s|>|\")";
            MatchCollection matches = Regex.Matches(PostContent, regExMatchPattern);


            StringBuilder sb = new StringBuilder("");
            foreach (Match currentMatch in matches)
            {
                sb.Append(currentMatch.Value + ANCHOR_LIST_DELIMITER);
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
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



        public void ChangeAllAnchorRefs(string anchorName)
        {
            IHTMLDocument2 document = EditorContent.getHtmlDocument2(owner);
            IHTMLElementCollection elements = document.anchors as IHTMLElementCollection;
            IHTMLElement selected = elements.item(anchorName) as IHTMLElement;
            selected.id = "MyNewAnchor";           
        }


        public Dictionary<string, string> ExistingAnchorNames()
        {
            IHTMLDocument2 document = EditorContent.getHtmlDocument2(owner);
            IHTMLElementCollection elements = document.anchors as IHTMLElementCollection;
            //IHTMLElement selected = elements.item(linkName) as IHTMLElement;

            var output = new Dictionary<string, string>();

            foreach (IHTMLElement item in elements)
            {
                IHTMLElement current = (IHTMLElement)item;
                string name = current.id;
                if (!string.IsNullOrEmpty(name))
                {
                    output.Add(name, name);
                }
            }

            return output;
        }


        public int getUniqueAnchorNameIndex(string proposedAnchorName)
        {
            Dictionary<string, string> existingAnchorNames = this.ExistingAnchorNames();
            int i = 0;
            string appendIndex = "";
            while (existingAnchorNames.ContainsKey(proposedAnchorName))
            {
                i++;
                appendIndex = "_" + i.ToString();
                proposedAnchorName = proposedAnchorName + appendIndex;
            }

            return i;
        }
    }
}
