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
using Common.Boolean;
using System.ComponentModel;

namespace ConfigurationTests.Tests
{
    public class IISInstalledTest : Test
    {
        private bool _shouldExist = true;

        [DefaultValue(true)]
        [Description("True to check if MSMQ exists")]
        [Category("IIS Properties")]
        public bool ShouldExist
        {
            get { return _shouldExist; }
            set { _shouldExist = value; }
        }

        public override void Run()
        {
            AssertState.Equal(ShouldExist, DoesIISExist(), string.Format("IIS is {0}present", ShouldExist.IfTrue("not ")));
        }

        private static bool DoesIISExist()
        {
            var services = ServiceController.GetServices().ToList();

            var iis = services.Find(o => o.ServiceName == "W3SVC");
            return iis != null;
        }

        public override List<Test> CreateExamples()
        {
           return new List<Test>
            {
                new IISInstalledTest(){ShouldExist = true, TestName = "Check that the IIS Webserver is intalled."}
            };
        }
    }
}