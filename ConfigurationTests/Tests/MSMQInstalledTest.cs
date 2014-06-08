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
using System.ServiceProcess;
using Common.Boolean;

namespace ConfigurationTests.Tests
{
    public class MsmqInstalledTest : Test
    {
        private bool _shouldExist = true;

        [DefaultValue(true)]
        [Description("True to check if MSMQ exists")]
        public bool ShouldExist
        {
            get{return _shouldExist;}
            set{_shouldExist = value;}
        }

        public override void Run()
        {
            AssertState.Equal(ShouldExist, DoesMsmqExist(), string.Format("MSMQ is {0}present", ShouldExist.IfTrue("not ")));
        }

        private static bool DoesMsmqExist()
        {
            var services = ServiceController.GetServices().ToList();

            var msQue = services.Find(o => o.ServiceName == "MSMQ");
            if (msQue == null) return false;

            return msQue.Status == ServiceControllerStatus.Running;
        }

        public override List<Test> CreateExamples()
        {
                return new List<Test>
                             {
                                 new MsmqInstalledTest
                                     {
                                         TestName = "Example executable",                                        
                                         ShouldExist = true
                                     },
                                 new MsmqInstalledTest
                                     {
                                         TestName = "Example config",                                         
                                         ShouldExist = true
                                     }
                             };
        }
    }
}
