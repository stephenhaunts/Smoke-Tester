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
using System.Reflection;
using Common.Reflection;

namespace Common.Security
{
    public abstract class CertificateException : Exception
    {
        protected CertificateException(string message)
            : base(message)
        {
        }

        public CertificateInfo CertificateInfo { get; set;}

        protected static string GetCertificateInfo(CertificateInfo certificateInfo)
        {
            string result = string.Empty;

            PropertyInfo[] propertyInfos = typeof (CertificateInfo).GetProperties();

            foreach (PropertyInfo info in propertyInfos)
            {
                object value = info.GetValue(certificateInfo);
                string reportedValue = value == null ? "null" : string.Format("\"{0}\"", value);

                result += string.Format("\t{0}: {1}\r\n", info.Name,reportedValue);
            }

            return result;
        }
    }
}