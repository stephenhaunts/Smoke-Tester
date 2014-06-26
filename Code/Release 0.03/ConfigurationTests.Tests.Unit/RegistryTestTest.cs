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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationTests.Tests.Unit
{
    [TestClass]
    public class RegistryTestTest
    {
        [TestMethod]
        public void CheckRegistryKeySettings()
        {

            var rkt = new RegistryKeyTest();

            var expectedEntries = new List<RegistryKeyTest.RegistryEntry>
                        {
                            new RegistryKeyTest.RegistryEntry
                            {ValueName = "JobMinimumRetryDelay", ExpectedValue = "0x258", ValueType = RegistryKeyTest.RegistryValueType.REG_QWORD},
                            new RegistryKeyTest.RegistryEntry
                            {ValueName = "JobInactivityTimeout", ExpectedValue = "0x0076a700" , ValueType = RegistryKeyTest.RegistryValueType.REG_QWORD}
                        };

            rkt.BaseKey = RegistryKeyTest.RegistryBaseKey.HKEY_LOCAL_MACHINE;
            rkt.Path = "Software/Microsoft/Windows/CurrentVersion/BITS";
            rkt.ExpectedEntries = expectedEntries;

            rkt.Run();

        }
    }
}
