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
using ConfigurationTests.Tests;
using Common.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationTestUnitTests
{
    [TestClass]
    public class CertificateExistsTestTests
    {
        [TestMethod]
        [ExpectedException(typeof(MultipleCertificatesException))]
        public void TestMultipleCertificatesFound()
        {
            var test = new CertificateExistsTest
            {
                CertificateStoreLocation = StoreLocation.LocalMachine,
                CertificateStoreName = StoreName.Root
            };

            test.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(CertificateNotFoundException))]
        public void TestNoCertificateFound()
        {
            var test = new CertificateExistsTest
            {
                CertificateStoreLocation = StoreLocation.LocalMachine,
                CertificateStoreName = StoreName.Root,
                SubjectName = "Bob"
            };

            test.Run();
        }
    }
}