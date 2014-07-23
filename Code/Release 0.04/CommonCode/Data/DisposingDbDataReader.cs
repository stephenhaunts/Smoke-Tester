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
using System.Data;
using System.Data.Common;

namespace Common.Data
{
    public sealed class DisposingDbDataReader : DbDataReader
    {
        private readonly IDataReader reader;
        private readonly IDbCommand command;
        internal bool IsTransactionDataReader;

        public override void Close()
        {
            reader.Close();
        }
      
        public override DataTable GetSchemaTable()
        {
            return reader.GetSchemaTable();
        }
    
        public override bool NextResult()
        {
            return reader.NextResult();
        }
      
        public override bool Read()
        {
            return reader.Read();
        }
   
        public override int Depth
        {
            get { return reader.Depth; }
        }
       
        public override bool IsClosed
        {
            get { return reader.IsClosed; }
        }

      
        public override int RecordsAffected
        {
            get { return reader.RecordsAffected; }
        }
     
        public override bool GetBoolean(int ordinal)
        {
            return reader.GetBoolean(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return reader.GetByte(ordinal);
        }
    
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }
      
        public override char GetChar(int ordinal)
        {
            return reader.GetChar(ordinal);
        }
    
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }
    
        public override Guid GetGuid(int ordinal)
        {
            return reader.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return reader.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return reader.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return reader.GetInt64(ordinal);
        }

      
        public override DateTime GetDateTime(int ordinal)
        {
            return reader.GetDateTime(ordinal);
        }

      
        public override string GetString(int ordinal)
        {
            return reader.GetString(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return reader.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            return reader.GetValues(values);
        }

      
        public override bool IsDBNull(int ordinal)
        {
            return reader.IsDBNull(ordinal);
        }
      
        public override int FieldCount
        {
            get { return reader.FieldCount; }
        }

        public override object this[int ordinal]
        {
            get { return reader[ordinal]; }
        }
      
        public override object this[string name]
        {
            get { return reader[name]; }
        }
       
        public override bool HasRows
        {
            get { return ((DbDataReader)reader).HasRows; }
        }
       
        public override decimal GetDecimal(int ordinal)
        {
            return reader.GetDecimal(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return reader.GetDouble(ordinal);
        }
      
        public override float GetFloat(int ordinal)
        {
            return reader.GetFloat(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return reader.GetName(ordinal);
        }
      
        public override int GetOrdinal(string name)
        {
            return reader.GetOrdinal(name);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (reader != null)
                {
                    if (!IsClosed)
                    {
                        Close();
                    }

                    reader.Dispose();
                }
                if (command != null)
                {
                    IDbConnection connection = command.Connection;

                    if (connection != null && !IsTransactionDataReader)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Dispose();
                    }

                    command.Dispose();
                }
            }
        }
    
        public override string GetDataTypeName(int ordinal)
        {
            return reader.GetDataTypeName(ordinal);
        }
       
        public override Type GetFieldType(int ordinal)
        {
            return reader.GetFieldType(ordinal);
        }
       
        public override IEnumerator GetEnumerator()
        {
            return ((DbDataReader)reader).GetEnumerator();
        }
        
        internal DisposingDbDataReader(IDbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            this.command = command;
            reader = command.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
