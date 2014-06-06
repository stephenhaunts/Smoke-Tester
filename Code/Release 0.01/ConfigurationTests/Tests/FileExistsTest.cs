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
using System.ComponentModel;
using Common.Boolean;

namespace ConfigurationTests.Tests
{
    public class FileExistsTest : FileTest
    {
        private bool _shouldExist = true;

        [DefaultValue(true)]
        [Description("True to check if file exist")]
        public bool ShouldExist
        {
            get{return _shouldExist;}
            set{_shouldExist = value;}
        }

        public override void Run()
        {
            AssertState.Equal(ShouldExist, File.Exists(FullFilePath), string.Format("File was {0}present", ShouldExist.IfTrue("not ")));
        }

        public override List<Test> CreateExamples()
        {
                return new List<Test>
                             {
                                 new FileExistsTest
                                     {
                                         TestName = "Example executable",
                                         Path = @"D:\Folder\Path",
                                         Filename = "Example.exe",
                                         ShouldExist = true
                                     },
                                 new FileExistsTest
                                     {
                                         TestName = "Example config",
                                         Path = @"D:\Folder\Path",
                                         Filename = "Example.exe.config",
                                         ShouldExist = true
                                     }
                             };
        }
    }
}
