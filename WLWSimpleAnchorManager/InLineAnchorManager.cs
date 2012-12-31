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

            _namedAnchorDictionary = this.getStaticAnchorsDictionary(currentEditor.getAnchorCollection());
            _namedLinkDictionary = this.getStaticLinksDictionary(currentEditor.getAnchorCollection());

            // Remember what was selected, in case the user cancels the operation:
            string _selectedText = "";
            string _selectedHtml = "";

            try
            {
                selectedElement = currentEditor.getSelectedElement();

                // REMEMBER THE INITIAL VALUES FROM THE SELECTED ELEMENT:
                
                // Subsequent operations in this scope modify these values in the editor, 
                // and we need to be able to reset them on cancel:
                _selectedText = selectedElement.innerText;
                _selectedHtml = selectedElement.outerHTML;

                selectedAnchor = currentEditor.getSelectedAnchor(selectedElement);

                anchorData.AnchorClass = AnchorTypeHelper.getAnchorTypeFromString(selectedAnchor.className);
                anchorData.AnchorID = selectedAnchor.id;
                anchorData.DisplayText = selectedAnchor.innerText;
                anchorData.TargetAnchorID = this.getAnchorIDFromLinkID(anchorData.AnchorID);
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }

            // Use a string array of anchor names to pass to the Link Editor Form:
            string[] anchorNamesArray = new string[_namedAnchorDictionary.Count];
            _namedAnchorDictionary.Keys.CopyTo(anchorNamesArray, 0);

            using (var editContentForm = new EditContentForm(anchorData, anchorNamesArray))
            {
                if (editContentForm.ShowDialog() == DialogResult.OK)
                {
                    switch (anchorData.AnchorClass)
                    {
                        case AnchorTypes.wlwStaticAnchor:

                            // MAKE SURE THE PROPOSED ANCHOR NAME IS UNIQUE:
                            anchorData.AnchorID = this.uniqueAnchorId(anchorData.AnchorID, selectedAnchor.id);

                            // Capture the original and new AnchorID/href for updating link references
                            // to the current anchor:
                            string oldAnchorID = selectedAnchor.id;
                            string oldHref= "#" + oldAnchorID;
                            string newHref = anchorData.LinkHref;

                            selectedAnchor.id = anchorData.AnchorID;
                            selectedAnchor.innerText = anchorData.DisplayText;
                            selectedAnchor.className = anchorData.AnchorClass.ToString();

                            if (oldAnchorID != selectedAnchor.id)
                            {
                                this.updateLinkReferences(oldHref, newHref);
                            }

                            break;
                        case AnchorTypes.wlwStaticLink:
                            if (selectedAnchor != null)
                            {
                                // MAKE SURE THE PROPOSED LINK NAME IS UNIQUE:
                                selectedAnchor.id = this.uniqueLinkId(anchorData, selectedAnchor.id);

                                selectedAnchor.className = anchorData.AnchorClass.ToString();
                                selectedAnchor.innerText = anchorData.DisplayText;
                                IHTMLAnchorElement anchor = (IHTMLAnchorElement)selectedAnchor;
                                anchor.href = anchorData.LinkHref;
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

            anchorNamesArray = null;
            selectedAnchor = null;
            selectedElement = null;
            anchorData = null;
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


        private string uniqueAnchorId(string newAnchorId, string currentAnchorId)
        {
            string output = newAnchorId;
            int uniqueNameIndex = this.getUniqueAnchorNameIndex(newAnchorId);
            if (uniqueNameIndex > 0 && newAnchorId != currentAnchorId)
            {
                output = newAnchorId + "_" + uniqueNameIndex;
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


        private string uniqueLinkId(AnchorData NewAnchorData, string currentLinkId)
        {
            string proposedID = NewAnchorData.AnchorID + ":" + NewAnchorData.AnchorClass.ToString();
            string output = proposedID;

            int uniqueNameIndex = this.getUniqueLinkNameIndex(output);
            if (uniqueNameIndex > 0 && proposedID != currentLinkId)
            {
                output = proposedID + "_" + uniqueNameIndex;
            }

            return output;
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
