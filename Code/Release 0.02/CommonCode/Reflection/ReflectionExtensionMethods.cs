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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Collections;

namespace Common.Reflection
{
    public class MergeFieldAttribute : Attribute
    {
    }
    public static class ReflectionExtensionMethods
    {
        private static readonly ICacheList<KeyValuePair<Type, string>, PropertyInfo> propertyCache = new CacheList<KeyValuePair<Type, string>, PropertyInfo>(kvp => kvp.Key.GetProperty(kvp.Value, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance), TimeSpan.MaxValue, true, 8000);
        private static readonly ICacheList<KeyValuePair<Type, string>, FieldInfo> fieldCache = new CacheList<KeyValuePair<Type, string>, FieldInfo>(kvp => kvp.Key.GetField(kvp.Value, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance), TimeSpan.MaxValue, true, 8000);

        public static bool HasParameterlessConstructor(this Type type, bool includePrivate = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            if (includePrivate)
            {
                bindingFlags |= BindingFlags.NonPublic;
                
            }

            return type.GetConstructors(bindingFlags).Any(c => !c.GetParameters().Any());
        }
        
        public static bool SetPropertyValue<T>(this object candidate, PropertyInfo property, T value)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException("candidate");
                
            }

            if (property == null)
            {
                throw new ArgumentNullException("property");
                
            }

            if (property.PropertyType != typeof(T) && !property.PropertyType.IsSubclassOf(typeof(T)))
            {
                Type declaringType = property.DeclaringType;
                throw new ArgumentException(String.Format("Cannot set value of Type {0} on Property {1}.{2} of Type {3}", typeof(T), declaringType != null ? declaringType.Name : "", property.Name, property.PropertyType));
            }

            try
            {
                PropertyAccessor.Create(property).Set(candidate, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetFieldValue<T>(this object candidate, string fieldName, T value)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException("candidate");
                
            }

            FieldInfo field = fieldCache[new KeyValuePair<Type, string>(candidate.GetType(), fieldName)];

            return SetFieldValue(candidate, field, value);
        }
        
