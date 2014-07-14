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
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.File_System)]
    public class FileListExistsTest : Test
    {
        private bool _shouldExist = true;
        private string _fileList;

        [DefaultValue(true)]
        [Category("File Properties")]
        [Description("List of filenames separated by a ';'. For example 'file1.txt;file2.txt;file3.txt'")]
        public string FileList
        {
            get { return _fileList; }
            set { _fileList = value; }
        }

        [DefaultValue("."), Description("Directory of file"), Category("File Properties")]
        public string Path { get; set; }

        private string FullFilePath(string file)
        {
            return System.IO.Path.Combine(Path, file);            
        }

        public override void Run()
        {
            var missingFile = false;
            var files = _fileList.Split(';');

            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(file))
                {
                    continue;
                }

                if (!File.Exists(FullFilePath(file)))
                {
                    missingFile = true;
                }
            }

            AssertState.Equal(false, missingFile, @"There was a file missing from the file list check.");
        }

        public override List<Test> CreateExamples()
        {
                return new List<Test>
                             {
                                 new FileListExistsTest
                                     {
                                         TestName = "Example executable",
                                         Path = @"D:\Folder\Path",
                                         FileList = "Example.exe;Example2.exe;Example3.exe"                                         
                                     },
                                 new FileListExistsTest
                                     {
                                        TestName = "Example executable",
                                         Path = @"D:\Folder\Path",
                                         FileList = "Example.exe"  
                                     }
                             };
        }
    }
}
