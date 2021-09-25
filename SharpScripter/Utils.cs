using Microsoft.WindowsAPICodePack.Shell;
using SharpScripter.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SharpScripter
{
    public static class Utils
    {
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        #region KEY CONSTS
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const uint MOUSEEVENTF_MOVE = 0x0001;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        public const uint MOUSEEVENTF_XDOWN = 0x0080;
        public const uint MOUSEEVENTF_XUP = 0x0100;
        public const uint MOUSEEVENTF_WHEEL = 0x0800;
        public const uint MOUSEEVENTF_HWHEEL = 0x01000;
        #endregion

        public static Point GetCursorPos()
        {
            return Control.MousePosition;
        }

        public static bool IsCursorInRect(Point mousePos, int x1, int y1, int x2, int y2)
        {
            return ((mousePos.X >= x1 && mousePos.Y >= y1) && (mousePos.X <= x2 && mousePos.Y <= y2));
        }

        public static void Click(int x = -1, int y = -1, bool doubleClick = false)
        {
            if (x != -1 && y != -1)
            {
                SetCursorPos(x, y);
            }

            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            if (doubleClick)
            {
                Thread.Sleep(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(200);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        public static void SetCursorDrag(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(200);
            int modX = x % 10;
            int modY = y % 10;
            x -= modX;
            y -= modY;
            int dX = x / 10;
            int dY = y / 10;
            for (int i = 0; i < 10; i++)
            {
                Point pos = GetCursorPos();
                SetCursorPos(pos.X + dX, pos.Y + dY);
                Thread.Sleep(10);
            }
            Point endPos = GetCursorPos();
            SetCursorPos(endPos.X + modX, endPos.Y + modY);
            Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static System.Drawing.Color GetPixelColor(Point position)
        {
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
                }
                return bitmap.GetPixel(0, 0);
            }
        }

        public static string GetScreenshot(int x, int y, int rectX, int rectY)
        {
            DateTime now = DateTime.Now;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Screenshot_" + now.Year.ToString() + "-" + now.Month.ToString() + "-" + now.Day.ToString() + "-" + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + ".png";
            Rectangle rect = new Rectangle(x, y, rectX, rectY);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            bmp.Save(path, ImageFormat.Png);
            return path;
        }

        public static List<App> GetAllApps(string like = null)
        {
            List<App> apps = new List<App>();
            var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
            ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);

            foreach (var app in (IKnownFolder)appsFolder)
            {
                // The friendly app name
                string name = app.Name;
                if (like != null && !name.Contains(like)) { continue; }

                // The ParsingName property is the AppUserModelID
                string appUserModelID = app.ParsingName; // or app.Properties.System.AppUserModel.ID

                Bitmap icon = app.Thumbnail.MediumBitmap;

                apps.Add(new App() { AppName = name, ExePath = appUserModelID, Icon = icon });
            }
            return apps;
        }

        public static App GetApp(string appName)
        {
            var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
            ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);

            foreach (var app in (IKnownFolder)appsFolder)
            {

                string name = app.Name;
                if (name != appName) { continue; }

                string appUserModelID = app.ParsingName;

                Bitmap icon = app.Thumbnail.MediumBitmap;

                return new App() { AppName = name, ExePath = appUserModelID, Icon = icon };
            }

            return null;
        }

        public static void StartProcess(string path)
        {
            Process.Start("explorer.exe", @" shell:appsFolder\" + path);
        }

        public static void KillProcess(string appName)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.MainWindowTitle == appName)
                {
                    process.Kill();
                }
                else if (process.MainWindowTitle.Contains(appName))
                {
                    process.Kill();
                }
            }
        }

        public static Rectangle FindImageOnScreen(Bitmap bmpMatch, Bitmap ScreenBmp, bool ExactMatch)
        {
            BitmapData ImgBmd = bmpMatch.LockBits(new Rectangle(0, 0, bmpMatch.Width, bmpMatch.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData ScreenBmd = ScreenBmp.LockBits(new Rectangle(0, 0, ScreenBmp.Width, ScreenBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            byte[] ImgByts = new byte[(Math.Abs(ImgBmd.Stride) * bmpMatch.Height) - 1 + 1];
            byte[] ScreenByts = new byte[(Math.Abs(ScreenBmd.Stride) * ScreenBmp.Height) - 1 + 1];

            Marshal.Copy(ImgBmd.Scan0, ImgByts, 0, ImgByts.Length);
            Marshal.Copy(ScreenBmd.Scan0, ScreenByts, 0, ScreenByts.Length);

            bool FoundMatch = false;
            Rectangle rct = Rectangle.Empty;
            int sindx, iindx;
            int spc, ipc;

            int skpx = Convert.ToInt32((bmpMatch.Width - 1) / (double)10);
            if (skpx < 1 | ExactMatch)
                skpx = 1;
            int skpy = Convert.ToInt32((bmpMatch.Height - 1) / (double)10);
            if (skpy < 1 | ExactMatch)
                skpy = 1;

            for (int si = 0; si <= ScreenByts.Length - 1; si += 3)
            {
                FoundMatch = true;
                for (int iy = 0; iy <= ImgBmd.Height - 1; iy += skpy)
                {
                    for (int ix = 0; ix <= ImgBmd.Width - 1; ix += skpx)
                    {
                        sindx = (iy * ScreenBmd.Stride) + (ix * 3) + si;
                        iindx = (iy * ImgBmd.Stride) + (ix * 3);
                        spc = Color.FromArgb(ScreenByts[sindx + 2], ScreenByts[sindx + 1], ScreenByts[sindx]).ToArgb();
                        ipc = Color.FromArgb(ImgByts[iindx + 2], ImgByts[iindx + 1], ImgByts[iindx]).ToArgb();
                        if (spc != ipc)
                        {
                            FoundMatch = false;
                            iy = ImgBmd.Height - 1;
                            ix = ImgBmd.Width - 1;
                        }
                    }
                }
                if (FoundMatch)
                {
                    double r = si / (double)(ScreenBmp.Width * 3);
                    double c = ScreenBmp.Width * (r % 1);
                    if (r % 1 >= 0.5)
                        r -= 1;
                    rct.X = System.Convert.ToInt32(c);
                    rct.Y = System.Convert.ToInt32(r);
                    rct.Width = bmpMatch.Width;
                    rct.Height = bmpMatch.Height;
                    break;
                }
            }

            bmpMatch.UnlockBits(ImgBmd);
            ScreenBmp.UnlockBits(ScreenBmd);
            if (FoundMatch)
                return rct;
            else
                return Rectangle.Empty;
        }
    }
}