using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoLevelCS.Scripts
{
    class OpenGame
    {
        public void LoginToSteam()
        {
            Console.WriteLine("Getting Next Login Details...");

            // Get next account login info
            NextAccount na = new NextAccount();
            var (username, password) = na.GetNextAccount();

            Console.WriteLine($"Logging in on {username}");
            Console.WriteLine("Opening CSGO...");

            // Open Steam and login
            // Args: support.steampowered.com/kb_article.php?ref=5623-QOSV-5250
            string steamPath = "C:\\Program Files (x86)\\Steam\\Steam.exe";
            string steamArgs = $"-login {username} {password} -silent -applaunch 730 -dev -novid -lv -noaafonts -nojoy -h 480 -w 640 -sw +exec AutoLevel";
            Process.Start("cmd.exe", $"/C start \"Steam\" \"{steamPath}\" {steamArgs}");

            // 35 secs for csgo to open
            Thread.Sleep(35000);

            MoveGame mg = new MoveGame();
            mg.Move();
        }
    }

    class MoveGame
    {
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public bool Move()
        {
            IntPtr id;

            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains("Counter-Strike: Global Offensive"))
                {
                    // TODO: With multiple csgo windows open calculate where
                    // the second window should go to be flush next to the first
                    // Maybe new IF for each process if their MainWindowTitle is different for each

                    // Move CSGO Window
                    id = pList.MainWindowHandle;
                    RECT Rect = new RECT();
                    Thread.Sleep(2000);
                    GetWindowRect(id, ref Rect);
                    MoveWindow(id, 0, 0, Rect.right - Rect.left, Rect.bottom - Rect.top, true);

                    Console.WriteLine("Moved CSGO Window");

                    return true;
                }
            }

            Console.WriteLine("Couldn't Move CSGO Window");

            return false;
        }
    }
}
