﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        void Logout(string username);
    }
}
