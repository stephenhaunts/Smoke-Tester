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
    public class ConnectionStringTestTests
    {
        [DeploymentItem("ConnectionStringTest.exe")]
        [DeploymentItem("ConnectionStringTest.exe.config")]
        [TestMethod]
        public void ReadSqlConnectionString()
        {
            var connectionStringSettings = new List<ConnectionStringTest.ConnectionStringSetting>
                        {   
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "Data Source", ExpectedValue= "10.99.175.100"},
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "Initial Catalog", ExpectedValue= "ExpressFinance"},
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "User ID", ExpectedValue= "WSUser"},
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "password", ExpectedValue= "grollyrabbit"},
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "MultipleActiveResultSets", ExpectedValue= "True"}
                        };

            var test = new ConnectionStringTest();
            string location = Assembly.GetExecutingAssembly().Location;
            test.Path = Path.GetDirectoryName(location);
            test.Filename = "ConnectionStringTest.exe.config";
            test.ConnectionStringName = "Nexus";
            test.StringSettings = connectionStringSettings;
            test.CheckConnectivity = false;

            test.Run();
        }


        [DeploymentItem("ConnectionStringTest.exe")]
        [DeploymentItem("ConnectionStringTest.exe.config")]
        [TestMethod]
        public void ReadOLEDBConnectionString()
        {
            var connectionStringSettings = new List<ConnectionStringTest.ConnectionStringSetting>
                        {   
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "Provider", ExpectedValue= "SQLOLEDB.1"},
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "Initial Catalog", ExpectedValue= "moneymart"},
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "Integrated Security", ExpectedValue= "SSPI"},
                            new ConnectionStringTest.ConnectionStringSetting{SettingName = "Data Source", ExpectedValue= @"ukuatefcosql02\sql2008"}
                        };

            var test = new ConnectionStringTest();
            string location = Assembly.GetExecutingAssembly().Location;
            test.Path = Path.GetDirectoryName(location);
            test.Filename = "ConnectionStringTest.exe.config";
            test.ConnectionStringName = "BounceProc";
            test.StringSettings = connectionStringSettings;
            test.CheckConnectivity = false;

            test.Run();
        }     

    }
}