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

        private IHTMLElement _selectedElement;
        private IHTMLElement _selectedAnchor;

        private string _editorHtml;
        private HTMLElementDictionary _namedAnchorDictionary;
        private string[] _anchorNames;

        private AnchorData _anchorData;


        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            EditorContent currentEditor = new EditorContent(dialogOwner.Handle);

            // Current editor contents:
            _editorHtml = currentEditor.EditorHtml;
            _namedAnchorDictionary = this.getStaticAnchorsDictionary(currentEditor.getAnchorCollection());
            _anchorNames = new string[_namedAnchorDictionary.Count];
            _namedAnchorDictionary.Keys.CopyTo(_anchorNames, 0);

            try
            {
                // Is a valid anchor element currently selected in the editor?
                _selectedAnchor = currentEditor.TryGetAnchorFromSelection();
                _selectedElement = currentEditor.TryGetCurrentElement();

                _anchorData = new AnchorData();

                if (_selectedAnchor == null)
                {
                    _anchorData.DisplayText = _selectedElement.innerText;
                    _anchorData.AnchorClass = AnchorTypes.None;
                }
                else
                {
                    _anchorData.AnchorClass = AnchorTypeHelper.getAnchorTypeFromString(_selectedAnchor.className);
                    _anchorData.AnchorID = _selectedAnchor.id;
                    _anchorData.DisplayText = _selectedAnchor.innerText;
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
                    /*
                     * make sure the proposed anchor ID is unique:
                     */
                    string proposedAnchorName = _anchorData.AnchorID;
                    int uniqueNameIndex = currentEditor.getUniqueAnchorNameIndex(proposedAnchorName);
                    if (uniqueNameIndex > 0 && _anchorData.AnchorID != _selectedAnchor.id)
                    {
                        _anchorData.AnchorID = _anchorData.AnchorID + "_" + uniqueNameIndex;
                    }

                    AnchorBuilderBase builder;

                    switch(_anchorData.AnchorClass)
                    {

                        case AnchorTypes.wlwStaticAnchor:
                            if (_selectedAnchor != null)
                            {
                                _selectedAnchor.id = _anchorData.AnchorID;
                                _selectedAnchor.innerText = _anchorData.DisplayText;
                                content = _selectedAnchor.outerHTML;
                            }
                            else
                            {
                                builder = new AnchorBuilder(_anchorData);
                                _selectedElement.innerHTML = builder.getPublishHtml(_selectedElement.innerText);
                                content = _selectedElement.outerHTML;
                            }

                            break;
                        case AnchorTypes.wlwStaticLink:
                            if (_selectedAnchor != null)
                            {
                                IHTMLAnchorElement anchor = (IHTMLAnchorElement)_selectedAnchor;
                                anchor.href = _anchorData.LinkReference;
                                _selectedAnchor.innerText = _anchorData.DisplayText;
                            }
                            else
                            {
                                builder = new LinkBuilder(_anchorData);
                                _selectedElement.innerHTML = builder.getPublishHtml(_selectedElement.innerText);
                            }
                            break;
                        default:

                            break;
                    }


                }
            }

            _anchorNames = null;

            //if (_selectedAnchor == null) // No anchor exists
            //{
            //    // does the current editor selection contain a WLWSAM Anchor or Link?

            //    return DialogResult.OK;
            //}
            //else // Otherwise, do nothing
            //{
            //    return DialogResult.Cancel;
            //}
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
    }
}
