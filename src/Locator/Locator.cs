using Brunet;
using Brunet.Applications;
using Brunet.Concurrent;
using Brunet.Messaging;
using System;
using System.Collections;
using System.Net;

namespace GridAppliance {
  public class Locator {
    /**
     * Simple program to find out if any Grid Appliances are running on the 
     * local system!
     */
    public static void Main(String[] args) {
      ISender sender = null;
      BrunetRpc brpc = new BrunetRpc();
      if(args.Length == 0) {
        // Step one:  Gather all non-VMware IP Addresses for the block list
        ArrayList local_ips = new ArrayList();
        IPAddresses ipaddrs = IPAddresses.GetIPAddresses();
        foreach(Hashtable ht in ipaddrs.AllInterfaces) {
          if(ht["inet addr"] == null) {
            continue;
          }
          string ifname = (string) ht["interface"];
          if(ifname.StartsWith("vmnet") || ifname.StartsWith("VMware")) {
            local_ips.Add(IPAddress.Parse((String) ht["inet addr"]));
          }
        }
        IPAddress[] lips = (IPAddress[]) local_ips.ToArray(typeof(IPAddress));
        sender = brpc.IPHandler.CreateMulticastSender(lips);
      }
      else {
        sender = brpc.IPHandler.CreateMulticastSender();
      }

      // Step two: Setup BrunetRpc using Multicast to find nodes
      BlockingQueue q = new BlockingQueue();
      brpc.Rpc.Invoke(sender, q, "Information.Info");
      Hashtable retrieved = new Hashtable();
      while(true) {
        RpcResult res = null;
        try {
          // Step three: Get result and print it, need something better for autmoated service
          bool timedout = false;
          res = (RpcResult) q.Dequeue(2000, out timedout);
          if(timedout) {
            break;
          }
          Hashtable ht = (Hashtable) res.Result;
          Hashtable neighbors = (Hashtable) ht["neighbors"];
          string self = (string) neighbors["self"];
          if(retrieved.Contains(self)) {
            continue;
          }
          retrieved[self] = true;

          ArrayList vips = ht["VirtualIPs"] as ArrayList;

          string vips_list = "";
          if(vips != null) {
            foreach(string ip in vips) {
              vips_list += ip + ", ";
            }
            vips_list = vips_list.Substring(0, vips_list.Length - 2);
          }

          ArrayList ips = (ArrayList) ht["localips"];
          string iplist = "";
          foreach(string ip in ips) {
            iplist += ip + ", ";
          }
          iplist = iplist.Substring(0, iplist.Length - 2);

          Console.WriteLine(vips_list + ": " + iplist);
        }
        //This occurs when all attempts are done, unless you close the queue first
        catch(InvalidOperationException) {
          break;
        }
        catch(Exception e) {}
      }
      //BrunetRpc has a timer thread, it needs to explicitly be closed
      brpc.Close();
    }
  }
}
