// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

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
    using NkjSoft.Extensions;
    /// <summary>
    /// A simple query mapping that attempts to infer mapping from naming conventions
    /// </summary>
    public class ImplicitMapping : BasicMapping
    {
        public ImplicitMapping()
        {
        }

        /// <summary>
        /// Determines the entity Id based on the type of the entity alone
        /// 获取对象到数据库表的映射名。
        /// </summary>
        /// <param name="type">数据库到对象的对象类型</param>
        /// <returns></returns>
        public override string GetTableId(Type type)
        {
            return this.InferTableName(type);
        }

        public override bool IsPrimaryKey(MappingEntity entity, MemberInfo member)
        {
            // Customers has CustomerID, Orders has OrderID, etc
            //if (this.IsColumn(entity, member)) 
            //{
            //    string name = NameWithoutTrailingDigits(member.Name);
            //    return member.Name.EndsWith("ID") && member.DeclaringType.Name.StartsWith(member.Name.Substring(0, member.Name.Length - 2)); 
            //}
            //return false;
            //TODO: 2010-7.17 更改 获取主键的方式;
            return base.IsPrimaryKey(entity, member);
        }

        private string NameWithoutTrailingDigits(string name)
        {
            int n = name.Length - 1;
            while (n >= 0 && char.IsDigit(name[n]))
            {
                n--;
            }
            if (n < name.Length - 1)
            {
                return name.Substring(0, n);
            }
            return name;
        }

        public override bool IsColumn(MappingEntity entity, MemberInfo member)
        {
            return IsScalar(TypeHelper.GetMemberType(member));
        }



        /// <summary>
        /// Determines if the property is an assocation relationship.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public override bool IsAssociationRelationship(MappingEntity entity, MemberInfo member)
        {
            if (IsMapped(entity, member) && !IsColumn(entity, member))
            {
                Type otherType = TypeHelper.GetElementType(TypeHelper.GetMemberType(member));
                return !this.IsScalar(otherType);
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is relationship source] [the specified entity].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="member">The member.</param>
        /// <returns>
        /// 	<c>true</c> if [is relationship source] [the specified entity]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRelationshipSource(MappingEntity entity, MemberInfo member)
        {
            if (IsAssociationRelationship(entity, member))
            {
                if (typeof(IEnumerable).IsAssignableFrom(TypeHelper.GetMemberType(member)))
                    return false;

                // is source of relationship if relatedKeyMembers are the related entity's primary keys
                MappingEntity entity2 = GetRelatedEntity(entity, member);
                var relatedPKs = new HashSet<string>(this.GetPrimaryKeyMembers(entity2).Select(m => m.Name));
                var relatedKeyMembers = new HashSet<string>(this.GetAssociationRelatedKeyMembers(entity, member).Select(m => m.Name));
                return relatedPKs.IsSubsetOf(relatedKeyMembers) && relatedKeyMembers.IsSubsetOf(relatedPKs);
            }
            return false;
        }

        public override bool IsRelationshipTarget(MappingEntity entity, MemberInfo member)
        {
            if (IsAssociationRelationship(entity, member))
            {
                if (typeof(IEnumerable).IsAssignableFrom(TypeHelper.GetMemberType(member)))
                    return true;

                // is target of relationship if the assoctions keys are the same as this entities primary key
                var pks = new HashSet<string>(this.GetPrimaryKeyMembers(entity).Select(m => m.Name));
                var keys = new HashSet<string>(this.GetAssociationKeyMembers(entity, member).Select(m => m.Name));
                return keys.IsSubsetOf(pks) && pks.IsSubsetOf(keys);
            }
            return false;
        }

        public override IEnumerable<MemberInfo> GetAssociationKeyMembers(MappingEntity entity, MemberInfo member)
        {
            List<MemberInfo> keyMembers;
            //List<MemberInfo> relatedKeyMembers;
            this.GetAssociationKeys(entity, member, out keyMembers);
            return keyMembers;
        }

        public override IEnumerable<MemberInfo> GetAssociationRelatedKeyMembers(MappingEntity entity, MemberInfo member)
        {
            //List<MemberInfo> keyMembers;
            List<MemberInfo> relatedKeyMembers;
            this.GetAssociationRelatedKey(entity, member, out relatedKeyMembers);//out keyMembers, 

            return relatedKeyMembers;
        }

        private void GetAssociationKeys(MappingEntity entity, MemberInfo member, out List<MemberInfo> keyMembers)
        {
            //TODO: 2010 - 7- 17 获取 外键引用列
            MappingEntity entity2 = GetRelatedEntity(entity, member);

            //  
            var map1 = this.GetMappedMembers(entity).Where(m => this.IsColumn(entity, m) ).ToDictionary(m => m.Name);
            var map2 = this.GetMappedMembers(entity2).Where(m => this.IsColumn(entity2, m) ).ToDictionary(m => m.Name);

            keyMembers = new List<MemberInfo>();
            var relatedKeyMembers = new List<MemberInfo>();
            //TODO:这边可能暂时有问题 ...待发现.. ..2010-7-17
            var ats = map1.Values.Where(p => GetKeyMember(entity2, p)).ToList();
            //var ats2 = map2.Values.Where(p => GetKeyMember(entity, p)).ToList();
            if (ats.Count() > 0)
                keyMembers.AddRange(ats);//
            //if (ats2.Count() > 0)
            //    relatedKeyMembers.AddRange(ats2);
            // 获取标记了 
            //[Association(Member = "Users", KeyMembers = "RoleID", RelatedEntityID = "Users", RelatedKeyMembers = "RoleID")]
            //的信息
            if (keyMembers.Count == 0)//relatedKeyMembers.Count == 0
            {
                var commonNames = map1.Keys.Intersect(map2.Keys).OrderBy(k => k);
                foreach (string name in commonNames)
                {
                    keyMembers.Add(map1[name]);//
                   // relatedKeyMembers.Add(map2[name]);
                }
            }
        }

        private void GetAssociationRelatedKey(MappingEntity entity, MemberInfo member, out List<MemberInfo> relatedKeyMembers)
        {
            //TODO: 2010 - 7- 17 获取 外键引用列
            MappingEntity entity2 = GetRelatedEntity(entity, member);

            //  
            var map1 = this.GetMappedMembers(entity).Where(m => this.IsColumn(entity, m) ).ToDictionary(m => m.Name);
            var map2 = this.GetMappedMembers(entity2).Where(m => this.IsColumn(entity2, m) ).ToDictionary(m => m.Name);

            var keyMembers = new List<MemberInfo>();
            relatedKeyMembers = new List<MemberInfo>();
            //TODO:这边可能暂时有问题 ...待发现.. ..2010-7-17
            //var ats = map1.Values.Where(p => GetKeyMember(entity2, p)).ToList();
            var ats2 = map2.Values.Where(p => GetKeyMember(entity, p)).ToList();
            //if (ats.Count() > 0)
            //    keyMembers.AddRange(ats);//
            if (ats2.Count() > 0)
                relatedKeyMembers.AddRange(ats2);
            // 获取标记了 
            //[Association(Member = "Users", KeyMembers = "RoleID", RelatedEntityID = "Users", RelatedKeyMembers = "RoleID")]
            //的信息
            if (relatedKeyMembers.Count == 0)//&& keyMembers.Count == 0
            {
                var commonNames = map1.Keys.Intersect(map2.Keys).OrderBy(k => k);
                foreach (string name in commonNames)
                {
                    //keyMembers.Add(map1[name]);//
                    relatedKeyMembers.Add(map2[name]);
                }
            }
        }


        /// <summary>
        /// Gets the key member.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        private bool GetKeyMember(MappingEntity entity, MemberInfo member)
        {
            var a = member.GetCustomAttribute<AssociationAttribute>(false);
            return a.Any(p => p.RelatedEntityID == entity.TableId.IfEmptyReplace(entity.EntityType.Name));
        }

        /// <summary>
        /// Gets the first association.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private bool GetFirstAssociation(MemberInfo entity)
        {
            return entity.GetCustomAttributes(typeof(AssociationAttribute), false).FirstOrDefault() != null;
        }

        /// <summary>
        /// The name of the corresponding database table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string GetTableName(MappingEntity entity)
        {
            return !string.IsNullOrEmpty(entity.TableId) ? entity.TableId : this.InferTableName(entity.EntityType);
        }

        /// <summary>
        /// Infers the name of the table.
        /// </summary>
        /// <param name="rowType">Type of the row.</param>
        /// <returns></returns>
        private string InferTableName(Type rowType)
        {
            return SplitWords(Plural(rowType.Name));
        }

        /// <summary>
        /// Splits the words.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string SplitWords(string name)
        {
            //TODO:2010-7-19 还是不要判断是否大小写了
            //StringBuilder sb = null;
            //bool lastIsLower = char.IsLower(name[0]);
            //for (int i = 0, n = name.Length; i < n; i++)
            //{
            //    bool thisIsLower = char.IsLower(name[i]);
            //    if (lastIsLower && !thisIsLower)
            //    {
            //        if (sb == null)
            //        {
            //            sb = new StringBuilder();
            //            sb.Append(name, 0, i);
            //        }
            //        //TODO:2010-7-19 ..干嘛加空格 ..!!!
            //        //sb.Append(" ");
            //    }
            //    if (sb != null)
            //    {
            //        sb.Append(name[i]);
            //    }
            //    lastIsLower = thisIsLower;
            //}
            //if (sb != null)
            //{
            //    return sb.ToString();
            //}
            return name;
        }

        /// <summary>
        /// Plurals the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string Plural(string name)
        {
            //TODO:2010-7-17 去掉拼写检查
            //if (name.EndsWith("x", StringComparison.InvariantCultureIgnoreCase) 
            //    || name.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase)
            //    || name.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase)) 
            //{
            //    return name + "es";
            //}
            //else if (name.EndsWith("y", StringComparison.InvariantCultureIgnoreCase)) 
            //{
            //    return name.Substring(0, name.Length - 1) + "ies";
            //}
            //else if (!name.EndsWith("s"))
            //{
            //    return name + "s";
            //}
            return name;
        }

        /// <summary>
        /// Singulars the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string Singular(string name)
        {
            //TODO:2010-7-17 去掉拼写检查
            //if (name.EndsWith("es", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    string rest = name.Substring(0, name.Length - 2);
            //    if (rest.EndsWith("x", StringComparison.InvariantCultureIgnoreCase)
            //        || name.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase)
            //        || name.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        return rest;
            //    }
            //}
            //if (name.EndsWith("ies", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    return name.Substring(0, name.Length - 3) + "y";
            //}
            //else if (name.EndsWith("s", StringComparison.InvariantCultureIgnoreCase)
            //    && !name.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    return name.Substring(0, name.Length - 1);
            //}
            return name;
        }
    }
}
