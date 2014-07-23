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
using Common.Collections;
using ConfigurationTests.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ConfigurationTests.Tests
{
    [TestCategory(Enums.TestCategory.Xml)]
    public class XMLDocumentTest : FileTest
    {
        protected static readonly CacheList<string, XDocument> knownDocuments =
            new CacheList<string, XDocument>(s =>
            {
                var doc = XDocument.Load(s);

                return doc;
            });

        [MandatoryField]
        [Category("XML query result")]
        public string ExpectedOutput
        {
            get;
            set;
        }

        [MandatoryField]
        [Category("XPath query")]
        public string XPathQuery
        {
            get;
            set;
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                           {
                               new XMLDocumentTest
                                   {
                                       TestName = "Example config expected value",
                                       Path = @".",
                                       Filename = "InstallationSmokeTest.exe.config",
                                       XPathQuery = "/appSettings/add",
                                       ExpectedOutput = ""
                                   },
                               new XMLDocumentTest
                                   {
                                       TestName = "Example web.config expected value",
                                       Path = @".",
                                       Filename = "web.config",
                                       XPathQuery = "/appSettings/add",
                                       ExpectedOutput = ""
                                   },
                           };

        }

        public override void Run()
        {
            if (!File.Exists(FullFilePath)) 
                throw new AssertionException("File Not Found: "+FullFilePath);
            
            XDocument doc = knownDocuments[FullFilePath];

            var queryResults = (IEnumerable)doc.XPathEvaluate(XPathQuery);

            string actualOutput = string.Empty;

            foreach (XObject xObject in queryResults)
            {
                if (xObject is XElement)
                    actualOutput += ((XElement)xObject).ToString();
                else if (xObject is XAttribute)
                    actualOutput += ((XAttribute)xObject).ToString();
            }

            if (string.IsNullOrEmpty(actualOutput))
                throw new AssertionException(string.Format("XPath query: {0} did not return anything.", XPathQuery));

            if (actualOutput != ExpectedOutput)
                throw new AssertionException(string.Format("Expected output: {0} does not match actual output: {1}.", ExpectedOutput, actualOutput));
        }

    }
}
