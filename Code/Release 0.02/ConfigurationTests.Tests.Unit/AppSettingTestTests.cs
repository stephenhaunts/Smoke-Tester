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
using System.Configuration;
using System.IO;
using System.Reflection;
using ConfigurationTests;
using ConfigurationTests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ConfigurationTestUnitTests
{
    [TestClass]
    public class AppSettingTestTests
    {
        private AppSettingTest _AppSettingTest;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _AppSettingTest = new AppSettingTest();
        }

        [TestMethod]
        public void AppSettingTestReadValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _AppSettingTest.Path = Path.GetDirectoryName(location);
            _AppSettingTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _AppSettingTest.Key = "Hello";
            _AppSettingTest.ExpectedValue = "World";
            _AppSettingTest.Run();
        }

        [TestMethod]
        public void AppSettingTestNoValueTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _AppSettingTest.Path = Path.GetDirectoryName(location);
            _AppSettingTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _AppSettingTest.Key = "Hallo";
            _AppSettingTest.ExpectedValue = "World";

            try
            {
                _AppSettingTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("AppSetting with Key [Hallo] was not found", ex.Message);
            }
        }

        [TestMethod]
        public void AppSettingTestNoMatchTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _AppSettingTest.Path = Path.GetDirectoryName(location);
            _AppSettingTest.Filename = string.Format("{0}.config", Path.GetFileName(location));

            _AppSettingTest.Key = "Hello";
            _AppSettingTest.ExpectedValue = "Everybody";

            try
            {
                _AppSettingTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException<string> ex)
            {
                Assert.AreEqual("Expected [Everybody] Actual [World]", ex.Message);
            }
        }

        [TestMethod]
        public void AppSettingTestFileNotFoundTest()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _AppSettingTest.Path = Path.GetDirectoryName(location);
            _AppSettingTest.Filename = string.Format("{0}.confug", Path.GetFileName(location));
            _AppSettingTest.Key = "Hello";
            _AppSettingTest.ExpectedValue = "Everybody";

            try
            {
                _AppSettingTest.Run();
                throw new AssertFailedException("Expected exception was not thrown");
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("File Not Found"));
            }
        }
      
        [TestMethod]
        public void GivenAnAppSettingTheSettingShouldBeReturned()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _AppSettingTest.Path = Path.GetDirectoryName(location);
            _AppSettingTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _AppSettingTest.Key = "Hello";
            _AppSettingTest.ExpectedValue = "World";

            string result = _AppSettingTest.GetSettingElement();
            Assert.AreEqual(_AppSettingTest.ExpectedValue, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void GivenAnInvalidAppSettingAnAssertionExceptionShouldBeThrown()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            _AppSettingTest.Path = Path.GetDirectoryName(location);
            _AppSettingTest.Filename = string.Format("{0}.config", Path.GetFileName(location));
            _AppSettingTest.Key = "error";
            _AppSettingTest.ExpectedValue = "World";

            string result = _AppSettingTest.GetSettingElement();
        }   
    }
}