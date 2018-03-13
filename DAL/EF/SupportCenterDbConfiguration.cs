using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace SC.DAL.EF {
    internal class SupportCenterDbConfiguration : DbConfiguration {
        public SupportCenterDbConfiguration() {
            SetDefaultConnectionFactory(new SqlConnectionFactory()); // SQL Server instantie op machine

            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);

            SetDatabaseInitializer(new SupportCenterDbInitializer());
        }
    }
}