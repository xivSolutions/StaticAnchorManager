using System;
using System.Windows.Forms;
using mshtml;
using WindowsLive.Writer.Api;

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

            content = "";

            EditorContent currentEditor = new EditorContent(dialogOwner.Handle);

            _namedAnchorDictionary = currentEditor.getStaticAnchorsDictionary();
            _namedLinkDictionary = currentEditor.getStaticLinksDictionary();

            // Remember what was selected, in case the user cancels the operation:
            string _selectedText = "";
            string _selectedHtml = "";

            try
            {
                selectedElement = currentEditor.getSelectedElement();

                // REMEMBER THE INITIAL VALUES FROM THE SELECTED 
                // ELEMENT BEFORE CALLING GetSelectedAnchor!

                // Subsequent operations in this scope modify these values in the editor, 
                // and we need to be able to reset them on cancel:
                _selectedText = selectedElement.innerText;
                _selectedHtml = selectedElement.outerHTML;

                // Processes within this call may modify contents of selectedElement. 
                // This call must FOLLOW CAPTURE of innerText and outerHtml as above:
                selectedAnchor = currentEditor.getSelectedAnchor(selectedElement);

                anchorData.AnchorClass = AnchorClassSelector.selectByName(selectedAnchor.className);
                anchorData.AnchorID = selectedAnchor.id;
                anchorData.DisplayText = selectedAnchor.innerText;

                /*
                 * The ID of the anchor that is the target of a static link can be parsed
                 * from the link ID (composed of anchorID + LInk Class Name and possibly
                 * an integer if more than one link to same anchor)
                 */ 
                anchorData.TargetAnchorID = this.getAnchorIDFromLinkID(anchorData.AnchorID);
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }

            // get string array of anchor names to pass to the Link Editor Form:
            string[] anchorNamesArray = new string[_namedAnchorDictionary.Count];
            _namedAnchorDictionary.Keys.CopyTo(anchorNamesArray, 0);

            using (var editContentForm = 
                new EditContentForm(anchorData, anchorNamesArray))
            {
                if (editContentForm.ShowDialog() == DialogResult.OK)
                {
                    switch (anchorData.AnchorClass)
                    {
                        case AnchorClass.wlwStaticAnchor:

                            anchorData.AnchorID = this.getUniqueAnchorId(anchorData.AnchorID, selectedAnchor.id);
                            /* 
                             * Capture the original and new AnchorID/href for updating 
                             * links which refer to the current anchor 
                             * (in case the AnchorID has been Modified):
                             */
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

                        case AnchorClass.wlwStaticLink:
                            selectedAnchor.id = this.getUniqueLinkId(anchorData, selectedAnchor.id);
                            selectedAnchor.className = anchorData.AnchorClass.ToString();
                            selectedAnchor.innerText = anchorData.DisplayText;
                            IHTMLAnchorElement anchor = (IHTMLAnchorElement)selectedAnchor;
                            anchor.href = anchorData.LinkHref;
                            break;
                    }
                }
                else
                {
                    // Reset the contents of the originally selected element:
                    selectedElement.outerHTML = _selectedHtml;
                    return DialogResult.Cancel;
                }
            }

            // Make sure that the text contained within the anchor is selected in the editor:
            currentEditor.MoveSelectionToElementText(selectedAnchor);

            if (selectedAnchor.innerText != null)
            {
                /*
                 * ref variable content will replace whatever the 
                 * current selection in the editor contains. Make sure the text that is replaced
                 * represents the inner text of the Anchor Element, or weird shit happens. 
                 */
                content = selectedAnchor.innerText;
            }

            /* 
             * These need to be nulled out, because they are not
             * re-initialized with successive uses of the plugin:
             */ 
            _namedAnchorDictionary = null;
            _namedLinkDictionary = null;

            return DialogResult.OK;
        }


        private string getUniqueAnchorId(string newAnchorId, string currentAnchorId)
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


        private string getUniqueLinkId(AnchorData NewAnchorData, string currentLinkId)
        {
            // Link Id's are a concatenation of the target anchorID and the Link Class:
            string proposedID = NewAnchorData.AnchorID + ":" + NewAnchorData.AnchorClass.ToString();
            string output = proposedID;

            // Uniqueness is created by incrementing integer:
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
            string oldAnchorID = oldHref.Replace("#", "");
            string newAnchorID = newHref.Replace("#", "");

            foreach (IHTMLElement link in _namedLinkDictionary.Values)
            {
                IHTMLAnchorElement anchorElement = (IHTMLAnchorElement)link;
                if (anchorElement.nameProp == oldHref)
                {
                    // Use the IHTMLElement Interface to replace the ID portion of the link ID
                    link.id = link.id.Replace(oldAnchorID, newAnchorID);

                    // Use the IHTMLAnchorElement interface to set the new href:
                    anchorElement.href = newHref;
                }
            }
        }

    }
}
