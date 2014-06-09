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
using System.Configuration;
using System.IO;
using Common.Collections;

namespace ConfigurationTests.Tests
{
    public abstract class ConfigurationTest : FileTest
    {
        private bool isWebConfig;
        private string exePath;

        private readonly CacheList<string, Configuration> configurationCache = new CacheList<string, Configuration>(ConfigurationManager.OpenExeConfiguration);

        protected Configuration GetConfig()
        {
            if (!File.Exists(FullFilePath))
            {
                throw new AssertionException(string.Format("File Not Found, {0}", FullFilePath));
            }

            exePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(FullFilePath), System.IO.Path.GetFileNameWithoutExtension(FullFilePath));

            isWebConfig = System.IO.Path.GetFileName(exePath).ToLower() == "web" && !File.Exists(exePath);

            if (!isWebConfig && !File.Exists(exePath))
            {
                throw new AssertionException(string.Format("Assembly Not Found, {0}", exePath));
            }

            if (isWebConfig)
            {
                File.Create(exePath).Close();
            }

            Configuration configuration = configurationCache[exePath];

            if (isWebConfig)
            {
                File.Delete(exePath);
            }

            return configuration;
        }
    }
}
