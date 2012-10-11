///////////////////////////////////////////////////////////////////////////////
// Contents originally created by some poor sole whose name I forget. If you
// wrote this code please let me know and I will be happy to give you credit.
//
// I simply take credit for some clean-up and refactoring to conform with the
// rest of the project.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WLWPluginBase.Win32
{
    #region Flags
    /// <summary>
    /// Window Style Flags
    /// </summary>
    [Flags]
    public enum WindowStyleFlags : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
    }

    /// <summary>
    /// Extended Windows Style Flags
    /// </summary>
    [Flags]
    public enum ExtendedWindowStyleFlags : int
    {
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_LAYERED = 0x00080000,
        WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
        WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000
    }
    #endregion Flags

    #region Win32EnumWindows
    /// <summary>
    /// Exposes Win32 functionality to enumerate windows.
    /// </summary>
    public class Win32EnumWindows
    {
        #region Static
        private static readonly Win32EnumWindows _theOne = new Win32EnumWindows();
        #endregion Static

        #region Delegates
        private delegate int EnumWindowsProc(IntPtr hwnd, int lParam);
        #endregion

        #region UnManagedMethods
        private class UnManagedMethods
        {
            [DllImport("user32")]
            public static extern int EnumWindows(
                EnumWindowsProc lpEnumFunc,
                int lParam);
            [DllImport("user32")]
            public static extern int EnumChildWindows(
                IntPtr hWndParent,
                EnumWindowsProc lpEnumFunc,
                int lParam);
        }
        #endregion

        #region Member Variables
        private Win32EnumWindowsCollection items = null;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the collection of windows returned by <see cref="GetWindows()"/>.
        /// </summary>
        public Win32EnumWindowsCollection Items
        {
            get { return items; }
        }
        #endregion Properties

        #region Operations
        /// <summary>
        /// Gets all top level windows on the system.
        /// </summary>
        public void GetWindows()
        {
            items = new Win32EnumWindowsCollection();
            UnManagedMethods.EnumWindows(WindowEnum, 0);
        }
        /// <summary>
        /// Gets all child windows of the specified window
        /// </summary>
        /// <param name="hWndParent">window handle for which to get children.</param>
        public void GetWindows(IntPtr hWndParent)
        {
            items = new Win32EnumWindowsCollection();
            UnManagedMethods.EnumChildWindows(hWndParent, WindowEnum, 0);
        }
        #endregion Operations

        #region Search Operations
        public static Win32EnumWindowsItem FindByClassName(IntPtr hWndParent, string className)
        {
            Win32EnumWindowsItem item = null;
            _theOne.GetWindows(hWndParent);
            foreach (Win32EnumWindowsItem wi in _theOne.Items)
            {
                if (className.Equals(wi.ClassName))
                {
                    item = wi;
                    break;
                }
            }
            return item;
        }
        #endregion Search Operations

        #region Win32EnumWindows callback
        /// <summary>
        /// The enum Windows callback.
        /// </summary>
        /// <param name="handle">Window Handle</param>
        /// <param name="lParam">Application defined value</param>
        /// <returns>1 to continue enumeration, 0 to stop</returns>
        private int WindowEnum(IntPtr handle, int lParam)
        {
            return (OnWindowEnum(handle) ? 1 : 0);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Called whenever a new window is about to be added
        /// by the Window enumeration called from GetWindows.
        /// If overriding this function, return true to continue
        /// enumeration or false to stop.  If you do not call
        /// the base implementation the Items collection will
        /// be empty.
        /// </summary>
        /// <param name="handle">Window handle to add</param>
        /// <returns>True to continue enumeration, False to stop</returns>
        protected virtual bool OnWindowEnum(IntPtr handle)
        {
            items.Add(handle);
            return true;
        }
        #endregion Event Handlers

    }
    #endregion Win32EnumWindows

    #region Win32EnumWindowsCollection
    /// <summary>
    /// Holds a collection of Windows returned by <see cref="Win32EnumWindows#GetWindows()"/>.
    /// </summary>
    public class Win32EnumWindowsCollection : ReadOnlyCollectionBase
    {
        /// <summary>
        /// Add a new Window to the collection.  Intended for
        /// internal use by Win32EnumWindows only.
        /// </summary>
        /// <param name="handle">Window handle to add</param>
        public void Add(IntPtr handle)
        {
            Win32EnumWindowsItem item = new Win32EnumWindowsItem(handle);
            InnerList.Add(item);
        }
        /// <summary>
        /// Gets the Window at the specified index
        /// </summary>
        public Win32EnumWindowsItem this[int index]
        {
            get { return (Win32EnumWindowsItem) InnerList[index]; }
        }
    }
    #endregion Win32EnumWindowsCollection

    #region Win32EnumWindowsItem
    /// <summary>
    /// Provides details about a Window returned by the enumeration.
    /// </summary>
    public class Win32EnumWindowsItem
    {
        #region Structures
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct FLASHWINFO
        {
            public int cbSize;
            public IntPtr hwnd;
            public int dwFlags;
            public int uCount;
            public int dwTimeout;
        }
        #endregion

        #region UnManagedMethods
        private class UnManagedMethods
        {
            [DllImport("user32")]
            public static extern int IsWindowVisible(
                IntPtr hWnd);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern int GetWindowText(
                IntPtr hWnd,
                StringBuilder lpString,
                int cch);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern int GetWindowTextLength(
                IntPtr hWnd);
            [DllImport("user32")]
            public static extern int BringWindowToTop(IntPtr hWnd);
            [DllImport("user32")]
            public static extern int SetForegroundWindow(IntPtr hWnd);
            [DllImport("user32")]
            public static extern int IsIconic(IntPtr hWnd);
            [DllImport("user32")]
            public static extern int IsZoomed(IntPtr hwnd);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern int GetClassName(
                IntPtr hWnd,
                StringBuilder lpClassName,
                int nMaxCount);
            [DllImport("user32")]
            public static extern int FlashWindow(
                IntPtr hWnd,
                ref FLASHWINFO pwfi);
            [DllImport("user32")]
            public static extern int GetWindowRect(
                IntPtr hWnd,
                ref RECT lpRect);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern int SendMessage(
                IntPtr hWnd,
                int wMsg,
                IntPtr wParam,
                IntPtr lParam);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern uint GetWindowLong(
                IntPtr hwnd,
                int nIndex);
            public const int WM_SYSCOMMAND = 0x112;
            public const int SC_RESTORE = 0xF120;
            public const int SC_MAXIMIZE = 0xF030;
            public const int SC_MINIMIZE = 0xF020;
            public const int GWL_STYLE = (-16);
            public const int GWL_EXSTYLE = (-20);
        }
        #endregion UnManagedMethods

        #region Fields
        /// <summary>
        /// The window handle.
        /// </summary>
        private readonly IntPtr handle = IntPtr.Zero;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the window's handle
        /// </summary>
        public IntPtr Handle
        {
            get { return handle; }
        }
        /// <summary>
        /// Gets the window's title (caption)
        /// </summary>
        public string Text
        {
            get
            {
                StringBuilder title = new StringBuilder(260, 260);
                UnManagedMethods.GetWindowText(handle, title, title.Capacity);
                return title.ToString();
            }
        }
        /// <summary>
        /// Gets the window's class name.
        /// </summary>
        public string ClassName
        {
            get
            {
                StringBuilder className = new StringBuilder(260, 260);
                UnManagedMethods.GetClassName(handle, className, className.Capacity);
                return className.ToString();
            }
        }
        /// <summary>
        /// Gets/Sets whether the window is iconic (mimimised) or not.
        /// </summary>
        public bool Iconic
        {
            get { return ((UnManagedMethods.IsIconic(handle) == 0) ? false : true); }
            set
            {
                UnManagedMethods.SendMessage(
                    handle,
                    UnManagedMethods.WM_SYSCOMMAND,
                    (IntPtr) UnManagedMethods.SC_MINIMIZE,
                    IntPtr.Zero);
            }
        }
        /// <summary>
        /// Gets/Sets whether the window is maximised or not.
        /// </summary>
        public bool Maximised
        {
            get { return ((UnManagedMethods.IsZoomed(handle) == 0) ? false : true); }
            set
            {
                UnManagedMethods.SendMessage(
                    handle,
                    UnManagedMethods.WM_SYSCOMMAND,
                    (IntPtr) UnManagedMethods.SC_MAXIMIZE,
                    IntPtr.Zero);
            }
        }
        /// <summary>
        /// Gets whether the window is visible.
        /// </summary>
        public bool Visible
        {
            get { return ((UnManagedMethods.IsWindowVisible(handle) == 0) ? false : true); }
        }
        /// <summary>
        /// Gets the bounding rectangle of the window
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                RECT rc = new RECT();
                UnManagedMethods.GetWindowRect(
                    handle,
                    ref rc);
                Rectangle rcRet = new Rectangle(
                    rc.Left, rc.Top,
                    rc.Right - rc.Left, rc.Bottom - rc.Top);
                return rcRet;
            }
        }
        /// <summary>
        /// Gets the location of the window relative to the screen.
        /// </summary>
        public Point Location
        {
            get
            {
                Rectangle rc = Rect;
                Point pt = new Point(
                    rc.Left,
                    rc.Top);
                return pt;
            }
        }
        /// <summary>
        /// Gets the size of the window.
        /// </summary>
        public Size Size
        {
            get
            {
                Rectangle rc = Rect;
                Size sz = new Size(rc.Right - rc.Left, rc.Bottom - rc.Top);
                return sz;
            }
        }
        public WindowStyleFlags WindowStyle
        {
            get
            {
                return (WindowStyleFlags) UnManagedMethods.GetWindowLong(
                    handle, UnManagedMethods.GWL_STYLE);
            }
        }
        public ExtendedWindowStyleFlags ExtendedWindowStyle
        {
            get
            {
                return (ExtendedWindowStyleFlags) UnManagedMethods.GetWindowLong(
                    handle, UnManagedMethods.GWL_EXSTYLE);
            }
        }
        #endregion Properties

        #region Initialization
        /// <summary>
        /// Constructs a new instance of this class for the specified Window Handle.
        /// </summary>
        /// <param name="handle">The Window Handle</param>
        public Win32EnumWindowsItem(IntPtr handle)
        {
            this.handle = handle;
        }
        #endregion Initialization

        #region Operations
        /// <summary>
        /// To allow items to be compared, the hash code is set to the window handle, so two
        /// <see cref="Win32EnumWindowsItem"/> objects for the same window will be equal.
        /// </summary>
        /// <returns>The Window Handle for this window</returns>
        public override Int32 GetHashCode()
        {
            return (Int32) handle;
        }
        /// <summary>
        /// Restores and Brings the window to the front, 
        /// assuming it is a visible application window.
        /// </summary>
        public void Restore()
        {
            if (Iconic)
            {
                UnManagedMethods.SendMessage(
                    handle,
                    UnManagedMethods.WM_SYSCOMMAND,
                    (IntPtr) UnManagedMethods.SC_RESTORE,
                    IntPtr.Zero);
            }
            UnManagedMethods.BringWindowToTop(handle);
            UnManagedMethods.SetForegroundWindow(handle);
        }
        #endregion Operations
    }
    #endregion EnumwindowsItem
}