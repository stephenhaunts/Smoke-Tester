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
using System.IO;
using System.Linq;
using System.Xml;
using ConfigurationTests.Attributes;
using System.ComponentModel;
using Common.Text;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Xml)]
    public class XMLElementTest : XMLElementTestBase
    {
        private List<Attribute> expectedValues = new List<Attribute>();

        [MandatoryField]
        [Category("XML Element Properties")]
        public List<Attribute> ExpectedValues
        {
            get { return expectedValues; }
            set { expectedValues = value; }
        }

        [DefaultProperty("Name")]
        public class Attribute
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }    

        public override void Run()
        {
            if (!File.Exists(FullFilePath)) throw new AssertionException("File Not Found");
            var doc = knownDocuments[FullFilePath];

            var nodeInstances = GetNodesWithName(XmlPath, doc);

            if (nodeInstances.Count > MaximumOccurrences)
            {
                throw new AssertionException(
                    string.Format("{1} instance{2} of {0} found where no more than {3} {4} expected", XmlPath,
                        nodeInstances.Count, nodeInstances.Count.pl("", "s"), MaximumOccurrences,
                        MaximumOccurrences.pl("was", "were")));
            }

            if (nodeInstances.Count < MinimumOccurrences)
            {
                throw new AssertionException(
                    string.Format("{1} instance{2} of {0} found where at least {3} {4} expected", XmlPath,
                        nodeInstances.Count, nodeInstances.Count.pl("", "s"), MinimumOccurrences,
                        MinimumOccurrences.pl("was", "were")));
            }

            var matchingAttributeCounts = nodeInstances.Select(GetMatchingAttributesCount).ToList();

            if (ExpectedValues.Count > 0 && matchingAttributeCounts.Count(c => c == ExpectedValues.Count) == 0)
            {
                throw new AssertionException(
                    string.Format("No instances of {0} found with all expected attributes matching (most was {1})",
                        XmlPath,
                        matchingAttributeCounts.Max()));
            }
        }

        private static List<XmlNode> GetNodesWithName(string xmlPath, XmlNode doc, string path = "")
        {
            var result = new List<XmlNode>();

            foreach (XmlNode n in doc.ChildNodes)
            {
                var nodePath = string.Format("{0}/{1}", path, n.Name);

                if (nodePath.EndsWith(xmlPath))
                {
                    result.Add(n);
                }

                if (n.HasChildNodes)
                {
                    result.AddRange(GetNodesWithName(xmlPath, n, nodePath));
                }
            }

            return result;
        }

        private int GetMatchingAttributesCount(XmlNode node)
        {
            int count = 0;

            if (node.Attributes != null)
            {
                foreach (var expectedValue in ExpectedValues)
                {
                    var attribute = node.Attributes.GetNamedItem(expectedValue.Name);

                    if (attribute != null && attribute.Value == expectedValue.Value)
                        count++;
                }
            }

            return count;
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                           {
                               new XMLElementTest
                                   {
                                       TestName = "Example config expected value",
                                       Path = @".",
                                       Filename = "InstallationSmokeTest.exe.config",
                                       XmlPath = "/appSettings/add",
                                       ExpectedValues =
                                           new List<Attribute>
                                               {
                                                   new Attribute {Name = "key", Value = "Setting"},
                                                   new Attribute
                                                       {
                                                           Name = "value",
                                                           Value = "SettingValue"
                                                       }
                                               }
                                   },
                               new XMLElementTest
                                   {
                                       TestName = "Example web.config expected value",
                                       Path = @".",
                                       Filename = "web.config",
                                       XmlPath = "/appSettings/add",
                                       ExpectedValues =
                                           new List<Attribute>
                                               {
                                                   new Attribute {Name = "key", Value = "Setting"},
                                                   new Attribute
                                                       {
                                                           Name = "value",
                                                           Value = "SettingValue"
                                                       }
                                               },
                                               MinimumOccurrences = 3
                                   },
                           };

        }
        
    }
}
