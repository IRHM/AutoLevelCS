using AutoLevelCS.Scripts;
using System;
using System.Diagnostics;
using System.Threading;

namespace AutoLevelCS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Login to next account and open game
            OpenGame og = new OpenGame();
            og.LoginToSteam();

            // 35 secs for csgo to open
            Thread.Sleep(35000);

            // Inject Cheats
            Injector inj = new Injector();
            inj.Inject();
        }
    }
}
