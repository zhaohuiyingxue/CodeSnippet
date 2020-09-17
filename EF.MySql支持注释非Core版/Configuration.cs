namespace Minitor_State1WebAPI.Migrations
{
    using Ministor_StateWebAPI.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            this.UseMysqlComment();
        }

        protected override void Seed(Ministor_StateWebAPI.Models.DB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}