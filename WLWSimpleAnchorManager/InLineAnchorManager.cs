using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsLive.Writer.Api;


namespace WLWSimpleAnchorManager
{
    [WriterPlugin("5439640A-F022-4A52-8BB6-3F9983D96153",
    "WLW Anchor Manager",
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

        private string _editorHtml;
        private string _selectedHtml = "";
        private string _selectedText = "";
        private string _selectedInnerHtml = "";
        private string[] _anchorsList;

        private string _currentAnchorName = "";
        private string _LinkTargetName = "";

        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {

        /* 
         * GET THE STUFF WE NEED TO:
         * ** CREATE OR EDIT AN IN_PAGE ANCHOR OR
         * ** LINK TO AN INPAGE ANCHOR:
        */

             /* currentEditor creates a reference to the IHTMLDocument2 representing the 
             * html in the WLW editor window, and provides methods to 
             * traverse/exctract/manipulate the DOM and html:
             */
            EditorContent currentEditor = new EditorContent(dialogOwner.Handle);

            // Current editor contents:
            _editorHtml = currentEditor.EditorHtml;

            /* 
             * A list of the user-friendly id names static anchors already present 
             * (created by this plug-in). 
             * This list includes the actual anchors, but not links to those anchors. 
             * This list will be displayed in a listview control from which the user
             * can choose to link to an existing anchor. 
             */
            _anchorsList = EditorContent.getAnchorNames(_editorHtml);

            
            try
            {

                SelectedEditorContent selectedContent = currentEditor.SelectedContent();
                _selectedHtml = selectedContent.SelectedHtml;
                _selectedInnerHtml = selectedContent.InnerHtml;
                _selectedText = selectedContent.SelectedText;

                ///* 
                // * the call to currentEditor.TryGetCurrentElement will throw an exception
                // * if the current editor selection is not a valid html item. This is related to the 
                // * underlying COM basis of the operation, hence, the TRY block here:
                // */
                //mshtml.IHTMLElement currentElement = currentEditor.TryGetCurrentElement();

                ///* 
                // * The current html seleScted in the editor, or within which the cursor 
                // * is currently located. This includes any relevant formatting tags surrounding the
                // * visible text, and any pre-existing WLW plug-in anchor or link-to-anchor markup:
                // * */
                //_selectedHtml = currentElement.outerHTML;

                ///*
                // * Visible text contained within the current element which has additional formatting
                // * tags (such as <em> or <pre>) will mess with our eventual insertion of a 
                // * well-formed anchor or link element. We need to be able to test for this later, 
                // * using the _selectedInnerHtml variable:
                // */
                //_selectedInnerHtml = currentElement.innerHTML;

                ///* The visible text contained within the html element */
                //_selectedText = currentElement.innerText;
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }

            // We need to know the user-friendly name assigned to the selected anchor, the type of anchor,
            // and the static page anchor targeted (if this anchor is a link to a in-page target):
            _currentAnchorName = AnchorData.getAnchorNameFromHtml(_selectedHtml);
            AnchorTypes anchorType = AnchorData.getAnchorTypeFromHtml(_selectedHtml);

            // This value will be an empty string if the current selection is not a link to an 
            // in-page anchor, or if the current selection represents a new anchor:
            _LinkTargetName = AnchorData.getFriendlyLinkTargetIdFromHtml(_selectedHtml);
            
            // Now that we have what we need to create or edit an anchor or link, strip previous anchor 
            // Markup created by this plug-in from the existing html. If there is not an existing anchor seleced, 
            // nothing wil be changed:
            _selectedHtml = AnchorData.stripAnchorHtml(_selectedHtml);


            _selectedInnerHtml = AnchorData.stripAnchorHtml(_selectedInnerHtml);

            _selectedHtml = AnchorData.stripLinkHtml(_selectedHtml);
            if (_selectedHtml == _selectedText)
            {
                _selectedHtml = "";
            }

            _selectedInnerHtml = AnchorData.stripLinkHtml(_selectedInnerHtml);

            var anchorSettings = new AnchorData(_currentAnchorName, _selectedText, anchorType);
            anchorSettings.LinkTargetAnchorId = _LinkTargetName;

            AnchorBuilderBase builder;

            using (var frm = new EditContentForm(anchorSettings, _anchorsList))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    
                    string proposedAnchorName = anchorSettings.currentInstanceID();
                    int uniqueNameIndex = currentEditor.getUniqueAnchorNameIndex(proposedAnchorName);
                    if (uniqueNameIndex > 0 && anchorSettings.FriendlyAnchorName != _currentAnchorName)
                    {
                        anchorSettings.FriendlyAnchorName = anchorSettings.FriendlyAnchorName + "_" + uniqueNameIndex;
                    }
                    
                    switch(anchorSettings.AnchorType)
                    {
                        case AnchorTypes.Anchor:
                            builder = new AnchorBuilder(anchorSettings);
                            break;
                        case AnchorTypes.Link:
                            builder = new LinkBuilder(anchorSettings);
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
                        if (_selectedInnerHtml != "" && _selectedText != _selectedInnerHtml)
                        {
                            _selectedText = _selectedInnerHtml;
                        }

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
    }
}
