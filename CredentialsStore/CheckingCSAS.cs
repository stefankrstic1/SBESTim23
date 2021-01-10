using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CredentialsStore
{
    public class CheckingCSAS : ICheckingCSAS
    {
        public static Dictionary<string, int> brojPokusaja = new Dictionary<string, int>();
        public static Dictionary<string, DateTime> vrijemeLokdauna = new Dictionary<string, DateTime>();

        public static Dictionary<string, string> kljucevi = new Dictionary<string, string>();

        public bool CheckIfAccExists(string message, byte[] sign)
        {
            //Debugger.Launch();
            string clientName = PomocneFunkcije.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);//"wcfclient";
            string clientNameSign = clientName + "_sign";
            X509Certificate2 certificateSign = PomocneFunkcije.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, clientNameSign);
            if (DigitalSignature.Verify(message, Common.HashAlgorithm.SHA1, sign, certificateSign))
            {
                Console.WriteLine("Sign is valid");
            }
            else
            {
                Console.WriteLine("Sign is invalid");
                return false;
            }

            //Validacija ulogovanog korisnika!!!!!
            var key = String.Empty;
            foreach (var item in kljucevi)
            {
                if (item.Key == clientName)
                {
                    key = item.Value;
                    break;
                }
            }

            string dekriptovana = AesCSAS.Decrypt(message, key);
            string[] niz = dekriptovana.Split(';');
            string username = niz[0];
            string password = niz[1];
            List<User> korisnici = new List<User>();
            korisnici = PomocneFunkcije.ReadUsers();
            key = "b14ca5898a4e4133bbce2ea2315a1916";
            string enkriptovanPassword = PomocneFunkcije.EncryptString(key, password);

            vrijemeLokdauna.TryGetValue(username, out var vrijeme);

            TimeSpan vreme = TimeSpan.FromSeconds(20);

            foreach (User u in korisnici)
            {
                if (u.Username == username && u.Password == enkriptovanPassword && DateTime.UtcNow - vrijeme > vreme)
                {
                    PomocneFunkcije.UpdateAccount(username, 4);
                    u.Locked = false;
                }

                if (u.Username == username && u.Password == enkriptovanPassword && u.IsEnabled == true && u.Locked == false)
                {
                    return true;
                }

            }

           
            if (brojPokusaja.ContainsKey(username))
            {
                brojPokusaja.TryGetValue(username, out var brojac);
                brojPokusaja[username] = ++brojac;

                if (brojac == 5)
                {
                    Debugger.Launch();
                    PomocneFunkcije.UpdateAccount(username, 1);
                    vrijemeLokdauna.Add(username, DateTime.UtcNow);
                    Console.WriteLine($"Profil {username} je zakljucan na 20 sekundi");
                }
            }
            else
            {
                brojPokusaja.Add(username, 1);
            }


            return false;

        }

        public string CriptoKey(string username)
        {
            X509Certificate2 srvCert = PomocneFunkcije.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, username);
            string key1 = AesCSAS.GenerateKey();
            if (kljucevi.ContainsKey(username))
            {
                kljucevi.Remove(username);
            }
            kljucevi.Add(username, key1);        
            return EncryptRsa(key1, srvCert);
        }

        private string EncryptRsa(string input,X509Certificate2 cert)
        {
            string output = string.Empty;
            using (RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key)
            {
                byte[] bytesData = Encoding.UTF8.GetBytes(input);
                byte[] bytesEncrypted = csp.Encrypt(bytesData, false);
                output = Convert.ToBase64String(bytesEncrypted);
            }
            return output;
        }



        //public void SendMessage(string message, byte[] sign)
        //{
        //    string clientName = PomocneFunkcije.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);//"wcfclient";
        //    string clientNameSign = clientName + "_sign";
        //    X509Certificate2 certificateSign = PomocneFunkcije.GetCertificateFromStorage(StoreName.TrustedPeople,
        //        StoreLocation.LocalMachine, clientNameSign);
        //    if (DigitalSignature.Verify(message, HashAlgorithm.SHA1, sign, certificateSign))
        //    {
        //        Console.WriteLine("Sign is valid");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Sign is invalid");
        //    }
        //}
    }
}
