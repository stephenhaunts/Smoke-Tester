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
using System.Linq;
using System.ServiceModel.Configuration;

namespace ConfigurationTests.Tests
{
    /// <summary>
    /// Class used to test the base address of a WCF service as defined in the service's configuration file.
    /// </summary>
    public class ServiceBaseAddressTest : BaseAddressTest
    {
        public override void Run()
        {
            ServicesSection servicesSection = (ServicesSection)GetConfig().GetSection("system.serviceModel/services");

            ServiceElementCollection services = servicesSection.Services;

            if (services.Count == 0)
            {
                throw new AssertionException("No services section could be found.");
            }

            BaseAddressElement baseAddress = null;

            foreach (ServiceElement se in services.OfType<ServiceElement>().Where(se=>se.Name==ServiceName))
            {
                IEnumerable<BaseAddressElement> baseAddressElements = se.Host.BaseAddresses.OfType<BaseAddressElement>();
                baseAddress = baseAddressElements.FirstOrDefault(e => e.BaseAddress == ExpectedBaseAddressValue);
                AssertState.NotNull(baseAddress, "Base address is incorrect or not present");
            }

            if (baseAddress == null)
            {
                throw new AssertionException(string.Format("Could not find Base address with name {0}", ServiceName));
            }
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                             {
                                  new ServiceBaseAddressTest
                                     {
                                         TestName = "Example Service Base address",
                                         Path = @"D:\Folder\Path",
                                         Filename = "Example.exe.config",
                                         ServiceName = "ServiceHost.MyService",
                                         ExpectedBaseAddressValue = "http://127.0.0.1:1234/"
                                     }
                             };
        }
    }
}


