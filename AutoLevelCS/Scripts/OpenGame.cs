using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace AutoLevelCS.Scripts
{
    class OpenGame
    {
        public void LoginToSteam()
        {
            // Get next account login info
            NextAccount na = new NextAccount();
            var (username, password) = na.GetNextAccount();

            // Open Steam and login
            string steamPath = "C:\\Program Files (x86)\\Steam\\Steam.exe";
            string steamArgs = $"-login {username} {password} -applaunch 730 -novid -lv -noaafonts -nojoy -h 480 -w 640 -startwindowed +exec AutoLevel";
            Process.Start("cmd.exe", $"/C start \"Steam\" \"{steamPath}\" {steamArgs}");
        }
    }
}
