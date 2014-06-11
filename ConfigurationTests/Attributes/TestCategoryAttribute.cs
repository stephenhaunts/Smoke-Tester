using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ConfigurationTests.Enums;

namespace ConfigurationTests.Attributes
{
    [XmlRoot(Namespace = "ConfigurationTests")]
    [AttributeUsage(AttributeTargets.Class)]
    public class TestCategoryAttribute : Attribute
    {
        public TestCategoryAttribute(TestCategory testCategory)
        {
            TestCatgory = testCategory;
        }

        public TestCategory TestCatgory { get; set; }
    }
}
