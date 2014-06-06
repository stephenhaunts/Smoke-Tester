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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Xml
{
    public sealed class XmlNullable<T> : IEquatable<XmlNullable<T>>, IEquatable<T>, IXmlSerializable where T : struct
    {
        private T actualValue;

        public XmlNullable(T actualValue)
        {
            this.actualValue = actualValue;
        }

        public XmlNullable()
        {
        }

        public static implicit operator T(XmlNullable<T> source)
        {
            return (source == null) ? default(T) : source.actualValue;
        }

        public static implicit operator XmlNullable<T>(T source)
        {
            return new XmlNullable<T>(source);
        }

        public bool Equals(XmlNullable<T> other)
        {
            return other != null && other.actualValue.Equals(actualValue);
        }

        public bool Equals(T other)
        {
            return other.Equals(actualValue);
        }

        public override string ToString()
        {
            return actualValue.ToString();
        }

        public override int GetHashCode()
        {
            return actualValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() == typeof (T))
            {
                return Equals((T) obj);
            }

            XmlNullable<T> xmlObj = obj as XmlNullable<T>;

            return xmlObj != null && Equals(xmlObj);
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            if (typeof(T).IsEnum)
            {
                writer.WriteValue(actualValue.ToString());
            }
            else
            {
                writer.WriteValue(actualValue);
            }
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            if (typeof(T).IsEnum)
            {
                string stringElement = reader.ReadElementContentAsString();

                actualValue = (T)Enum.Parse(typeof(T), stringElement);
            }
            else
            {
                actualValue = (T)reader.ReadElementContentAs(typeof(T), null);
            }
        }

        public XmlSchema GetSchema() { return null; }
    }
}
