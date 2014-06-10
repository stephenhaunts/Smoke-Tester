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
using System.Linq;

namespace Common.Text
{
    public struct MatchType
    {
        private readonly MatchTypeEnum value;

        [Flags]
        private enum MatchTypeEnum
        {
            Numeric = 1,
            Alphabetic = 2,
            Address = 4,
            Space = 8,
            Protocol = 16
        }
        
        public static MatchType Numeric = new MatchType(MatchTypeEnum.Numeric);

        public static MatchType Alphabetic = new MatchType(MatchTypeEnum.Alphabetic);

        public static MatchType Alphanumeric = new MatchType(MatchTypeEnum.Alphabetic | MatchTypeEnum.Numeric);

        public static MatchType Words = new MatchType(MatchTypeEnum.Alphabetic | MatchTypeEnum.Space);

        public static MatchType Domain = new MatchType(MatchTypeEnum.Address);

        public static MatchType Email = new MatchType(MatchTypeEnum.Alphabetic | MatchTypeEnum.Numeric | MatchTypeEnum.Address);

        public static MatchType Space = new MatchType(MatchTypeEnum.Space);

        public static MatchType Protocol = new MatchType(MatchTypeEnum.Protocol);

        public static MatchType Url = new MatchType(MatchTypeEnum.Numeric | MatchTypeEnum.Alphabetic | MatchTypeEnum.Address | MatchTypeEnum.Protocol);

        public static readonly Dictionary<int, string> MatchCharacters =
            new Dictionary<int, string>
                {
                    {1, "0123456789"},
                    {2, "abcdefghijklmnopqrstuvwxyz"},
                    {4, ".-_@"},
                    {8, " \t\r\n"},
                    {16, ":/"}
                };

        public static int MaxValue = Enum.GetValues(typeof(MatchTypeEnum)).Cast<int>().Max();

        private MatchType(MatchTypeEnum value)
            : this()
        {
            this.value = value;
        }

        public static implicit operator int(MatchType value)
        {
            return (int)value.value;
        }

        public static implicit operator MatchType(int value)
        {
            return new MatchType((MatchTypeEnum)value);
        }
    }
}