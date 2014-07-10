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
    public class XMLElementTestTests
    {
        private XMLElementTest _XMLElementTest;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _XMLElementTest = new XMLElementTest();
        }

        [TestMethod]
        public void XmlElementTestReadValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/appSettings/add";
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "key", Value = "Hello" });
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "value", Value = "World" });
            _XMLElementTest.Run();
        }

        [TestMethod]
        public void XmlElementTestReadValueFromElementsWithDifferentAttributesTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/system.serviceModel/bindings/basicHttpBinding/binding";
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "name", Value = "BasicHttpBinding_ILoggingService" });
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "closeTimeout", Value = "00:01:00" });
            _XMLElementTest.Run();
        }

        [TestMethod]
        public void XmlElementTestOneInstanceTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/appSettings/add";
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "key", Value = "Hello" });
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "value", Value = "World" });
            _XMLElementTest.MaximumOccurrences = 1;

            try
            {
                _XMLElementTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("5 instances of /appSettings/add found where no more than 1 was expected", ex.Message);
            }
        }

        [TestMethod]
        public void XmlElementTestNoValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/appSettings/bob";
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "key", Value = "Hello" });
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "value", Value = "World" });

            try
            {
                _XMLElementTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("0 instances of /appSettings/bob found where at least 1 was expected", ex.Message);
            }
        }

        [TestMethod]
        public void XmlElementTestNoMatchTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _XMLElementTest.XmlPath = "/appSettings/add";
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "key", Value = "Hello" });
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "value", Value = "Everybody" });

            try
            {
                _XMLElementTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("No instances of /appSettings/add found with all expected attributes matching (most was 1)", ex.Message);
            }
        }

        [TestMethod]
        public void XmlElementTestFileNotFoundTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            _XMLElementTest.Path = Path.GetDirectoryName(location);
            _XMLElementTest.Filename = string.Format("{0}.confug", Path.GetFileName(location));
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "key", Value = "Hello" });
            _XMLElementTest.ExpectedValues.Add(new XMLElementTest.Attribute { Name = "value", Value = "Everybody" });

            try
            {
                _XMLElementTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("File Not Found", ex.Message);
            }
        }
    }
}


