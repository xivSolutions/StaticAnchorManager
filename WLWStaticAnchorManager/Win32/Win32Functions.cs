using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WLWPluginBase.Win32
{
    /// <summary>
    /// Grouping of functions exposing Win32 functionality.
    /// </summary>
    /// <remarks>
    /// Definitions have been scanvenged from the net, but mostly come from PInvoke (www.pinvoke.net).
    /// </remarks>
    public static class Win32Functions
    {
        #region Flags

        #region user32.dll
        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }
        #endregion user32.dll

        #endregion Flags

        #region DLL Imports

        #region oleacc.dll
        [DllImport("oleacc.dll", PreserveSig = false)]
        [return : MarshalAs(UnmanagedType.Interface)]
        public static extern object ObjectFromLresult(UIntPtr lResult,
                                                      [MarshalAs(UnmanagedType.LPStruct)] Guid refiid, IntPtr wParam);
        #endregion oleacc.dll

        #region user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindow(string ClassName, string WindowText);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref Win32Structures.WINDOWPLACEMENT lpwndpl);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg,
                                                       UIntPtr wParam, UIntPtr lParam, SendMessageTimeoutFlags fuFlags,
                                                       uint uTimeout, out UIntPtr lpdwResult);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref Win32Structures.WINDOWPLACEMENT lpwndpl);
        #endregion user32.dll

        #endregion DLL Imports
    }
}