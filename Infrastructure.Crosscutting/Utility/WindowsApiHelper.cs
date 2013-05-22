/*
 *名称：WindowsApiHelper
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-22 11:55:47
 *修改时间：
 *备注：
 */

using System;
using System.Runtime.InteropServices;

namespace Infrastructure.Crosscutting.Utility
{
    public class WindowsApiHelper
    {
        #region var

        public const int LWA_ALPHA = 0x00000002;
        public const int RDW_INVALIDATE = 1;
        public const int RDW_ERASE = 4;
        public const int RDW_ALLCHILDREN = 0x80;
        public const int RDW_FRAME = 0x400;
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x00080000;

        #endregion

        #region Windows Api

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(
            IntPtr hWnd, int crKey, byte bAlpha, int dwFlags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate,
            IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
                                                  string lpszWindow);

        #endregion

        #region Public Method

        /// <summary>
        /// 设置某个窗口的透明度        
        /// </summary>
        /// <example>
        ///   SetWindowsOpacity("Shell_TrayWnd"); //Shell_TrayWnd 任务栏 
        ///   SetWindowsOpacity("StandardWindow"); //工作台
        ///   SetWindowsOpacity("StandardFrame");   //主窗口
        ///   SetWindowsOpacity(null, "系统消息");//系统提示消息
        /// </example>
        /// <param name="lpszClass">类名</param>
        /// <param name="lpszWindow">窗口标题</param>
        /// <param name="bAlpha">透明度值</param>
        public static void SetWindowsOpacity(string lpszClass, string lpszWindow = null, byte bAlpha = 255/2)
        {
            //设置客户工作台窗口透明名
            IntPtr vHandle = FindWindow(lpszClass, lpszWindow);
            // 这里换成你获得的窗体句柄，测试的时候可以用记事本。
            SetWindowLong(vHandle, GWL_EXSTYLE, GetWindowLong(vHandle, GWL_EXSTYLE) | WS_EX_LAYERED);
            SetLayeredWindowAttributes(vHandle, 0, bAlpha /*透明度*/, LWA_ALPHA);
        }

        /// <summary>
        /// 恢复窗口透明度
        /// </summary>
        /// <example>
        ///  RestoreOpacity("StandardWindow");//恢复工作台
        ///  RestoreOpacity("StandardFrame");//恢复主窗口
        ///  RestoreOpacity("Shell_TrayWnd"); //恢复任务栏 
        /// </example>
        /// <param name="lpszClass">类名</param>
        /// <param name="lpszWindow">窗口标题</param>
        public static void RestoreOpacity(string lpszClass, string lpszWindow = null)
        {
            //恢复 
            IntPtr vHandle = FindWindow(lpszClass, lpszWindow);
            SetWindowLong(vHandle, GWL_EXSTYLE, GetWindowLong(vHandle, GWL_EXSTYLE) & ~WS_EX_LAYERED);
            RedrawWindow(vHandle, IntPtr.Zero, IntPtr.Zero, RDW_ERASE | RDW_INVALIDATE | RDW_FRAME | RDW_ALLCHILDREN);
        }

        #endregion
 
    }
}