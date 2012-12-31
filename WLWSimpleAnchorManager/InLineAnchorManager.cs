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

        //private IHTMLElement _selectedElement;
        //private IHTMLElement _selectedAnchor;

        private HTMLElementDictionary _namedAnchorDictionary;
        private HTMLElementDictionary _namedLinkDictionary;
        private string[] _anchorNames;

        //private AnchorData _anchorData;


        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            AnchorData _anchorData = new AnchorData();
            IHTMLElement _selectedElement;
            IHTMLElement _selectedAnchor;

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
                _selectedElement = currentEditor.TryGetCurrentElement();
                if (_selectedElement == null)
                {
                    _selectedElement = currentEditor.InsertNewContainerElement();

                }


                // Is a valid anchor element currently selected in the editor?
                _selectedAnchor = currentEditor.TryGetAnchorFromSelection();

                if (_selectedAnchor == null)
                {
                    IHTMLElementCollection children = (IHTMLElementCollection)_selectedElement.children;
                    foreach (IHTMLElement child in children)
                    {
                        if (child.tagName == "A")
                        {
                            _selectedAnchor = child;
                        }
                    }
                }

                // Remember what was selected, and case the user cancels the operation
                _selectedText = _selectedElement.innerText;
                _selectedHtml = _selectedElement.outerHTML;

                
                if (_selectedAnchor == null)
                {
                    // We need to zero these out before adding the new 
                    // Child, or they are appended to the existing values (kind of). 
                    // We are essentially moving the visible content of the existing html from
                    // the currently selected element into the new anchor element:
                    _selectedElement.innerText = null;
                    _selectedElement.innerHTML = null;

                    // We need an IHTMLDOMNode interface to use the appendChild method
                    // on the parent element:
                    IHTMLDOMNode parent = (IHTMLDOMNode)_selectedElement;
                    _selectedAnchor = currentEditor.InsertNewAnchor();
                    _selectedAnchor.innerText = _selectedText;

                    // Once we have created the new anchor element, we need to 
                    // cast it as an IHTMLDOMNode in order to append to the parent:
                    IHTMLDOMNode anchorAsDom = (IHTMLDOMNode)_selectedAnchor;
                    parent.appendChild(anchorAsDom);

                    _anchorData.DisplayText = _selectedAnchor.innerText;
                }
                else
                {
                    _anchorData.AnchorClass = AnchorTypeHelper.getAnchorTypeFromString(_selectedAnchor.className);
                    _anchorData.AnchorID = _selectedAnchor.id;
                    _anchorData.DisplayText = _selectedAnchor.innerText;
                    _anchorData.TargetAnchorID = this.getAnchorIDFromLinkID(_selectedAnchor.id);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }


            using (var editContentForm = new EditContentForm(_anchorData, _anchorNames))
            {
                if (editContentForm.ShowDialog() == DialogResult.OK)
                {
                    switch (_anchorData.AnchorClass)
                    {

                        case AnchorTypes.wlwStaticAnchor:

                            /*
                             * make sure the proposed anchor ID is unique:
                             */
                            int uniqueNameIndex = this.getUniqueAnchorNameIndex(_anchorData.AnchorID);
                            if (uniqueNameIndex > 0 && _anchorData.AnchorID != _selectedAnchor.id)
                            {
                                _anchorData.AnchorID = _anchorData.AnchorID + "_" + uniqueNameIndex;
                            }

                            // Capture the original AnchorID for updating link references:
                            string oldAnchorID = _selectedAnchor.id;
                            string oldHref= "#" + oldAnchorID;
                            string newHref = _anchorData.LinkHref;

                            if (_selectedAnchor != null)
                            {
                                _selectedAnchor.id = _anchorData.AnchorID;
                                _selectedAnchor.innerText = _anchorData.DisplayText;
                                _selectedAnchor.className = _anchorData.AnchorClass.ToString();
                            }

                            if (oldAnchorID != _selectedAnchor.id)
                            {
                                this.updateLinkReferences(oldHref, newHref);
                            }

                            break;
                        case AnchorTypes.wlwStaticLink:
                            if (_selectedAnchor != null)
                            {
                                /*
                                 * Make sure the proposed Link ID is unique:
                                 */
                                string proposedID = _anchorData.AnchorID + ":" + _anchorData.AnchorClass.ToString();
                                uniqueNameIndex = this.getUniqueLinkNameIndex(proposedID);
                                if (uniqueNameIndex > 0 && proposedID != _selectedAnchor.id)
                                {
                                    _selectedAnchor.id = proposedID + "_" + uniqueNameIndex;
                                }
                                else
                                {
                                    _selectedAnchor.id = proposedID;
                                }

                                _selectedAnchor.className = _anchorData.AnchorClass.ToString();
                                _selectedAnchor.innerText = _anchorData.DisplayText;
                                IHTMLAnchorElement anchor = (IHTMLAnchorElement)_selectedAnchor;
                                anchor.href = _anchorData.LinkHref;
                                //content = _selectedAnchor.innerText;
                            }
                            break;
                    }
                }
                else
                {
                    _selectedElement.outerHTML = _selectedHtml;
                    return DialogResult.Cancel;
                }
            }

            currentEditor.MoveSelectionToElementText(_selectedAnchor);

            if (_selectedAnchor.innerText != null)
            {
                content = _selectedAnchor.innerText;
            }

            _anchorNames = null;
            _selectedAnchor = null;
            _selectedElement = null;
            _anchorData = null;
            _namedAnchorDictionary = null;
            _namedLinkDictionary = null;

            return DialogResult.OK;

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
