﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CredentialsStore
{
    public class CheckingCSAS : ICheckingCSAS
    {
        public static Dictionary<string, int> brojPokusaja = new Dictionary<string, int>();
        public static Dictionary<string, DateTime> vrijemeLokdauna = new Dictionary<string, DateTime>(); 


        public bool CheckIfAccExists(string username, string password)
        {
              

            //Validacija ulogovanog korisnika!!!!!
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            List<User> korisnici = new List<User>();
            korisnici = PomocneFunkcije.ReadUsers();
            string enkriptovanPassword = PomocneFunkcije.EncryptString(key, password);

            vrijemeLokdauna.TryGetValue(username, out var vrijeme);


            foreach(User u in korisnici)
            {
                if(u.Username == username && u.Password == enkriptovanPassword && DateTime.Now.Ticks - vrijeme.Ticks > 20)
                {
                    PomocneFunkcije.UpdateAccount(username, 4);
                    u.Locked = false;
                }

                if (u.Username == username && u.Password == enkriptovanPassword && u.IsEnabled == true && u.Locked == false)
                {
                    return true;               
                }               

            }


          if(brojPokusaja.ContainsKey(username))
            {
                brojPokusaja.TryGetValue(username, out var brojac);
                brojPokusaja[username] = ++brojac;

                if(brojac == 5)
                {
                    PomocneFunkcije.UpdateAccount(username, 1);
                    vrijemeLokdauna.Add(username, DateTime.Now);
                }
            }
          else
            {
                brojPokusaja.Add(username, 1);
            }


            return false;

        }
    }
}
