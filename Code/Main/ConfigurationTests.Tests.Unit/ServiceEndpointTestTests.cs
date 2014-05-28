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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace ConfigurationTests.Tests.Unit
{    
    [TestClass]    
    public class ServiceEndpointTestTests
    {

        [DeploymentItem("ServiceEndpointTest.exe")]
        [DeploymentItem("ServiceEndpointTest.exe.config")]
        [TestMethod]
        [TestCategory("Integration")]
        public void CheckKnownServiceEndpoint()
        {
            var test = new ServiceEndpointTest { TestName = "Service Endpoint" };
            string location = Assembly.GetExecutingAssembly().Location;
            test.Path = Path.GetDirectoryName(location);
            test.Filename = "ServiceEndpointTest.exe.config";
            test.ServiceName = "Service.Service";
            test.EndpointName = "ServiceEndPoint";
            test.ExpectedAddress = "net.msmq://server/private/service.svc";
            test.ExpectedBinding = "netMsmqBinding";
            test.ExpectedBindingConfiguration = "netMsmq";
            test.ExpectedContract = "Service.IService";
            test.CheckConnectivity = false;
            test.Run();
        }
    }
}
