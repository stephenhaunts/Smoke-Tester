/**
* Smoke Tester Tool : Post deployment smoke testing tool.
* 
* http://www.stephenhaunts.com
* 
* This file is part of Smoke Tester Tool.
* 
* Smoke Tester Tool is free software: you can redistribute it and/or modify it under the terms of the
* GNU General Public License as published by the Free Software Foundation, either version 2 of the
* License, or (at your option) any later version.
* 
* Smoke Tester Tool is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* 
* See the GNU General Public License for more details <http://www.gnu.org/licenses/>.
* 
* Curator: Stephen Haunts
*/
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Common.Security
{
    public class Certificates
    {
        public static X509Certificate GetCertificateFromFile(string certificateFileName, string password = null)
        {
            X509Certificate result;

            if (string.IsNullOrWhiteSpace(password))
            {
                result = X509Certificate.CreateFromCertFile(certificateFileName);
            }
            else
            {
                result = new X509Certificate(certificateFileName, password, X509KeyStorageFlags.MachineKeySet);
            }

            return result;
        }

        public static X509Certificate GetCertificateFromStore(StoreName storeName, StoreLocation storeLocation, CertificateInfo certificateInfo)
        {
            X509Store store = new X509Store(storeName, storeLocation);

            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection collection = store.Certificates;

            foreach (
                KeyValuePair<X509FindType, string> search in
                    certificateInfo.Searches.Where(s => !string.IsNullOrWhiteSpace(s.Value)))
            {
                collection = collection.Find(search.Key, search.Value, true);
            }

            X509Certificate[] certificates = collection.OfType<X509Certificate>().ToArray();

            if (certificates.Length > 1)
            {
                throw new MultipleCertificatesException(certificateInfo, certificates);
            }

            if (certificates.Length == 0)
            {
                throw new CertificateNotFoundException(certificateInfo);
            }

            X509Certificate certificate = certificates.First();

            return certificate;
        }
    }
}
