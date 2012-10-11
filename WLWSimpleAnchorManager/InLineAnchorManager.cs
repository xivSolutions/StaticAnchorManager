using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WindowsLive.Writer.Api;
using System.Windows.Forms;
using System.Drawing;
using WLWPluginBase.Win32;

using System.Runtime.InteropServices;


namespace WLWSimpleAnchorManager
{
    [WriterPlugin("88472252-60F6-4EEC-A26D-F2001A2E1392",
    "WLW Inline Anchors",
    PublisherUrl = "http://TypeCaseException.com",
    Description =
    "Insert inline html anchors and manage the links to" +
    "anchors within your post from a list",
    ImagePath = "writer.png",
    HasEditableOptions = false)]
    [InsertableContentSource("Inline Anchor")]
    public class InLineAnchorManager : ContentSource
    {
        private bool _initialized = false;

        private static string ANCHOR_ICON_KEY = Properties.Resources.ANCHOR_IMAGE_KEY;
        private static string LINK_ICON_KEY = Properties.Resources.LINK_IMAGE_KEY;
        private const string WNDCLSNAME_IE_SERVER = "Internet Explorer_Server";

        private string _editorHtml;
        private string _selectedHtml;
        private string _selectedText;


        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            return base.CreateContent(dialogOwner, ref content);
        }


        private string ExtractSelectedText(IntPtr owner)
        {
            string selectedText = "";
            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(owner, WNDCLSNAME_IE_SERVER);
            // Determine if it is visible (i.e. active at the time of the request).
            if (item != null)
            {
                selectedText = Win32IEHelper.GetSelectedText(item.Handle);
            }
            return selectedText;
        }


        private string ExtractHtml(IntPtr owner)
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


        private string ExtractSelectedHtml(IntPtr owner)
        {
            string selectedText = "";
            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(owner, WNDCLSNAME_IE_SERVER);
            // Determine if it is visible (i.e. active at the time of the request).
            if (item != null)
            {
                selectedText = Win32IEHelper.GetSelectedHtml(item.Handle);
            }
            return selectedText;
        }


        private String ExtractDelimitedAnchorsList(string PostContent)
        {
            String regExMatchPattern = "(?<=<!--wlwSmartAnchorName:).*?(?=-->)";
            //String regExMatchPattern = "<!--wlwSmartAnchorName:.*?-->";
            MatchCollection matches = Regex.Matches(PostContent, regExMatchPattern);


            StringBuilder sb = new StringBuilder("");
            foreach (Match currentMatch in matches)
            {
                sb.Append(currentMatch.Value + "|");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}
