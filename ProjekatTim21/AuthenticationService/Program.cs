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

			Console.WriteLine("Korisnik koji je pokrenuo servera :" + WindowsIdentity.GetCurrent().Name);

			Console.WriteLine("Servis je pokrenut.");

			Console.ReadLine();

			/*string srvCertCN = "wcfservice";

			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			/// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
			X509Certificate2 srvCert = PomocneFunkcije.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
			EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:2500/Receiver"),
									  new X509CertificateEndpointIdentity(srvCert));

			using (WCFAuthenticationService proxy = new WCFAuthenticationService(binding, address))
			{
				/// 1. Communication test
				proxy.TestCommunication();
				Console.WriteLine("TestCommunication() finished. Press <enter> to continue ...");
				Console.ReadLine();
			}*/
		}
    }
}
