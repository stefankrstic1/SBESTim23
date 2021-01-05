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

            string username = "";
            string password = "";
            string ulogovan = "";

            bool isAdmin = Common.PomocneFunkcije.CheckUserGroup(windowsIdentity);
            string address = "";
            if (!isAdmin)
            {
                address = "net.tcp://localhost:1234/AuthenticationService";

                EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));

                using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
                {
                   /* proxy.Login("ime", "sifra");
                    Console.ReadLine();
                    proxy.Logout("ime");
                    Console.ReadLine(); */


                    while (true)
                    {
                        ispisiUserMenu();
                        int izbor = Convert.ToInt32(Console.ReadLine());


                        switch (izbor)
                        {
                            case 1:                                
                                Console.WriteLine("Upisi username za logovanje: ");
                                username = Console.ReadLine();
                                Console.WriteLine("Upisi password za logovanje: ");
                                password = Console.ReadLine();                           
                                proxy.Login(username, password);
                                break;
                            case 2:
                                proxy.Logout(username);
                                break;
                            
                        }
                    }

                }
            }
            else
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
                                proxy.CreateAccount(username, password);
                                Console.WriteLine("Kreirano.");
                                break;
                            case 2:
                                Console.WriteLine("Upisi username koji zelis obrisati: ");
                                username = Console.ReadLine();
                                proxy.DeleteAccount(username);
                                Console.WriteLine("Obrisano.");
                                break;
                            case 3:
                                Console.WriteLine("Upisi username ciji account zelis da zakljucas: ");
                                username = Console.ReadLine();
                                proxy.LockAccount(username);
                                Console.WriteLine("Zakljucano.");
                                break;
                            case 4:
                                Console.WriteLine("Upisi username ciji account zelis da omogucis: ");
                                username = Console.ReadLine();
                                proxy.EnableAccount(username);
                                Console.WriteLine("Omoguceno.");
                                break;
                            case 5:
                                Console.WriteLine("Upisi username ciji account zelis da onemogucis: ");
                                username = Console.ReadLine();
                                proxy.DisableAccount(username);
                                Console.WriteLine("Onemogucis.");
                                break;
                        }
                    }
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
            Console.WriteLine("2) Logout: ");
            Console.Write("\r\nIzaberi opciju: ");
        }
    }
}
