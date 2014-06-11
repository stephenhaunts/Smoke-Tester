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
using System.ComponentModel;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.OS)]
    public class EnvironmentVariableContainsTest : EnvironmentVariableTest
    {
        [MandatoryField]
        [Category("Environment Variable Properties")]
        public string ExpectedValue{ get; set; }

        [DefaultValue(false)]
        [Category("Environment Variable Properties")]
        public bool CaseSensitive{ get; set; }

        public override void Run()
        {
            var s = Environment.GetEnvironmentVariable(Variable) ?? string.Empty;

            if (!CaseSensitive)
            {
                s = s.ToUpper();
                ExpectedValue = ExpectedValue.ToUpper();
            }

            if (!s.Contains(ExpectedValue))
            {
                throw new AssertionException(
                    string.Format("Environment variable {0} did not contain expected text [{1}]", Variable,
                        ExpectedValue));
            }
        }

        public override List<Test> CreateExamples()
        {
                return new List<Test>
                           {
                               new EnvironmentVariableContainsTest
                                   {
                                       TestName = "Path Example",
                                       Variable = "PATH",
                                       ExpectedValue = @"C:\Windows",
                                       CaseSensitive = false
                                   }
                           };
        }
    }
}