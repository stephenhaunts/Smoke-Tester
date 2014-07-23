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

namespace CommonCode.Reports.Writer
{
   public class CsvReportWriter : IReportWriter
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

            var csvReport = new StringBuilder();
            csvReport.AppendLine("TestName, Result, Start Time, Finish Time, Error Message");

            foreach (var entry in reportEntries)
            {
                csvReport.AppendLine(string.Format("{0},{1},{2},{3},{4}", entry.TestName, entry.Result, entry.TestStartTime, entry.TestStopTime, entry.ErrorMessage));
            }

            using (var file = new StreamWriter(fileName))
            {
                file.WriteLine(csvReport.ToString());
                file.Close();
            }
        }
        public string Extension { get { return "csv"; } }
        public string Code { get { return "RunWithCsvReport"; } }
        public override string ToString()
        {
            return "Csv Report";
        }
    }
}
