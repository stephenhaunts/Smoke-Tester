using ConfigurationTests.Tests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ConfigurationTests
{
    public static class Exensions
    {
        public static Test Copy(this Test test)
        {
            if (test == null)
                throw new InvalidOperationException("Cannot call Copy on a null object");
            var newTest = Activator.CreateInstance(test.GetType());
            foreach (var property in newTest.GetType().GetProperties())
            {
                object value = property.GetValue(test, null);
                property.SetValue(newTest, value, null);
            }
            return (Test) newTest;
        }
    }
}
