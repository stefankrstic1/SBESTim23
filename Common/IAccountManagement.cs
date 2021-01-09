using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IAccountManagement
    {
        [OperationContract]
        bool CreateAccount(string username, string password);

        [OperationContract]
        bool DeleteAccount(string username);

        [OperationContract]
        bool LockAccount(string username);

        [OperationContract]
        bool EnableAccount(string username);

        [OperationContract]
        bool DisableAccount(string username);
    }
}
