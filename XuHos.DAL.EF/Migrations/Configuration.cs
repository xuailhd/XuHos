using XuHos.Entity;

namespace XuHos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<XuHos.DAL.EF.DBEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());    // This will add our MySQLClient as SQL Generator
        }

        protected override void Seed(XuHos.DAL.EF.DBEntities context)
        {
            
        }
    }

}
