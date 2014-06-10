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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using ConfigurationTests.Attributes;

namespace ConfigurationTests.Tests
{
    public class SettingsClassTest : Test
    {
        [MandatoryField]
        [Category("Class Properties")]
        public string ClassName { get; set; }

        [MandatoryField]
        [Category("Class Properties")]
        public string AssemblyPath { get; set; }

        [MandatoryField]
        [Category("Class Properties")]
        public string SettingName { get; set; }

        [MandatoryField]
        [Category("Class Properties")]
        public string ExpectedValue { get; set; }

        public override void Run()
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(AssemblyPath);
            }
            catch (FileNotFoundException)
            {
                throw new AssertionException(string.Format("File Not Found: {0}", AssemblyPath));
            }

            Type classType = assembly.GetTypes().FirstOrDefault(t => t.Name == ClassName);

            if (classType == null)
            {
                throw new AssertionException(string.Format("Class Not Found: {0}", ClassName));
            }

            if (!classType.IsSubclassOf(typeof (ApplicationSettingsBase)))
            {
                throw new AssertionException(string.Format("Class Does Not Implement ApplicationSettingsBase: {0}",
                    ClassName));
            }

            PropertyInfo propertyInfo = classType.GetProperty(SettingName);

            if (propertyInfo == null)
            {
                throw new AssertionException(string.Format("Setting Not Found: {0}", SettingName));
            }

            object instance = Activator.CreateInstance(classType);
            object value = propertyInfo.GetValue(instance, null);

            AssertState.Equal(ExpectedValue, value.ToString());
        }

        public override List<Test> CreateExamples()
        {
            List<Test> examples = new List<Test>();

            examples.Add(new SettingsClassTest
                             {
                                 TestName = "Example Settings Class Test",
                                 AssemblyPath = "D:\\Services\\InstalledService\\bin\\MyAssembly.dll",
                                 ClassName = "Settings",
                                 SettingName = "OtherServiceURI",
                                 ExpectedValue = "http://PRODSERVICESERVER/Services/Service.svc"
                             });

            return examples;
        }
    }
}