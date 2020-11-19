using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Installer
{
    public class Program
    {
        static void Main(string[] args)
        {
            byte[] mac = GetMAC();

            System.IO.File.WriteAllBytes("D:/GitHub/Information-Security/Lab1/Lab1/bin/Debug/netcoreapp3.1/licence.txt", mac);
        }

        private static byte[] GetMAC()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().GetAddressBytes())
                .FirstOrDefault();
        }

        public static bool VerifyCurrentMachine(byte[] data)
        {
            byte[] mac = GetMAC();

            return mac.SequenceEqual(data);
        }
    }
}
