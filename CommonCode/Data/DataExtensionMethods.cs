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
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Common.Collections;
using Common.Data.Attributes;
using Common.Reflection;

namespace Common.Data
{
    public static class DataExtensionMethods
    {
        public static void PopulateFromDataReader<T>(this T obj, IDataReader reader) where T : class, new()
        {
            obj.PopulateFromDataReader(reader, true);
        }

        private static readonly ICacheList<Type, PropertyInfo[]> propertiesCache = new CacheList<Type, PropertyInfo[]>(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance), TimeSpan.MaxValue, true, 800);
        private static readonly ICacheList<Type, FieldInfo[]> fieldsCache = new CacheList<Type, FieldInfo[]>(t => t.GetFields(BindingFlags.Public | BindingFlags.Instance), TimeSpan.MaxValue, true, 800);
        private static readonly ICacheList<KeyValuePair<Type, string>, PropertyInfo> propertyCache = new CacheList<KeyValuePair<Type, string>, PropertyInfo>(kvp => propertiesCache[kvp.Key].FirstOrDefault(p => p.Name == kvp.Value || (p.GetCustomAttribute<DataFieldAttribute>() ?? DataFieldAttribute.Empty).DataFieldName == kvp.Value), TimeSpan.MaxValue, true, 8000);
        private static readonly ICacheList<KeyValuePair<Type, string>, FieldInfo> fieldCache = new CacheList<KeyValuePair<Type, string>, FieldInfo>(kvp => fieldsCache[kvp.Key].FirstOrDefault(f => f.Name == kvp.Value || (f.GetCustomAttribute<DataFieldAttribute>() ?? DataFieldAttribute.Empty).DataFieldName == kvp.Value), TimeSpan.MaxValue, true, 8000);
        private static readonly ICacheList<Type, MethodInfo> tryParseMethodCache = new CacheList<Type, MethodInfo>(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m => m.Name == "TryParse" && m.GetParameters().Length == 2 && m.ReturnType == typeof(bool)));
        private static readonly ICacheList<Type, MethodInfo> parseMethodCache = new CacheList<Type, MethodInfo>(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m => m.Name == "Parse" && m.GetParameters().Length == 1 && m.ReturnType == t));
        private static readonly ICacheList<Type, Type[]> genericArgumentsCache = new CacheList<Type, Type[]>(t => t.GetGenericArguments());

        public static ReadOnlyCollection<T> ExecuteToReadOnlyCollection<T>(this IDbCommand command) where T : class, new()
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command.Connection == null) throw new InvalidOperationException("There is no Connection associated with the command object");

            List<T> result = new List<T>();

            using (IDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    result.Add(dataReader.ToObject<T>());
                }
            }

            return new ReadOnlyCollection<T>(result);
        }

        public static DataTable ExecuteToDataTable(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command.Connection == null) throw new InvalidOperationException("There is no Connection associated with the command object");

            DataTable result = new DataTable();

            using (IDataReader dataReader = command.ExecuteReader())
            {
                result.Load(dataReader);
            }

            return result;
        }

        public static List<T> ExecuteToList<T>(this IDbCommand command) where T : class, new()
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command.Connection == null) throw new InvalidOperationException("There is no Connection associated with the command object");

            using (IDataReader dataReader = command.ExecuteReader())
            {
                return dataReader.ToList<T>();
            }
        }

        public static List<T> ToList<T>(this IDataReader reader) where T : class,new()
        {
            List<T> result = new List<T>();
            while (reader.Read())
                result.Add(reader.ToObject<T>());
            return result;
        }

        public static List<TList> ToList<TList, TContents>(this IDataReader reader) where TContents : class, TList, new()
        {
            List<TList> result = new List<TList>();

            while (reader.Read())
            {
                result.Add(reader.ToObject<TContents>());
            }

            return result;
        }

        public static T ToObject<T>(this IDataReader reader) where T : class, new()
        {
            T result = new T();
            result.PopulateFromDataReader(reader, false);

            return result;
        }
        
        public static void PopulateFromDataReader<T>(this T obj, IDataReader reader, bool tryRead) where T : class, new()
        {
            if (obj == null)
            {
                throw new InvalidOperationException(
                    "Cannot call PopulateFromDataReader<T> on a null object - initialise it before populating.");
            }

            if (!tryRead || reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    KeyValuePair<Type, string> key = new KeyValuePair<Type, string>(typeof (T), name);
                    object value = reader[i].IsDbNull() ? null : reader[i];

                    PropertyInfo propertyInfo = propertyCache[key];
                    if (propertyInfo != null)
                    {
                        try
                        {
                            PropertyAccessor.Create(propertyInfo).Set(obj, value);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                string.Format("Failed to populate property {0} of type {1} with value of type {2}", name,
                                    propertyInfo.PropertyType, value != null ? value.GetType().Name : "null"), ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            FieldInfo fieldInfo = fieldCache[key];

                            if (fieldInfo != null)
                            {
                                fieldInfo.SetValue(obj, value);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            throw new InvalidOperationException(
                                string.Format("Failed to populate property or field {0} with value of type {1}", name,
                                    value != null ? value.GetType().Name : "null"), ex);
                        }
                    }
                }
            }

        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Conflicts with Microsoft's practices - e.g. IDbConnection - and Resharper's recommendations.")]
        public static bool IsDbNull(this object value)
        {
            return DBNull.Value.Equals(value);
        }

        public static T CoerceValue<T>(this object source)
        {
            if (source == null || DBNull.Value.Equals(source)) return default(T);
            if (source is T) return (T)source;

            object result;

            if (typeof (T).IsGenericType && typeof (T).Name.StartsWith("Nullable`"))
            {
                Type innerType = genericArgumentsCache[typeof (T)].First();

                if (source is string && string.IsNullOrWhiteSpace((string) source))
                {
                    result = Activator.CreateInstance(typeof (T));
                }
                else
                {
                    result = Activator.CreateInstance(typeof (T), CoerceValue(source, innerType));
                }
            }
            else
            {
                result = CoerceValue(source, typeof (T), default(T));
            }

            return (T)result;
        }
        
        public static object CoerceValue(this object source, Type targetType, object defaultValue = null)
        {
            if (source == null || DBNull.Value.Equals(source)) return defaultValue;

            if (source is DateTime && targetType == typeof (string))
            {
                source = (((DateTime) source).ToString("S"));
            }

            if (source is IConvertible)
            {
                try
                {
                    return Convert.ChangeType(source, targetType);
                }
                catch
                {
                }
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, source.ToString());
            }

            try
            {
                MethodInfo parse = parseMethodCache[targetType];

                if (source is string && parse != null)
                {
                    return parse.Invoke(null, new[] {source});
                }
            }
            catch
            {
            }

            if (targetType != typeof (TimeSpan))
            {
                try
                {
                    MethodInfo tryParse = tryParseMethodCache[targetType];

                    if (source is string && tryParse != null)
                    {
                        object result = defaultValue;
                        bool success = (bool) tryParse.Invoke(null, new[] {source, result});

                        if (success)
                        {
                            return result;
                        }
                    }
                }
                catch
                {
                }
            }

            try
            {
                if (targetType == typeof (string))
                {
                    return Convert.ChangeType(source.ToString(), targetType);
                }
            }
            catch
            {
            }

            return defaultValue;
        }

        public static T ToType<T>(this object value, bool throwExceptionIfNull = false)
        {
            if (value == null || value.GetType() == DBNull.Value.GetType())
            {
                if (throwExceptionIfNull)
                {
                    throw new ArgumentNullException("value", "Parameter cannot be null");
                }
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture); 
                
            }
            catch { return default(T); }
        }
    }
}
