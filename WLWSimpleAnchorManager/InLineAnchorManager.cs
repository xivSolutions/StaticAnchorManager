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

        private HTMLElementDictionary _namedAnchorDictionary;
        private HTMLElementDictionary _namedLinkDictionary;
        private string[] _anchorNames;

        //private AnchorData _anchorData;


        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            AnchorData anchorData = new AnchorData();
            IHTMLElement selectedElement;
            IHTMLElement selectedAnchor;

            // The content variable is passed in by ref, and when returned, marks the insertion point
            // for text contained within any elements created. Appears to behave differeently when 
            // the selection point in the editor is at the end of the currrent editor content. 

            // Keep this set to an empty string until this method is ready to return:
            content = "";

            EditorContent currentEditor = new EditorContent(dialogOwner.Handle);

            // Set up the Dictionary of existing anchors, and get a list of
            // the anchor names for use in creating links to anchors:
            _namedAnchorDictionary = this.getStaticAnchorsDictionary(currentEditor.getAnchorCollection());

            // Use a string array of anchor names to pass to the Link Editor Form:
            _anchorNames = new string[_namedAnchorDictionary.Count];
            _namedAnchorDictionary.Keys.CopyTo(_anchorNames, 0);

            // Dictionary of Static Links for link ID validation:
            _namedLinkDictionary = this.getStaticLinksDictionary(currentEditor.getAnchorCollection());

            // Remember what was selected, and case the user cancels the operation
            string _selectedText = "";
            string _selectedHtml = "";

            try
            {
                // get a reference to the smallest block of html in which the current
                // selection is located (if no selection, expands from the current cursor
                // location:
                selectedElement = currentEditor.TryGetCurrentElement();

                // If the cursor is not contained by a valid element, create one
                // as the current selection:
                if (selectedElement == null)
                {
                    selectedElement = currentEditor.InsertNewContainerElement();
                }


                // Is a valid anchor element currently selected in the editor?
                selectedAnchor = currentEditor.TryGetAnchorFromSelection();

                if (selectedAnchor == null)
                {
                    // No achor is currently selected, but one might be contained within the currently
                    // selected element:
                    IHTMLElementCollection children = (IHTMLElementCollection)selectedElement.children;
                    foreach (IHTMLElement child in children)
                    {
                        if (child.tagName == "A")
                        {
                            selectedAnchor = child;
                        }
                    }
                }

                // Remember what was selected in the originally selected element, 
                // in case the user cancels the operation. Subsequent operations in this scope
                // modify these values in the editor, and we need to be able to reset them on cancel:
                _selectedText = selectedElement.innerText;
                _selectedHtml = selectedElement.outerHTML;

                
                if (selectedAnchor == null)
                {
                    selectedAnchor = this.CreateNewSelectedAnchor(currentEditor, selectedElement);
                    anchorData.DisplayText = selectedAnchor.innerText;
                }
                else
                {
                    anchorData.AnchorClass = AnchorTypeHelper.getAnchorTypeFromString(selectedAnchor.className);
                    anchorData.AnchorID = selectedAnchor.id;
                    anchorData.DisplayText = selectedAnchor.innerText;
                    anchorData.TargetAnchorID = this.getAnchorIDFromLinkID(anchorData.AnchorID);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }


            using (var editContentForm = new EditContentForm(anchorData, _anchorNames))
            {
                if (editContentForm.ShowDialog() == DialogResult.OK)
                {
                    switch (anchorData.AnchorClass)
                    {

                        case AnchorTypes.wlwStaticAnchor:

                            /*
                             * make sure the proposed anchor ID is unique:
                             */
                            int uniqueNameIndex = this.getUniqueAnchorNameIndex(anchorData.AnchorID);
                            if (uniqueNameIndex > 0 && anchorData.AnchorID != selectedAnchor.id)
                            {
                                anchorData.AnchorID = anchorData.AnchorID + "_" + uniqueNameIndex;
                            }

                            // Capture the original AnchorID for updating link references:
                            string oldAnchorID = selectedAnchor.id;
                            string oldHref= "#" + oldAnchorID;
                            string newHref = anchorData.LinkHref;

                            if (selectedAnchor != null)
                            {
                                selectedAnchor.id = anchorData.AnchorID;
                                selectedAnchor.innerText = anchorData.DisplayText;
                                selectedAnchor.className = anchorData.AnchorClass.ToString();
                            }

                            if (oldAnchorID != selectedAnchor.id)
                            {
                                this.updateLinkReferences(oldHref, newHref);
                            }

                            break;
                        case AnchorTypes.wlwStaticLink:
                            if (selectedAnchor != null)
                            {
                                /*
                                 * Make sure the proposed Link ID is unique:
                                 */
                                string proposedID = anchorData.AnchorID + ":" + anchorData.AnchorClass.ToString();
                                uniqueNameIndex = this.getUniqueLinkNameIndex(proposedID);
                                if (uniqueNameIndex > 0 && proposedID != selectedAnchor.id)
                                {
                                    selectedAnchor.id = proposedID + "_" + uniqueNameIndex;
                                }
                                else
                                {
                                    selectedAnchor.id = proposedID;
                                }

                                selectedAnchor.className = anchorData.AnchorClass.ToString();
                                selectedAnchor.innerText = anchorData.DisplayText;
                                IHTMLAnchorElement anchor = (IHTMLAnchorElement)selectedAnchor;
                                anchor.href = anchorData.LinkHref;
                                //content = _selectedAnchor.innerText;
                            }
                            break;
                    }
                }
                else
                {
                    selectedElement.outerHTML = _selectedHtml;
                    return DialogResult.Cancel;
                }
            }

            currentEditor.MoveSelectionToElementText(selectedAnchor);

            if (selectedAnchor.innerText != null)
            {
                content = selectedAnchor.innerText;
            }

            _anchorNames = null;
            selectedAnchor = null;
            selectedElement = null;
            anchorData = null;
            _namedAnchorDictionary = null;
            _namedLinkDictionary = null;

            return DialogResult.OK;

        }


        private IHTMLElement CreateNewSelectedAnchor(EditorContent currentEditor, IHTMLElement parentElement)
        {
            IHTMLElement newAnchor;

            string _selectedText = parentElement.innerText;

            parentElement.innerText = null;
            parentElement.innerHTML = null;

            // We need an IHTMLDOMNode interface to use the appendChild method
            // on the parent element:
            IHTMLDOMNode parent = (IHTMLDOMNode)parentElement;
            newAnchor = currentEditor.InsertNewAnchor();
            newAnchor.innerText = _selectedText;

            // Once we have created the new anchor element, we need to 
            // cast it as an IHTMLDOMNode in order to append to the parent:
            IHTMLDOMNode anchorAsDom = (IHTMLDOMNode)newAnchor;
            parent.appendChild(anchorAsDom);

            return newAnchor;
        }


        private HTMLElementDictionary getStaticAnchorsDictionary(IHTMLElementCollection anchors)
        {
            var output = new HTMLElementDictionary();

            foreach(IHTMLElement element in anchors)
            {
                if(element.className == AnchorTypes.wlwStaticAnchor.ToString())
                {
                    output.Add(element.id, element);
                }
            }

            return output;
        }


        private HTMLElementDictionary getStaticLinksDictionary(IHTMLElementCollection anchors)
        {
            var output = new HTMLElementDictionary();

            foreach (IHTMLElement element in anchors)
            {
                if (element.className == AnchorTypes.wlwStaticLink.ToString())
                {
                    output.Add(element.id, element);
                }
            }

            return output;
        }


        private int getUniqueAnchorNameIndex(string proposedAnchorName)
        {
            int i = 0;
            string appendIndex = "";
            while (_namedAnchorDictionary.ContainsKey(proposedAnchorName))
            {
                i++;
                appendIndex = "_" + i.ToString();
                proposedAnchorName = proposedAnchorName + appendIndex;
            }

            return i;
        }


        private int getUniqueLinkNameIndex(string proposedLinkName)
        {
            int i = 0;
            string appendIndex = "";
            while (_namedLinkDictionary.ContainsKey(proposedLinkName))
            {
                i++;
                appendIndex = "_" + i.ToString();
                proposedLinkName = proposedLinkName + appendIndex;
            }

            return i;
        }


        private string getAnchorIDFromLinkID(string linkID)
        {
            char[] delim = { ':' };
            string[] arr = linkID.Split(delim);

            return arr[0];
        }


        private void updateLinkReferences(string oldHref, string newHref)
        {
            foreach (IHTMLElement link in _namedLinkDictionary.Values)
            {
                IHTMLAnchorElement anchorElement = (IHTMLAnchorElement)link;
                if (anchorElement.nameProp == oldHref)
                {
                    anchorElement.href = newHref;
                }
            }
        }
    }
}
