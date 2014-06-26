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
using System.DirectoryServices;
using Common.Boolean;
using System.ComponentModel;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Networking)]
    public class IISDoesWebsiteExist : Test
    {
        private bool _shouldExist = true;

        [DefaultValue(true)]
        [MandatoryField]
        [Description("True to check if Website exists")]
        [Category("IIS Properties")]
        public bool ShouldExist
        {
            get { return _shouldExist; }
            set { _shouldExist = value; }
        }

        [DefaultValue("localhost")]
        [MandatoryField]
        [Description("Name of server that website lives, eg. localhost")]
        [Category("IIS Properties")]
        public string ServerName { get; set; }

        [MandatoryField]
        [Description("Name of the website to check for.")]
        [Category("IIS Properties")]
        public string WebsiteName { get; set; }

        public override void Run()
        {
            AssertState.Equal(ShouldExist, DoesWebsiteExist(ServerName, WebsiteName), string.Format("The website {0} is {1}present on {2}", WebsiteName, ShouldExist.IfTrue("not "), ServerName));
        }

        public bool DoesWebsiteExist(string serverName, string websiteName)
        {
            var result = false;

            var w3Svc = new DirectoryEntry(string.Format("IIS://{0}/w3svc", serverName));

            foreach (DirectoryEntry site in w3Svc.Children)
            {
                if (site.Properties["ServerComment"] == null) continue;
                if (site.Properties["ServerComment"].Value == null) continue;

                if (String.CompareOrdinal(site.Properties["ServerComment"].Value.ToString(), websiteName) == 0)
                {
                    result = true;
                }
            }

            return result;
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