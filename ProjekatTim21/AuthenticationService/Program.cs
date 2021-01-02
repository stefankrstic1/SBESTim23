using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {
			NetTcpBinding binding = new NetTcpBinding();
			string address = "net.tcp://localhost:1234/AuthenticationService";

			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

			ServiceHost host = new ServiceHost(typeof(AuthenticationService));
			host.AddServiceEndpoint(typeof(IAuthenticationService), binding, address);

			host.Open();

			Console.WriteLine("Korisnik koji je pokrenuo server :" + WindowsIdentity.GetCurrent().Name);

			Console.WriteLine("Servis je pokrenut.");

			Console.ReadLine();
		}
    }
}
