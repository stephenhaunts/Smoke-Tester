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

namespace Common
{
    public class GenericComparer<T> : EqualityComparer<T>, IComparer<T>
    {
        private Func<T, T, bool> equals;
        private Func<T, int> getHashCode = t => t.GetHashCode();
        private Func<T, T, int> compare;

        public GenericComparer()
        {
            compare = compare ?? ((t1, t2) =>
                          {
                              int hash1, hash2;
                              return t1 is IComparable
                                                   ? ((IComparable)t1).CompareTo(t2)
                                                   : ((hash1 = getHashCode(t1)) == (hash2 = getHashCode(t2))
                                                              ? 0
                                                              : hash1 > hash2 ? 1 : -1);
                          });

            equals = equals ?? ((t1, t2) => t1 is IEquatable<T> ? t1.Equals(t2) : getHashCode(t1) == getHashCode(t2));
        }

        public Func<T, T, int> CompareMethod
        {
            get { return compare; }
            set { compare = value; }
        }

        public Func<T, T, bool> EqualsMethod
        {
            get { return equals; }
            set { equals = value; }
        }

        public Func<T, int> GetHashCodeMethod
        {
            get { return getHashCode; }
            set { getHashCode = value; }
        }

        public GenericComparer(Func<T, T, int> compare)
            : this()
        {
            this.compare = compare;
        }

        public GenericComparer(Func<T, int> getHashCode)
            : this()
        {
            this.getHashCode = getHashCode;
        }

        public GenericComparer(Func<T, T, bool> equals)
            : this()
        {
            this.equals = equals;
        }

        public GenericComparer(Func<T, int> getHashCode, Func<T, T, bool> equals): this(getHashCode)
        {
            this.equals = equals;
        }

        public GenericComparer(Func<T, T, bool> equals, Func<T, T, int> compare) : this(equals)
        {
            this.compare = compare;
        }

        public GenericComparer(Func<T, T, int> compare, Func<T, int> getHashCode) : this(compare)
        {
            this.getHashCode = getHashCode;
        }
        
        public GenericComparer(Func<T, int> getHashCode, Func<T, T, bool> equals, Func<T, T, int> compare)
            : this(getHashCode, equals)
        {
            this.compare = compare;
        }

        public override bool Equals(T x, T y)
        {
            return equals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return getHashCode(obj);
        }

        public int Compare(T x, T y)
        {
            return compare(x, y);
        }
    }
}
