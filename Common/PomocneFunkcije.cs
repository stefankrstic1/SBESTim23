using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Common;

namespace Common
{
   public class PomocneFunkcije
    {

        public static List<User> ReadUsers()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var iconPath = Path.Combine(outPutDirectory, "ccc.txt");
            var uri = new Uri(iconPath);
            var path = Path.GetFileName(uri.AbsolutePath);
            List<User> korisnici = new List<User>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            Enum.Titula titula = Enum.Titula.KLIJENT;
            bool locked = false;
            bool isEnabled = false;
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                if (tokens[3] == "0")
                {
                    locked = false;
                }
                else
                {
                    locked = true;
                }

                if (tokens[0] == "ADMIN")
                {
                    titula = Enum.Titula.ADMIN;
                }
                else
                {
                    titula = Enum.Titula.KLIJENT;
                }

                if (tokens[4].Equals("true"))
                {
                    isEnabled = true;
                }
                else if (tokens[4].Equals("false"))
                {
                    isEnabled = false;
                }

                User u = new User(tokens[1], tokens[2], titula, locked, isEnabled);
                korisnici.Add(u);
            }

            sr.Close();
            stream.Close();

            return korisnici;
        }

        public static void Write(string username, string password, string titula)
        {
            using (StreamWriter writetext = new StreamWriter("ccc.txt", true))
            {
                writetext.WriteLine(titula + ";" + username + ";" + password + ";" + "0"+ ";" + "true");
                writetext.Close();
            }
        }

        public static void DeleteAccount(string username)
        {
            List<User> users = ReadUsers();
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var iconPath = Path.Combine(outPutDirectory, "ccc.txt");
            var uri = new Uri(iconPath);
            var path = Path.GetFileName(uri.AbsolutePath);
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);
            foreach (User user in users)
            {
                if (!user.Username.Equals(username))
                {
                    sw.WriteLine(user.Titula + ";" + user.Username + ";" + user.Password + ";" + (user.Locked == true ? "1" : "0") + ";" + user.IsEnabled.ToString());
                }

            }
            sw.Close();
            stream.Close();
        }

        public static void UpdateAccount(string username, int number)
        {

            List<User> users = ReadUsers();
            User u = new User();

            foreach (User user in users)
            {
                if (user.Username.Equals(username))
                {
                    u = user;
                    break;
                }
            }
            if (number.Equals(1))
            {
                //Lock
                foreach (User user in users)
                {
                    if (user.Username.Equals(u.Username))
                    {
                        user.Locked = true;
                    }
                }

            }
            else if (number.Equals(2))
            {
                //Enable
                foreach (User user in users)
                {
                    if (user.Username.Equals(u.Username))
                    {
                        user.IsEnabled = true;
                    }
                }

            }
            else if (number.Equals(3))
            {
                //Disable
                foreach (User user in users)
                {
                    if (user.Username.Equals(u.Username))
                    {
                        user.IsEnabled = false;
                    }
                }
            }

            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var iconPath = Path.Combine(outPutDirectory, "ccc.txt");
            var uri = new Uri(iconPath);
            var path = Path.GetFileName(uri.AbsolutePath);
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);
            foreach (User user in users)
            {
                sw.WriteLine(user.Titula + ";" + user.Username + ";" + user.Password + ";" + (user.Locked == true ? "1" : "0") + ";" + user.IsEnabled.ToString());
            }
            sw.Close();
            stream.Close();
        }

        // kriptovanje preko simetricnog kljuca
        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);

            /// Check whether the subjectName of the certificate is exactly the same as the given "subjectName"
            foreach (X509Certificate2 c in certCollection)
            {
                if (c.SubjectName.Name.Equals(string.Format("CN={0}", subjectName)))
                {
                    return c;
                }
            }

            return null;
        }

        public static string ParseName(string winLogonName)
        {
            string[] parts = new string[] { };

            if (winLogonName.Contains("@"))
            {
                ///UPN format
                parts = winLogonName.Split('@');
                return parts[0];
            }
            else if (winLogonName.Contains("\\"))
            {
                /// SPN format
                parts = winLogonName.Split('\\');
                return parts[1];
            }
            else
            {
                return winLogonName;
            }
        }

        public static bool CheckUserGroup(WindowsIdentity windowsIdentity)
        {
            foreach(IdentityReference group in windowsIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount)).ToString();
                name = ParseName(name);

                if(name == "AccountAdmins")
                {
                    return true;
                }
            }

            return false;
        }

    }
}
