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

namespace Common.Collections
{
    public sealed class CacheList<TKey1, TKey2, TValue> : CacheList<Tuple<TKey1, TKey2>, TValue>
        where TKey1 : struct
        where TKey2 : struct
    {
    }

    public sealed class CacheList<TKey1, TKey2, TKey3, TValue> : CacheList<Tuple<TKey1, TKey2, TKey3>, TValue>
        where TKey1 : struct
        where TKey2 : struct
        where TKey3 : struct
    {
    }

    public class CacheList<TKey, TValue> : ICacheList<TKey, TValue>
    {
        private readonly bool enableTouch;
        private Func<TKey, TValue> getter;
        private readonly List<CacheRecord> internalList = new List<CacheRecord>();
        private readonly int maxCacheSize = int.MaxValue;
        private readonly TimeSpan timeout = TimeSpan.MaxValue;

        public CacheList() { }

        public CacheList(Func<TKey, TValue> getter)
        {
            this.getter = getter;
        }

        public CacheList(Func<TKey, TValue> getter, TimeSpan timeout)
            : this(getter)
        {
            this.timeout = timeout;
        }

        public CacheList(Func<TKey, TValue> getter, TimeSpan timeout, bool enableTouch)
            : this(getter, timeout)
        {
            this.enableTouch = enableTouch;
        }

        public CacheList(Func<TKey, TValue> getter, TimeSpan timeout, bool enableTouch, int maxCacheSize)
            : this(getter, timeout, enableTouch)
        {
            this.maxCacheSize = maxCacheSize;
        }
        public TValue this[TKey key]
        {
            get
            {
                lock (this)
                {
                    CacheRecord cacheRecord = GetCacheRecord(key);
                    return cacheRecord.Value;
                }
            }
            set
            {
                lock (this)
                {
                    if (!ContainsKey(key))
                    {
                        if (internalList.Count == maxCacheSize)
                        {
                            internalList.Remove(internalList.OrderBy(cr => cr.LastTouched).First());
                        }

                        CacheRecord cacheRecord = new CacheRecord(key, getter, timeout, enableTouch);
                        cacheRecord.Value = value;
                        internalList.Add(cacheRecord);
                    }
                    else
                    {
                        CacheRecord cacheRecord = GetCacheRecord(key);
                        cacheRecord.Value = value;
                    }
                }
            }
        }

        private CacheRecord GetCacheRecord(TKey key)
        {
            CacheRecord cacheRecord;

            if (!ContainsKey(key))
            {
                if (internalList.Count == maxCacheSize)
                {
                    internalList.Remove(internalList.OrderBy(cr => cr.LastTouched).First());
                }

                cacheRecord = new CacheRecord(key, getter, timeout, enableTouch);
                internalList.Add(cacheRecord);
            }
            else
            {
                cacheRecord = internalList.First(cr => cr.Key.Equals(key));
            }
            return cacheRecord;
        }

        protected bool ContainsKey(TKey key)
        {
            return internalList.Any(cr => cr.Key.Equals(key));
        }

        public void Clear()
        {
            internalList.Clear();
        }

        /// <summary>
        /// Updates the Getter method for undiscovered or not-yet-loaded items.
        /// </summary>
        /// <param name="func"></param>
        public void SetGetterMethod(Func<TKey, TValue> func)
        {
            getter = func;
        }

        private sealed class CacheRecord
        {
            private readonly bool enableTouch;
            private readonly Func<TKey, TValue> getterFunction;
            private readonly TKey key;
            private readonly TimeSpan timeout = TimeSpan.MaxValue;
            private bool hasValue;
            private DateTime lastTouched = DateTime.Now;
            private TValue value;

            public CacheRecord(TKey key, Func<TKey, TValue> getterFunction, TimeSpan timeout, bool enableTouch)
            {
                this.key = key;
                this.getterFunction = getterFunction;
                this.timeout = timeout;
                this.enableTouch = enableTouch;
            }

            public DateTime LastTouched
            {
                get { return lastTouched; }
            }

            public TKey Key
            {
                get { return key; }
            }

            public TValue Value
            {
                get
                {
                    if ((!hasValue || DateTime.Now - LastTouched > timeout))
                    {
                        value = getterFunction(Key);
                        hasValue = true;
                    }

                    if (enableTouch) lastTouched = DateTime.Now;
                    {
                        return value;
                    }
                }
                set
                {
                    this.value = value;
                    hasValue = true;

                    if (enableTouch)
                    {
                        lastTouched = DateTime.Now;
                    }
                }
            }
        }
    }
}
