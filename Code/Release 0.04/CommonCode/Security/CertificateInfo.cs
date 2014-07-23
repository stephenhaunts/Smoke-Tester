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
using System.Security.Cryptography.X509Certificates;
using Common.Collections;

namespace Common.Security
{
    public class CertificateInfo
    {
        internal AutoDictionary<X509FindType, string> Searches = new AutoDictionary<X509FindType, string>();

        public string ThumbPrint
        {
            set { Searches[X509FindType.FindByThumbprint] = value; }
            get { return Searches[X509FindType.FindByThumbprint]; }
        }
        public string SerialNumber
        {
            set { Searches[X509FindType.FindBySerialNumber] = value; }
            get { return Searches[X509FindType.FindBySerialNumber]; }
        }
        public string SubjectName
        {
            set { Searches[X509FindType.FindBySubjectName] = value; }
            get { return Searches[X509FindType.FindBySubjectName]; }
        }
        public string KeyUsage
        {
            set { Searches[X509FindType.FindByKeyUsage] = value; }
            get { return Searches[X509FindType.FindByKeyUsage]; }
        }
        public string IssuerName
        {
            set { Searches[X509FindType.FindByIssuerName] = value; }
            get { return Searches[X509FindType.FindByIssuerName]; }
        }
    }
}