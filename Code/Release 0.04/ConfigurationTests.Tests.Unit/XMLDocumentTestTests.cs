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
using System.IO;
using System.Reflection;
using ConfigurationTests;
using ConfigurationTests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ConfigurationTestUnitTests
{
    [TestClass]
    public class XMLDocumentTestTests
    {
        private XMLDocumentTest _XMLDocumentTest;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _XMLDocumentTest = new XMLDocumentTest();
        }

        [TestMethod]
        public void XmlDocumentTestReadValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLDocumentTest.Path = Path.GetDirectoryName(location);
            _XMLDocumentTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLDocumentTest.XPathQuery = "/configuration/appSettings/add[@key='Hello']/@value";
            _XMLDocumentTest.ExpectedOutput = "value=\"World\"";
            _XMLDocumentTest.Run();
        }

        [TestMethod]
        public void XmlDocumentTestNoValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _XMLDocumentTest.Path = Path.GetDirectoryName(location);
            _XMLDocumentTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLDocumentTest.XPathQuery = "/appSettings/bob";
            _XMLDocumentTest.ExpectedOutput = "";

            try
            {
                _XMLDocumentTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("XPath query: /appSettings/bob did not return anything.", ex.Message);
            }
        }

        [TestMethod]
        public void XmlDocumentTestNoMatchTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLDocumentTest.Path = Path.GetDirectoryName(location);
            _XMLDocumentTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLDocumentTest.XPathQuery = "/configuration/appSettings/add[@key='Help']/@value";
            _XMLDocumentTest.ExpectedOutput = "";

            try
            {
                _XMLDocumentTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("XPath query: /configuration/appSettings/add[@key='Help']/@value did not return anything.", ex.Message);
            }
        }

        [TestMethod]
        public void XmlDocumentTestOutputNoMatchTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLDocumentTest.Path = Path.GetDirectoryName(location);
            _XMLDocumentTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLDocumentTest.XPathQuery = "/configuration/appSettings/add[@key='Hello']/@value";
            _XMLDocumentTest.ExpectedOutput = "value=\"Fake\"";

            try
            {
                _XMLDocumentTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("Expected output: value=\"Fake\" does not match actual output: value=\"World\".", ex.Message);
            }
        }

        [TestMethod]
        public void XmlDocumentTestFileNotFoundTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLDocumentTest.Path = Path.GetDirectoryName(location);
            _XMLDocumentTest.Filename = string.Format("{0}.confug", Path.GetFileName(location));
            _XMLDocumentTest.XPathQuery = "";
            _XMLDocumentTest.ExpectedOutput = "";

            try
            {
                _XMLDocumentTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("File Not Found: ", ex.Message.Substring(0, 16));
            }
        }

        [TestMethod]
        public void XmlDocumentTestQueryReturnsAllElementsTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLDocumentTest.Path = Path.GetDirectoryName(location);
            _XMLDocumentTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLDocumentTest.XPathQuery = "/configuration/appSettings/add";
            _XMLDocumentTest.ExpectedOutput = "<add key=\"AmberLive_UserId\" value=\"AmberNetWS\" /><add key=\"AmberLive_Password\" value=\"OFT123_\" /><add key=\"Hello\" value=\"World\" /><add key=\"EncryptedHello\" value=\"/VkaDsFycfHJOJTr3Sh+Cw==\" /><add key=\"HelloAgain\" value=\"Everyone\" />";
            _XMLDocumentTest.Run();
        }
    }
}


