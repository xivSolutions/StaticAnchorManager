﻿using System;
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

        public static string LinkTagRegexPattern = "<A\\s.*name=" + AnchorBuilderBase.wlwLinkToAnchorFlag + ":.*?(>|\\s+>)";
        public static string AnchorTagRegexPattern = "<A\\sname=" + AnchorBuilderBase.wlwAnchorFlag + ":.*?(>|\\s+>)";


        public static string getAnchorNameFromHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                String regExMatchPattern = "(?<=name=" + AnchorBuilderBase.wlwAnchorFlag + ":).*?(?=\\s|>|\")";
                Match anchorMatch = Regex.Match(selectedHtml, regExMatchPattern);
                if (anchorMatch.Success)
                {
                    output = anchorMatch.Value;
                }
            }

            return output;
        }


        public static AnchorTypes getAnchorTypeFromHtml(string selectedHtml)
        {

            AnchorTypes output = AnchorTypes.None;

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                String regExMatchPattern = "(?<=name=)(" + AnchorBuilderBase.wlwAnchorFlag + "|" + AnchorBuilderBase.wlwLinkToAnchorFlag + ").*?(?=:)(?=.*?(\\s+|\"|>))";
                Match anchorTypeMatch = Regex.Match(selectedHtml, regExMatchPattern);
                if (anchorTypeMatch.Success)
                {
                    output = AnchorTypeHelper.getAnchorTypeFromString(anchorTypeMatch.Value.Replace("wlw", ""));
                }
            }

            return output;
        }

        
        public static string ExtractDelimitedAnchorsList(string PostContent)
        {
            String regExMatchPattern = "(?<=name=" + AnchorBuilderBase.wlwAnchorFlag + ":).*?(?=\\s|>|\")";
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
                Regex rgx = new Regex(WLWPostContentHelper.AnchorTagRegexPattern);
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }


        public static string stripLinkHtml(string selectedHtml)
        {
            string output = "";

            if (!string.IsNullOrEmpty(selectedHtml))
            {
                Regex rgx = new Regex(WLWPostContentHelper.LinkTagRegexPattern);
                output = rgx.Replace(selectedHtml, "");
                output = output.Replace("</A>", "");
            }

            return output;
        }
    }
}
