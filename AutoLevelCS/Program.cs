﻿using AutoLevelCS.Scripts;
using System;
using System.Diagnostics;

namespace AutoLevelCS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Login to next account
            OpenGame og = new OpenGame();
            og.LoginToSteam();

            // Inject Cheats
            //Injector inj = new Injector();
            //inj.Inject();
        }
    }
}
