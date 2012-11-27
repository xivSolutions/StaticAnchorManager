using System;
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

        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            EditorContent currentEditor = new EditorContent(dialogOwner.Handle);
            _editorHtml = currentEditor.EditorHtml;
            _anchorsList = EditorContent.getAnchorNames(_editorHtml);

            try
            {
                // the call to currentEditor.TryGetCurrentElement mwill throw an exception
                // if the current editor selection is not a valid html item. This is related to the 
                // underlying COM basis of the operation, hence, the TRY block here:
                mshtml.IHTMLElement currentElement = currentEditor.TryGetCurrentElement();
                _selectedHtml = currentElement.outerHTML;
                _selectedInnerHtml = currentElement.innerHTML;
                _selectedText = currentElement.innerText;


            }
            catch (Exception)
            {
                MessageBox.Show("You cannot attach an anchor to this type of object");
                return DialogResult.Cancel;
            }

            _currentAnchorName = AnchorData.getAnchorNameFromHtml(_selectedHtml);
            AnchorTypes anchorType = AnchorData.getAnchorTypeFromHtml(_selectedHtml);
            

            _selectedHtml = AnchorData.stripAnchorHtml(_selectedHtml);
            _selectedInnerHtml = AnchorData.stripAnchorHtml(_selectedInnerHtml);

            _selectedHtml = AnchorData.stripLinkHtml(_selectedHtml);
            _selectedInnerHtml = AnchorData.stripLinkHtml(_selectedInnerHtml);

            var anchorSettings = new AnchorData(_currentAnchorName, _selectedText, anchorType);
            AnchorBuilderBase builder;

            using (var frm = new EditContentForm(anchorSettings, _anchorsList))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
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

                        if (_selectedText != _selectedInnerHtml)
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
