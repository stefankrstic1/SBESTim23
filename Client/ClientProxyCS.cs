using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Client
{
    public class ClientProxyCS : ChannelFactory<IAccountManagement>, IAccountManagement, IDisposable
    {
        IAccountManagement factory;

        public ClientProxyCS(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public ClientProxyCS(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
            //Credentials.Windows.AllowNtlm = false;
        }

        public bool CreateAccount(string username, string password)
        {
            try
            {
                return factory.CreateAccount(username, password);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return false;
        }

        public bool DeleteAccount(string username)
        {
            try
            {
                bool isDeleted = factory.DeleteAccount(username);
                if(isDeleted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }

        public bool DisableAccount(string username)
        {
            try
            {
                bool isDisabled = factory.DisableAccount(username);
                if (isDisabled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }

        public bool EnableAccount(string username)
        {
            try
            {
                bool isEnabled = factory.EnableAccount(username);
                if (isEnabled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }

        public bool LockAccount(string username)
        {
            try
            {
                bool isLocked = factory.LockAccount(username);
                if (isLocked)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }
    }
}
