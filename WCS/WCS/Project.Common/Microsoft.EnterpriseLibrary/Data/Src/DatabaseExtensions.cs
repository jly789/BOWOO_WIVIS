//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Class that contains extension methods that apply on <see cref="Database"/>.
    /// </summary>
    static public class DatabaseExtensions
    {
        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor<TResult>(database, procedureName).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor<TResult>(database, procedureName, parameterMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, rowMapper).Execute(parameterValues);
        }
        
        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, parameterMapper, rowMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, parameterMapper, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, procedureName, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, procedureName, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);

            return new SprocAccessor<TResult>(database, procedureName, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);

            return new SprocAccessor<TResult>(database, procedureName, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            
            return new SprocAccessor<TResult>(database, procedureName, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);

            return new SprocAccessor<TResult>(database, procedureName, parameterMapper, resultSetMapper);
        }

        /// <summary>
        /// Executes a Transact-SQL query and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSqlStringAccessor<TResult>(this Database database, string sqlString)
            where TResult : new()
        {
            return CreateSqlStringAccessor<TResult>(database, sqlString).Execute();   
        }


        /// <summary>
        /// Executes a Transact-SQL query and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSqlStringAccessor<TResult>(this Database database, string sqlString, IResultSetMapper<TResult> resultSetMapper)
        {
            return CreateSqlStringAccessor(database, sqlString, resultSetMapper).Execute();
        }


        /// <summary>
        /// Executes a Transact-SQL query and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        static public IEnumerable<TResult> ExecuteSqlStringAccessor<TResult>(this Database database, string sqlString, IRowMapper<TResult> rowMapper)
        {
            return CreateSqlStringAccessor(database, sqlString, rowMapper).Execute();
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return new SqlStringAccessor<TResult>(database, sqlString, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return new SqlStringAccessor<TResult>(database, sqlString, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IRowMapper<TResult> rowMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IResultSetMapper<TResult> resultSetMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        static public DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, parameterMapper, resultSetMapper);
        }
		
		
		
		
		
		
		
		

		#region 추가함수

		/// <summary>
		/// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
		/// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
		/// </summary>
		/// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
		/// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
		/// <param name="procedureName">The name of the stored procedure that will be executed.</param>
		/// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
		/// <param name="type">Modifier of Property</param>
		/// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
		static public List<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, PropertyType type, params object[] parameterValues)
			where TResult : new()
		{
			return CreateSprocAccessor<TResult>(database, procedureName, type).Execute(parameterValues).ToList();
		}

		/// <summary>
		/// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
		/// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
		/// </summary>
		/// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
		/// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
		/// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
		/// <param name="type">Modifier of Property</param>
		/// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
		static public DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, PropertyType type)
			where TResult : new()
		{
			IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties(type);

			return CreateSprocAccessor(database, procedureName, defaultRowMapper);
		}

		/// <summary>
		/// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
		/// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
		/// </summary>
		/// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
		/// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
		/// <param name="reader">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
		/// <param name="type">Modifier of Property</param>
		/// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
		static public List<TResult> MapSprocAccessor<TResult>(this Database database, IDataReader reader, PropertyType type)
			where TResult : new()
		{
			IResultSetMapper<TResult> defaultResultSetMapper = new CommandAccessor<TResult>.DefaultDataReaderMapper(MapBuilder<TResult>.BuildAllProperties(type));

			return new SprocAccessor<TResult>(database, "Null", defaultResultSetMapper).MapReader(reader).ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedureName"></param>
		/// <param name="typeT"></param>
		/// <param name="typeU"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		static public EntitySet ExecuteSprocAccessor<T, U>(this Database database, string procedureName, PropertyType typeT, PropertyType typeU, params object[] parameterValues)
			where T : new() 
			where U : new()
		{
			EntitySet es = new EntitySet();

			IDataReader reader = database.ExecuteReader(procedureName, parameterValues);

			es.Add(MapSprocAccessor<T>(database, reader, typeT));
			reader.NextResult();
			es.Add(MapSprocAccessor<U>(database, reader, typeU));

			return es;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedureName"></param>
		/// <param name="typeT"></param>
		/// <param name="typeU"></param>
		/// <param name="typeV"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		static public EntitySet ExecuteSprocAccessor<T, U, V>(this Database database, string procedureName, PropertyType typeT, PropertyType typeU, PropertyType typeV, params object[] parameterValues)
			where T : new()
			where U : new()
			where V : new()
		{
			EntitySet es = new EntitySet();

			IDataReader reader = database.ExecuteReader(procedureName, parameterValues);

			es.Add(MapSprocAccessor<T>(database, reader, typeT));
			reader.NextResult();
			es.Add(MapSprocAccessor<U>(database, reader, typeU));
			reader.NextResult();
			es.Add(MapSprocAccessor<V>(database, reader, typeV));

			return es;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <typeparam name="W"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedureName"></param>
		/// <param name="typeT"></param>
		/// <param name="typeU"></param>
		/// <param name="typeV"></param>
		/// <param name="typeW"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		static public EntitySet ExecuteSprocAccessor<T, U, V, W>(this Database database, string procedureName, PropertyType typeT, PropertyType typeU, PropertyType typeV, PropertyType typeW, params object[] parameterValues)
			where T : new()
			where U : new()
			where V : new()
			where W : new()
		{
			EntitySet es = new EntitySet();

			IDataReader reader = database.ExecuteReader(procedureName, parameterValues);

			es.Add(MapSprocAccessor<T>(database, reader, typeT));
			reader.NextResult();
			es.Add(MapSprocAccessor<U>(database, reader, typeU));
			reader.NextResult();
			es.Add(MapSprocAccessor<V>(database, reader, typeV));
			reader.NextResult();
			es.Add(MapSprocAccessor<W>(database, reader, typeW));

			return es;
		}

		#endregion
	}
}
