using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AuthenticationService
{
    public class WCFCheckingCSAS : ChannelFactory<ICheckingCSAS>, ICheckingCSAS, IDisposable
    {
        ICheckingCSAS factory;

        public WCFCheckingCSAS(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = PomocneFunkcije.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = PomocneFunkcije.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public bool CheckIfAccExists(string message, byte[] sign)
        {
            try
            {
                bool existst = factory.CheckIfAccExists(message, sign);
                if (existst)
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
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
                return false;
            }
        }

        /*public bool SendMessage(string message, byte[] sign)
        {
            try
            {
                return factory.SendMessage(message, sign);
            }
            catch (Exception e)
            {
                Console.WriteLine("[SendMessage] ERROR = {0}", e.Message);
                return false;
            }
        }*/

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
