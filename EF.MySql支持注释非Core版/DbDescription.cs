using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Reflection;
using WebGrease.Css.Extensions;
/*
 用法：
0、如有需要，更改命名空间
1、实体类以及字段上添加特性[DbDescription("数据库描述")]
2、Migrations目录下的Configuration类，修改构造函数，增加以下语句
    this.UseMysqlComment();
3、DB（DbContext）类重写OnModelCreating方法，增加语句modelBuilder.ConfigDatabaseDescription();：
        /// <summary>
        /// 创建模型时执行方法重写。
        /// </summary>
        /// <param name="modelBuilder">模型构造器。</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigDatabaseDescription();
            base.OnModelCreating(modelBuilder);
        }
 */
namespace Minitor_State1WebAPI.Migrations
{
    /// <summary>
    /// 实体在数据库中的表和列的注释
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class DbDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">表或列的注释</param>
        public DbDescriptionAttribute(string description) { Description = description; }

        /// <summary>
        /// 数据库备注
        /// </summary>
        public virtual string Description { get; }
    }
    /// <summary>
    /// 表列注释相关的扩展方法
    /// </summary>
    public static class DbDescriptionExtensions
    {
        #region 基础配置及扩展方法
        /// <summary>
        /// 数据库注释在模型里的字典键名。
        /// </summary>
        public const string key = "DbDescription";
        /// <summary>
        /// 是否仅包含数据库注释。
        /// </summary>
        /// <param name="o">保存注释的对象。</param>
        /// <returns>是否仅包含数据库注释。</returns>
        public static bool HasCommentOnly(this IDictionary<string, AnnotationValues> o) { return o.HasComment() && o.Count == 1; }
        /// <summary>
        /// 是否包含数据库注释。
        /// </summary>
        /// <param name="o">保存注释的对象。</param>
        /// <returns>是否包含数据库注释。</returns>
        public static bool HasComment(this IDictionary<string, AnnotationValues> o) { return o != null && o.ContainsKey(key); }
        /// <summary>
        /// 去除表名中的构架名。
        /// </summary>
        /// <param name="table">要去除构架名的表名。</param>
        /// <returns>去除构架名后的表名。</returns>
        public static string TrimSchemaPrefix(this string table)
        {
            if (table.StartsWith("dbo.")) return table.Replace("dbo.", string.Empty);
            return table;
        }
        #endregion

        #region ColumnBuilder添加列增加参数comments
        private static IDictionary<string, AnnotationValues> AddComment(this IDictionary<string, AnnotationValues> annotations, string comment)
        {
            if (comment == null) return annotations;
            if (annotations == null) annotations = new Dictionary<string, AnnotationValues>();
            annotations.Add(key, new AnnotationValues(null, comment));
            return annotations;
        }
        private static IDictionary<string, object> AddComment(this IDictionary<string, object> annotations, string comment)
        {
            if (comment == null) return annotations;
            if (annotations == null) annotations = new Dictionary<string, object>();
            annotations.Add(key, comment);
            return annotations;
        }
        public static ColumnModel Binary(this ColumnBuilder b, string comment = null, bool? nullable = null, int? maxLength = null, bool? fixedLength = null, byte[] defaultValue = null, string defaultValueSql = null, bool timestamp = false, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Binary(nullable, maxLength, fixedLength, defaultValue, defaultValueSql, timestamp, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Boolean(this ColumnBuilder b, string comment = null, bool? nullable = null, bool? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Boolean(nullable, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Byte(this ColumnBuilder b, string comment = null, bool? nullable = null, bool identity = false, byte? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Byte(nullable, identity, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel DateTime(this ColumnBuilder b, string comment = null, bool? nullable = null, byte? precision = null, DateTime? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.DateTime(nullable, precision, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel DateTimeOffset(this ColumnBuilder b, string comment = null, bool? nullable = null, byte? precision = null, DateTimeOffset? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.DateTimeOffset(nullable, precision, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Decimal(this ColumnBuilder b, string comment = null, bool? nullable = null, byte? precision = null, byte? scale = null, decimal? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, bool identity = false, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Decimal(nullable, precision, scale, defaultValue, defaultValueSql, name, storeType, identity, annotations.AddComment(comment));
        }
        public static ColumnModel Double(this ColumnBuilder b, string comment = null, bool? nullable = null, double? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Double(nullable, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Geography(this ColumnBuilder b, string comment = null, bool? nullable = null, DbGeography defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Geography(nullable, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Geometry(this ColumnBuilder b, string comment = null, bool? nullable = null, DbGeometry defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Geometry(nullable, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Guid(this ColumnBuilder b, string comment = null, bool? nullable = null, bool identity = false, Guid? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Guid(nullable, identity, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Int(this ColumnBuilder b, string comment = null, bool? nullable = null, bool identity = false, int? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Int(nullable, identity, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Long(this ColumnBuilder b, string comment = null, bool? nullable = null, bool identity = false, long? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Long(nullable, identity, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Short(this ColumnBuilder b, string comment = null, bool? nullable = null, bool identity = false, short? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Short(nullable, identity, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Single(this ColumnBuilder b, string comment = null, bool? nullable = null, float? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Single(nullable, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel String(this ColumnBuilder b, string comment = null, bool? nullable = null, int? maxLength = null, bool? fixedLength = null, bool? unicode = null, string defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.String(nullable, maxLength, fixedLength, unicode, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        public static ColumnModel Time(this ColumnBuilder b, string comment = null, bool? nullable = null, byte? precision = null, TimeSpan? defaultValue = null, string defaultValueSql = null, string name = null, string storeType = null, IDictionary<string, AnnotationValues> annotations = null)
        {
            return b.Time(nullable, precision, defaultValue, defaultValueSql, name, storeType, annotations.AddComment(comment));
        }
        #endregion

        #region DbMigration创建修改表或列增加参数comments
        public static void TableComment(this DbMigration d, string name, string comment)
        {
            (d as IDbMigration).AddOperation(new SqlOperation($"ALTER TABLE `{name}` COMMENT '{comment}'") { SuppressTransaction = false });
        }
        public static TableBuilder<TColumns> CreateTable<TColumns>(this DbMigration d, string name, Func<ColumnBuilder, TColumns> columnsAction, object anonymousArguments = null, string comment = null)
        {
            return d.CreateTableWithComment(name, columnsAction, new Dictionary<string, object>().AddComment(comment), anonymousArguments);
        }
        public static TableBuilder<TColumns> CreateTable<TColumns>(this DbMigration d, string name, Func<ColumnBuilder, TColumns> columnsAction, IDictionary<string, object> annotations, object anonymousArguments = null, string comment = null)
        {
            return d.CreateTableWithComment(name, columnsAction, (annotations ?? new Dictionary<string, object>()).AddComment(comment), anonymousArguments);
        }
        /// <summary>
        /// 如果用DbMigration自带的CreateTable方法，会造成死循环。
        /// </summary>
        /// <typeparam name="TColumns"></typeparam>
        /// <param name="d"></param>
        /// <param name="name"></param>
        /// <param name="columnsAction"></param>
        /// <param name="annotations"></param>
        /// <param name="anonymousArguments"></param>
        /// <returns></returns>
        private static TableBuilder<TColumns> CreateTableWithComment<TColumns>(this DbMigration d, string name, Func<ColumnBuilder, TColumns> columnsAction, IDictionary<string, object> annotations, object anonymousArguments = null)
        {
            if (string.IsNullOrEmpty(name)) throw new Exception("创建表时必须设置表名。");
            if (columnsAction == null) throw new Exception("创建表时必须设置列对象。");
            CreateTableOperation createTableOperation = new CreateTableOperation(name, annotations, anonymousArguments);
            (d as IDbMigration).AddOperation(createTableOperation);
            //添加列
            TColumns columns = columnsAction(new ColumnBuilder());
            columns.GetType().GetNonIndexerProperties().ForEach(p =>
            {
                ColumnModel columnModel = p.GetValue(columns, null) as ColumnModel;
                if (columnModel != null)
                {
                    //columnModel..ApiPropertyInfo = p;
                    columnModel.GetType().GetField("_apiPropertyInfo", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(columnModel, p);
                    if (string.IsNullOrWhiteSpace(columnModel.Name)) columnModel.Name = p.Name;
                    createTableOperation.Columns.Add(columnModel);
                }
            });
            return new TableBuilder<TColumns>(createTableOperation, d);
        }
        public static IEnumerable<PropertyInfo> GetNonIndexerProperties(this Type type)
        {
            return from p in type.GetRuntimeProperties()
                   where p.IsPublic() && !p.GetIndexParameters().Any<ParameterInfo>()
                   select p;
        }
        public static bool IsPublic(this PropertyInfo property)
        {
            MethodInfo methodInfo = property.GetMethod;
            MethodAttributes methodAttributes = (methodInfo == null) ? MethodAttributes.Private : (methodInfo.Attributes & MethodAttributes.MemberAccessMask);
            MethodInfo methodInfo2 = property.GetMethod;
            MethodAttributes methodAttributes2 = (methodInfo2 == null) ? MethodAttributes.Private : (methodInfo2.Attributes & MethodAttributes.MemberAccessMask);
            MethodAttributes methodAttributes3 = (methodAttributes > methodAttributes2) ? methodAttributes : methodAttributes2;
            return methodAttributes3 == MethodAttributes.Public;
        }
        #endregion

        #region 写入注释扩展方法
        /// <summary>
        /// 写入列注释
        /// </summary>
        /// <param name="o">保存注释的对象。</param>
        /// <param name="f">原有生成代码的方法。</param>
        /// <param name="f">写入注释的方法。</param>
        public static void WriteComment(this IDictionary<string, AnnotationValues> o, Action gen, Action<string> f)
        {
            bool hasComment = o.ContainsKey(key);
            AnnotationValues comment = hasComment ? o[key] : null;
            //开始执行
            if (hasComment) o.Remove(key);
            gen();
            if (hasComment)
            {
                f(comment.NewValue as string ?? string.Empty);
                o.Add(key, comment);
            }
        }
        /// <summary>
        /// 写入表注释
        /// </summary>
        /// <param name="o">保存注释的对象。</param>
        /// <param name="f">原有生成代码的方法。</param>
        /// <param name="f">写入注释的方法。</param>
        public static void WriteComment(this IDictionary<string, object> o, Action gen, Action<string> f)
        {
            bool hasComment = o.ContainsKey(key);
            string comment = hasComment ? o[key] as string : null;
            //开始执行
            if (hasComment) o.Remove(key);
            gen();
            if (hasComment)
            {
                f(comment ?? string.Empty);
                o.Add(key, comment);
            }
        }
        #endregion

        /// <summary>
        /// 配置数据库表和列注释，在DB类（DbContext）的OnModelCreating方法中使用。
        /// </summary>
        /// <param name="modelBuilder">模型构造器</param>
        /// <returns>模型构造器</returns>
        public static DbModelBuilder ConfigDatabaseDescription(this DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Configure(column =>
            {
                var attr = column.ClrPropertyInfo.GetCustomAttribute<DbDescriptionAttribute>();
                if (attr != null)
                {
                    var propertyType = column.ClrPropertyInfo.PropertyType;
                    //如果该列的实体属性是枚举类型，把枚举的说明追加到列说明
                    var enumDbDescription = string.Empty;
                    if (propertyType.IsEnum
                        || (propertyType.IsSubclassOf(typeof(Nullable<>)) && propertyType.GenericTypeArguments[0].IsEnum))
                    {
                        var @enum = propertyType.IsSubclassOf(typeof(Nullable<>)) ? propertyType.GenericTypeArguments[0] : propertyType;
                        var descList = new List<string>();
                        foreach (var field in @enum?.GetFields() ?? new FieldInfo[0])
                        {
                            if (!field.IsSpecialName)
                            {
                                var desc = field.GetCustomAttribute<DbDescriptionAttribute>()?.Description;
                                descList.Add($@"{field.GetRawConstantValue()} : {(string.IsNullOrEmpty(desc) ? field.Name : desc)}");
                            }
                        }
                        var isFlags = @enum?.GetCustomAttribute<FlagsAttribute>() != null;
                        var enumTypeDbDescription = (@enum?.GetCustomAttribute<DbDescriptionAttribute>())?.Description;
                        enumTypeDbDescription += (isFlags ? " [标志位枚举]" : string.Empty);
                        enumDbDescription = $@"( {enumTypeDbDescription}{string.Join("; ", descList)} )";
                    }
                    column.HasColumnAnnotation(key, attr.Description + enumDbDescription);
                }
            });
            //添加表说明
            modelBuilder.Types().Configure(table =>
            {
                var attr = table.ClrType.GetCustomAttribute<DbDescriptionAttribute>();
                if (attr != null) table.HasTableAnnotation(key, attr.Description);

            });
            return modelBuilder;
        }
        /// <summary>
        /// 配置使用Mysql注释。在Migration类（DbMigrationsConfiguration<DB>）的构造函数中使用。
        /// </summary>
        /// <param name="c"></param>
        public static void UseMysqlComment(this DbMigrationsConfiguration c)
        {
            c.CodeGenerator = new ZMySqlMigrationCodeGenerator();
            c.SetSqlGenerator("MySql.Data.MySqlClient", new ZMySqlMigrationSqlGenerator());
        }
    }
    /// <summary>
    /// MYSQL迁移代码生成器
    /// </summary>
    public class ZMySqlMigrationCodeGenerator : MySqlMigrationCodeGenerator
    {
        /// <summary>
        /// 列对象迁移代码生成。
        /// </summary>
        /// <param name="column">列模型对象。</param>
        /// <param name="writer">写入器</param>
        /// <param name="emitName">列模型对象是否包括列名。</param>
        protected override void Generate(ColumnModel column, IndentedTextWriter writer, bool emitName = false)
        {
            column.Annotations.WriteComment(() => base.Generate(column, writer, emitName), o =>
            {
                StringWriter stringWriter = writer.InnerWriter as StringWriter;
                if (stringWriter == null) return;
                stringWriter.GetStringBuilder().Remove(stringWriter.ToString().LastIndexOf(")"), stringWriter.ToString().Length - stringWriter.ToString().LastIndexOf(")"));
                writer.Write($", comment: {base.Quote(o)})");
            });
        }
        /// <summary>
        /// 修改表注释迁移代码生成。
        /// </summary>
        /// <param name="alterTableOperation">表注释模型对象。</param>
        /// <param name="writer">写入器</param>
        protected override void Generate(AlterTableOperation alterTableOperation, IndentedTextWriter writer)
        {
            if (alterTableOperation == null || writer == null) return;
            //是否只设置了表注释
            bool hasCommentOnly = alterTableOperation.Annotations.HasCommentOnly();
            //如果有AnonymousArguments就认为不走设置表注释
            if (hasCommentOnly && alterTableOperation.AnonymousArguments != null && alterTableOperation.AnonymousArguments.Count > 0)
                hasCommentOnly = false;
            alterTableOperation.Annotations.WriteComment(() => { if (!hasCommentOnly) base.Generate(alterTableOperation, writer); }, o =>
            {
                //不管如何，都设置一下表注释，除非前后注释一样。
                writer.Write($"this.TableComment({base.Quote(alterTableOperation.Name.TrimSchemaPrefix())}, {base.Quote(o)});");
                writer.WriteLine();
            });
        }
        /// <summary>
        /// 创建表注释迁移代码生成。
        /// </summary>
        /// <param name="alterTableOperation">表注释模型对象。</param>
        /// <param name="writer">写入器</param>
        protected override void Generate(CreateTableOperation createTableOperation, IndentedTextWriter writer)
        {
            if (createTableOperation == null || writer == null) return;
            int startPos = writer.InnerWriter.ToString().Length;
            createTableOperation.Annotations.WriteComment(() => base.Generate(createTableOperation, writer), o =>
            {
                StringWriter stringWriter = writer.InnerWriter as StringWriter;
                if (stringWriter == null) return;
                string s = stringWriter.ToString();
                int pos_PrimaryKey = s.IndexOf(".PrimaryKey(", startPos);
                int pos_ForeignKey = s.IndexOf(".ForeignKey(", startPos);
                int pos_Index = s.IndexOf(".Index(", startPos);
                if (pos_PrimaryKey == -1) pos_PrimaryKey = int.MaxValue;
                if (pos_ForeignKey == -1) pos_ForeignKey = int.MaxValue;
                if (pos_Index == -1) pos_Index = int.MaxValue;
                int pos_CreateEnd = Math.Min(Math.Min(pos_PrimaryKey, pos_ForeignKey), pos_Index);
                int pos = s.Substring(0, pos_CreateEnd).LastIndexOf(")");
                stringWriter.GetStringBuilder().Insert(pos, $", comment: {base.Quote(o)}");
                stringWriter.GetStringBuilder().Insert(s.IndexOf("CreateTable(", startPos), "this.");
            });
        }
    }

    public class ZMySqlMigrationSqlGenerator : MySqlMigrationSqlGenerator
    {
        /// <summary>
        /// 修复创建索引错误，统一为btree模式。
        /// </summary>
        /// <param name="op">创建索引操作对象。</param>
        /// <returns>生成的SQL对象。</returns>
        protected override MigrationStatement Generate(CreateIndexOperation op)
        {
            op.AnonymousArguments.Add("Type", "BTrees");
            return base.Generate(op);
        }
        /// <summary>
        /// 列对象迁移SQL代码生成。
        /// </summary>
        /// <param name="column">列模型对象。</param>
        /// <returns>生成的SQL。</returns>
        protected override string Generate(ColumnModel op)
        {
            string s = string.Empty;
            op.Annotations.WriteComment(() => { s += base.Generate(op); }, o =>
            {
                s += $" COMMENT '{o}'";
            });
            return s;
        }
        /// <summary>
        /// 创建表对象迁移SQL代码生成。
        /// </summary>
        /// <param name="column">创建表操作对象。</param>
        /// <returns>生成的SQL。</returns>
        protected override MigrationStatement Generate(CreateTableOperation op)
        {
            MigrationStatement s = null;
            op.Annotations.WriteComment(() => { s = base.Generate(op); }, o =>
            {
                s.Sql += $" COMMENT '{o}'";
            });
            return s;
        }
        /*AlterTableOperation 基类并未实现，添加表注释操作已转换为SQL方法，此处不再重写*/
    }
}