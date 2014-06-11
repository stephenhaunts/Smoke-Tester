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

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using ConfigurationTests.Attributes;
using System.ComponentModel;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.File_System)]
    public class MD5Checksum : Test
    {        
        [MandatoryField]
        [Description("Filename to check")]
        [Category("Checksum Details")]
        public string Filename { get; set; }

        [MandatoryField]
        [Category("Checksum Details")]
        [Description("MD5 Checksum")]
        public string Checksum { get; set; }

        public override void Run()
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(Filename))
                {                    
                    var actualChecksum = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty).ToLower();
                    AssertState.Equal(Checksum.ToLower(), actualChecksum);
                }
            }  
        }

        public override List<Test> CreateExamples()
        {
           return new List<Test>
            {
                new MD5Checksum{Checksum = "4563FD", Filename = @"c:\foo.txt", TestName = "Check file checksum"}
            };
        }
    }
}