        public static bool SetFieldValue<T>(this object candidate, FieldInfo field, T value)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException("candidate");
                
            }

            if (field == null)
            {
                throw new ArgumentNullException("field");
                
            }

            if (field.FieldType != typeof(T) && !field.FieldType.IsSubclassOf(typeof(T)))
            {
                Type declaringType = field.DeclaringType;
                throw new ArgumentException(String.Format("Cannot set value of Type {0} on Field {1}.{2} of Type {3}", typeof(T), declaringType != null ? declaringType.Name : "", field.Name, field.ReflectedType));
            }

            try
            {
                field.SetValue(candidate, value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetPropertyValue<T>(this object candidate, string propertyName, T value)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException("candidate");
            }

            PropertyInfo property = propertyCache[new KeyValuePair<Type, string>(candidate.GetType(), propertyName)];

            return SetPropertyValue(candidate, property, value);
        }

        public static MemberInfo[] GetValueMembers(this Type type, params string[] memberNames)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
                
            }

            if (memberNames == null || memberNames.Length == 0)
            {
                return type.GetProperties().Cast<MemberInfo>().Union(type.GetFields()).ToArray();
            }

            return memberNames.Select(name => (MemberInfo)type.GetField(name) ?? type.GetProperty(name)).ToArray();
        }
        
        public static MemberInfo[] GetValueMembers<T>(this Type type, params string[] memberNames)
        {
            MemberInfo[] allPropertiesAndFields = type.GetValueMembers(memberNames);

            return
                allPropertiesAndFields.Where(
                    m =>
                    m is PropertyInfo
                        ? ((PropertyInfo)m).PropertyType == typeof(T)
                        : ((FieldInfo)m).FieldType == typeof(T)).ToArray();
        }

        public static Type GetReturnType(this MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    return null;
            }
        }

        public static bool ReturnTypeIs(this MemberInfo member, Type type)
        {
            Type returnType = member.GetReturnType();

            return returnType != null && (returnType.IsSubclassOf(type) || returnType == type);
        }

        public static bool Implements(this Type type, Type @interface)
        {
            return type.GetInterfaces().Contains(@interface);
        }

        private static MethodInfo memberwiseClone;       
        public static T MemberwiseClone<T>(this T source) where T : class
        {
            if (source == null)
            {
                return null;                
            }

            if (memberwiseClone == null)
            {
                memberwiseClone = typeof (object)
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .First(mi => mi.Name == "MemberwiseClone" && mi.GetParameters().Length == 0);
            }

            return (T)memberwiseClone.Invoke(source, null);
        }

        private static IEnumerable<string> assemblySearchPaths;

        public static IEnumerable<Type> GetAllTypes(this Assembly assembly)
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Union(
                new[]
                    {
                        Assembly.GetCallingAssembly(), Assembly.GetEntryAssembly(), Assembly.GetExecutingAssembly()
                    }).Distinct().Where(a => a != null && !a.IsDynamic).ToArray();

            assemblySearchPaths = loadedAssemblies.Select(a => Path.GetDirectoryName(a.Location)).Distinct().ToArray();
            IEnumerable<string> assemblyFiles = assemblySearchPaths.SelectMany(p => Directory.GetFiles(p, "*.exe")).Union(assemblySearchPaths.SelectMany(p => Directory.GetFiles(p, "*.dll")));
            IEnumerable<Assembly> assemblies = assemblyFiles.Select(f => f.LoadAssembly()).Where(a => a != null).Distinct();
            List<Type> types = new List<Type>();

            foreach (Assembly a in assemblies.Union(loadedAssemblies).Distinct())
            {
                try
                {
                    types.AddRange(a.GetTypes());
                }
                catch (ReflectionTypeLoadException e)
                {
                    types.AddRange(e.Types.Where(t => t != null));
                }
            }

            Type[] allTypes = types.ToArray();

            return allTypes;
        }
        
        public static Assembly LoadAssembly(this string f)
        {
            try
            {
                return Assembly.LoadFrom(f);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T GetCustomAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return (T)type.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
        }

        public static T GetCustomAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return (T)member.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(T));
        }

        public static void SetValue(this PropertyInfo propertyInfo, object obj, object value)
        {
            PropertyAccessor.Create(propertyInfo).Set(obj, value);
        }

        public static object GetValue(this PropertyInfo propertyInfo, object obj)
        {
            return PropertyAccessor.Create(propertyInfo).Get(obj);
        }

        public static TOut Transpose<TIn, TOut>(this TIn input)
        {
            Type outType = typeof(TOut);
            Type inType = typeof(TIn);

            TOut result;

            if (!outType.IsValueType && outType.HasParameterlessConstructor())
            {
                result = Activator.CreateInstance<TOut>();
            }
            else
            {
                result = default(TOut);
            }

            FieldInfo[] fields = inType.GetFields();
            IEnumerable<Tuple<FieldInfo, FieldInfo>> fieldInfos = fields.Select(f => new Tuple<FieldInfo, FieldInfo>(f, outType.GetField(f.Name)));

            foreach (Tuple<FieldInfo, FieldInfo> tuple in fieldInfos.Where(t => t.Item2 != null))
            {
                FieldInfo sourceField = tuple.Item1;
                FieldInfo targetField = tuple.Item2;

                if (!sourceField.FieldType.IsValueType && sourceField.FieldType != typeof (string) &&
                    !sourceField.FieldType.GetInterfaces().Contains(typeof (ICloneable)))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Cannot transpose from field {0} of type {2} because it is of type {1} which is not a value type or ICloneable",
                            sourceField.Name, sourceField.FieldType.Name, inType.Name));
                }

                if (!targetField.FieldType.IsValueType && targetField.FieldType != typeof (string) &&
                    !targetField.FieldType.GetInterfaces().Contains(typeof (ICloneable)))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Cannot transpose to field {0} of type {2} because it is of type {1} which is not a value type or ICloneable",
                            targetField.Name, targetField.FieldType.Name, outType.Name));
                }

                if (sourceField.FieldType != targetField.FieldType)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Cannot transpose field {0} because in type {1} it is of type {2} and in type {3} it is of type {4}",
                            sourceField.Name, inType.Name, sourceField.FieldType.Name, outType.Name,
                            targetField.FieldType.Name));
                }

                object value = sourceField.GetValue(input);

                if (value is ICloneable)
                {
                    value = ((ICloneable) value).Clone();
                }

                targetField.SetValue(result, value);
            }

            PropertyInfo[] properties = inType.GetProperties();
            IEnumerable<Tuple<PropertyInfo, PropertyInfo>> propertyInfos =
                properties.Select(p => new Tuple<PropertyInfo, PropertyInfo>(p, outType.GetProperty(p.Name)));

            foreach (Tuple<PropertyInfo, PropertyInfo> tuple in propertyInfos.Where(t => t.Item2 != null && t.Item2.GetSetMethod() != null))
            {
                PropertyInfo sourceProperty = tuple.Item1;
                PropertyInfo targetProperty = tuple.Item2;

                if (!sourceProperty.PropertyType.IsValueType && sourceProperty.PropertyType != typeof(string) && !sourceProperty.PropertyType.GetInterfaces().Contains(typeof(ICloneable)))
                {
                    throw new InvalidOperationException(string.Format("Cannot transpose from property {0} of type {2} because it is of type {1} which is not a value type or ICloneable", sourceProperty.Name, sourceProperty.PropertyType.Name, inType.Name));
                }

                if (!targetProperty.PropertyType.IsValueType && targetProperty.PropertyType != typeof(string) && !targetProperty.PropertyType.GetInterfaces().Contains(typeof(ICloneable)))
                {
                    throw new InvalidOperationException(string.Format("Cannot transpose to property {0} of type {2} because it is of type {1} which is not a value type or ICloneable", targetProperty.Name, targetProperty.PropertyType.Name, outType.Name));
                }

                if (sourceProperty.PropertyType != targetProperty.PropertyType)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Cannot transpose property {0} because in type {1} it is of type {2} and in type {3} it is of type {4}",
                            sourceProperty.Name, inType.Name, sourceProperty.PropertyType.Name, outType.Name,
                            targetProperty.PropertyType.Name));
                }

                object value = sourceProperty.GetValue(input);

                if (value is ICloneable)
                {
                    value = ((ICloneable) value).Clone();
                }

                PropertyAccessor.Create(targetProperty).Set(result, value);
            }

            return result;
        }

        private static MethodInfo transposeExactMethod;
        private static MethodInfo TransposeExactMethod
        {
            get
            {
                return transposeExactMethod ?? (transposeExactMethod = Assembly.GetExecutingAssembly()
                                                                                 .GetTypes()
                                                                                 .First(t => t.Name == "ReflectionExtensionMethods").GetMethods()
                                                                                 .First(m => m.Name == "TransposeExact"));
            }
        }

        private static object InvokeTransposeExact(object inObject, Type inType, Type outType)
        {
            return TransposeExactMethod
                    .MakeGenericMethod(new[] { inType, outType })
                    .Invoke(inType, new[] { inObject });
        }

        public static TOut TransposeExact<TIn, TOut>(this TIn input)
        {
            Type inType = typeof(TIn);
            Type outType = typeof(TOut);
            TOut returnValue = Activator.CreateInstance<TOut>();

            foreach (var property in inType.GetProperties())
            {
                var outProperty = outType.GetProperty(property.Name);
                var value = property.GetValue(input, null);

                if (property.PropertyType.Name.Contains("Collection") || property.PropertyType.Name.Contains("List"))
                {
                    var collectionEntryType = property.PropertyType.GetGenericArguments().Single();

                    if (!collectionEntryType.IsValueType && collectionEntryType != typeof(string))
                    {
                        object collection = PropertyAccessor.Create(property).Get(input);
                        object instance = Activator.CreateInstance(outProperty.PropertyType);
                        IList instanceAsList = (IList)instance;

                        foreach (object c in ((IList)collection))
                        {
                            object outc = InvokeTransposeExact(c, c.GetType(), outProperty.PropertyType.GetGenericArguments().Single());
                            instanceAsList.Add(outc);
                        }

                        PropertyAccessor.Create(outProperty).Set(returnValue, instance);
                        continue;
                    }

                    PropertyAccessor.Create(outProperty).Set(returnValue, value);
                    continue;

                }
                if (!property.PropertyType.IsValueType && property.PropertyType != typeof (string))
                {
                    value = InvokeTransposeExact(PropertyAccessor.Create(property).Get(input), property.PropertyType,
                        outProperty.PropertyType);
                }

                PropertyAccessor.Create(outProperty).Set(returnValue, value);
            }

            return returnValue;
        }

        public static void MergeInto<T>(this T source, T target)
        {
            Type type = typeof(T);

            IEnumerable<FieldInfo> fields = type.GetFields().Where(f => f.GetCustomAttribute<MergeFieldAttribute>() != null);

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(source);
                object targetValue = field.GetValue(target);
                value = GetLatestVersion(targetValue, value);
                field.SetValue(target, value);
            }

            IEnumerable<PropertyInfo> properties = type.GetProperties().Where(p => p.GetCustomAttribute<MergeFieldAttribute>() != null);

            foreach (PropertyInfo property in properties)
            {
                PropertyAccessor propertyAccessor = PropertyAccessor.Create(property);
                object value = propertyAccessor.Get(source);
                object targetValue = propertyAccessor.Get(target);
                value = GetLatestVersion(targetValue, value);
                propertyAccessor.Set(target, value);
            }
        }

        private static object GetLatestVersion(object targetValue, object value)
        {
            if (value is IList)
            {
                if (targetValue != null && ((IList) value).Count < ((IList) targetValue).Count)
                {
                    return value;                    
                }
            }

            if (value is int)
            {
                value = Math.Max((int) value, (int) targetValue);
            }

            return value;
        }

        public static object InvokeStaticMethod(this Type candidate, string methodName, params object[] args)
        {
            return candidate
                     .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                     .Invoke(null, args);
        }

        public static object GetFieldValue(this object candidate, string fieldName)
        {
            return fieldCache[new KeyValuePair<Type, string>(candidate.GetType(), fieldName)].GetValue(candidate);
        }

        public static PropertyAccessor GetPropertyAccessor(this Type candidate, string propertyName)
        {
            return PropertyAccessor.Create(candidate.GetProperty(propertyName));
        }

        public static PropertyAccessor GetAccessor(this PropertyInfo propertyInfo)
        {
            return PropertyAccessor.Create(propertyInfo);
        }

        public static bool IsNumeric(this Type candidate)
        {
            return candidate.IsValueType && numericTypes.Contains(candidate);
        }

        public static bool IsNumericType(this object candidate)
        {
            return candidate is Type ? IsNumeric((Type)candidate) : candidate.GetType().IsNumeric();
        }

        private static readonly List<Type> numericTypes = new List<Type>
            {
                    typeof(byte),
                    typeof(sbyte),
                    typeof(Int16),
                    typeof(UInt16),
                    typeof(Int32),
                    typeof(UInt32),
                    typeof(Int64),
                    typeof(UInt64),
                    typeof(Single),
                    typeof(Double),
                    typeof(Decimal)
            };
    }
}