using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CredentialsStore
{
    public class CheckingCSAS : ICheckingCSAS
    {
        public bool CheckIfAccExists(string username, string password)
        {
            
            //Validacija ulogovanog korisnika!!!!!
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            List<User> korisnici = new List<User>();
            korisnici = PomocneFunkcije.ReadUsers();
            string enkriptovanPassword = PomocneFunkcije.EncryptString(key, password);

            foreach(User u in korisnici)
            {
                if(u.Username == username && u.Password == enkriptovanPassword && u.IsEnabled == true && u.Locked == false)
                {
                    return true;               
                }             
            }
            return false;

        }
    }
}
