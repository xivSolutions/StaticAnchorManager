using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WLWPluginBase.Win32
{
    /// <summary>
    /// Grouping of structures exposing Win32 functionality.
    /// </summary>
    /// <remarks>
    /// Definitions have been scanvenged from the net, but mostly come from PInvoke (www.pinvoke.net).
    /// </remarks>
    public class Win32Structures
    {
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
            public static implicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }
            public static implicit operator POINT(Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }
            public int Height
            {
                get { return Bottom - Top; }
            }
            public int Width
            {
                get { return Right - Left; }
            }
            public Size Size
            {
                get { return new Size(Width, Height); }
            }
            public Point Location
            {
                get { return new Point(Left, Top); }
            }
            // Handy method for converting to a System.Drawing.Rectangle
            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }
            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }
            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                    ^ ((Width << 0x1a) | (Width >> 6))
                        ^ ((Height << 7) | (Height >> 0x19));
            }

            #region Operator overloads
            public static implicit operator Rectangle(RECT rect)
            {
                return rect.ToRectangle();
            }
            public static implicit operator RECT(Rectangle rect)
            {
                return FromRectangle(rect);
            }
            #endregion
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int Length;
            public int Flags;
            public int ShowCmd;
            public POINT MinPosition;
            public POINT MaxPosition;
            public RECT NormalPosition;
            public static WINDOWPLACEMENT Default
            {
                get
                {
                    WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                    result.Length = Marshal.SizeOf(result);
                    return result;
                }
            }
        }
    }
}