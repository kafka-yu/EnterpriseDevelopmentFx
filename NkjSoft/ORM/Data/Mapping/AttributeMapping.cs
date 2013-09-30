
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace NkjSoft.ORM.Data.Mapping
{
    using Common;
    using NkjSoft.ORM.Core;

    public abstract class MappingAttribute : Attribute
    {
    }

    public abstract class TableBaseAttribute : MappingAttribute
    {
        public string Name { get; set; }
        public string Alias { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TableAttribute : TableBaseAttribute
    {
        public Type EntityType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ExtensionTableAttribute : TableBaseAttribute
    {
        public string KeyColumns { get; set; }
        public string RelatedAlias { get; set; }
        public string RelatedKeyColumns { get; set; }
    }

    public abstract class MemberAttribute : MappingAttribute
    {
        public string Member { get; set; }
    }

    /// <summary>
    /// 表示表中字段的描述。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ColumnAttribute : MemberAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
        /// </summary>
        public ColumnAttribute()
        {
            this.DbType = string.Empty;
            this.Length = 0;
            this.DefaultValue = string.Empty;
            this.Name = string.Empty;
            this.IsNullable = true;
            this.Length = 50;
            this.InsertOrUpdatable = true;
            this.IsComputed = false;
            this.IsPrimaryKey = false;
            this.IsIdentity = false;
            this.Ignore = false;
        }

        /// <summary>
        /// 获取或设置列名称
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置列别名
        /// </summary>
        /// <value>The alias.</value>
        public string Alias { get; set; }
        /// <summary>
        /// 获取或设置数据库类型
        /// </summary>
        /// <value>The type of the db.</value>
        public string DbType { get; set; }
        /// <summary>
        /// 获取或设置是否是计算列
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is computed; otherwise, <c>false</c>.
        /// </value>
        public bool IsComputed { get; set; }
        /// <summary>
        /// 获取或设置列是否是主键.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// 获取或设置列是否自动增长
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is generated; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity { get; set; }

        ///// <summary>
        ///// 获取或设置一个值，表示该成员是否是自动增长列。
        ///// </summary>
        //public bool IsIdentity { get; set; }

        /// <summary>
        /// 获取或设置字段值的长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 获取或设置是否允许DbNull的空值。
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 获取或设置默认值。
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 获取或设置表示 该字段成员是否参与 Insert or Update 操作 。默认（true）
        /// </summary>
        public bool InsertOrUpdatable { get; set; }



        private string[] notNeedLengthSytax = { "int", "datetime", "float", "decimal", "double" };
        /// <summary>
        /// Buildes the length.
        /// </summary>
        /// <param name="insteadType">Type of the instead.</param>
        /// <returns></returns>
        public string BuildeLength(string insteadType)
        {
            if (string.IsNullOrEmpty(this.DbType))
                this.DbType = insteadType;
            if (this.Length == 0 || notNeedLengthSytax.Contains(DbType.ToLowerInvariant()))
            {
                this.Length = 50;
            }
            if (notNeedLengthSytax.Contains(insteadType))
                return string.Empty;
            return string.Format("({0})", this.Length.ToString());
        }

        //TODO:2010-11-22 解决 自定义属性..(即 数据库实体映射类中包含非数据库字读成员)..
        /// <summary>
        /// 获取或设置一个值,表示被标记的成员不参与数据库的映射到 CLR 属性。
        /// </summary>
        public bool Ignore { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class AssociationAttribute : MemberAttribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the key members.
        /// </summary>
        /// <value>The key members.</value>
        public string KeyMembers { get; set; }
        /// <summary>
        /// Gets or sets the related entity ID.
        /// </summary>
        /// <value>The related entity ID.</value>
        public string RelatedEntityID { get; set; }
        /// <summary>
        /// Gets or sets the type of the related entity.
        /// </summary>
        /// <value>The type of the related entity.</value>
        public Type RelatedEntityType { get; set; }
        /// <summary>
        /// Gets or sets the related key members.
        /// </summary>
        /// <value>The related key members.</value>
        public string RelatedKeyMembers { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is foreign key.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is foreign key; otherwise, <c>false</c>.
        /// </value>
        public bool IsForeignKey { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AttributeMapping : AdvancedMapping
    {
        Type contextType;
        Dictionary<string, MappingEntity> entities = new Dictionary<string, MappingEntity>();
        ReaderWriterLock rwLock = new ReaderWriterLock();

        public AttributeMapping(Type contextType)
        {
            this.contextType = contextType;
        }

        public override MappingEntity GetEntity(MemberInfo contextMember)
        {
            Type elementType = TypeHelper.GetElementType(TypeHelper.GetMemberType(contextMember));
            return this.GetEntity(elementType, contextMember.Name);
        }

        public override MappingEntity GetEntity(Type type, string tableId)
        {
            return this.GetEntity(type, tableId, type);
        }

        private MappingEntity GetEntity(Type elementType, string tableId, Type entityType)
        {
            MappingEntity entity;
            rwLock.AcquireReaderLock(Timeout.Infinite);
            if (!entities.TryGetValue(tableId, out entity))
            {
                rwLock.ReleaseReaderLock();
                rwLock.AcquireWriterLock(Timeout.Infinite);
                if (!entities.TryGetValue(tableId, out entity))
                {
                    entity = this.CreateEntity(elementType, tableId, entityType);
                    this.entities.Add(tableId, entity);
                }
                rwLock.ReleaseWriterLock();
            }
            else
            {
                rwLock.ReleaseReaderLock();
            }
            return entity;
        }

        protected virtual IEnumerable<MappingAttribute> GetMappingAttributes(string rootEntityId)
        {
            var contextMember = this.FindMember(this.contextType, rootEntityId);
            return (MappingAttribute[])Attribute.GetCustomAttributes(contextMember, typeof(MappingAttribute));
        }

        public override string GetTableId(Type entityType)
        {
            if (contextType != null)
            {
                foreach (var mi in contextType.GetMembers(BindingFlags.Instance | BindingFlags.Public))
                {
                    FieldInfo fi = mi as FieldInfo;
                    if (fi != null && TypeHelper.GetElementType(fi.FieldType) == entityType)
                        return fi.Name;
                    PropertyInfo pi = mi as PropertyInfo;
                    if (pi != null && TypeHelper.GetElementType(pi.PropertyType) == entityType)
                        return pi.Name;
                }
            }
            return entityType.Name;
        }

        private MappingEntity CreateEntity(Type elementType, string tableId, Type entityType)
        {
            if (tableId == null)
                tableId = this.GetTableId(elementType);
            var members = new HashSet<string>();
            var mappingMembers = new List<AttributeMappingMember>();
            int dot = tableId.IndexOf('.');
            var rootTableId = dot > 0 ? tableId.Substring(0, dot) : tableId;
            var path = dot > 0 ? tableId.Substring(dot + 1) : "";
            var mappingAttributes = this.GetMappingAttributes(rootTableId);
            var tableAttributes = mappingAttributes.OfType<TableBaseAttribute>()
                .OrderBy(ta => ta.Name);
            var tableAttr = tableAttributes.OfType<TableAttribute>().FirstOrDefault();
            if (tableAttr != null && tableAttr.EntityType != null && entityType == elementType)
            {
                entityType = tableAttr.EntityType;
            }
            var memberAttributes = mappingAttributes.OfType<MemberAttribute>()
                .Where(ma => ma.Member.StartsWith(path))
                .OrderBy(ma => ma.Member);

            foreach (var attr in memberAttributes)
            {
                if (string.IsNullOrEmpty(attr.Member))
                    continue;
                string memberName = (path.Length == 0) ? attr.Member : attr.Member.Substring(path.Length + 1);
                MemberInfo member = null;
                MemberAttribute attribute = null;
                AttributeMappingEntity nested = null;
                if (memberName.Contains('.')) // additional nested mappings
                {
                    string nestedMember = memberName.Substring(0, memberName.IndexOf('.'));
                    if (nestedMember.Contains('.'))
                        continue; // don't consider deeply nested members yet
                    if (members.Contains(nestedMember))
                        continue; // already seen it (ignore additional)
                    members.Add(nestedMember);
                    member = this.FindMember(entityType, nestedMember);
                    string newTableId = tableId + "." + nestedMember;
                    nested = (AttributeMappingEntity)this.GetEntity(TypeHelper.GetMemberType(member), newTableId);
                }
                else
                {
                    if (members.Contains(memberName))
                    {
                        throw new InvalidOperationException(string.Format("AttributeMapping: more than one mapping attribute specified for member '{0}' on type '{1}'", memberName, entityType.Name));
                    }
                    member = this.FindMember(entityType, memberName);
                    attribute = attr;
                }
                mappingMembers.Add(new AttributeMappingMember(member, attribute, nested));
            }
            return new AttributeMappingEntity(elementType, tableId, entityType, tableAttributes, mappingMembers);
        }

        private static readonly char[] dotSeparator = new char[] { '.' };

        private MemberInfo FindMember(Type type, string path)
        {
            MemberInfo member = null;
            string[] names = path.Split(dotSeparator);
            foreach (string name in names)
            {
                member = type.GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase).FirstOrDefault();
                if (member == null)
                {
                    throw new InvalidOperationException(string.Format("AttributMapping: the member '{0}' does not exist on type '{1}'", name, type.Name));
                }
                type = TypeHelper.GetElementType(TypeHelper.GetMemberType(member));
            }
            return member;
        }

        public override string GetTableName(MappingEntity entity)
        {
            AttributeMappingEntity en = (AttributeMappingEntity)entity;
            var table = en.Tables.FirstOrDefault();
            return this.GetTableName(table);
        }

        private string GetTableName(MappingEntity entity, TableBaseAttribute attr)
        {
            string name = (attr != null && !string.IsNullOrEmpty(attr.Name))
                ? attr.Name
                : entity.TableId;
            return name;
        }

        public override IEnumerable<MemberInfo> GetMappedMembers(MappingEntity entity)
        {
            return ((AttributeMappingEntity)entity).MappedMembers;
        }

        public override bool IsMapped(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return mm != null;
        }

        public override bool IsColumn(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return mm != null && mm.Column != null;
        }

        public override bool IsComputed(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return mm != null && mm.Column != null && mm.Column.IsComputed;
        }

        public override bool IsIdentity(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return mm != null && mm.Column != null && mm.Column.IsIdentity;
        }

        public override bool IsPrimaryKey(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return mm != null && mm.Column != null && mm.Column.IsPrimaryKey;
        }

        public override string GetColumnName(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            if (mm != null && mm.Column != null && !string.IsNullOrEmpty(mm.Column.Name))
                return mm.Column.Name;
            return base.GetColumnName(entity, member);
        }

        public override string GetColumnDbType(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            if (mm != null && mm.Column != null && !string.IsNullOrEmpty(mm.Column.DbType))
                return mm.Column.DbType;
            return null;
        }

        public override bool IsAssociationRelationship(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return mm != null && mm.Association != null;
        }

        public override bool IsRelationshipSource(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            if (mm != null && mm.Association != null)
            {
                if (mm.Association.IsForeignKey && !typeof(IEnumerable).IsAssignableFrom(TypeHelper.GetMemberType(member)))
                    return true;
            }
            return false;
        }

        public override bool IsRelationshipTarget(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            if (mm != null && mm.Association != null)
            {
                if (!mm.Association.IsForeignKey || typeof(IEnumerable).IsAssignableFrom(TypeHelper.GetMemberType(member)))
                    return true;
            }
            return false;
        }

        public override bool IsNestedEntity(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return mm != null && mm.NestedEntity != null;
        }

        public override MappingEntity GetRelatedEntity(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingEntity thisEntity = (AttributeMappingEntity)entity;
            AttributeMappingMember mm = thisEntity.GetMappingMember(member.Name);
            if (mm != null)
            {
                if (mm.Association != null)
                {
                    Type elementType = TypeHelper.GetElementType(TypeHelper.GetMemberType(member));
                    Type entityType = (mm.Association.RelatedEntityType != null) ? mm.Association.RelatedEntityType : elementType;
                    return this.GetReferencedEntity(elementType, mm.Association.RelatedEntityID, entityType, "Association.RelatedEntityID");
                }
                else if (mm.NestedEntity != null)
                {
                    return mm.NestedEntity;
                }
            }
            return base.GetRelatedEntity(entity, member);
        }

        private static readonly char[] separators = new char[] { ' ', ',', '|' };

        public override IEnumerable<MemberInfo> GetAssociationKeyMembers(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingEntity thisEntity = (AttributeMappingEntity)entity;
            AttributeMappingMember mm = thisEntity.GetMappingMember(member.Name);
            if (mm != null && mm.Association != null)
            {
                return this.GetReferencedMembers(thisEntity, mm.Association.KeyMembers, "Association.KeyMembers", thisEntity.EntityType);
            }
            return base.GetAssociationKeyMembers(entity, member);
        }

        public override IEnumerable<MemberInfo> GetAssociationRelatedKeyMembers(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingEntity thisEntity = (AttributeMappingEntity)entity;
            AttributeMappingEntity relatedEntity = (AttributeMappingEntity)this.GetRelatedEntity(entity, member);
            AttributeMappingMember mm = thisEntity.GetMappingMember(member.Name);
            if (mm != null && mm.Association != null)
            {
                return this.GetReferencedMembers(relatedEntity, mm.Association.RelatedKeyMembers, "Association.RelatedKeyMembers", thisEntity.EntityType);
            }
            return base.GetAssociationRelatedKeyMembers(entity, member);
        }

        private IEnumerable<MemberInfo> GetReferencedMembers(AttributeMappingEntity entity, string names, string source, Type sourceType)
        {
            return names.Split(separators).Select(n => this.GetReferencedMember(entity, n, source, sourceType));
        }

        private MemberInfo GetReferencedMember(AttributeMappingEntity entity, string name, string source, Type sourceType)
        {
            var mm = entity.GetMappingMember(name);
            if (mm == null)
            {
                throw new InvalidOperationException(string.Format("AttributeMapping: The member '{0}.{1}' referenced in {2} for '{3}' is not mapped or does not exist", entity.EntityType.Name, name, source, sourceType.Name));
            }
            return mm.Member;
        }

        private MappingEntity GetReferencedEntity(Type elementType, string name, Type entityType, string source)
        {
            var entity = this.GetEntity(elementType, name, entityType);
            if (entity == null)
            {
                throw new InvalidOperationException(string.Format("The entity '{0}' referenced in {1} of '{2}' does not exist", name, source, entityType.Name));
            }
            return entity;
        }

        public override IList<MappingTable> GetTables(MappingEntity entity)
        {
            return ((AttributeMappingEntity)entity).Tables;
        }

        public override string GetAlias(MappingTable table)
        {
            return ((AttributeMappingTable)table).Attribute.Alias;
        }

        /// <summary>
        /// 获取列别名
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public override string GetAlias(MappingEntity entity, MemberInfo member)
        {
            AttributeMappingMember mm = ((AttributeMappingEntity)entity).GetMappingMember(member.Name);
            return (mm != null && mm.Column != null) ? mm.Column.Alias : null;
        }

        public override string GetTableName(MappingTable table)
        {
            var amt = (AttributeMappingTable)table;
            return this.GetTableName(amt.Entity, amt.Attribute);
        }

        public override bool IsExtensionTable(MappingTable table)
        {
            return ((AttributeMappingTable)table).Attribute is ExtensionTableAttribute;
        }

        public override string GetExtensionRelatedAlias(MappingTable table)
        {
            var attr = ((AttributeMappingTable)table).Attribute as ExtensionTableAttribute;
            return (attr != null) ? attr.RelatedAlias : null;
        }

        public override IEnumerable<string> GetExtensionKeyColumnNames(MappingTable table)
        {
            var attr = ((AttributeMappingTable)table).Attribute as ExtensionTableAttribute;
            if (attr == null) return new string[] { };
            return attr.KeyColumns.Split(separators);
        }

        public override IEnumerable<MemberInfo> GetExtensionRelatedMembers(MappingTable table)
        {
            var amt = (AttributeMappingTable)table;
            var attr = amt.Attribute as ExtensionTableAttribute;
            if (attr == null) return new MemberInfo[] { };
            return attr.RelatedKeyColumns.Split(separators).Select(n => this.GetMemberForColumn(amt.Entity, n));
        }

        private MemberInfo GetMemberForColumn(MappingEntity entity, string columnName)
        {
            foreach (var m in this.GetMappedMembers(entity))
            {
                if (this.IsNestedEntity(entity, m))
                {
                    var m2 = this.GetMemberForColumn(this.GetRelatedEntity(entity, m), columnName);
                    if (m2 != null)
                        return m2;
                }
                else if (this.IsColumn(entity, m) && string.Compare(this.GetColumnName(entity, m), columnName, true) == 0)
                {
                    return m;
                }
            }
            return null;
        }

        public override QueryMapper CreateMapper(QueryTranslator translator)
        {
            return new AttributeMapper(this, translator);
        }

        class AttributeMapper : AdvancedMapper
        {
            AttributeMapping mapping;

            public AttributeMapper(AttributeMapping mapping, QueryTranslator translator)
                : base(mapping, translator)
            {
                this.mapping = mapping;
            }
        }

        class AttributeMappingMember
        {
            MemberInfo member;
            MemberAttribute attribute;
            AttributeMappingEntity nested;

            internal AttributeMappingMember(MemberInfo member, MemberAttribute attribute, AttributeMappingEntity nested)
            {
                this.member = member;
                this.attribute = attribute;
                this.nested = nested;
            }

            internal MemberInfo Member
            {
                get { return this.member; }
            }

            internal ColumnAttribute Column
            {
                get { return this.attribute as ColumnAttribute; }
            }

            internal AssociationAttribute Association
            {
                get { return this.attribute as AssociationAttribute; }
            }

            internal AttributeMappingEntity NestedEntity
            {
                get { return this.nested; }
            }
        }

        class AttributeMappingTable : MappingTable
        {
            AttributeMappingEntity entity;
            TableBaseAttribute attribute;

            internal AttributeMappingTable(AttributeMappingEntity entity, TableBaseAttribute attribute)
            {
                this.entity = entity;
                this.attribute = attribute;
            }

            public AttributeMappingEntity Entity
            {
                get { return this.entity; }
            }

            public TableBaseAttribute Attribute
            {
                get { return this.attribute; }
            }
        }

        class AttributeMappingEntity : MappingEntity
        {
            string tableId;
            Type elementType;
            Type entityType;
            ReadOnlyCollection<MappingTable> tables;
            Dictionary<string, AttributeMappingMember> mappingMembers;

            internal AttributeMappingEntity(Type elementType, string tableId, Type entityType, IEnumerable<TableBaseAttribute> attrs, IEnumerable<AttributeMappingMember> mappingMembers)
            {
                this.tableId = tableId;
                this.elementType = elementType;
                this.entityType = entityType;
                this.tables = attrs.Select(a => (MappingTable)new AttributeMappingTable(this, a)).ToReadOnly();
                this.mappingMembers = mappingMembers.ToDictionary(mm => mm.Member.Name);
            }

            public override string TableId
            {
                get { return this.tableId; }
            }

            public override Type ElementType
            {
                get { return this.elementType; }
            }

            public override Type EntityType
            {
                get { return this.entityType; }
            }

            internal ReadOnlyCollection<MappingTable> Tables
            {
                get { return this.tables; }
            }

            internal AttributeMappingMember GetMappingMember(string name)
            {
                AttributeMappingMember mm = null;
                this.mappingMembers.TryGetValue(name, out mm);
                return mm;
            }

            internal IEnumerable<MemberInfo> MappedMembers
            {
                get { return this.mappingMembers.Values.Select(mm => mm.Member); }
            }


        }
    }
}
