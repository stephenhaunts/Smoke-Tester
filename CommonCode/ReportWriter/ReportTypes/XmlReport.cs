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
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace CommonCode.ReportWriter.ReportTypes
{
    public class XmlReport : IReportType
   {
        public void WriteReport(string fileName, List<ReportEntry> reportEntries)
        {
            var root = new XDocument();

            var tests = new XElement("Tests");

            root.Add(tests);            

            foreach (var entry in reportEntries)
            {
                var test = new XElement("Test1");
                var attribute = new XAttribute("TestName", entry.TestName);
                var attribute2 = new XAttribute("Result", entry.Result);
                var attribute3 = new XAttribute("StartTime", entry.TestStartTime);
                var attribute4 = new XAttribute("StopTime", entry.TestStopTime);

                test.Add(attribute);
                test.Add(attribute2);
                test.Add(attribute3);
                test.Add(attribute4);

                tests.Add(test);
            }                        
           
            root.Save(fileName);
        }
    }
}
