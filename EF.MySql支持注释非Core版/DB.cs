using Minitor_State1WebAPI.Migrations;
using Minitor_State1WebAPI.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Ministor_StateWebAPI.Models
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DB : DbContext
    {
        /// <summary>
        /// name=DBConnection，DBConnection为数据库连接的名字，即web.config配置文件节点connectionStrings，name值为DBConnection的数据库连接字符串
        /// </summary>
        public DB(): base("name=DBConnection")
        {
            Database.SetInitializer<DbContext>(null);
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<DB, Migrations.Configuration>());
        }

        #region 配置所有的数据库表

        public DbSet<JC_DICTIONARY> jc_dictionary { set; get; }
        public DbSet<SB_RAILWAY> sb_railway { set; get; }
        public DbSet<SB_SECTION> sb_section { set; get; }
        public DbSet<JG_ORGANIZATION> jg_organization { set; get; }
        public DbSet<JC_ACCOUNT> jc_account { set; get; }
        public DbSet<SB_TRACK> sb_track { set; get; }
        public DbSet<JG_ORGANIZATION_SCOPE> jg_organization_scope { set; get; }
        public DbSet<SB_ANCHOR> sb_anchor { set; get; }
        public DbSet<AS_STANDARD_MANAGE> as_standard_manage { set; get; }
        public DbSet<SB_SEGMENT> sb_segment { set; get; }
        public DbSet<SB_SENSOR> sb_sensor { set; get; }
        public DbSet<AS_SENSOR_SCODE> as_sensor_scode { set; get; }
        public DbSet<AS_GIS> as_gis { set; get; }
        public DbSet<SB_ANCHOR_RELATION> sb_anchor_relation { set; get; }
        #endregion
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw ex.HandleDbUpdateException();
            }
        }
        /// <summary>
        /// 创建模型时执行方法重写。
        /// </summary>
        /// <param name="modelBuilder">模型构造器。</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigDatabaseDescription();
            base.OnModelCreating(modelBuilder);
        }
    }

}