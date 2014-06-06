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

namespace ConfigurationTests.Tests
{
    public class MinimumDiskSpaceTest : Test
    {
        [MandatoryField]
        public string Drive { get; set; }

        [MandatoryField]
        public long RequiredMegabytes { get; set; }

        public override void Run()
        {
            DriveInfo d = new DriveInfo(Drive);
            long actual = d.AvailableFreeSpace / 1024;

            if (actual < RequiredMegabytes)
            {
                throw new AssertionException<long>(RequiredMegabytes, actual,
                    string.Format("Insufficient space on drive {0}", Drive));
            }
        }

        public override List<Test> CreateExamples()
        {
                return new List<Test>
                             {
                                 new MinimumDiskSpaceTest{Drive="C",RequiredMegabytes = 123456,TestName = "Check space on C:"},
                                 new MinimumDiskSpaceTest{Drive="D",RequiredMegabytes = 12,TestName = "Check space on D:"}
                             };
        }
    }
}