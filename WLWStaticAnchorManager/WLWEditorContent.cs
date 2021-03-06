﻿using System;
using mshtml;
using WLWPluginBase.Win32;

namespace WLWStaticAnchorManager
{
    /// <summary>
    /// A custom class that sits between the Win32 stuff and us, provides methods to 
    /// query and manipulate the current editor content in specific ways. 
    /// </summary>
    public class WLWEditorContent
    {
        private const string WNDCLSNAME_IE_SERVER = "Internet Explorer_Server";
        private const char ANCHOR_LIST_DELIMITER = '|';
        
        IntPtr _owner;
        private IHTMLDocument2 _htmlDocument;
        private IHTMLElementCollection _anchorCollection;

        /// <summary>
        /// initializes a class instance using the WLW editor handle to 
        /// obtain access to the current HTML document through IHTMLDocument2
        /// </summary>
        public WLWEditorContent(IntPtr wlwEditorHandle)
        {
            _owner = wlwEditorHandle;

            // Everything else in this class depends upon successful initialization of _htmlDocument
            _htmlDocument = WLWEditorContent.getHtmlDocument2(wlwEditorHandle);
            _anchorCollection = this.getAnchorCollection();
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


        /// <summary>
        /// Returns a collection of all "a" tags in the editor document which have either an id or class attribute
        /// </summary>
        public IHTMLElementCollection getAnchorCollection()
        {
            return _htmlDocument.anchors;
        }


        /// <summary>
        /// Returns an HTMLElementDictionary containing references to all
        /// "a" tags in the current document where the element class is
        /// wlwStaticAnchor
        /// </summary>
        public HTMLElementDictionary getStaticAnchorsDictionary()
        {
            var output = new HTMLElementDictionary();

            foreach (IHTMLElement element in _anchorCollection)
            {
                if (element.className == AnchorClass.wlwStaticAnchor.ToString())
                {
                    output.Add(element.id, element);
                }
            }

            return output;
        }


        /// <summary>
        /// Returns an HTMLElementDictionary containing references to all
        /// "a" tags in the current document where the element class is
        /// wlwStaticLink
        /// </summary>
        public HTMLElementDictionary getStaticLinksDictionary()
        {
            var output = new HTMLElementDictionary();

            foreach (IHTMLElement element in _anchorCollection)
            {
                if (element.className == AnchorClass.wlwStaticLink.ToString())
                {
                    output.Add(element.id, element);
                }
            }

            return output;
        }


        /// <summary>
        /// Returns a reference to the element which contains the currently 
        /// selected text in the editor, or the element within which the cursor
        /// is located. 
        /// </summary>
        /// <returns></returns>
        public IHTMLElement getSelectedElement()
        {
            /*
             * When associating a static anchor with existing, formatted
             * text, we want the anchor to be contained such that the visible text
             * retains the formatting characterstics of the original and/or can be reformatted
             * without destroying the anchor.
             */ 

            // GET THE ELEMENT CURRENTLY SELECTED IN THE EDITOR . . .
            IHTMLElement selectedElement = this.TryGetCurrentElement();

            // . . . OR CREATE ONE AT THE CURRENT INSERTION POINT:
            if (selectedElement == null)
            {
                selectedElement = this.InsertNewContainerElement();
            }

            return selectedElement;
        }


        private IHTMLElement TryGetCurrentElement()
        {
            IHTMLSelectionObject selection = _htmlDocument.selection;

            // This line will throw an exception if an Image or other non-Html
            // item is selected in the html editor. Allow the exception to propegate
            // up the call stack for handling at the UI level. 
            IHTMLTxtRange rng = selection.createRange() as IHTMLTxtRange;
            IHTMLElement elmt = null;

            try
            {
                // Try to get the nearest enclosing element that is valid for 
                // containing an anchor element:
                elmt = this.getFirstValidSelectionElement(rng.parentElement());
            }
            catch (Exception)
            {
                // Null return should cause an exception at UI level, or be otherwise handled:
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
            // Recursively expand the element outward until a valid containing element is
            // found. If none is found, return null. 

            if (Array.IndexOf(this.validSelectionElementClassNames(), intialElement.GetType().Name) >= 0)
            {
                // This current element is valid as a container for an anchor element:
                return intialElement;
            }
            else
            {
                /* 
                 * The current element is not a valid container, but may be the 
                 * child of either an anchor, or a valid anchor container:
                 */
                if (Array.IndexOf(this.CheckParentSelectionElementClassNames(), intialElement.GetType().Name) >= 0)
                {
                    IHTMLElement parent = intialElement.parentElement;

                    // If the parent belongs to either group, recurse:
                    if (Array.IndexOf(this.validSelectionElementClassNames(), parent.GetType().Name) >= 0
                    || Array.IndexOf(this.CheckParentSelectionElementClassNames(), parent.GetType().Name) >= 0)
                    {
                        return this.getFirstValidSelectionElement(parent);
                    }
                    else
                    {
                        // Otherise, return what we have (which may be invalid)
                        return intialElement;
                    }
                }
                else
                {
                    // Nothing was found to be a valid anchor container:
                    return null;
                }
            }
        }


        /// <summary>
        /// Array of element classes which are suitable containers for an anchor element
        /// </summary>
        /// <returns></returns>
        private string[] validSelectionElementClassNames()
        {
            // These elements represent valid container blocks for an anchor element:
            return new string[] { 
                "HTMLHeaderElementClass", "HTMLLIElementClass", "HTMLListElementClass", 
                "HTMLOListElementClass", "HTMLParaElementClass", 
                "HTMLSpanElementClass", "HTMLTableCaptionClass", "HTMLTableCellClass", 
                "HTMLTableRowClass" };
        }


        /// <summary>
        /// Array of element class names which might be children of a valid
        /// anchor containing element (one might already exist, or we might add one).
        /// </summary>
        /// <returns></returns>
        private string[] CheckParentSelectionElementClassNames()
        {
            // These elements are not ideal for our outermost selection, but may be contained
            // within another element which is:
            return new string[] {
                "HTMLAnchorElementClass", "HTMLBaseFontElementClass", "HTMLFontElementClass", 
                "HTMLLinkElementClass", "HTMLPhraseElementClass" };
        }


        private IHTMLElement InsertNewContainerElement()
        {
            // Creates a new Anchor Element and inserts it into 
            // the the parent element of the current selection range
            // (usually will be the Post Body or equivelent).
            // OR
            // Use to insert raw anchor within the editor when no 
            // containing element is present.

            IHTMLSelectionObject selection = _htmlDocument.selection;

            // This line will throw an exception if an Image or other non-Html
            // item is selected in the html editor. Allow the exception to propegate
            // up the call stack for handling at the UI level. 
            IHTMLTxtRange rng = selection.createRange() as IHTMLTxtRange;

            IHTMLElement elmt = this.CreateNewAnchorElement();
            IHTMLDOMNode DOMelmt = (IHTMLDOMNode)elmt;

            IHTMLDOMNode parent = (IHTMLDOMNode)rng.parentElement();
            parent.appendChild(DOMelmt);

            elmt.innerText = "newAnchor";
            rng.moveToElementText(elmt);
            rng.select();

            return elmt;
        }

        /// <summary>
        /// Creates a new IHTMLAnchorElement within the html document, but does not associate with a parent. 
        /// The returned anchor must be added to the child collection of a suitable parent element in 
        /// order to be used. 
        /// </summary>
        private IHTMLElement CreateNewAnchorElement()
        {
            IHTMLElement newAnchor = (HTMLAnchorElementClass)_htmlDocument.createElement("a");
            return newAnchor;
        }


        public IHTMLElement getSelectedAnchor(IHTMLElement currentSelectedElement)
        {
            // Is a valid anchor element currently selected in the editor?
            IHTMLElement selectedAnchor = this.TryGetAnchorFromSelection();

            if (selectedAnchor == null)
            {
                // No achor is currently selected, but one might be contained within the currently
                // selected element:
                IHTMLElementCollection children = (IHTMLElementCollection)currentSelectedElement.children;
                foreach (IHTMLElement child in children)
                {
                    if (child.tagName == "A")
                    {
                        selectedAnchor = child;
                    }
                }

                // Otherwise . . .
                if (selectedAnchor == null)
                {
                    // . . . Create one:
                    selectedAnchor = this.CreateNewSelectedAnchor(currentSelectedElement);
                }
            }
            return selectedAnchor;
        }


        private IHTMLElement TryGetAnchorFromSelection()
        {
            IHTMLElement elmt = null;
            IHTMLSelectionObject selection = _htmlDocument.selection;

            if (selection.type == "Control")
            {
                return null;
            }
            else
            {
                // This line will throw an exception if an Image or other non-Html
                // item is selected in the html editor. Allow the exception to propegate
                // up the call stack for handling at the UI level. 
                IHTMLTxtRange rng = selection.createRange() as IHTMLTxtRange;
                if (rng != null)
                {
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


        /// <summary>
        /// Returns an anchor element if one is contained within the element passed in. otherwise returns null.
        /// </summary>
        /// <param name="initialElement"></param>
        /// <returns></returns>
        private IHTMLElement getAnchorFromSelection(IHTMLElement initialElement)
        {
            if (initialElement.GetType().Name == "HTMLAnchorElementClass")
            {
                return initialElement;
            }
            else
            {
                // See if the current element is potentially a child within an anchor element:
                if (Array.IndexOf(this.CheckParentSelectionElementClassNames(), initialElement.GetType().Name) >= 0)
                {
                    IHTMLElement parent = initialElement.parentElement;

                    // The parent of the current element may be an anchor, or may itself be a child of an anchor:
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


        /// <summary>
        /// Creates a new anchor element within the parent element passed in. 
        /// </summary>
        /// <param name="parentElement"></param>
        /// <returns></returns>
        private IHTMLElement CreateNewSelectedAnchor(IHTMLElement parentElement)
        {
            IHTMLElement newAnchor;

            // We will move existing text content from the parent element
            // into the new Anchor (child) Element:
            string _selectedText = parentElement.innerText;

            // These need to be zeroed out so that the addition of
            // a new child does not append to the existing content. 
            parentElement.innerText = null;
            parentElement.innerHTML = null;

            newAnchor = this.CreateNewAnchorElement();
            newAnchor.innerText = _selectedText;

            // We need to use the IHTMLDOMNode interface to use appendChild method
            IHTMLDOMNode parent = (IHTMLDOMNode)parentElement;
            IHTMLDOMNode anchorAsDom = (IHTMLDOMNode)newAnchor;
            parent.appendChild(anchorAsDom);

            return newAnchor;
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
                // Don't leave the element innerText null or
                // Bad things will happen later:
                element.innerText = "";
            }
            rng.select();
        }
    }
}
