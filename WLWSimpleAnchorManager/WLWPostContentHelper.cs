using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WindowsLive.Writer.Api;
using System.Windows.Forms;
using System.Drawing;
using WLWPluginBase.Win32;
using mshtml;

namespace WLWSimpleAnchorManager
{
    public class WLWPostContentHelper
    {
        private const string WNDCLSNAME_IE_SERVER = "Internet Explorer_Server";
        private const char ANCHOR_LIST_DELIMITER = '|';

        public static IHTMLDocument2 getHtmlDocument(IntPtr owner)
        {
            IHTMLDocument2 output = null;

            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(owner, WNDCLSNAME_IE_SERVER);
            // Determine if it is visible (i.e. active at the time of the request).
            if (item != null)
            {
                output = Win32IEHelper.getHtmlDocument(item.Handle);
            }
            return output;
        }


        public static string ExtractSelectedText(IntPtr owner)
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


        public static string ExtractHtml(IntPtr owner)
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


        public static string ExtractSelectedHtml(IntPtr owner)
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


        public static string getAnchorNameFromHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                String regExMatchPattern = "(?<=wlwSmartAnchorName:).*?(?=\\s|>|\")";
                Match anchorMatch = Regex.Match(selectedHtml, regExMatchPattern);
                if (anchorMatch.Success)
                {
                    output = anchorMatch.Value;
                }
            }

            return output;
        }

        
        public static string ExtractDelimitedAnchorsList(string PostContent)
        {
            String regExMatchPattern = "(?<=wlwSmartAnchorName:).*?(?=\\s|>|\")";
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


        public static string[] getAnchorNames(string editorHtml)
        {
            string delimitedList = WLWPostContentHelper.ExtractDelimitedAnchorsList(editorHtml);
            return WLWPostContentHelper.getAnchorNamesFromDelimitedString(delimitedList);
        }


        public static string[] getAnchorNamesFromDelimitedString(string delimitedAnchorsList)
        {
            return delimitedAnchorsList.Split(ANCHOR_LIST_DELIMITER);
        }


        public static string stripAnchorHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                Regex rgx = new Regex("<A\\sname=wlwSmartAnchorName:.*?(>|\\s+>)");
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }
    }
}
