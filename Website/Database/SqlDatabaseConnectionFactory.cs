using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Website.Configurations;

namespace Website.Database
{
    /// <summary>
    /// Denne klasse er en "hjælpe" klasse, som giver dig en forbindelse til din SQL Server. Du kan se bruge denne klasse hver gang at du skal lave noget i databasen.
    /// Den gør alt arbejdet med, at skaffe find en connection string frem og lave forbindelsen.
    /// </summary>
    public class SqlDatabaseConnectionFactory
    {
        private readonly IOptions<SqlServerConfiguration> _sqlServerConfiguration;

        public SqlDatabaseConnectionFactory(IOptions<SqlServerConfiguration> sqlServerConfiguration)
        {
            _sqlServerConfiguration = sqlServerConfiguration;
        }

        public SqlConnection CreateSqlConnection()
        {
            var sqlConnection = new SqlConnection(_sqlServerConfiguration.Value.SqlServerConnectionString);
            return sqlConnection;
        }
    }
}
