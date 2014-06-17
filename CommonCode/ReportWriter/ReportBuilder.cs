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

namespace CommonCode.ReportWriter
{
    public sealed class ReportBuilder
    {
        private readonly List<ReportEntry> _reportEntries;
        private readonly ReportWriter _reportWriter;

        public ReportBuilder()
        {
            _reportEntries = new List<ReportEntry>();
            _reportWriter = new ReportWriter();
        }

        public void AddEntry(ReportEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            _reportEntries.Add(entry);
        }

        public void ClearEntries()
        {
            _reportEntries.Clear();
        }

        public void WriteReport(string fileName, ReportType reportType)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            _reportWriter.WriterReport(fileName, reportType, _reportEntries);
        }
    }
}
