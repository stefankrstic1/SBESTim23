using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface ICheckingCSAS
    {
        [OperationContract]
        bool CheckIfAccExists(string message, byte[] sign);

        [OperationContract]
        string CriptoKey(string username); 
        //[OperationContract]
        //bool SendMessage(string message, byte[] sign);
    }
}
