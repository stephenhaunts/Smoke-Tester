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
using System.Xml.Linq;

namespace CommonCode.Reports.Writer
{
    public class XmlReportWriter : IReportWriter
   {
        public void WriteReport(string fileName, List<ReportEntry> reportEntries)
        {
            var root = new XDocument();

            var tests = new XElement("Tests");

            root.Add(tests);

            foreach (var reportEntry in reportEntries)
            {
                var test = new XElement("Test");
                var attribute = new XAttribute("TestName", reportEntry.TestName ?? string.Empty);
                var attribute2 = new XAttribute("Result", reportEntry.Result);
                var attribute3 = new XAttribute("StartTime", reportEntry.TestStartTime);
                var attribute4 = new XAttribute("StopTime", reportEntry.TestStopTime);
                test.Add(attribute);
                test.Add(attribute2);
                test.Add(attribute3);
                test.Add(attribute4);
                tests.Add(test);
            }

            root.Save(fileName);
        }
        public string Extension { get { return "xml"; } }
        public string Code { get { return "RunWithXmlReport"; } }
        public override string ToString()
        {
            return "Xml Report";
        }
    }
}
