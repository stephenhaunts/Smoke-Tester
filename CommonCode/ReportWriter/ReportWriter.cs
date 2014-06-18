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
using CommonCode.ReportWriter.ReportTypes;

namespace CommonCode.ReportWriter
{
    public sealed class ReportWriter
    {
        public void WriteReport(string fileName, ReportType reportType, List<ReportEntry> entries)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (entries.Count == 0)
            {
                throw new InvalidOperationException("entries");
            }

            IReportType reportWriter = null;

            switch (reportType)
            {
                case ReportType.CsvReport:
                    reportWriter = new CsvReport();
                    break;

                case ReportType.TextReport:
                    reportWriter = new TextReport();
                    break;

                case ReportType.XmlReport:
                    reportWriter = new XmlReport();
                    break;
            }

            if (reportWriter == null)
            {
                throw new ArgumentNullException("reportWriter");
            }

            reportWriter.WriteReport(fileName, entries);
        }
    }
}
