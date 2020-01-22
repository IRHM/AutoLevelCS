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
            // Login and open game
            OpenGame og = new OpenGame();
            og.LoginToSteam();

            // Inject Cheats
            Injector inj = new Injector();
            inj.Inject();
        }
    }
}
