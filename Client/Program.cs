using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

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

            string username = "";
            string password = "";
            string ulogovan = "";
            bool logged = false;
            DateTime vremeLogovanja = new DateTime();

            string isAdmin = Common.PomocneFunkcije.CheckUserGroup(windowsIdentity);
            string address = "";
            if (isAdmin == "AccountUsers")
            {
                address = "net.tcp://localhost:1234/AuthenticationService";

                EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));

                using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
                {

                    while (true)
                    {
                        ispisiUserMenu();
                        int izbor = 0;
                        string broj = "";
                        do
                        {
                            broj = Console.ReadLine();
                           
                        } while (broj != "1");
                        izbor = Convert.ToInt32(broj);

                        switch (izbor)
                        {
                            case 1:
                                Console.WriteLine("Upisi username za logovanje: ");
                                username = Console.ReadLine();
                                Console.WriteLine("Upisi password za logovanje: ");
                                password = Console.ReadLine();
                                if(proxy.Login(username, password))
                                {
                                    Console.Clear();
                                    Console.WriteLine("Ulogovani ste");
                                    Console.WriteLine("Pritisnite enter da bi se izlogovali");
                                    Console.Read();
                                    proxy.Logout(username);
                                }                             
                                break;
                        }
                    }

                }
            }
            else if(isAdmin == "AccountAdmins")
            {
                address = "net.tcp://localhost:1888/CredentialsStore";
                EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));
                using (ClientProxyCS proxy = new ClientProxyCS(binding, endpointAddress))
                {
                    while (true)
                    {
                        ispisiMenu();
                        int izbor = Convert.ToInt32(Console.ReadLine());


                        switch (izbor)
                        {
                            case 1:
                                Console.WriteLine("Upisi username za kreiranje: ");
                                username = Console.ReadLine();
                                Console.WriteLine("Upisi password za kreiranje: ");
                                password = Console.ReadLine();
                                if (proxy.CreateAccount(username, password))
                                    Console.WriteLine("Kreirano.");
                                else
                                    Console.WriteLine("Korisnicko ime vec postoji");
                                break;
                            case 2:
                                Console.WriteLine("Upisi username koji zelis obrisati: ");
                                username = Console.ReadLine();                         
                                if (proxy.DeleteAccount(username))
                                {
                                    Console.WriteLine("Obrisano");
                                }
                                else
                                {
                                    Console.WriteLine("Korisnicko ime ne postoji u bazi");
                                }
                                break;
                            case 3:
                                Console.WriteLine("Upisi username ciji account zelis da zakljucas: ");
                                username = Console.ReadLine();                              
                                if (proxy.LockAccount(username))
                                {
                                    Console.WriteLine("Zakljucano.");
                                }
                                else
                                {
                                    Console.WriteLine("Korisnicko ime ne postoji u bazi");
                                }
                                break;
                            case 4:
                                Console.WriteLine("Upisi username ciji account zelis da omogucis: ");
                                username = Console.ReadLine();
                                if (proxy.EnableAccount(username))
                                {

                                    Console.WriteLine("Omoguceno.");
                                }
                                else
                                {
                                    Console.WriteLine("Korisnicko ime ne postoji u bazi");
                                }
                                break;
                            case 5:
                                Console.WriteLine("Upisi username ciji account zelis da onemogucis: ");
                                username = Console.ReadLine();
                                if (proxy.DisableAccount(username))
                                {
                                    Console.WriteLine("Onemogucis.");
                                }
                                else
                                {
                                    Console.WriteLine("Korisnicko ime ne postoji u bazi");
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Prijavljeni korisnik ne pripada nijednoj grupi");
            }

            Console.Read();
        }

        public static bool IsAnyKeyPressed()
        {
            var allPossibleKeys = Enum.GetValues(typeof(Key));
            bool results = false;
            foreach (var currentKey in allPossibleKeys)
            {
                Key key = (Key)currentKey;
                if (key != Key.None)
                    if (Keyboard.IsKeyDown((Key)currentKey)) { results = true; break; }
            }
            return results;
        }


        public static void ispisiMenu()
        {
            Console.WriteLine("\nMeni za odabir:\n");
            Console.WriteLine("1) Create Account");
            Console.WriteLine("2) Delete Account");
            Console.WriteLine("3) Lock Account");
            Console.WriteLine("4) Enable Account");
            Console.WriteLine("5) Disable Account");
            Console.Write("\r\nIzaberi opciju: ");

        }


        public static void ispisiUserMenu()
        {
            Console.WriteLine("\nMeni za odabir:\n");
            Console.WriteLine("1) Login: ");
            Console.Write("\r\nIzaberi opciju: ");
        }
    }
}
