using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace PuttyWrap
{
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATA
    {
        public uint dwData;
        public uint cbData;
        public IntPtr lpData;
    }

    static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool onlyInstance = false;
            Mutex mutex = new Mutex(true, "PuttyWrap", out onlyInstance);
            if (!onlyInstance)
            {
                string strArgs = "";
				if(args.Length > 0)
				{
					strArgs += args[0];
					
					for (int i = 1; i < args.Length; i++)
					{
						strArgs += " " + args[i];
					}
				}

                COPYDATA cd = new COPYDATA
                                  {
                                      dwData = 0,
                                      cbData = (uint) strArgs.Length + 1,
                                      lpData = Marshal.StringToHGlobalAnsi(strArgs)
                                  };

                IntPtr lpPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cd));
                Marshal.StructureToPtr(cd, lpPtr, true);
                Process[] plist = Process.GetProcessesByName("PuttyWrap");
                foreach (Process spProcess in plist)
                {
                    SendMessage(spProcess.MainWindowHandle, 0x004A, 0, lpPtr);
                }
                Marshal.FreeHGlobal(lpPtr);
            }
            else
            {
            
#if DEBUG
            	Logger.OnLog += Console.WriteLine;
#endif

            	Application.EnableVisualStyles();
            	Application.SetCompatibleTextRenderingDefault(false);
            	Application.Run(new frmPuttyWrap(args));
            	GC.KeepAlive(mutex);
            }
        }
    }
}
