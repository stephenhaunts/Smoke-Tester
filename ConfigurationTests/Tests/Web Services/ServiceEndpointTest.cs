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
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.ServiceModel.Configuration;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{

    /// <summary>
    /// Class used to test the endpoint of a WCF service as defined in the service's configuration file.
    /// </summary>
    public class ServiceEndpointTest : EndpointTest
    {
        [MandatoryField]
        [Category("Web Service Properties")]
        public string ServiceName { get; set; }
        private readonly Func<ServiceEndpointElement, string, bool> addressMatchPredicate = (a, e) => a.Address.ToString() == e;
        private readonly Func<ServiceEndpointElement, string, bool> nameMatchPredicate = (a, e) => a.Name == e;

        public override void Run()
        {
            ServicesSection servicesSection = (ServicesSection)GetConfig().GetSection("system.serviceModel/services");

            ServiceElementCollection services = servicesSection.Services;

            if (services.Count == 0)
            {
                throw new AssertionException("No services section could be found.");
            }

            bool serviceWasFound = false;

            foreach (ServiceElement se in services)
            {
                if (se.Name == ServiceName)
                {
                    serviceWasFound = true;

                    Func<ServiceEndpointElement, bool> predicate;

                    if (string.IsNullOrWhiteSpace(EndpointName))
                    {
                        predicate = a => addressMatchPredicate(a, ExpectedAddress);
                    }
                    else
                    {
                        predicate = a => nameMatchPredicate(a, EndpointName);
                    }

                    ServiceEndpointElement endpoint = se.Endpoints.OfType<ServiceEndpointElement>().FirstOrDefault(predicate);

                    if (endpoint == null)
                    {
                        throw new AssertionException(string.Format("Could not find an Endpoint for Service [{0}] with the specified properties", ServiceName));
                    }

                    AssertState.Equal(ExpectedAddress, endpoint.Address.ToString(), "Address is incorrect");
                    AssertState.Equal(ExpectedBehaviourConfiguration, endpoint.BehaviorConfiguration, "BehaviourConfiguration is incorrect");
                    AssertState.Equal(ExpectedBinding, endpoint.Binding, "Binding is incorrect");
                    AssertState.Equal(ExpectedBindingConfiguration, endpoint.BindingConfiguration, "BindingConfiguration is incorrect");
                    AssertState.Equal(ExpectedContract, endpoint.Contract, "Contract is incorrect");
                    AssertState.Equal(ExpectedEndpointConfiguration, endpoint.EndpointConfiguration, "EndpointConfiguration is incorrect");
                }
            }

            if (!serviceWasFound)
            {
                throw new AssertionException(string.Format("No Service found with Name [{0}]", ServiceName));
            }

            if (CheckConnectivity.GetValueOrDefault())
            {
                WebRequest request = WebRequest.Create(ExpectedAddress);
                request.Timeout = (ConnectionTimeout ?? 10) * 1000;
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                AssertState.Equal(HttpStatusCode.OK, webResponse.StatusCode, "While attempting to check connectivity");

                request.Abort();
            }
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                             {
                                  new ServiceEndpointTest
                                     {
                                         TestName = "Example Service Endpoint",
                                         Path = @"D:\Folder\Path",
                                         Filename = "Example.exe.config",
                                         ServiceName = "IService",
                                         EndpointName = "IServiceEndpoint",
                                         ExpectedAddress =
                                             "net.msmq://APPSVR/private/queueName.svc",
                                         ExpectedBinding = "netMsmqBinding",
                                         ExpectedBindingConfiguration = "netMsmq",
                                         CheckConnectivity = true,
                                         ConnectionTimeout = 30
                                     },
                                  new ServiceEndpointTest
                                     {
                                         TestName = "Example HTTP Endpoint 2",
                                         Path = @"D:\Folder\Path",
                                         Filename = "Example.exe.config",
                                         ServiceName = "IService2_Client",
                                         ExpectedAddress =
                                             "http://APPSVR01/AppService/OtherService.svc",
                                         ExpectedContract = "IService2",
                                         CheckConnectivity = false,
                                     }
                             };
        }
    }
}
