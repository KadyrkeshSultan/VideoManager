using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace VMInterfaces
{
    public static class ConnectionTools
    {
        public static void ChangeDatabase(this DbContext source, string initialCatalog = "", string dataSource = "", string userId = "", string password = "", bool integratedSecuity = true, string configConnectionStringName = "")
        {
            try
            {
                SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings[string.IsNullOrEmpty(configConnectionStringName) ? source.GetType().Name : configConnectionStringName].ConnectionString).ProviderConnectionString);
                if (!string.IsNullOrEmpty(initialCatalog))
                    connectionStringBuilder.InitialCatalog = initialCatalog;
                if (!string.IsNullOrEmpty(dataSource))
                    connectionStringBuilder.DataSource = dataSource;
                if (!string.IsNullOrEmpty(userId))
                    connectionStringBuilder.UserID = userId;
                if (!string.IsNullOrEmpty(password))
                    connectionStringBuilder.Password = password;
                connectionStringBuilder.IntegratedSecurity = integratedSecuity;
                source.Database.Connection.ConnectionString = connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
