using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationTests.Tests.Unit
{
    [TestClass]
    public class ExensionsTest
    {
        [TestMethod]
        public void CopyReturnANewInstanceTestWithTheSameValues()
        {
            var target = new ConnectionStringTest() 
            { 
                Filename = "Filename", 
                Path = "Path", 
                StringSettings = new System.Collections.Generic.List<ConnectionStringTestBase.ConnectionStringSetting> 
                { 
                    new ConnectionStringTestBase.ConnectionStringSetting 
                    { 
                        SettingName = "SettingName", 
                        ExpectedValue = "ExpectedValue" 
                    } 
                }, 
                TestName = "TestName" 
            };

            var actual = target.Copy() as ConnectionStringTest;
            Assert.AreEqual(target.Filename, actual.Filename);
            Assert.AreEqual(target.Path, actual.Path);
            Assert.AreEqual(target.StringSettings[0].SettingName, actual.StringSettings[0].SettingName);
            Assert.AreEqual(target.StringSettings[0].ExpectedValue, actual.StringSettings[0].ExpectedValue);
            Assert.AreEqual(target.TestName, actual.TestName);
        }
        [TestMethod]
        public void CopyCreateANewInstanceTestWithTheSameValues()
        {
            var target = new ConnectionStringTest();

            var actual = target.Copy() as ConnectionStringTest;

            Assert.AreNotSame(target, actual);
        }
    }
}
