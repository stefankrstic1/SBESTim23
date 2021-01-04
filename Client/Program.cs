using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();

            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            Console.WriteLine("Korisnik koji je pokrenuo klijenta je : " + windowsIdentity.Name);

            

            bool isAdmin = Common.PomocneFunkcije.CheckUserGroup(windowsIdentity);
            string address = "";
            if (!isAdmin)
            {
                address = "net.tcp://localhost:1234/AuthenticationService";

                EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));

                using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
                {
                    proxy.Login("ime", "sifra");
                    Console.ReadLine();
                    proxy.Logout("ime");
                    Console.ReadLine();
                }
            }
            else
            {
                address = "net.tcp://localhost:1888/CredentialsStore";
                EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));
                using (ClientProxyCS proxy = new ClientProxyCS(binding, endpointAddress))
                {
                    Console.ReadLine();
                }
            }
            
            

            /*//string address = "net.tcp://localhost:1000/"

            //ADMIN: USER(srdjan), SIFRA(123)
            string username;
            string password;

            Console.WriteLine("Unesi username:");
            username = Console.ReadLine();
            Console.WriteLine("Unesi password:");
            password = Console.ReadLine();*/


        }
    }
}
