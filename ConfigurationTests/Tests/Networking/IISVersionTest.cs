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
using System.Linq;
using System.ServiceProcess;
using Common.Boolean;
using System.ComponentModel;
using ConfigurationTests.Attributes;
using Microsoft.Win32;

namespace ConfigurationTests.Tests
{
    public class IISVersionTest : Test
    {        
        [DefaultValue(true)]
        [Description("Version if IIS Installed")]
        [MandatoryField]
        public string Version { get; set; }

        public override void Run()
        {            
            AssertState.Equal(Version, GetIisVersion().ToString());
        }

        public Version GetIisVersion()
        {
            using (RegistryKey componentsKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp", false))
            {
                if (componentsKey != null)
                {
                    var majorVersion = (int)componentsKey.GetValue("MajorVersion", -1);
                    var minorVersion = (int)componentsKey.GetValue("MinorVersion", -1);

                    if (majorVersion != -1 && minorVersion != -1)
                    {
                        return new Version(majorVersion, minorVersion);
                    }
                }

                return new Version(0, 0);
            }
        }

        public override List<Test> CreateExamples()
        {
           return new List<Test>
            {
                new IISVersionTest(){TestName = "Check the IIS version number."}
            };
        }
    }
}