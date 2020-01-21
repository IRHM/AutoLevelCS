using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AutoLevelCS.Scripts
{
    class Injector
    {
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

		private string processName = "csgo";
		private string dllPath = "D:\\Osiris\\Release\\Osiris.dll";
		private static readonly IntPtr INTPTR_ZERO = (IntPtr)0;
		private uint PID;
		private Process[] _procs;

		private bool InjectModule(uint pid, string dllpath)
		{
			Console.WriteLine($"Injecting: {dllpath}");

			IntPtr intPtr = OpenProcess(1082u, 1, pid);
			if (intPtr == INTPTR_ZERO)
			{
				return false;
			}

			IntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
			if (procAddress == INTPTR_ZERO)
			{
				return false;
			}

			IntPtr intPtr2 = VirtualAllocEx(intPtr, (IntPtr)null, (IntPtr)dllpath.Length, 12288u, 64u);
			if (intPtr2 == INTPTR_ZERO)
			{
				return false;
			}

			byte[] bytes = Encoding.ASCII.GetBytes(dllpath);
			if (WriteProcessMemory(intPtr, intPtr2, bytes, (uint)bytes.Length, 0) == 0)
			{
				return false;
			}

			if (CreateRemoteThread(intPtr, (IntPtr)null, INTPTR_ZERO, procAddress, intPtr2, 0u, (IntPtr)null) == INTPTR_ZERO)
			{
				return false;
			}

			CloseHandle(intPtr);

			return true;
		}

		private bool GetCSGOPid()
		{
			_procs = Process.GetProcesses();

			for (int i = 0; i < _procs.Length; i++)
			{
				if (_procs[i].ProcessName == processName)
				{
					PID = (uint)_procs[i].Id;
					return true;
				}
			}

			return false;
		}

		public void Inject()
		{
			if (GetCSGOPid() == true)
			{
				Console.WriteLine($"Found CSGO PID: {PID.ToString()}");
			}
			else
			{
				Console.WriteLine("Couldn't find CSGO PID");
			}
			
			if (InjectModule(PID, dllPath) == true)
			{
				Console.WriteLine("Done Injecting");
			}
			else
			{
				Console.WriteLine("Failed Whilst Injecting");
			}
		}
	}
}
