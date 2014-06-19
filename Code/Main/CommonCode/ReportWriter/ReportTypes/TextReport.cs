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
using System.IO;
using System.Text;

namespace CommonCode.ReportWriter.ReportTypes
{
    public class TextReport : IReportType
   {
        public void WriteReport(string fileName, List<ReportEntry> reportEntries)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (reportEntries.Count == 0)
            {
                throw new ArgumentNullException("reportEntries");
            }

            var textReport = new StringBuilder();
            textReport.AppendLine("Report Generated : " + DateTime.Now);
            textReport.AppendLine("Test Execution Machine : " + System.Environment.MachineName);
            textReport.AppendLine("");

            var count = 1;

            foreach (var entry in reportEntries)
            {
                textReport.AppendLine("Test Number : " + count);
                textReport.AppendLine("-----------------");
                textReport.AppendLine("");
                textReport.AppendLine("Test Name : " + entry.TestName);

                var result = "Failed";

                if (entry.Result)
                {
                    result = "Passed";
                }

                textReport.AppendLine("Test Result : " + result);
                textReport.AppendLine("Test Start Time : " + entry.TestStartTime);
                textReport.AppendLine("Test Stop Time : " + entry.TestStopTime);

                if (!entry.Result)
                {
                    textReport.AppendLine("Error Message : " + entry.ErrorMessage);
                }

                textReport.AppendLine("");
                textReport.AppendLine("");

                count++;
            }

            var file = new StreamWriter(fileName);
            file.WriteLine(textReport.ToString());
            file.Close();         
        }
    }
}
