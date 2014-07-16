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
using System.Reflection;
using System.Globalization;

namespace CommonCode.Reports
{
    public class ReportHelper
    {

        public static string GetReportFileName(string extension)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:ddMMyyyy_hhmm}.{1}", DateTime.Now, (extension ?? string.Empty).Replace(".",string.Empty));
        }

        public static IEnumerable<IReportWriter> GetReportWriters()
        {
            var writerTypes = new List<IReportWriter>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterface("CommonCode.Reports.IReportWriter") == null) continue;

                var reportWriter =(IReportWriter) Activator.CreateInstance(type);
                writerTypes.Add(reportWriter);
            }
            return writerTypes;
        }
    }
}
