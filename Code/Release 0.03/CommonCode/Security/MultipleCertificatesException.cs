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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Common.Security
{
    public sealed class MultipleCertificatesException : CertificateException
    {
        public X509Certificate[] Certificates { get; set; }

        public MultipleCertificatesException(CertificateInfo certificateInfo, X509Certificate[] certificates)
            : base(string.Format("Multiple certificates were found with the supplied CertificateInfo parameters:\r\n{0}\r\nCertificates:\r\n{1}", GetCertificateInfo(certificateInfo),GetCertificateSubjects(certificates)))
        {
            Certificates = certificates;
            CertificateInfo = certificateInfo;
        }

        private static object GetCertificateSubjects(IEnumerable<X509Certificate> certificates)
        {
            return string.Join(Environment.NewLine, certificates.Select(c => string.Format("\t{0}", c.Subject)));
        }
    }
}