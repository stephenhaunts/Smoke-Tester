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

using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Assembly)]
    public class AssemblyVersionTest : Test
    {
        [MandatoryField]
        [Description("The filename of the assembly you want to check.")]
        [Category("Assembly Properties")]
        public string DllName { get; set; }

        [MandatoryField]
        [Description("The version string for your assembly, eg 1.0.20.3")]
        [Category("Assembly Properties")]
        public string Version { get; set; }

        public override void Run()
        {
            AssertState.Equal(Version, GetAssemblyVersion());        
        }

        private string GetAssemblyVersion()
        {            
            var fvi = FileVersionInfo.GetVersionInfo(DllName);
            return fvi.FileVersion;
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                        {
                            new AssemblyVersionTest
                                {
                                    DllName = "MyDll.dll",
                                    Version = "1.3.0.0",
                                    TestName = "Assembly Registration Example"
                                }
                        };
        }
    }
}
