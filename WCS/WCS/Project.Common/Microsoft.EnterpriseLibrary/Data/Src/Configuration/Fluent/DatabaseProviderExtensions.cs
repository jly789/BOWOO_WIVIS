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

using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    ///<summary>
    /// Provides extensions for common database providers.
    ///</summary>
    static public class DatabaseProviderExtensions
    {
        ///<summary>
        /// A Sql database for use with the System.Data.SqlClient namespace.
        ///</summary>
        /// <param name="context">Configuration context</param>
        ///<returns></returns>
        /// <seealso cref="System.Data.SqlClient"/>
        static public IDatabaseSqlDatabaseConfiguration ASqlDatabase(this IDatabaseConfigurationProviders context)
        {
            return new SqlDatabaseConfigurationExtension(context);
        }

        /// <summary>
        /// A Sql CE database for use with the System.Data.SqlServerCe namespace.
        /// </summary>
        /// <param name="context">Configuration context</param>
        /// <returns></returns>   
        static public IDatabaseSqlCeDatabaseConfiguration ASqlCeDatabase(this IDatabaseConfigurationProviders context)
        {
            return new SqlCeDatabaseConfigurationExtension(context);
        }

        /// <summary>
        /// An OleDb database for use with the <see cref="System.Data.OleDb"/> namespace.
        /// </summary>
        /// <returns></returns>
        static public IOleDbDatabaseConfiguration AnOleDbDatabase(this IDatabaseConfigurationProviders context)
        {
            return new OleDbConfigurationExtension(context);
        }

        /// <summary>
        /// An Odbc database for use with the <see cref="System.Data.Odbc"/> namespace.
        /// </summary>
        /// <returns></returns>
        static public IOdbcDatabaseConfiguration AnOdbcDatabase(this IDatabaseConfigurationProviders context)
        {
            return new OdbcConfigurationExtension(context);
        }

        ///<summary>
        /// An Oracle database for use with the System.Data.OracleClient namespace.
        ///</summary>
        ///<returns></returns>
        ///<seealso cref="System.Data.OracleClient"/>
        static public IDatabaseOracleConfiguration AnOracleDatabase(this IDatabaseConfigurationProviders context)
        {
            return new OracleConfigurationExtension(context);
        }

        ///<summary>
        /// A database with the specified database provider name.
        ///</summary>
        /// <param name="context">Extension context for fluent-interface</param>
        /// <param name="providerName">The provider name to use for this database connection</param>
        ///<returns></returns>
        /// <seealso cref="DbProviderFactories"/>
        static public IDatabaseAnotherDatabaseConfiguration AnotherDatabaseType(this IDatabaseConfigurationProviders context, string providerName)
        {
            return new AnotherDatabaseConfigurationExtensions(context, providerName);
        }
    }
}
