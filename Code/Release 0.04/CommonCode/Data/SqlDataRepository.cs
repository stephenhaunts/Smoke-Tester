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
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Data
{
    public sealed class SqlDataRepository : DataRepository<SqlConnection>
    {        
        protected override string ConnectionString { get; set; }

        public SqlDataRepository(string connectionName, AppSettingsSection appSettingsSection, ConnectionStringsSection connectionStringSettingsSection)           
        {
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                throw new ArgumentException("Value cannot be null, empty or have whitespace.", "connectionName");
            }

            if (appSettingsSection == null)
            {
                throw new ArgumentNullException("appSettingsSection");
            }

            if (connectionStringSettingsSection == null)
            {
                throw new ArgumentNullException("connectionStringSettingsSection");
            }

            NameValueCollection appSettings = new NameValueCollection();

            foreach (KeyValueConfigurationElement setting in appSettingsSection.Settings)
            {
                appSettings.Add(setting.Key, setting.Value);
            }

            SetConnectionString(connectionName, connectionStringSettingsSection.ConnectionStrings, appSettings);
        }

        public SqlDataRepository(string connectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                throw new ArgumentException("Value cannot be null, empty or have whitespace.", "connectionName");
            }

            SetConnectionString(connectionName, ConfigurationManager.ConnectionStrings, ConfigurationManager.AppSettings);
        }

        private void SetConnectionString(string connectionName, ConnectionStringSettingsCollection connectionStringSettingsCollection, NameValueCollection settings)
        {
            if (connectionStringSettingsCollection == null || connectionStringSettingsCollection.Count == 0)
            {
                throw new InvalidOperationException(string.Format("No connectionStrings section were found in the config file, {0}.", ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).FilePath));
            }

            ConnectionStringSettings connectionStringSettings = connectionStringSettingsCollection[connectionName];

            if (connectionStringSettings == null)
            {
                StringBuilder message = new StringBuilder();

                for (int i = 0; i < connectionStringSettingsCollection.Count; i++)
                {
                    message.AppendFormat("{0}, ", connectionStringSettingsCollection[i].ConnectionString);
                }

                throw new InvalidOperationException(String.Format("ConnectionName {0} was not in the ConnectionStrings found: {1}", connectionName, message));
            }

            if (!connectionStringSettings.ConnectionString.Contains(';'))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Connection string {0} appears to be improperly formatted: Key Value Pairs are expected to be delimited with a semicolon. If there is only one Key Value Pair, terminate the connection string with a semicolon.",
                        connectionName));
            }

            string[] elements = connectionStringSettings.ConnectionString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
            string badElement = elements.FirstOrDefault(e => !e.Contains('='));

            if (badElement != null)
            {
                throw new InvalidOperationException(
                    string.Format("Connection string Element \"{0}\" is not a Key Value Pair, e.g. \"Key=Value\"",
                        badElement));
            }

            Dictionary<string, string> splitConnectionString = elements.Select(s => s.Split('=')).ToDictionary(s => s[0].Trim().ToUpper(), s => s[1].Trim());

            string connectionUserIdKey = string.Format("{0}_UserId", connectionName);
            string connectionPasswordKey = string.Format("{0}_Password", connectionName);
            string connectionCommandTimeoutKey = string.Format("{0}_CommandTimeout", connectionName);

            if (splitConnectionString.ContainsKey("USER ID"))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Sql Connection Strings must not contain a User Id, even if blank. Instead, add a key with the name {0} to the AppSettings section of the config file.",
                        connectionUserIdKey));
            }

            if (splitConnectionString.ContainsKey("PASSWORD"))
            {
                throw new InvalidOperationException(string.Format("Sql Connection Strings must not contain a Password, even if blank. Instead, add a key with the name {0} to the AppSettings section of the config file.", connectionPasswordKey));
            }

            ConnectionString = string.Join(";", splitConnectionString.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)));

            string connectionCommandTimeout = settings[connectionCommandTimeoutKey];

            if (connectionCommandTimeout != null)
            {
                int timeoutValue = connectionCommandTimeout.CoerceValue<int>();
                BeforePrepareCommand =
                    c =>
                    {
                        c.CommandTimeout = timeoutValue;
                    };
            }
        }
    }
}