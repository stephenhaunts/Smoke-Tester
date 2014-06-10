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
using System.ComponentModel;
using System.Xml;
using ConfigurationTests.Attributes;
using Common.Collections;

namespace ConfigurationTests.Tests
{
    public abstract class XMLElementTestBase : FileTest
    {
        protected int maximumOccurences = int.MaxValue;
        protected int minimumOccurences = 1;

        protected static readonly CacheList<string, XmlDocument> knownDocuments =
            new CacheList<string, XmlDocument>(s =>
                                                   {
                                                       var doc = new XmlDocument();
                                                       doc.Load(s);
                                                       return doc;
                                                   });

        [DefaultValue(int.MaxValue)]
        [Category("XML Element Properties")]
        public int MaximumOccurrences
        {
            get { return maximumOccurences; }
            set { maximumOccurences = value; }
        }

        [DefaultValue(1)]
        [Category("XML Element Properties")]
        public int MinimumOccurrences
        {
            get { return minimumOccurences; }
            set { minimumOccurences = value; }
        }

        [MandatoryField]
        [Category("XML Element Properties")]
        public string XmlPath { get; set; }
    }
}