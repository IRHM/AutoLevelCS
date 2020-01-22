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
            // Get next account login info
            NextAccount na = new NextAccount();
            var (username, password) = na.GetNextAccount();

            Console.WriteLine($"Logging in on {username}");
            Console.Write("\n\rOpening CSGO...");

            // Open Steam and login
            // Args: support.steampowered.com/kb_article.php?ref=5623-QOSV-5250
            string steamPath = "C:\\Program Files (x86)\\Steam\\Steam.exe";
            string steamArgs = $"-login {username} {password} -silent -applaunch 730 -dev -novid -lv -noaafonts -nojoy -h 480 -w 640 -sw +exec AutoLevel";
            Process.Start("cmd.exe", $"/C start \"Steam\" \"{steamPath}\" {steamArgs}");

            Console.Write("\rCSGO Opened        \n");

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

        public void Move()
        {
            bool foundCS = false;
            int timesTried = 0;
            IntPtr id;

            // Create goto checkpoint
            checkProcessesAgain:

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

                    foundCS = true;
                }
            }

            // Restart foreach loop to check for csgo again
            if (foundCS == false && timesTried < 10)
            {
                Console.WriteLine("Couldn't Move CSGO Window");

                // Coundown for 10 seconds
                Countdown(0, 0, 10);

                Console.Write($"\rTrying Again Now \n");

                timesTried++;

                // Restart foreach loop to search for csgo process
                goto checkProcessesAgain;
            }
        }

        private void Countdown(int h, int m, int s)
        {
            float secondsLeft;
            var stopwatch = new Stopwatch();
            var startTime = DateTime.UtcNow;

            stopwatch.Start();

            // Create new TimeSpan to subtract stopwatch.Elapsed from
            TimeSpan remainingTime = new TimeSpan(h, m, s);

            while (DateTime.UtcNow - startTime < remainingTime)
            {
                // Subtract stopwatch.Elapsed from TimeSpan to get remaining seconds
                secondsLeft = remainingTime.Subtract(stopwatch.Elapsed).Seconds;

                // Leave space at end or when counting down..
                // from higher than 10 the 0 at the end is never overwritten
                Console.Write($"\rTrying Again In {secondsLeft} "); 
            }

            stopwatch.Stop();
        }
    }
}
