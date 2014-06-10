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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.Collections
{
    public static class CollectionsExtensionMethods
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
        }

        public static bool ContainsSameItemsAs<T>(this IEnumerable<T> target, IEnumerable<T> candidate)
        {
            if (target == candidate) return true;
            if (target == null) return false;
            if (candidate == null) return false;

            T[] targetArray = target.ToArray(); 
            T[] candidateArray = candidate.ToArray(); 
            if (targetArray.Length != candidateArray.Length) return false; 

            return !candidateArray.Any(item => !targetArray.Contains(item)) &&
                   !targetArray.Any(item => !candidateArray.Contains(item));
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null && action != null)
            {
                lock (enumerable)
                {
                    foreach (T t in enumerable)
                    {
                        action(t);
                    }
                }
            }
        }

        private struct ListSearchConfiguration
        {
            public int ListSize;
            public int LowerBound;
            public int UpperBound;
            public int SectionSize;
            public int SectionCount;
            public int InitialCutSize;
        }

        static readonly Dictionary<IList, ListSearchConfiguration> listSearchConfigCache = new Dictionary<IList, ListSearchConfiguration>();
        
        public static int[] FindIndexBinary<T>(this IList list, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            ListSearchConfiguration configuration = default(ListSearchConfiguration);

            if (listSearchConfigCache.ContainsKey(list))
            {
                configuration = listSearchConfigCache[list];
            }

            if (configuration.ListSize == 0 || list.Count > configuration.SectionCount*configuration.SectionSize ||
                Math.Abs(list.Count - configuration.ListSize) > configuration.SectionSize)
            {
                if (listSearchConfigCache.ContainsKey(list))
                {
                    listSearchConfigCache[list] = UpdateListSearchConfiguration(list);
                }
                else
                {
                    listSearchConfigCache.Add(list, configuration = UpdateListSearchConfiguration(list));
                    
                }
            }

            if (list.Count == 0 || comparer.Compare(item, (T) list[configuration.LowerBound]) == -1)
            {
                return new[] {-1, 0};
            }

            if (comparer.Compare(item, (T) list[configuration.UpperBound]) == 1)
            {
                return new[] {configuration.UpperBound, configuration.UpperBound + 1};
            }

            return FindIndexBinaryCore(list, item, comparer, configuration);
        }

        private static int[] FindIndexBinaryCore<T>(IList list, T item, IComparer<T> comparer, ListSearchConfiguration configuration)
        {
            int startPoint = GetBestStartPoint(list, item, comparer, configuration);
            int[] result = { 0, startPoint };
            int cutSize = configuration.InitialCutSize;
            int nextCompareResult = comparer.Compare(item, (T)list[result[1]]);

            while (true)
            {
                result[0] = result[1];

                if (nextCompareResult == 0)
                {
                    return result;
                }

                if (cutSize > 1)
                {
                    cutSize /= 2;
                }

                result[1] += cutSize * nextCompareResult;

                int compareResult = nextCompareResult;
                nextCompareResult = result[1] < configuration.LowerBound ? 1 : comparer.Compare(item, (T)list[result[1]]);

                if (compareResult == 1 && nextCompareResult == -1 && result[1] - result[0] == 1)
                {
                    return result;
                }
            }
        }

        private static int GetBestStartPoint<T>(IList list, T item, IComparer<T> comparer, ListSearchConfiguration configuration)
        {
            int startPoint = configuration.UpperBound - configuration.InitialCutSize;

            for (int i = configuration.LowerBound; i <= configuration.UpperBound; i += configuration.SectionSize)
            {
                if (comparer.Compare(item, (T) list[i]) == -1)
                {
                    startPoint = i - configuration.InitialCutSize;
                    break;
                }
            }

            return startPoint;
        }

        private const int InitialSectionSize = 32768; // Should be a power of 2 for clean division.
        private const int MaximumSectionCount = 128; // This is the maximum granularity for GetBestStartPoint

        private static ListSearchConfiguration UpdateListSearchConfiguration(IList list)
        {
            ListSearchConfiguration configuration = default(ListSearchConfiguration);

            if (list is Array)
            {
                configuration.LowerBound = ((Array)list).GetLowerBound(0);
                configuration.UpperBound = ((Array)list).GetUpperBound(0);
            }
            else
            {
                configuration.LowerBound = 0;
                configuration.UpperBound = list.Count - 1;
            }

            configuration.ListSize = list.Count;
            configuration.SectionSize = InitialSectionSize;

            while (configuration.SectionSize > 0 && (list.Count / configuration.SectionSize) > MaximumSectionCount)
            {
                configuration.SectionSize *= 2;
            }

            while (configuration.SectionSize > 0 && (list.Count / configuration.SectionSize) < 1)
            {
                configuration.SectionSize /= 2;
            }

            configuration.InitialCutSize = (configuration.SectionSize / 2);
            configuration.SectionCount = configuration.SectionSize > 0 ? (list.Count / configuration.SectionSize) : 0;

            return configuration;
        }
    }
}
