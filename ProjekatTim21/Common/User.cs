﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class User
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private Enum.Titula titula;

        public Enum.Titula Titula
        {
            get { return titula; }
            set { titula = value; }
        }

        private bool locked;

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        public User(string username, string password, Enum.Titula titula, bool locked)
        {
            this.username = username;
            this.password = password;
            this.titula = titula;
            this.locked = locked;          
        }

        public User()
        {
            this.username = "";
            this.password = "";
            this.titula = Enum.Titula.KLIJENT;
            this.locked = false;
        }
    }
}
