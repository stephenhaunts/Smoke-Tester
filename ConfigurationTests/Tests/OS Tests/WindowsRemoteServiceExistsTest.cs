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
using System.ServiceProcess;
using System.ComponentModel;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    public class WindowsRemoteServiceExistsTest : Test
    {
        [Description("Name of service")]
        [Category("Windows Servicess Properties")]
        [MandatoryField]
        public string ServiceName { get; set; }

        [Description("Machine name of remote windows service.")]
        [Category("Windows Servicess Properties")]

        [MandatoryField]
        public string MachineName { get; set; }

        public override void Run()
        {
            var ctl = ServiceController.GetServices(MachineName).FirstOrDefault(s => s.ServiceName == ServiceName);            
            if (ctl == null) throw new AssertionException(string.Format("Service with name [{0}] was not found on {1}", ServiceName, MachineName));
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test> { new WindowsServiceExistsTest { ServiceName = "Bob" } };
        }
    }
}