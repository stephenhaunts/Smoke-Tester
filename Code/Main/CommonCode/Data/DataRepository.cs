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
using System.Data;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace Common.Data
{
    public abstract class DataRepository<TConnection> : IDataRepository where TConnection : DbConnection, new()
    {
        private class DatabaseResult<T>
        {
            public T FunctionReturnValue {get; set;}
            public object SqlReturnValue {get; set;}
        }

        private int? commandTimeout;

        public static readonly Dictionary<Guid?, IDbTransaction> TransactionDictionary =
                new Dictionary<Guid?, IDbTransaction>(10);

        protected abstract string ConnectionString {get; set;}
       
        public DbDataReader ExecuteReader(string storedProcedure, params SimpleDbParam[] parameters)
        {
            return ExecuteReader(storedProcedure, CommandType.StoredProcedure, null, parameters);
        }

        public DbDataReader ExecuteReader(string commandText, CommandType commandType, params SimpleDbParam[] parameters)
        {
            return ExecuteReader(commandText, commandType, null, parameters);
        }

        public object ExecuteScalar(string storedProcedure, params SimpleDbParam[] parameters)
        {
            return ExecuteScalar(storedProcedure, null, parameters);
        }

        public object ExecuteScalar(string commandText, CommandType commandType, params SimpleDbParam[] parameters)
        {
            return ExecuteScalar(commandText, commandType, null, parameters);
        }

        public int ExecuteNonQuery(string storedProcedure, params SimpleDbParam[] parameters)
        {
            return ExecuteNonQuery(storedProcedure, null, parameters);
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType, params SimpleDbParam[] parameters)
        {
            return ExecuteNonQuery(commandText, commandType, null, parameters);
        }

        public DataTable ExecuteToDataTable(string commandText, CommandType commandType,
                                            params SimpleDbParam[] parameters)
        {
            return ExecuteToDataTable(commandText, commandType, null, parameters);
        }

        public ReadOnlyCollection<T> ExecuteToReadOnlyCollection<T>(string commandText, CommandType commandType,
                                                                    params SimpleDbParam[] parameters)
                where T : class, new()
        {
            return ExecuteToReadOnlyCollection<T>(commandText, commandType, null, parameters);
        }

        public List<T> ExecuteToList<T>(string commandText, CommandType commandType, params SimpleDbParam[] parameters)
                where T : class, new()
        {
            return ExecuteToList<T>(commandText, commandType, null, parameters);
        }

        public DbDataReader ExecuteReader(string storedProcedure, Guid? transactionId, params SimpleDbParam[] parameters)
        {
            return ExecuteReader(storedProcedure, CommandType.StoredProcedure, transactionId, parameters);
        }

        public Guid BeginTransaction(IsolationLevel isolationLevel)
        {
            Guid transactionId = Guid.NewGuid();
            DbConnection connection = new TConnection {ConnectionString = ConnectionString};
            connection.Open();
            DbTransaction transaction = connection.BeginTransaction(isolationLevel);
            TransactionDictionary.Add(transactionId, transaction);

            return transactionId;
        }

        private readonly Action<IDbTransaction> commit = t => t.Commit();
        private readonly Action<IDbTransaction> rollback = t => t.Rollback();
        private Action<IDbCommand> beforePrepareCommand = c => { };

        public void CommitTransaction(Guid transaction)
        {
            DisposeTransaction(transaction, commit);
        }

        public void AbortTransaction(Guid transactionId)
        {
            DisposeTransaction(transactionId, rollback);
        }

        private void DisposeTransaction(Guid transactionId, Action<IDbTransaction> preDispose)
        {
            IDbTransaction transaction = TransactionDictionary[transactionId];
            IDbConnection connection = transaction.Connection;
            preDispose(transaction);
            TransactionDictionary.Remove(transactionId);
            transaction.Dispose();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Dispose();
        }
        
        public DbDataReader ExecuteReader(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters)
        {
            IDbConnection connection = GetConnection(transactionId);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            IDbCommand command = connection.CreateCommand();

            if (commandTimeout.HasValue) command.CommandTimeout = commandTimeout.Value;
            {
                PrepareCommand(commandText, commandType, parameters, command, BeforePrepareCommand);
            }

            DisposingDbDataReader disposingDbDataReader = new DisposingDbDataReader(command);

            RewireParameterResults(command, parameters);
            disposingDbDataReader.IsTransactionDataReader = transactionId != null;

            return disposingDbDataReader;
        }

        private IDbConnection GetConnection(Guid? transactionId)
        {
            IDbConnection connection = transactionId.HasValue
                                              ? TransactionDictionary[transactionId].Connection
                                              : new TConnection { ConnectionString = ConnectionString };
            return connection;
        }

        private static void AddParametersToCommand(IEnumerable<SimpleDbParam> parameters, IDbCommand command)
        {
            foreach (SimpleDbParam simpleDbParam in parameters)
            {
                DbParameter dbParameter = (DbParameter)command.CreateParameter();
                dbParameter.ParameterName = simpleDbParam.ParameterName;
                dbParameter.Value = simpleDbParam.Value;
                dbParameter.Direction = simpleDbParam.Direction;

                if (simpleDbParam.Size.HasValue)
                {
                    dbParameter.Size = simpleDbParam.Size.Value;
                }

                if (simpleDbParam.DbType.HasValue)
                {
                    dbParameter.DbType = simpleDbParam.DbType.Value;
                }

                command.Parameters.Add(dbParameter);
            }
        }

        public object ExecuteScalar(string storedProcedure, Guid? transactionId, params SimpleDbParam[] parameters)
        {
            return ExecuteScalar(storedProcedure, CommandType.StoredProcedure, transactionId, parameters);
        }

        public T ExecuteScalar<T>(string storedProcedure, Guid transactionId, params SimpleDbParam[] parameters)
        {
            return ExecuteScalar<T>(storedProcedure, CommandType.StoredProcedure, transactionId, parameters);
        }

        public object ExecuteScalar(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters)
        {
            return ExecuteFunc(commandText, commandType, transactionId, parameters, c => c.ExecuteScalar()).FunctionReturnValue;
        }

        public T ExecuteScalar<T>(string commandText, CommandType commandType, Guid transactionId, params SimpleDbParam[] parameters)
        {
            return ExecuteFunc(commandText, commandType, transactionId, parameters, c => c.ExecuteScalar()).FunctionReturnValue.CoerceValue<T>();
        }

        public int ExecuteNonQuery(string storedProcedure, Guid? transactionId, params SimpleDbParam[] parameters)
        {
            return ExecuteNonQuery(storedProcedure, CommandType.StoredProcedure, transactionId, parameters);
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType, Guid? transactionId, params SimpleDbParam[] parameters)
        {
            return ExecuteFunc(commandText, commandType, transactionId, parameters, c => c.ExecuteNonQuery()).FunctionReturnValue;
        }

        public DataTable ExecuteToDataTable(string commandText, CommandType commandType, Guid? transactionId,
                                            params SimpleDbParam[] parameters)
        {
            return ExecuteFunc(commandText, commandType, transactionId, parameters, DataExtensionMethods.ExecuteToDataTable).FunctionReturnValue;
        }

        public ReadOnlyCollection<T> ExecuteToReadOnlyCollection<T>(string commandText, CommandType commandType, Guid? transactionId,
                                                                    params SimpleDbParam[] parameters)
                where T : class, new()
        {
            return ExecuteFunc(commandText, commandType, transactionId, parameters, DataExtensionMethods.ExecuteToReadOnlyCollection<T>).FunctionReturnValue;
        }

        public List<T> ExecuteToList<T>(string commandText, CommandType commandType, Guid? transactionId = null, params SimpleDbParam[] parameters)
                where T : class, new()
        {
            return ExecuteFunc(commandText, commandType, transactionId, parameters, DataExtensionMethods.ExecuteToList<T>).FunctionReturnValue;
        }

        internal Action<IDbCommand> BeforePrepareCommand
        {
            get {return beforePrepareCommand;}
            set {beforePrepareCommand = value;}
        }

        private DatabaseResult<T> ExecuteFunc<T>(string commandText, CommandType commandType, Guid? transactionId, IEnumerable<SimpleDbParam> parameters,
                                 Func<IDbCommand, T> func)
        {
            IDbConnection connection = GetConnection(transactionId);

            try
            {
                DatabaseResult<T> databaseResult = new DatabaseResult<T>();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbCommand command = connection.CreateCommand())
                {
                    PrepareCommand(commandText, commandType, parameters, command,BeforePrepareCommand);
                    databaseResult.FunctionReturnValue = func(command);

                    for (int i = command.Parameters.Count - 1; i >= 0; i--)
                    {
                        DbParameter dbParameter = ((DbParameter)command.Parameters[i]);

                        if (dbParameter.ParameterName.ToUpper() == "@RETURN_VALUE")
                        {
                            databaseResult.SqlReturnValue = dbParameter.Value;
                            break;
                        }
                    }

                    RewireParameterResults(command, parameters);
                }

                return databaseResult;
            }
            finally
            {
                if (!transactionId.HasValue)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    connection.Dispose();
                }
            }
        }

        private static void RewireParameterResults(IDbCommand command, IEnumerable<SimpleDbParam> parameters)
        {
            foreach (SimpleDbParam simpleDbParam in parameters)
            {
                DbParameter dbParameter = (DbParameter)command.Parameters[simpleDbParam.ParameterName];

                if (dbParameter != null)
                {
                    simpleDbParam.Value = dbParameter.Value;
                }
            }
        }

        private static void PrepareCommand(string commandText, CommandType commandType,
                                           IEnumerable<SimpleDbParam> parameters,
                                           IDbCommand command, Action<IDbCommand> beforePrepareCommand)
        {
            beforePrepareCommand(command);
            command.CommandType = commandType;
            command.CommandText = commandText;
            AddParametersToCommand(parameters, command);
        }
    }
}