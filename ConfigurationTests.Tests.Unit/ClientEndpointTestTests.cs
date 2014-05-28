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

namespace ConfigurationTestUnitTests
{
    [TestClass]
    public class ClientEndpointTestTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void CheckKnownEndpoint()
        {
            var test = new ClientEndpointTest { TestName = "ChargeService Endpoint" };
            string location = Assembly.GetExecutingAssembly().Location;

            test.Path = Path.GetDirectoryName(location);
            test.Filename = string.Format("{0}.config", Path.GetFileName(location));
            test.EndpointName="BasicHttpBinding_MyEndpoint";
            test.ExpectedAddress="http://server/service/MyService.svc";
            test.ExpectedBinding="basicHttpBinding";
            test.ExpectedBindingConfiguration="BasicHttpBinding_MyBindingConfig";
            test.ExpectedContract="MyContract.Contract";
            test.CheckConnectivity=false;

            test.Run();
        }
    }
}
