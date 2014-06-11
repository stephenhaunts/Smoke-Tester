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
using System.Data.SqlClient;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Database)]
    public class ConnectionStringTest : ConnectionStringTestBase
    {
        protected override void DoConnectivityCheck()
        {
            using (var sqlConnection = (new SqlConnection(ConnectionString)))
            {
                sqlConnection.Open();
            }
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                        {
                                 
                            new ConnectionStringTest
                                {
                                    TestName = "Example config connectionstring",
                                    Path = @"D:\Folder\Path\bin",
                                    Filename = "Example.exe.config",
                                    ConnectionStringName = "PrimaryConnectionString",
                                    StringSettings =
                                    new List<ConnectionStringSetting>
                                        {new ConnectionStringSetting{SettingName = "Initial Catalog", ExpectedValue= "AmberLive"}},
                                    CheckConnectivity = true
                                },
                            new ConnectionStringTest
                                {
                                    TestName = "Example web.config connectionstring",
                                    Path = @"D:\Folder\Path",
                                    Filename = "web.config",
                                    StringSettings =
                                    new List<ConnectionStringSetting>
                                        {new ConnectionStringSetting{SettingName="Data Source", ExpectedValue = @"sqlserverbox\SQL2008"}},
                                    CheckConnectivity = false
                                }
                        };
        }
    }
}