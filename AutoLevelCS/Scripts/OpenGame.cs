using System;
using System.Collections.Generic;
using System.Text;

namespace AutoLevelCS.Scripts
{
    class OpenGame
    {
        public void LoginToSteam()
        {
            string steamPath = "C:\\Program Files (x86)\\Steam\\Steam.exe";

            System.Diagnostics.Process.Start("cmd.exe", $"/C start \"Steam\" \"{steamPath}\" -login user pass");
        }
    }
}
