using Brunet;
using System; 
using System.Diagnostics;


namespace Brunet.Inject 
{ 
  public class HostActivity 
  { 
    public HostActivity(Node node) 
    { 
      node.Rpc.AddHandler("HostActivity", this); 
    } 


    public void UpdateConsole() 
    {  
      start_proc("touch", "/dev/console");
    }

    public string start_proc(string cmds, string args) {
      ProcessStartInfo cmd = new ProcessStartInfo(cmds);
      cmd.Arguments = args;
      cmd.UseShellExecute = false;
      cmd.RedirectStandardOutput = true;
      Process p = Process.Start(cmd);
      string result = p.StandardOutput.ReadToEnd();
      p.WaitForExit();
      return result;
    }
  }
}
