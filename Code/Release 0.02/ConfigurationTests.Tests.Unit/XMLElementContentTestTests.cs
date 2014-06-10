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

namespace ConfigurationTestUnitTests
{
    [TestClass]
    public class XMLElementContentTestTests
    {
        private XMLElementContentTest _XMLElementTest;

        [TestInitialize]
        public void MyTestIntialize()
        {
            _XMLElementTest = new XMLElementContentTest();            
        }

        [TestMethod]
        public void XmlContentTestReadValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/mySettings/xmlcontent";
            _XMLElementTest.ExpectedValue = "Hello World";
            _XMLElementTest.Run();
        }

        [TestMethod]
        public void XmlContentTestOneInstanceTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/mySettings/xmlcontent";
            _XMLElementTest.ExpectedValue = "Hello World";
            _XMLElementTest.MaximumOccurrences = 1;

            try
            {
                _XMLElementTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("2 instances of /mySettings/xmlcontent found where no more than 1 was expected", ex.Message);
            }
        }

        [TestMethod]
        public void XmlContentTestNoValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/mySettings/bob";
            _XMLElementTest.ExpectedValue = "Hello World";

            try
            {
                _XMLElementTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("0 instances of /mySettings/bob found where at least 1 was expected", ex.Message);
            }
        }

        [TestMethod]
        public void XmlContentTestNoMatchTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/mySettings/xmlcontent";
            _XMLElementTest.ExpectedValue = "Hello Bob";

            try
            {
                _XMLElementTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("No instances of /mySettings/xmlcontent found with the expected content", ex.Message);
            }
        }
    }
}