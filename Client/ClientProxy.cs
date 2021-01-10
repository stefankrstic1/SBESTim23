using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    public class ClientProxy : ChannelFactory<IAuthenticationService>, IAuthenticationService, IDisposable
    {
        IAuthenticationService factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
            //Credentials.Windows.AllowNtlm = false;
        }

        public bool Login(string username, string password)
        {
            try
            {
                return factory.Login(username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            return false;
        }

        public void Logout(string username)
        {
            try
            {
                factory.Logout(username);
            }           
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            
        }
    }
}
