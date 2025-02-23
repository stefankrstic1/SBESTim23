﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace Common
{
    public class ClientCertValidator : X509CertificateValidator
    {
		public override void Validate(X509Certificate2 certificate)
		{
			if (certificate.Subject.Equals(certificate.Issuer))
			{
				throw new Exception("Certificate is self-issued.");
			}
		}
	}
}
