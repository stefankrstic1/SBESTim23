using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        public static Dictionary<string, string> LoggedUserAccountsDB = new Dictionary<string, string>();
        public void Login(string username, string password)
        {         
            if (!LoggedUserAccountsDB.ContainsKey(username))
            {
                string srvCertCN = "WCFSERVIS";
               

                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                X509Certificate2 srvCert = PomocneFunkcije.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
                EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:2500/Receiver"),
                                          new X509CertificateEndpointIdentity(srvCert));

                using (WCFCheckingCSAS proxy = new WCFCheckingCSAS(binding, address))
                {
                    /// 1. Communication test
                    
                   bool a = proxy.CheckIfAccExists(username, password);
                        if(a)
                    {
                        Console.WriteLine("Ulogovan");
                        LoggedUserAccountsDB.Add(username, username);

                    }
                        else
                    {
                        Console.WriteLine("Nije ulogovan jer nema kor. ime u bazi");
                      
                    }
                }
            
            }          
            else
            {
                Console.WriteLine($"Korisnik sa korisnickim imenom {username} je vec ulogovan");
            }
        }

        public void Logout(string username)
        {
            if (LoggedUserAccountsDB.ContainsKey(username))
            {
                LoggedUserAccountsDB.Remove(username);
                Console.WriteLine($"Korisnik sa korisnickim imenom {username} je izlogovan.");
            }
            else
            {
                Console.WriteLine($"Korisnik sa korisnickim imenom {username} nije ulogovan");
            }
        }
    }
}
