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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Common.Collections;
using Common.Reflection;

namespace Common.Xml
{
    public static class XmlExtensionMethods
    {
        public static T Deserialize<T>(this string xml, bool useDataContractSerializer = false)
        {
            return ToObject<T>(xml, useDataContractSerializer);
        }

        public static T ToObject<T>(this string xml, bool useDataContractSerializer = false)
        {
            try
            {
                if (useDataContractSerializer)
                {
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                        return
                            (T)
                                dataContractSerializerCache[typeof (T)].ReadObject(
                                    XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8,
                                        new XmlDictionaryReaderQuotas(), null));
                }
                else
                {
                    using (XmlReader xmlReader = new XmlTextReader(new StringReader(xml)))
                    {
                        return (T) serializerCache[typeof (T)].Deserialize(xmlReader);
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }

        private static readonly ICacheList<Assembly, Type[]> extraTypesCache =
            new CacheList<Assembly, Type[]>(a => a.GetTypes().Where(t => !t.IsInterface && t.IsPublic && t.HasParameterlessConstructor() && !t.IsGenericTypeDefinition).ToArray());

        private static readonly ICacheList<Type, XmlSerializer> serializerCache =
            new CacheList<Type, XmlSerializer>(t => new XmlSerializer(t, extraTypesCache[t.Assembly]));

        private static readonly ICacheList<Type, XmlObjectSerializer> dataContractSerializerCache =
            new CacheList<Type, XmlObjectSerializer>(t => new DataContractSerializer(t));
        
        public static string ToXmlString<T>(this T obj)
        {
            string result = string.Empty;
            try
            {
                result = obj.Serialize();
            }
            catch
            {
                try
                {
                    if (string.IsNullOrEmpty(result))
                        result = obj.BadSerialize();
                }
                catch
                {
                    return null;
                }
            }
            return result;
        }

        public static string Serialize<T>(this T obj, bool useDataContractSerializer = false)
        {
            string result = string.Empty;

            if (!obj.Equals(default(T)))
            {
                if (useDataContractSerializer)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        dataContractSerializerCache[typeof (T)].WriteObject(memoryStream, obj);
                        result = Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
                else
                {
                    using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
                    {
                        serializerCache[typeof (T)].Serialize(stringWriter, obj);
                        result = stringWriter.ToString();
                    }
                }
            }

            return result;
        }

        private static string BadSerialize(this object obj)
        {
            if (obj == null) return string.Empty;
            if (obj.GetType().IsValueType || obj.GetType().FullName == "System.String") return obj.ToString();

            string output = string.Empty;
            Type type = obj.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (PropertyInfo info in propertyInfos)
            {
                object value = info.GetValue(obj);

                if (value != null)
                {
                    output += string.Format("<{0}>", info.Name);

                    try
                    {
                        output += value.BadSerialize();
                    }
                    catch (Exception ex)
                    {
                        output += ex.Message;
                    }

                    output += string.Format("</{0}>", info.Name);
                }
            }

            return output;
        }
    }
}
