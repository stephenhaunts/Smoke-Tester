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
using System.ComponentModel;
using ConfigurationTests.Attributes;
using System;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.OS)]
    public class COMClassTest : Test
    {
        [MandatoryField]
        [Category("COM Class Properties")]
        public string ClassName { get; set; }

        [MandatoryField]
        [Category("COM Class Properties")]
        public string DllName { get; set; }

        public override void Run()
        {
            if (Type.GetTypeFromProgID(ClassName, false) == null)
            {
                throw new AssertionException(string.Format(@"{0} was not found to be registered", DllName));
            }
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                        {
                            new COMClassTest
                                {
                                    ClassName = "clLoggedInUser",
                                    DllName = "AppSecurityController.dll",
                                    TestName = "Class Name Example"
                                }
                        };
        }
    }
}
