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
using System.IO;
using System.Reflection;
using ConfigurationTests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ConfigurationTestUnitTests
{
    [TestClass]    
    public class SqlConnectionStringTestTests
    {
        [DeploymentItem("SqlConnectionStringTest.exe")]
        [DeploymentItem("SqlConnectionStringTest.exe.config")]
        [TestMethod]
        [TestCategory("Integration")]
        public void ReadSqlConnectionString()
        {
            var connectionStringSettings = new List<SqlConnectionStringTest.ConnectionStringSetting>
                        {   
                            new SqlConnectionStringTest.ConnectionStringSetting{SettingName = "Data Source", ExpectedValue= "sqlbox"},
                            new SqlConnectionStringTest.ConnectionStringSetting{SettingName = "Initial Catalog", ExpectedValue= "mydatabase"}
                        };

            var appSettings = new List<SqlConnectionStringTest.AppSetting>
                        {   
                            new SqlConnectionStringTest.AppSetting{SettingName = "UserId", ExpectedValue= "dbUser"},
                            new SqlConnectionStringTest.AppSetting{SettingName = "Password", ExpectedValue= "password"}
                        };

            var test = new SqlConnectionStringTest();
            string location = Assembly.GetExecutingAssembly().Location;
            test.Path = Path.GetDirectoryName(location);
            test.Filename = "SqlConnectionStringTest.exe.config";
            test.ConnectionStringName = "ConnectionString";
            test.StringSettings = connectionStringSettings;
            test.AppSettings = appSettings;
            test.CheckConnectivity = true;

            test.Run();
        }

        [DeploymentItem("SqlConnectionStringTest.exe")]
        [DeploymentItem("SqlConnectionStringTest.exe.config")]
        [TestMethod]
        [TestCategory("Integration")]
        public void ReadSqlConnectionStringThatHasNoPassword()
        {
            var connectionStringSettings = new List<SqlConnectionStringTest.ConnectionStringSetting>
                        {   
                            new SqlConnectionStringTest.ConnectionStringSetting{SettingName = "Data Source", ExpectedValue= "sqlbox"},
                            new SqlConnectionStringTest.ConnectionStringSetting{SettingName = "Initial Catalog", ExpectedValue= "mydatabase"}
                        };
            var appSettings = new List<SqlConnectionStringTest.AppSetting>
                        {   
                            new SqlConnectionStringTest.AppSetting{SettingName = "UserId", ExpectedValue= "dbUser"}
                        };

            var test = new SqlConnectionStringTest();
            string location = Assembly.GetExecutingAssembly().Location;
            test.Path = Path.GetDirectoryName(location);
            test.Filename = "SqlConnectionStringTest.exe.config";
            test.ConnectionStringName = "AmberLive";
            test.StringSettings = connectionStringSettings;
            test.AppSettings = appSettings;
            test.CheckConnectivity = true;

            test.Run();
        }
    }
}