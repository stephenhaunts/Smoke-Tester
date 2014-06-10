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
using ConfigurationTests.Attributes;
using Microsoft.Win32;
using System.ComponentModel;
using System;

namespace ConfigurationTests.Tests
{
    public class RegistryKeyTest : Test
    {

        [DefaultProperty("ValueName")]
        [Category("Registry Properties")]
        public class RegistryEntry
        {
            public string ValueName { get; set; }

            public string ExpectedValue { get; set; }

            public RegistryValueType ValueType { get; set; }

            public override string ToString()
            {
                return ValueName;
            }
        }

        public enum RegistryBaseKey
        {            
            HKEY_CLASSES_ROOT,
            HKEY_LOCAL_MACHINE,
            HKEY_CURRENT_CONFIG,
            HKEY_CURRENT_USER,
            HKEY_PERFORMANCE_DATA,
            HKEY_USERS            
        }
        
        public enum RegistryValueType
        {            
            REG_NONE,
            REG_SZ,
            REG_EXPAND_SZ,
            REG_BINARY,
            REG_DWORD,
            REG_DWORD_LITTLE_ENDIAN,
            REG_DWORD_BIG_ENDIAN,
            REG_LINK,
            REG_MULTI_SZ,
            REG_RESOURCE_LIST,
            REG_FULL_RESOURCE_DESCRIPTOR,
            REG_RESOURCE_REQUIREMENTS_LIST,
            REG_QWORD,
            REG_QWORD_LITTLE_ENDIAN            
        }

        [MandatoryField]
        [Category("Registry Properties")]
        public RegistryBaseKey BaseKey
        {
            get
            {
                return _BaseKey;
            }
            set
            {
                _BaseKey = value;
            }
        }

        [MandatoryField]
        [Category("Registry Properties")]
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }

        [MandatoryField]
        [Category("Registry Properties")]
        public List<RegistryEntry> ExpectedEntries
        {
            get
            {
                return _ExpectedEntries ?? (_ExpectedEntries = new List<RegistryEntry>());
            }
            set
            {
                _ExpectedEntries = value;
            }
        }

        protected object GetValue(string entryName)
        {
            return Key.GetValue(entryName);
        }

        private string _Path;
        private RegistryBaseKey _BaseKey;
        private List<RegistryEntry> _ExpectedEntries;
        private RegistryKey key;

        private readonly Dictionary<RegistryBaseKey, RegistryKey> baseKeys =
            new Dictionary<RegistryBaseKey, RegistryKey>
                    {
                    {RegistryBaseKey.HKEY_CLASSES_ROOT,Registry.ClassesRoot},
                    {RegistryBaseKey.HKEY_LOCAL_MACHINE, Registry.LocalMachine},
                    {RegistryBaseKey.HKEY_CURRENT_CONFIG,Registry.CurrentConfig},
                    {RegistryBaseKey.HKEY_PERFORMANCE_DATA,Registry.PerformanceData},
                    {RegistryBaseKey.HKEY_USERS,Registry.Users},
                    {RegistryBaseKey.HKEY_CURRENT_USER,Registry.CurrentUser}
                    };

        private RegistryKey Key
        {
            get
            {
                if (key != null) return key;

                key = baseKeys[BaseKey];
                var path = Path;

                if (!path.EndsWith("/")) path += "/";

                while (path.Length > 0)
                {
                    var keyName = path.Substring(0, path.IndexOf('/'));
                    key = key.OpenSubKey(keyName);
                    path = path.Substring(path.IndexOf('/') + 1);
                }

                return key;
            }
        }

        public override void Run()
        {
            foreach (var entry in ExpectedEntries)
            {
                object actual = GetValue(entry.ValueName);
                object expected = entry.ExpectedValue;

                switch (entry.ValueType)
                {
                    case  RegistryValueType.REG_BINARY:
                        break;
                    case RegistryValueType.REG_DWORD:
                    case RegistryValueType.REG_DWORD_BIG_ENDIAN:
                    case RegistryValueType.REG_DWORD_LITTLE_ENDIAN:
                        actual = Convert.ToInt32(actual);
                        expected = Convert.ToInt32(entry.ExpectedValue);
                        break;
                    case RegistryValueType.REG_SZ:
                    case RegistryValueType.REG_EXPAND_SZ:
                        actual = Convert.ToString(actual);
                        expected = Convert.ToString(entry.ExpectedValue);
                        break;
                    case RegistryValueType.REG_MULTI_SZ:
                        actual = Convert.ToInt32(actual);
                        expected = Convert.ToInt32(entry.ExpectedValue, 16);
                        break;
                    case RegistryValueType.REG_QWORD:
                    case RegistryValueType.REG_QWORD_LITTLE_ENDIAN:
                        actual = Convert.ToInt32(actual);
                        expected = Convert.ToInt64(entry.ExpectedValue, 16);
                        break;                    
                }

                AssertState.Equal(expected.ToString(), actual.ToString());
            }
        }

        public override List<Test> CreateExamples()
        {
            return new List<Test>
                           {
                               new RegistryKeyTest
                                   {
                                       BaseKey = RegistryBaseKey.HKEY_CURRENT_CONFIG,
                                       Path = "Software/Fonts",
                                       ExpectedEntries =
                                           new List<RegistryEntry>
                                               {new RegistryEntry {ValueName = "LogPixels", ExpectedValue = "60"}}
                                   },
                               new RegistryKeyTest
                                   {
                                       BaseKey = RegistryBaseKey.HKEY_LOCAL_MACHINE,
                                       Path = "Software/Microsoft/Windows/CurrentVersion/BITS",
                                       ExpectedEntries =
                                           new List<RegistryEntry>
                                               {
                                                   new RegistryEntry
                                                       {ValueName = "IGDSercherDLL", ExpectedValue = @"bitsigd.dll", ValueType = RegistryValueType.REG_SZ},
                                                   new RegistryEntry
                                                       {ValueName = "JobInactivityTimeout", ExpectedValue = "0x0076a700", ValueType = RegistryValueType.REG_QWORD}
                                               }
                                   }

                           };
        }
    }
}

