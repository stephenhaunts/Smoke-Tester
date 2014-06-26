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
using System.Data;

namespace Common.Data
{
    public sealed class SimpleDbParam
    {
        public static SimpleDbParam ReturnValue<T>()
        {
            SimpleDbParam result = new SimpleDbParam("@RETURN_VALUE", default(T));
            result.Direction = ParameterDirection.ReturnValue;

            return result;
        }

        public static SimpleDbParam ReturnValue()
        {
            SimpleDbParam result = new SimpleDbParam("@RETURN_VALUE");
            result.Direction = ParameterDirection.ReturnValue;

            return result;
        }

        public static SimpleDbParam ReturnValue(DbType dbType)
        {
            SimpleDbParam result = new SimpleDbParam("@RETURN_VALUE");
            result.DbType = dbType;
            result.Direction = ParameterDirection.ReturnValue;

            return result;
        }

        public string ParameterName;
        public object Value;
        private ParameterDirection direction;

        public ParameterDirection Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                if (direction == ParameterDirection.ReturnValue || direction == ParameterDirection.Output)
                {
                    if (!DbType.HasValue)
                    {
                        DbType = System.Data.DbType.String;
                    }

                    if (!Size.HasValue)
                    {
                        Size = -1;
                    }
                }
            }
        }

        public int? Size;

        public DbType? DbType;

        public SimpleDbParam(string parameterName, object value)
        {
            ParameterName = parameterName;
            Value = value;
        }

        public SimpleDbParam(string parameterName)
        {
            ParameterName = parameterName;
        }

        public T GetValue<T>()
        {
            return Value.CoerceValue<T>();
        }
    }
}
