using System;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using Brunet;
using Brunet.Concurrent;
using Brunet.Messaging;
using Brunet.Applications;
using System.Net;
using System.Runtime.InteropServices;

namespace gridtool {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      GuestConnectivity gc = GuestConnectivity.GetGuestConnectivity();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1(gc));

      gc.Close();
    }
  }

  public class GuestConnectivityLinux: GuestConnectivity {
    public override bool Enable {
      get { return true; }
      set { Console.WriteLine(value); }
    }
  }

  public class GuestConnectivityWindows: GuestConnectivity {
    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    //Declare the hook handle as an int.
    static int hHookKB = 0;
    static int hHookM = 0;

    //Declare the hook constants
    public const int WH_KEYBOARD_LL = 13;
    public const int WH_MOUSE_LL = 14;

    //Declare HookProcedures as a HookProc type.
    HookProc KeyboardHookProcedure;
    HookProc MouseHookProcedure;

    // Import necessary dlls from unmanaged code
    [DllImport("user32.dll", CharSet = CharSet.Auto,
     CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
    IntPtr hInstance, int threadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto,
     CallingConvention = CallingConvention.StdCall)]
    public static extern bool UnhookWindowsHookEx(int idHook);

    [DllImport("user32.dll", CharSet = CharSet.Auto,
     CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode,
    IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    protected static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", EntryPoint = "DestroyIcon")]
    static extern bool DestroyIcon(IntPtr hIcon);

    public override bool Enable {
      get { return hHookKB != 0 && hHookM != 0; }
      set {
        Console.WriteLine(value);
        if(value) {
          lock(_sync) {
            KeyboardHook();
            MouseHook();
          }
        } else {
          lock(_sync) {
            KeyboardUnhook();
            MouseUnhook();
          }
        }
      }
    }


    public GuestConnectivityWindows()
    {
      _ipaddrs = new IPAddressesWindows();
    }

    // Registers hook to listen to keyboard
    protected void KeyboardHook()
    {
      if (hHookKB == 0) {
        // Create an instance of HookProc.
        KeyboardHookProcedure = new HookProc(KeyboardHookProc);

        hHookKB = SetWindowsHookEx(WH_KEYBOARD_LL,
              KeyboardHookProcedure,
              GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
        //If the SetWindowsHookEx function fails.
        if (hHookKB == 0) {
          MessageBox.Show("SetWindowsHookEx failed for keyboard");
          return;
        }
      }
    }

    protected void KeyboardUnhook()
    { 
      if (hHookKB != 0) {
        bool ret = UnhookWindowsHookEx(hHookKB);
        //If the UnhookWindowsHookEx function fails.
        if (ret == false) {
          MessageBox.Show("UnhookWindowsHookEx failed for keyboard");
          return;
        }
        hHookKB = 0;
      }
    }

    // Registers hook to listen to keyboard
    protected void MouseHook()
    {
      if (hHookM == 0) {
        // Create an instance of HookProc.
        MouseHookProcedure = new HookProc(MouseHookProc);

        hHookM = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure,
              GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
        //If the SetWindowsHookEx function fails.
        if (hHookM == 0) {
          MessageBox.Show("SetWindowsHookEx failed for mouse");
          return;
        }
      }      
    }

    protected void MouseUnhook()
    { 
      if (hHookM != 0)
      {
        bool ret = UnhookWindowsHookEx(hHookM);
        //If the UnhookWindowsHookEx function fails.
        if (ret == false)
        {
          MessageBox.Show("UnhookWindowsHookEx failed for mouse");
          return;
        }
        hHookM = 0;
      }
    
    }

    // Callback procedure that gets called
    public int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode < 0)
      {
        return CallNextHookEx(hHookKB, nCode, wParam, lParam);
      }
      else
      {
        InvokeRpc();
        return CallNextHookEx(hHookKB, nCode, wParam, lParam);
      }
    }

    // Callback procedure that gets called
    public int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode < 0)
      {
        return CallNextHookEx(hHookM, nCode, wParam, lParam);
      }
      else
      {
        InvokeRpc();
        return CallNextHookEx(hHookM, nCode, wParam, lParam);
      }
    }
  }

  public abstract class GuestConnectivity {
    protected BrunetRpc _brpc;
    protected DateTime _last_check;
    protected object _sync;
    protected IPAddresses _ipaddrs;
    public abstract bool Enable { get; set; }

    public GuestConnectivity()
    {
      _brpc = new BrunetRpc();
      _last_check = DateTime.MinValue;
      _sync = new object();
    }

    public static GuestConnectivity GetGuestConnectivity()
    {
       GuestConnectivity gc = null;
       if(OSDependent.OSVersion == OSDependent.OS.Linux) {
         gc = new GuestConnectivityLinux();
       } else if(OSDependent.OSVersion == OSDependent.OS.Windows) {
         gc = new GuestConnectivityWindows();
       } else {
         throw new Exception("Unknown OS!");
       }
       return gc;
    }

    public void InvokeRpc()
    {
      DateTime now = DateTime.UtcNow;
      lock(_sync) {
        if(_last_check.AddSeconds(30) < now) {
          _last_check = now;
        } else {
          return;
        }
      }

      // This is a nifty little utility that utillizes ifconfig (or ipconfig in Windows)
      ArrayList to_send = new ArrayList();

      foreach (Hashtable ht in _ipaddrs.GetAddresses()) {
        if (ht["inet addr"] == null) {
          continue;
        }

        string ifname = (string)ht["interface"];

        // We only send over VMWare network interfaces.
        if (ifname.StartsWith("VMware Network")) {
          to_send.Add(IPAddress.Parse((string) ht["inet addr"]));
        }
      }

      BlockingQueue q = new BlockingQueue();
      q.CloseEvent += EndRpc;
      IPAddress[] ips = (IPAddress[]) to_send.ToArray(typeof(IPAddress));
      ISender sender = _brpc.IPHandler.CreateMulticastSender(ips);
      _brpc.Rpc.Invoke(sender, q, "HostActivity.UpdateConsole");
    }

    protected void EndRpc(Object o, EventArgs ea)
    {
      // We could potentially put an error message here
    }

    public void Close()
    {
      //BrunetRpc has a timer thread, it needs to explicitly be closed
      _brpc.Close();
    }
  }
}
