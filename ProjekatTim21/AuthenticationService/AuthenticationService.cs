using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        public static Dictionary<string, string> UserAccountsDB = new Dictionary<string, string>();

        public void Login(string username, string password)
        {
            if (!UserAccountsDB.ContainsKey(username))
            {
                UserAccountsDB.Add(username, username);
            }
            else
            {
                Console.WriteLine($"Korisnik sa korisnickim imenom {username} vec postoji u bazi");
            }
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
