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
using System.Runtime.Serialization;

namespace ConfigurationTests
{
    [Serializable]
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class AssertionException<T> : AssertionException
    {
        private readonly T expected;
        private readonly T actual;
        private readonly bool messageSpecified;
        private readonly bool valuesSpecified;

        public AssertionException(T expected, T actual, string message = null)
            : base(message)
        {
            this.expected = expected;
            this.actual = actual;
            messageSpecified = message != null;
            valuesSpecified = true;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Expected", expected.ToString());
            info.AddValue("Actual", actual.ToString());
        }

        public override string Message
        {
            get
            {
                if (valuesSpecified)
                {
                    string message = string.Empty;

                    if (messageSpecified)
                    {
                        message = string.Format(" ({0})", base.Message);
                    }

                    return string.Format("Expected [{0}] Actual [{1}]{2}", expected, actual, message);
                }

                return base.Message;
            }
        }
    }
}