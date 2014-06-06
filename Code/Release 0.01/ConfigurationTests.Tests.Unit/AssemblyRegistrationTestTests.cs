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
    public class AssemblyRegistrationTestTests
    {
        [TestMethod]
        public void CheckAssemblyRegistered()
        {
            var rkt = new AssemblyRegistrationTest();
            rkt.DllName = "mscorlib.dll";
            rkt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void CheckAssemblyNotRegistered()
        {
            try
            {
                var rkt = new AssemblyRegistrationTest();
                rkt.DllName = "No such DLL";
                rkt.Run();
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual("No such DLL was not found to be registered in the GAC", ex.Message);
                throw;
            }
        }
    }
}
