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

            return true;
        }
    }
}
