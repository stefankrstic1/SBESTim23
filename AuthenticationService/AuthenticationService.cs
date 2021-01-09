using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
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
                string srvCertCN = "wcfservice";

                string signCertCN = PomocneFunkcije.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";

                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                X509Certificate2 srvCert = PomocneFunkcije.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
                EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:2500/Receiver"),
                                          new X509CertificateEndpointIdentity(srvCert));

                using (WCFCheckingCSAS proxy = new WCFCheckingCSAS(binding, address))
                {
                    /// 1. Communication test
                    //Debugger.Launch();
                    string cltCertCN = PomocneFunkcije.ParseName(WindowsIdentity.GetCurrent().Name);

                    string key1 = proxy.CriptoKey(cltCertCN);

                    X509Certificate2 certificate4 = PomocneFunkcije.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

                    string key = decryptRsa(key1, certificate4);

                    string poruka = username + ";" + password;
                    string enkriptovana = AesCSAS.Encrypt(poruka, key);

                    X509Certificate2 certificate2 = PomocneFunkcije.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);
                    byte[] signature = DigitalSignature.Create(enkriptovana, Common.HashAlgorithm.SHA1, certificate2);
                    //proxy.CheckIfAccExists(poruka, signature);

                    bool a = proxy.CheckIfAccExists(enkriptovana, signature);

                    if (a)
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

        private string decryptRsa(string encrypted, X509Certificate2 cert)
        {
            string text = string.Empty;
            
            using (RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey)
            {
                byte[] bytesEncrypted = Convert.FromBase64String(encrypted);
                byte[] bytesDecrypted = csp.Decrypt(bytesEncrypted, false);
                text = Encoding.UTF8.GetString(bytesDecrypted);
            }
            return text;
        }

        /*private X509Certificate2 getCertificate(string certificateName)
        {
            X509Store my = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            my.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = my.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false);
            if (collection.Count == 1)
            {
                return collection[0];
            }
            else if (collection.Count > 1)
            {
                throw new Exception(string.Format("More than one certificate with name '{0}' found in store LocalMachine/My.", certificateName));
            }
            else
            {
                throw new Exception(string.Format("Certificate '{0}' not found in store LocalMachine/My.", certificateName));
            }
        }*/
    }
}
