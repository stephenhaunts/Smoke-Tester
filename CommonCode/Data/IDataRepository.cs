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
using System.Data.Common;

namespace Common.Data
{
    public interface IDataRepository
    {       
        DbDataReader ExecuteReader(string storedProcedure, params SimpleDbParam[] parameters);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        DbDataReader ExecuteReader(string commandText, CommandType commandType, params SimpleDbParam[] parameters);
        object ExecuteScalar(string storedProcedure, params SimpleDbParam[] parameters);
        object ExecuteScalar(string commandText, CommandType commandType, params SimpleDbParam[] parameters);
        int ExecuteNonQuery(string storedProcedure, params SimpleDbParam[] parameters);
        int ExecuteNonQuery(string commandText, CommandType commandType, params SimpleDbParam[] parameters);
        DataTable ExecuteToDataTable(string commandText, CommandType commandType, params SimpleDbParam[] parameters);
        ReadOnlyCollection<T> ExecuteToReadOnlyCollection<T>(string commandText, CommandType commandType, params SimpleDbParam[] parameters) where T : class, new();
        List<T> ExecuteToList<T>(string commandText, CommandType commandType, params SimpleDbParam[] parameters) where T : class, new();

        DbDataReader ExecuteReader(string storedProcedure, Guid? transactionId, params SimpleDbParam[] parameters);        
        DbDataReader ExecuteReader(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters);
        object ExecuteScalar(string storedProcedure, Guid? transactionId, params SimpleDbParam[] parameters);
        object ExecuteScalar(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters);
        int ExecuteNonQuery(string storedProcedure, Guid? transactionId, params SimpleDbParam[] parameters);
        int ExecuteNonQuery(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters);
        DataTable ExecuteToDataTable(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters);
        ReadOnlyCollection<T> ExecuteToReadOnlyCollection<T>(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters) where T : class, new();
        List<T> ExecuteToList<T>(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters) where T : class, new();
        Guid BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction(Guid transactionId);
        void AbortTransaction(Guid transactionId);
    }
}