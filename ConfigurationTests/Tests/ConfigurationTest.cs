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
        private bool _isWebConfig;
        private string _exePath;

        private readonly CacheList<string, Configuration> _configurationCache = new CacheList<string, Configuration>(ConfigurationManager.OpenExeConfiguration);

        protected Configuration GetConfig()
        {
            if (!File.Exists(FullFilePath))
            {
                throw new AssertionException(string.Format("File Not Found, {0}", FullFilePath));
            }

            _exePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(FullFilePath), System.IO.Path.GetFileNameWithoutExtension(FullFilePath));

            _isWebConfig = System.IO.Path.GetFileName(_exePath).ToLower() == "web" && !File.Exists(_exePath);

            if (!_isWebConfig && !File.Exists(_exePath))
            {
                throw new AssertionException(string.Format("Assembly Not Found, {0}", _exePath));
            }

            if (_isWebConfig)
            {
                File.Create(_exePath).Close();
            }

            Configuration configuration = _configurationCache[_exePath];

            if (_isWebConfig)
            {
                File.Delete(_exePath);
            }

            return configuration;
        }
    }
}
