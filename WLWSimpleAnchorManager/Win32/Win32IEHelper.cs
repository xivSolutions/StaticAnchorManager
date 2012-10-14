using System;
using System.Runtime.InteropServices;
using mshtml;

namespace WLWPluginBase.Win32
{
    /// <summary>
    /// Helper class exposing a subset of functionality related to the Win32 object whose
    /// class is <c>Internet Explorer_Server</c>.
    /// </summary>
    public static class Win32IEHelper
    {
        #region Constants
        private const string IE_CLASS_NAME = "Internet Explorer_Server";
        private const string SEL_OBJ_TYPE_NONE = "none";
        private const string SEL_OBJ_TYPE_TEXT = "text";
        private const string SEL_OBJ_TYPE_CONTROL = "control";
        #endregion Constants

        #region Document
        /// <summary>
        /// Gets the Internet Explorer <see cref="IHTMLDocument2"/> object for the given
        /// IE Server control window handle.
        /// </summary>
        /// <param name="hWnd">window handle for which to return the associated <see cref="IHTMLDocument2"/> instance.</param>
        /// <returns><see cref="IHTMLDocument2"/> instance for the given window handle, if any. Otherwise, <c>null</c>.</returns>
        private static IHTMLDocument2 GetIEDocumentFromWindowHandle(IntPtr hWnd)
        {
            // Assume we did not find the document handle.
            IHTMLDocument2 htmlDocument = null;
            if (hWnd != IntPtr.Zero)
            {
                // Register the WM_HTML_GETOBJECT message so it can be used
                // to communicate with the Internet Explorer instance
                uint lMsg = Win32Functions.RegisterWindowMessage("WM_HTML_GETOBJECT");
                // Sends the above registered message to the IE window and
                // waits for it to process it
                UIntPtr lResult;
                Win32Functions.SendMessageTimeout(hWnd, lMsg, UIntPtr.Zero, UIntPtr.Zero,
                    Win32Functions.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out lResult);
                if (lResult != UIntPtr.Zero)
                {
                    // Casts the value returned by the IE window into 
                    //an IHTMLDocument2 interface
                    htmlDocument = Win32Functions.ObjectFromLresult(lResult, typeof(IHTMLDocument).GUID, IntPtr.Zero) as IHTMLDocument2;
                    if (htmlDocument == null)
                    {
                        throw new COMException("Unable to cast to an object of type IHTMLDocument");
                    }
                }
            }
            // Return the resulting document handle.
            return htmlDocument;
        }


        public static IHTMLDocument2 getHtmlDocument(IntPtr hWnd)
        {
            return Win32IEHelper.GetIEDocumentFromWindowHandle(hWnd);
        }


        /// <summary>
        /// Get the underlying HTML code for the given window handle.
        /// </summary>
        /// <param name="handle">IE Server control window handle.</param>
        /// <returns>Underlying HTML code.</returns>
        public static string GetHtml(IntPtr handle)
        {
            IHTMLDocument2 htmlDoc = GetIEDocumentFromWindowHandle(handle);
            return htmlDoc.body.innerHTML;
        }
        /// <summary>
        /// Get the underlying text for the given window handle.
        /// </summary>
        /// <param name="handle">IE Server control window handle.</param>
        /// <returns>Underlying text.</returns>
        public static string GetText(IntPtr handle)
        {
            IHTMLDocument2 htmlDoc = GetIEDocumentFromWindowHandle(handle);
            return htmlDoc.body.innerText;
        }
        /// <summary>
        /// Get the selected HTML code for the given window handle.
        /// </summary>
        /// <param name="handle">IE Server control window handle.</param>
        /// <returns>Selected HTML code.</returns>
        public static string GetSelectedHtml(IntPtr handle)
        {
            string htmlText = string.Empty;
            IHTMLDocument2 htmlDoc = GetIEDocumentFromWindowHandle(handle);
            IHTMLSelectionObject selection = htmlDoc.selection;
            switch (selection.type.ToLower())
            {
                case SEL_OBJ_TYPE_NONE:
                case SEL_OBJ_TYPE_TEXT:
                    {
                    IHTMLTxtRange range = selection.createRange() as IHTMLTxtRange;
                    if (range != null)
                    {
                        htmlText = range.htmlText;
                    }
                    }
                    break;
                case SEL_OBJ_TYPE_CONTROL:
                    {
                    IHTMLControlRange range = selection.createRange() as IHTMLControlRange;
                    if ((range != null) && (range.length == 1))
                    {
                        htmlText = range.item(0).innerHTML;
                    }
                    }
                    break;
            }
            return htmlText;
        }
        /// <summary>
        /// Get the selected text for the given window handle.
        /// </summary>
        /// <param name="handle">IE Server control window handle.</param>
        /// <returns>Selected text.</returns>
        public static string GetSelectedText(IntPtr handle)
        {
            string text = string.Empty;
            IHTMLDocument2 htmlDoc = GetIEDocumentFromWindowHandle(handle);
            IHTMLSelectionObject selection = htmlDoc.selection;
            switch (selection.type.ToLower())
            {
                case SEL_OBJ_TYPE_NONE:
                case SEL_OBJ_TYPE_TEXT:
                    {
                    IHTMLTxtRange range = selection.createRange() as IHTMLTxtRange;
                    if (range != null)
                    {
                        text = range.text;
                    }
                    }
                    break;
                case SEL_OBJ_TYPE_CONTROL:
                    {
                    IHTMLControlRange range = selection.createRange() as IHTMLControlRange;
                    if ((range != null) && (range.length == 1))
                    {
                        text = range.item(0).innerText;
                    }
                    }
                    break;
            }
            return text;
        }
        #endregion Document

        #region Commands
        /// <summary>
        /// Raises the View Source event on the given IE Server control window handle.
        /// </summary>
        /// <param name="handle">IE Server control window handle.</param>
        public static void ViewSource(IntPtr handle)
        {
            Win32EnumWindowsItem item = Win32EnumWindows.FindByClassName(handle, IE_CLASS_NAME);
            if (null != item)
            {
                Win32Functions.SendMessage(item.Handle, (int) Win32Messages.WM_COMMAND,
                    new IntPtr((int) IEMessages.ID_IE_CONTEXTMENU_VIEWSOURCE), IntPtr.Zero);
            }
        }
        #endregion Commands
    }
}
