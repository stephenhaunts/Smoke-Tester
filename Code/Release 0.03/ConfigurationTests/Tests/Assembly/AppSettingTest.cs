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
using System.Configuration;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Assembly)]
    public class AppSettingTest : ConfigurationTest
    {
        [MandatoryField]
        [Category("App Settings Check")]
        public string Key { get; set; }

        [Category("App Settings Check")]
        public string ExpectedValue { get; set; }

        [DefaultValue(false)]
        public bool CaseSensitive { get; set; }

        public override void Run()
        {
            AssertState.Equal(ExpectedValue, GetSettingElement(), CaseSensitive);
        }

        protected internal string GetSettingElement()
        {
            var appSettingsSection = (AppSettingsSection)GetConfig().GetSection("appSettings");
            var element = appSettingsSection.Settings[Key];

            if (element == null)
            {
                throw new AssertionException(string.Format(@"AppSetting with Key [{0}] was not found", Key));
            }

            return element.Value;
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                       {
                           new AppSettingTest
                               {
                                   TestName = "Example AppSetting Test",
                                   Key="UserId",
                                   ExpectedValue = "1234",
                                   Filename = "MyProgram.exe.config"
                               }
                       };
        }
    }
}