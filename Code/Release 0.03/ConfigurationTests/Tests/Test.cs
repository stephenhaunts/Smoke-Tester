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
using ConfigurationTests.Attributes;
using System.ComponentModel;

namespace ConfigurationTests.Tests
{
    [DefaultProperty("TestName")]   
    public abstract class Test
    {
        public abstract void Run();

        public abstract List<Test> CreateExamples();

        [MandatoryField]
        [Description("Name of test")]
        [Category("Common Test Properties")]
        public string TestName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", GetType().Name, TestName);
        }
    }
}