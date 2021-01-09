using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum HashAlgorithm { SHA1, SHA256 }
    public class DigitalSignature
    {
        public static byte[] Create(string message, HashAlgorithm hashAlgorithm, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;
            
            if (csp == null)
            {
                throw new Exception("Valid certificate was not found");
            }
            byte[] signature = null;
            byte[] hash = null;
            byte[] data = null;

            UnicodeEncoding encoding = new UnicodeEncoding();
            data = encoding.GetBytes(message);

            if (hashAlgorithm.Equals(HashAlgorithm.SHA1))
            {
                SHA1Managed sHA1 = new SHA1Managed();
                hash = sHA1.ComputeHash(data);
            }
            else if (hashAlgorithm.Equals(HashAlgorithm.SHA256))
            {
                SHA256Managed sHA256 = new SHA256Managed();
                hash = sHA256.ComputeHash(data);
            }

            signature = csp.SignHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()));

            return signature;
        }


        public static bool Verify(string message, HashAlgorithm hashAlgorithm, byte[] signature, X509Certificate2 certificate)
        {

            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;
            
            if (csp == null)
                throw new Exception("Valid certificate wasn't found");

            byte[] hash = null;
            byte[] data = null;

            UnicodeEncoding encoding = new UnicodeEncoding();
            data = encoding.GetBytes(message);

            if (hashAlgorithm.Equals(HashAlgorithm.SHA1))
            {
                SHA1Managed sHA1 = new SHA1Managed();
                hash = sHA1.ComputeHash(data);
            }
            else if (hashAlgorithm.Equals(HashAlgorithm.SHA256))
            {
                SHA256Managed sHA256 = new SHA256Managed();
                hash = sHA256.ComputeHash(data);
            }

            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()), signature);

        }
    }
}
