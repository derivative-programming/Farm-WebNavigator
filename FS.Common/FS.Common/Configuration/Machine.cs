using System;
using System.Configuration;
using System.Linq;

namespace FS.Common.Configuration
{
    public class Machine
    {
        public Machine()
        {
        }
        public static string GetMachineName()
        {
            return System.Net.Dns.GetHostName();
        }

        public static string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            System.Net.IPAddress[] addr = ipEntry.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();

            return addr[addr.Length - 1].ToString();
        }

    }
}
