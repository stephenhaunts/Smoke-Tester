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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationTests.Tests.Unit
{
    [TestClass]
    public class COMClassTestTest
    {        
        [TestMethod]
        public void CheckDllIsRegistered()
        {
            var rkt = new COMClassTest();
            rkt.ClassName = "InternetExplorer.Application";
            rkt.DllName = "Some Internet Explorer DLL";
            rkt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void CheckDllIsNotRegistered()
        {
            try
            {
                var rkt = new COMClassTest();
                rkt.ClassName = "FishFood";
                rkt.DllName = "No such DLL";
                rkt.Run();
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("No such DLL was not found to be registered", ex.Message);
                throw;
            }
        }
    }
}
