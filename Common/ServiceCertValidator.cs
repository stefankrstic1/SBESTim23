using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ServiceCertValidator : X509CertificateValidator
    {
		public override void Validate(X509Certificate2 certificate)
		{
			/// This will take service's certificate from storage
			X509Certificate2 srvCert = PomocneFunkcije.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
				PomocneFunkcije.ParseName(WindowsIdentity.GetCurrent().Name));

			if (!certificate.Issuer.Equals(srvCert.Issuer))
			{
				throw new Exception("Certificate is not from the valid issuer.");
			}
		}
	}
}
