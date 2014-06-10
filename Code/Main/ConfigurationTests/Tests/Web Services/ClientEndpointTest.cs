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
using System.Linq;
using System.Net;
using System.ServiceModel.Configuration;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    public class ClientEndpointTest : EndpointTest
    {
        [MandatoryField]
        [Category("Web Service Properties")]
        public override string EndpointName { get; set; }

        public override void Run()
        {
            ClientSection clientSection = (ClientSection)GetConfig().GetSection("system.serviceModel/client");

            ChannelEndpointElementCollection endpoints = clientSection.Endpoints;
            ChannelEndpointElement endpoint = endpoints.OfType<ChannelEndpointElement>().FirstOrDefault(e => e.Name == EndpointName);

            if (endpoint == null)
            {
                throw new AssertionException(string.Format("Could not find Endpoint with name [{0}]", EndpointName));
            }

            AssertState.Equal(ExpectedAddress, endpoint.Address.ToString(), "Address is incorrect");
            AssertState.Equal(ExpectedBehaviourConfiguration, endpoint.BehaviorConfiguration, "BehaviourConfiguration is incorrect");
            AssertState.Equal(ExpectedBinding, endpoint.Binding, "Binding is incorrect");
            AssertState.Equal(ExpectedBindingConfiguration, endpoint.BindingConfiguration, "BindingConfiguration is incorrect");
            AssertState.Equal(ExpectedContract, endpoint.Contract, "Contract is incorrect");
            AssertState.Equal(ExpectedEndpointConfiguration, endpoint.EndpointConfiguration, "EndpointConfiguration is incorrect");

            if (CheckConnectivity.GetValueOrDefault())
            {
                WebRequest request = WebRequest.Create(ExpectedAddress);
                request.Timeout = (ConnectionTimeout ?? 10) * 1000;
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                AssertState.Equal(HttpStatusCode.OK, webResponse.StatusCode, "While attempting to check connectivity");
            }
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                             {
                                  new ClientEndpointTest
                                     {
                                         TestName = "Example Client Endpoint",
                                         Path = @"D:\Folder\Path",
                                         Filename = "Example.exe.config",
                                         EndpointName = "IService_Client",
                                         ExpectedAddress =
                                             "http://server/AppService/AppService.svc",
                                         ExpectedBehaviourConfiguration = "ServiceBehaviourConfig",
                                         ExpectedBinding = "IService_Binding",
                                         ExpectedBindingConfiguration = "ServiceBindingConfig",
                                         ExpectedContract = "IService",
                                         CheckConnectivity = true,
                                         ConnectionTimeout = 30
                                     },
                                  new ClientEndpointTest
                                     {
                                         TestName = "Example Client Endpoint 2",
                                         Path = @"D:\Folder\Path",
                                         Filename = "Example.exe.config",
                                         EndpointName = "IService2_Client",
                                         ExpectedAddress =
                                             "http://server/AppService/OtherService.svc",
                                         ExpectedContract = "IService2"
                                     }
                             };
        }
    }
}