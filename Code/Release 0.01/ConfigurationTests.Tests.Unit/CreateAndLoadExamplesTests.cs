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
using System.Text;
using Common.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationTests.Tests.Unit
{
    [TestClass]
    public class CreateAndLoadExamplesTests
    {
        [TestMethod]
        public void SerializeAndDeserializeExamplesWithFile()
        {
            var configurationInformation = new ConfigurationTestSuite();
            configurationInformation.CreateExampleData();
            string xmlString = configurationInformation.ToXmlString();
            File.WriteAllText("test.xml", xmlString, Encoding.Unicode);
            string loadedString = File.ReadAllText("test.xml", Encoding.Unicode);
            loadedString.ToObject<ConfigurationTestSuite>();
        }

        [TestMethod]
        public void SerializeAndDeserializeExamplesInMemory()
        {
            var configurationInformation = new ConfigurationTestSuite();
            configurationInformation.CreateExampleData();
            string xmlString = configurationInformation.ToXmlString();
            xmlString.ToObject<ConfigurationTestSuite>();
        }

        [TestMethod]
        public void SerializeAndDeserializeExamplesWithRawXml()
        {
            const string xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
"<ConfigurationTestSuite xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
"  <Name>ChequelessManagerSmokeTests</Name>" +
"  <Description>This is a test for the ChequelessManager config file called Web.Config</Description>" +
"  <Tests>                                                                                          " +
"                                                                                                   " +
"    <Test xsi:type=\"EncryptedConnectionStringTest\">                                              " +
"      <TestName>MoneyMart Connectionstring</TestName>                                              " +
"      <Path>D:\\IIS-Root\\TMS-Internal\\ChequelessManager</Path>                                      " +
"      <Filename>Web.Config</Filename>                                                              " +
"      <ConnectionStringName>MoneyMart</ConnectionStringName>                                       " +
"      <StringSettings>                                                                             " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Provider</SettingName>                                                      " +
"          <ExpectedValue xsi:type=\"xsd:string\">SQLOLEDB.1</ExpectedValue>                        " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>User ID</SettingName>                                                       " +
"          <ExpectedValue xsi:type=\"xsd:string\">hardshipmanager</ExpectedValue>                   " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Password</SettingName>                                                      " +
"          <ExpectedValue xsi:type=\"xsd:string\">Test</ExpectedValue>                              " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Initial Catalog</SettingName>                                               " +
"          <ExpectedValue xsi:type=\"xsd:string\">moneymart</ExpectedValue>                         " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Timeout</SettingName>                                                       " +
"          <ExpectedValue xsi:type=\"xsd:string\">10000</ExpectedValue>                             " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Data Source</SettingName>                                                   " +
"          <ExpectedValue xsi:type=\"xsd:string\">ukuatefcosql02\\sql2008</ExpectedValue>           " +
"        </ConnectionStringSetting>                                                                 " +
"      </StringSettings>                                                                            " +
"      <CheckConnectivity>false</CheckConnectivity>                                                 " +
"    </Test>                                                                                        " +
"    <Test xsi:type=\"EncryptedConnectionStringTest\">                                              " +
"      <TestName>EntityFramework connectionstring</TestName>                                        " +
"      <Path>D:\\IIS-Root\\TMS-Internal\\ChequelessManager</Path>                                   " +
"      <Filename>Web.Config</Filename>                                                              " +
"      <ConnectionStringName>LoanProduct</ConnectionStringName>                                     " +
"      <StringSettings>                                                                             " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>User ID</SettingName>                                                       " +
"          <ExpectedValue xsi:type=\"xsd:string\">hardshipmanager</ExpectedValue>                   " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Password</SettingName>                                                      " +
"          <ExpectedValue xsi:type=\"xsd:string\">Test</ExpectedValue>                              " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Initial Catalog</SettingName>                                               " +
"          <ExpectedValue xsi:type=\"xsd:string\">LoanProduct</ExpectedValue>                       " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Connection Timeout</SettingName>                                            " +
"          <ExpectedValue xsi:type=\"xsd:string\">10000</ExpectedValue>                             " +
"        </ConnectionStringSetting>                                                                 " +
"        <ConnectionStringSetting>                                                                  " +
"          <SettingName>Data Source</SettingName>                                                   " +
"          <ExpectedValue xsi:type=\"xsd:string\">UKUATEFCOSQL02\\SQL2008</ExpectedValue>           " +
"        </ConnectionStringSetting>                                                                 " +
"      </StringSettings>                                                                            " +
"      <CheckConnectivity>true</CheckConnectivity>                                                  " +
"    </Test>                                                                                        " +
"    <Test xsi:type=\"EncryptedAppSettingTest\">                                                    " +
"      <TestName>CCLApps ConnectionString AppSetting Test</TestName>                                " +
"      <Path>D:\\IIS-Root\\TMS-Internal\\ChequelessManager</Path>                                   " +
"      <Filename>Web.Config</Filename>                                                              " +
"      <Key>ConnectionString</Key>                                                                  " +
"      <ExpectedValue>Password=Test;Persist Security Info=True;User ID=cclapps;Initial Catalog=Amberlive; Data Source=UKUATEFCOSQL02;</ExpectedValue>" +
"      <CaseSensitive>false</CaseSensitive>                                                         " +
"    </Test>                                                                                        " +
"    <Test xsi:type=\"RegistryKeyTest\">                                                            " +
"      <BaseKey>HKEY_LOCAL_MACHINE</BaseKey>                                                        " +
"      <Path>\\SOFTWARE\\Classes\\TMS_HyperString.TTMS_HyperString</Path>                           " +
"      <ExpectedEntries>                                                                            " +
"        <RegistryEntry>                                                                            " +
"          <ValueName></ValueName>                                                                  " +
"          <ExpectedValue xsi:type=\"xsd:string\">TTMS_HyperString Object</ExpectedValue>           " +
"        </RegistryEntry>                                                                           " +
"      </ExpectedEntries>                                                                           " +
"    </Test>                                                                                        " +
"    <Test xsi:type=\"ClientEndpointTest\">                                                         " +
"      <TestName>BounceProxyService Client Endpoint</TestName>                                      " +
"      <Path>D:\\IIS-Root\\TMS-Internal\\ChequelessManager</Path>                                   " +
"      <Filename>Web.config</Filename>                                                              " +
"      <EndpointName>BasicHttpBinding_IBounceProcService</EndpointName>                             " +
"      <ExpectedAddress>http://UKUATEFCOAPP02/BounceProxyService/BounceProcService.svc</ExpectedAddress>" +
"      <ExpectedBinding>basicHttpBinding</ExpectedBinding>                                          " +
"      <ExpectedBindingConfiguration>BasicHttpBinding_IBounceProcService</ExpectedBindingConfiguration>" +
"      <ExpectedContract>BounceProxyServiceReference.IBounceProcService</ExpectedContract>          " +
"      <CheckConnectivity>true</CheckConnectivity>                                                  " +
"      <ConnectionTimeout>30</ConnectionTimeout>                                                    " +
"    </Test>                                                                                        " +
"    <Test xsi:type=\"ClientEndpointTest\">                                                         " +
"      <TestName>AmbercardWebService Client Endpoint</TestName>                                     " +
"      <Path>D:\\IIS-Root\\TMS-Internal\\ChequelessManager</Path>                                   " +
"      <Filename>Web.config</Filename>                                                              " +
"      <EndpointName>WSDAmberCardSoap</EndpointName>                                                " +
"      <ExpectedAddress>http://UKUATEFCOAPP02/BounceProxyService/BounceProcService.svc</ExpectedAddress>" +
"      <ExpectedBinding>basicHttpBinding</ExpectedBinding>                                          " +
"      <ExpectedBindingConfiguration>WSDAmberCard</ExpectedBindingConfiguration>                    " +
"      <ExpectedContract>AmberCardWS.WSDAmberCardSoap</ExpectedContract>                            " +
"      <CheckConnectivity>true</CheckConnectivity>                                                  " +
"      <ConnectionTimeout>30</ConnectionTimeout>                                                    " +
"    </Test>                                                                                        " +
"  </Tests>                                                                                         " +
"</ConfigurationTestSuite>                                                                          ";
            xmlString.ToObject<ConfigurationTestSuite>();
        }
    }
}
