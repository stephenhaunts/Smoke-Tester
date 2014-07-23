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
using System.Messaging;
using Common.Boolean;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Message_Queues)]
    public class MsmqLocalQueueInstalled : Test
    {
        private bool _shouldExist = true;
        private string _queueName = @".\private$\TestQueue";

        [DefaultValue(true)]
        [Description("True to check if MSMQ exists")]
        [Category("MSMQ Properties Properties")]
        public bool ShouldExist
        {
            get{return _shouldExist;}
            set{_shouldExist = value;}
        }

        [MandatoryField]
        [Description(@"The name of the local queue to check for, eg .\private$\TestQueue")]
        [Category("MSMQ Properties Properties")]
        public string QueueName
        {
            get { return _queueName; }
            set { _queueName = value; }
        }

        public override void Run()
        {
            AssertState.Equal(ShouldExist, IsQueueAvailable(_queueName), string.Format("MSMQ local queue {0} is {1}present", _queueName, ShouldExist.IfTrue("not ")));
        }

        private static bool IsQueueAvailable(string queueName)
        {
            return MessageQueue.Exists(queueName);
        }

        public override List<Test> CreateExamples()
        {
                return new List<Test>
                             {
                                 new MsmqLocalQueueInstalled
                                     {
                                         TestName = "Example Queue",
                                         QueueName = @".\private$\TestQueue",
                                         ShouldExist = true
                                     }                              
                             };
        }
    }
}
