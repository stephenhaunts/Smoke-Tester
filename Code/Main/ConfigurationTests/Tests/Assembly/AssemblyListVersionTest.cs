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
using System.IO;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Assembly)]
    public class AssemblyListVersionTest : Test
    {
        [MandatoryField]
        [Description("The filenames of the assembly you want to check separated by a ';'. For example 'file1.dll;file2.dll;file3.dll'")]
        [Category("Assembly Properties")]
        public string DllNames { get; set; }

        [MandatoryField]
        [Description("Path that contains the assemblies to be tested.")]
        [Category("Assembly Properties")]
        public string Path { get; set; }

        [MandatoryField]
        [Description("The version string for your assembly, eg 1.0.20.3")]
        [Category("Assembly Properties")]
        public string Version { get; set; }

        public override void Run()
        {
            var missingFile = false;
            var files = DllNames.Split(';');

            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(file))
                {
                    continue;
                }

                if (!File.Exists(FullFilePath(file)))
                {
                    missingFile = true;
                    break;
                }

                AssertState.Equal(Version, GetAssemblyVersion(file));        
            }

            AssertState.Equal(false, missingFile, @"There was a file missing from the file list check.");            
        }

        private string FullFilePath(string file)
        {
            return System.IO.Path.Combine(Path, file);
        }

        private string GetAssemblyVersion(string fileName)
        {
            var fvi = FileVersionInfo.GetVersionInfo(fileName);
            return fvi.FileVersion;
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                        {
                            new AssemblyListVersionTest
                                {
                                    DllNames = "MyDll.dll",
                                    Path="c:\folder",
                                    Version = "1.3.0.0",
                                    TestName = "Assembly Registration Example"
                                }
                        };
        }
    }
}
