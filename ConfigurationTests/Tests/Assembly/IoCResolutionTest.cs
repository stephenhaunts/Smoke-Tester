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
using InversionOfControl;
using Common.Reflection;

namespace ConfigurationTests.Tests
{
    public class IoCResolutionTest : Test
    {
        public string Interface;
        public string ExpectedType;

        private IEnumerable<Type> allTypes;

        public override void Run()
        {
            allTypes = allTypes ?? Assembly.GetCallingAssembly().GetAllTypes();
            IEnumerable<Type> matchingInterfaces = allTypes.Where(t => t.Name == Interface).ToArray();

            if (matchingInterfaces.Count() > 1)
            {
                throw new AssertionException(string.Format("Interface {0} defined multiple times", Interface));
            }

            if (!matchingInterfaces.Any())
            {
                throw new AssertionException(string.Format("No interface found with the name {0}", Interface));
            }

            IEnumerable<Type> matchingTypes = allTypes.Where(t => t.Name == ExpectedType).ToArray();

            if (matchingTypes.Count() > 1)
            {
                throw new AssertionException(string.Format("Type {0} defined multiple times", ExpectedType));
            }

            if (!matchingTypes.Any())
            {
                throw new AssertionException(string.Format("No Type found with the name {0}", ExpectedType));
            }

            Type interfaceType = matchingInterfaces.First();
            AssertState.Equal(matchingTypes.First(), IoC.ResolveType(interfaceType));
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                       {
                           new IoCResolutionTest
                               {
                                   TestName = "IoC Resolution Example",
                                   Interface = "IRobot",
                                   ExpectedType = "Gort"
                               }
                       };
        }
    }
}
