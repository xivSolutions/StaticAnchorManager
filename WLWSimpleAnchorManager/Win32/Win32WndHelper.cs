using System;
using System.Runtime.InteropServices;

namespace WLWPluginBase.Win32
{
    public static class Win32WndHelper
    {
        public static Win32Structures.WINDOWPLACEMENT GetPlacement(IntPtr handle)
        {
            Win32Structures.WINDOWPLACEMENT wp = new Win32Structures.WINDOWPLACEMENT();
            wp.Length = Marshal.SizeOf(wp);
            Win32Functions.GetWindowPlacement(handle, ref wp);
            return wp;
        }
        public static void SetPlacement(IntPtr handle, Win32Structures.WINDOWPLACEMENT wp)
        {
            Win32Functions.SetWindowPlacement(handle, ref wp);
        }
    }
}
