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
using ConfigurationTests.Attributes;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

namespace ConfigurationTests.Tests
{
    public abstract class ConnectionStringTestBase : ConfigurationTest
    {
        protected ConnectionStringTestBase()
        {
            StringSettings = new List<ConnectionStringSetting>();
        }

        public class ConnectionStringSetting
        {
            public string SettingName { get; set; }
            public string ExpectedValue { get; set; }
            public override string ToString()
            {
                return SettingName;
            }
        }

        [MandatoryField]
        public string ConnectionStringName { get; set; }

        [MandatoryField]
        public List<ConnectionStringSetting> StringSettings { get; set; }

        public bool CheckConnectivity { get; set; }
        protected string ConnectionString { get; set; }
        protected abstract void DoConnectivityCheck();

        protected virtual void CheckAssertions()
        {
            CheckConnectionString(ConnectionString);

            if (CheckConnectivity)
            {
                DoConnectivityCheck();
            }
        }

        public override void Run()
        {
            var connectionStringSettings = GetConnectionStringSettings();

            ConnectionString = GetConnectionString(connectionStringSettings);
            
            CheckAssertions();
        }

        protected void CheckConnectionString(string connectionString)
        {
            DbConnectionStringBuilder dbConnection = new DbConnectionStringBuilder { ConnectionString = connectionString };

            foreach (ConnectionStringSetting setting in StringSettings)
            {
                if (dbConnection.ContainsKey(setting.SettingName))
                {
                    AssertState.Equal(setting.ExpectedValue, dbConnection[setting.SettingName].ToString());
                }
                else
                {
                    throw new AssertionException(string.Format("Connection String setting [{0}] not found", setting.SettingName));
                }
            }

            if (dbConnection.Keys != null)
            {
                AssertState.Equal(StringSettings.Count, dbConnection.Keys.Count);
            }
            else
            {
                throw new AssertionException("No StringSetting values were found");
            }
        }

        protected static string GetConnectionString(ConnectionStringSettings connectionStringSettings)
        {
            return connectionStringSettings.ConnectionString;
        }

        protected ConnectionStringSettings GetConnectionStringSettings()
        {
            ConnectionStringsSection connectionStrings = (ConnectionStringsSection)GetConfig().GetSection("connectionStrings");
            ConnectionStringSettings connectionStringSettings = connectionStrings.ConnectionStrings[ConnectionStringName];

            if (connectionStringSettings == null)
            {
                throw new AssertionException(string.Format(@"Connection String [{0}] not found", ConnectionStringName));
            }

            return connectionStringSettings;
        }

    }
}