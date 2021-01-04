using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CredentialsStore
{
    public class AccountManagement : IAccountManagement
    {       
        public void CreateAccount(string username, string password)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var enkriptovanaSifra = PomocneFunkcije.EncryptString(key, password);
            var titula = Common.Enum.Titula.KLIJENT;
            PomocneFunkcije.Write(username, enkriptovanaSifra, titula.ToString());
        }
        public bool DeleteAccount(string username)
        {
            try
            {
                PomocneFunkcije.DeleteAccount(username);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool DisableAccount(string username)
        {
            try
            {
                PomocneFunkcije.UpdateAccount(username, 3);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool EnableAccount(string username)
        {
            try
            {
                PomocneFunkcije.UpdateAccount(username, 2);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool LockAccount(string username)
        {
            try
            {
                PomocneFunkcije.UpdateAccount(username, 1);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
