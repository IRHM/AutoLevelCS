using AutoLevelCS.Scripts;
using System;
using System.Diagnostics;

namespace AutoLevelCS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get next account
            //Scripts.NextAccount.GetNextAccount();

            Injector inj = new Injector();

            inj.Inject();
        }
    }
}
