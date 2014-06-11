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
using System.IO;
using ConfigurationTests.Attributes;
using System.ComponentModel;
using Common.Boolean;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.File_System)]
    public class FolderExistsTest : Test
    {
        private bool _shouldExist = true;

        [MandatoryField]
        [Category("Folder Properties")]
        [Description("Folder to Check")]
        public string Path { get; set; }

        [DefaultValue(true)]
        [Category("Folder Properties")]
        [Description("True to check if file exist")]
        public bool ShouldExist 
        {
            get { return _shouldExist; }
            set { _shouldExist = value; }
        }

        public override void Run()
        {
            if (Directory.Exists(Path) != ShouldExist)
            {
                throw new AssertionException(string.Format("Folder was {0}present", ShouldExist.IfTrue("not ")));
            }
        }

        public override List<Test> CreateExamples()
        {
           return new List<Test>
            {
                new FolderExistsTest{Path=@"c:\Windows\System32",TestName = "Check System32 folder"}
            };
        }
    }
}