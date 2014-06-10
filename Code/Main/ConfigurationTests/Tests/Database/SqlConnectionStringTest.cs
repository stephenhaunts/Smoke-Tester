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
using ConfigurationTests.Attributes;
using Common.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace ConfigurationTests.Tests
{
    public class SqlConnectionStringTest : ConnectionStringTestBase
    {
        private AppSettingsSection _appSettingSection;

        public SqlConnectionStringTest()
        {
            AppSettings = new List<AppSetting>();
        }
        
        public class AppSetting
        {
            public string SettingName { get; set; }
            public string ExpectedValue { get; set; }
            public override string ToString()
            {
                return SettingName;
            }
        }

        [MandatoryField]
        [Category("App Setting Properties")]
        public List<AppSetting> AppSettings { get; set; }

        protected override void CheckAssertions()
        {
            CheckConnectionString(ConnectionString);
            CheckAppSettings();

            if (CheckConnectivity)
            {
                DoConnectivityCheck();
            }
        }

        private void CheckAppSettings()
        {
            _appSettingSection = (AppSettingsSection)GetConfig().GetSection("appSettings");

            if (_appSettingSection == null)
            {
                throw new AssertionException("No AppSettings section was found");
            }

            if (_appSettingSection.Settings.Count == 0)
            {
                throw new AssertionException("No AppSettings found in config");
            }

            ValidateAppSetting(_appSettingSection, "_UserId");
            ValidateAppSetting(_appSettingSection, "_Password", true);
        }

        private void ValidateAppSetting(AppSettingsSection appSettings, string appSettingName, bool isOptional = false)
        {
            string appSettingIncludingConnectionStringName = string.Format("{0}{1}", ConnectionStringName, appSettingName);

            var setting = appSettings.Settings[appSettingIncludingConnectionStringName];

            if (setting == null)
            {
                throw new AssertionException(String.Format("AppSetting {0} not found in config",
                    appSettingIncludingConnectionStringName));
            }

            if (isOptional)
            {
                return;
            }

            var appSettingFound = AppSettings.Find(a => a.SettingName == appSettingIncludingConnectionStringName);

            if (appSettingFound == null)
            {
                throw new AssertionException(String.Format("AppSetting {0} not found in test",
                    appSettingIncludingConnectionStringName));
            }

            AssertState.Equal(setting.Value, appSettingFound.ExpectedValue);
        }

        protected override void DoConnectivityCheck()
        {
            var connectionStringSettings = GetConfig().ConnectionStrings;
            var repository = new SqlDataRepository(ConnectionStringName, _appSettingSection, connectionStringSettings);
            var guid = repository.BeginTransaction(IsolationLevel.Unspecified);

            repository.AbortTransaction(guid);
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                             {
                                 
                                 new SqlConnectionStringTest
                                     {
                                         TestName = "Example exe.config connectionstring",
                                         Path = @"D:\Folder\Path\bin",
                                         Filename = "Example.exe.config",
                                         ConnectionStringName = "PrimaryConnectionString",
                                         StringSettings =
                                           new List<ConnectionStringSetting>
                                               {new ConnectionStringSetting{SettingName = "Data Source", ExpectedValue= "ukfakesql01"},
                                                new ConnectionStringSetting{SettingName = "Initial Catalog", ExpectedValue= "FakeDatabase"}},
                                         AppSettings =
                                           new List<AppSetting>
                                               {new AppSetting{SettingName = "FakeDatabase_UserId", ExpectedValue= "FakeUser"},
                                                new AppSetting{SettingName = "FakeDatabase_Password", ExpectedValue= "password"}},
                                         CheckConnectivity = false
                                     },
                                 new SqlConnectionStringTest
                                     {
                                         TestName = "Example web.config connectionstring without password",
                                         Path = @"D:\Folder\Path",
                                         Filename = "web.config",
                                         StringSettings = new List<ConnectionStringSetting>{
                                                new ConnectionStringSetting{SettingName="Data Source", ExpectedValue = @"fakesql01\sql2008"},
                                                new ConnectionStringSetting{SettingName = "Initial Catalog", ExpectedValue= "FakeDatabase"}
                                         },
                                         AppSettings = new List<AppSetting>{
                                                new AppSetting{SettingName = "FakeDatabase_UserId", ExpectedValue= "FakeUser"}
                                         },
                                         CheckConnectivity = true
                                     }
                             };
        }
    }
}