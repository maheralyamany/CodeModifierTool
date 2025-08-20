using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
/// <summary>Represents: ShellIconHelper</summary>

public static class ShellIconHelper
{
    /// <summary>Performs s h get file info</summary>
    /// <param name="pszPath">The pszPath</param>
    /// <param name="dwFileAttributes">The dwFileAttributes</param>
    /// <param name="psfi">The psfi</param>
    /// <param name="cbFileInfo">The cbFileInfo</param>
    /// <param name="uFlags">The uFlags</param>
    /// <returns>The result of the s h get file info</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when dwFileAttributes is null</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when psfi is null</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when cbFileInfo is null</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when uFlags is null</exception>

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    [CLSCompliant(false)]

    public static extern IntPtr SHGetFileInfo(string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

    /// <summary>Represents struct: SHFILEINFO</summary>

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    public const uint SHGFI_ICON = 0x000000100;
    public const uint SHGFI_SMALLICON = 0x000000001;


    /// <summary>Gets small icon</summary>
    /// <param name="filePath">The filePath</param>
    /// <returns>The retrieved small icon</returns>

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Icon GetSmallIcon(string filePath)
    {
        SHFILEINFO shinfo = new SHFILEINFO();
        SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
        return Icon.FromHandle(shinfo.hIcon);
    }
}
