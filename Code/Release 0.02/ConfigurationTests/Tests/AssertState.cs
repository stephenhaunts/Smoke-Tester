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

namespace ConfigurationTests.Tests
{
    public static class AssertState
    {
        public static void Equal<T>(T expected, T actual, string message = null)
        {
            if ((typeof (T).IsValueType || expected != null) && !expected.Equals(actual))
            {
                throw new AssertionException<T>(expected, actual, message);
            }
        }

        public static void Equal(string expected, string actual, bool caseSensitive, string message = null)
        {
            if (expected != null &&
                !expected.Equals(actual,
                    caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase))
            {
                throw new AssertionException<string>(expected, actual, message);
            }
        }

        public static void NotNull<T>(T item, string message = null) where T : class
        {
            if (item == null)
            {
                throw new AssertionException<string>("not null", "null", message);
            }
        }
    }
}