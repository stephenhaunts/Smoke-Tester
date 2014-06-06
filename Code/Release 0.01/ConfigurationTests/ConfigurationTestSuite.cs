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
using System.Reflection;
using ConfigurationTests.Tests;

namespace ConfigurationTests
{
    public class ConfigurationTestSuite
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private List<Test> tests;
        public List<Test> Tests
        {
            get
            {
                return tests ?? (tests = new List<Test>());
            }
            set
            {
                tests = value;
            }
        }

        public void CreateExampleData()
        {
            Name = "Example";
            Description = "This is an Example Configuration Test Suite to illustrate the usage of the various Test types.";

            Tests = new List<Test>();

            var testsTypes = typeof(Test).Assembly.GetTypes()
                            .Where(type => type.IsSubclassOf(typeof(Test)) && !type.IsAbstract);

            foreach (var type in testsTypes)
            {
                object instance = Activator.CreateInstance(type);
                var method = type.GetMethod("CreateExamples");
                var examples = (List<Test>)((MethodInfo)method).Invoke(instance, null);
                Tests.AddRange(examples);
            }
        }
    }
}
