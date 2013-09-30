
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NkjSoft.ORM.Core;
using NkjSoft.ORM.Data.Mapping;

using NkjSoft.Extensions;
namespace NkjSoft.ORM.Data.Common
{
    /// <summary>
    /// 表示提供了抽象实体映射映射信息的能力。
    /// </summary>
    public abstract class MappingEntity
    {
        /// <summary>
        /// Gets the table id.
        /// </summary>
        /// <value>The table id.</value>
        public abstract string TableId { get; }
        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public abstract Type ElementType { get; }
        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public abstract Type EntityType { get; }

    }

    /// <summary>
    /// 实体信息。
    /// </summary>
    public struct EntityInfo
    {
        object instance;
        MappingEntity mapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityInfo"/> struct.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="mapping">The mapping.</param>
        public EntityInfo(object instance, MappingEntity mapping)
        {
            this.instance = instance;
            this.mapping = mapping;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance
        {
            get { return this.instance; }
        }

        /// <summary>
        /// Gets the mapping.
        /// </summary>
        /// <value>The mapping.</value>
        public MappingEntity Mapping
        {
            get { return this.mapping; }
        }
    }

    /// <summary>
    /// 定义映射的关系对象集。
    /// </summary>
    public interface IHaveMappingEntity
    {
        MappingEntity Entity { get; }
    }

    /// <summary>
    /// 定义数据对象的映射信息、规则以此来支持查询提供程序的查询工作。
    /// </summary>
    public abstract class QueryMapping
    {
        /// <summary>
        /// Determines the entity Id based on the type of the entity alone
        /// 获取对象到数据库表的映射名。
        /// </summary>
        /// <param name="type">数据库到对象的对象类型</param>
        /// <returns></returns>
        public virtual string GetTableId(Type type)
        {
            return type.Name;
        }

        /// <summary> 
        /// 获取数据库对象映射到对象之后的（CLR类型）映射信息。
        /// </summary>
        /// <param name="type">数据库对象映射到的对象类型</param>
        /// <returns></returns>
        public virtual MappingEntity GetEntity(Type type)
        {
            return this.GetEntity(type, this.GetTableId(type));
        }

        /// <summary>
        /// 设置一个主键ID，获取数据库对象映射到对象之后的（CLR类型）映射信息。
        /// </summary>
        /// <param name="elementType">数据库对象映射到的对象类型</param>
        /// <param name="entityID">主键ID</param>
        /// <returns></returns>
        public abstract MappingEntity GetEntity(Type elementType, string entityID);

        /// <summary>
        /// 通过查询上下文获取映射实体对象。
        /// </summary>
        /// <param name="contextMember"></param>
        /// <returns></returns>
        public abstract MappingEntity GetEntity(MemberInfo contextMember);

        /// <summary>
        /// 获取映射实体的成员列表。
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public abstract IEnumerable<MemberInfo> GetMappedMembers(MappingEntity entity);

        /// <summary>
        /// 判断指定的映射实体成员是否是主键列。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public abstract bool IsPrimaryKey(MappingEntity entity, MemberInfo member);

        /// <summary>
        /// Gets the primary key members.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual IEnumerable<MemberInfo> GetPrimaryKeyMembers(MappingEntity entity)
        {
            return this.GetMappedMembers(entity).Where(m => this.IsPrimaryKey(entity, m));
        }

        /// <summary>
        /// 判断指定映射实体的成员是否被影射为关联实体。
        /// </summary>
        /// <param name="entity">指定映射实体</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public abstract bool IsRelationship(MappingEntity entity, MemberInfo member);

        /// <summary>
        /// Determines if a relationship property refers to a single entity (as opposed to a collection.)
        /// 判断知道的映射实体的成员是否是单一实体映射关联。
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="member">The member.</param>
        /// <returns>
        /// 	<c>true</c> if [is singleton relationship] [the specified entity]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsSingletonRelationship(MappingEntity entity, MemberInfo member)
        {
            if (!this.IsRelationship(entity, member))
                return false;
            Type ieType = TypeHelper.FindIEnumerable(TypeHelper.GetMemberType(member));
            return ieType == null;
        }

        /// <summary>
        /// Determines whether a given expression can be executed locally.
        /// (It contains no parts that should be translated to the target environment.)
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can be evaluated locally] the specified expression; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanBeEvaluatedLocally(Expression expression)
        {
            // any operation on a query can't be done locally
            ConstantExpression cex = expression as ConstantExpression;
            if (cex != null)
            {
                IQueryable query = cex.Value as IQueryable;
                if (query != null && query.Provider == this)
                    return false;
            }
            MethodCallExpression mc = expression as MethodCallExpression;
            if (mc != null &&
                (mc.Method.DeclaringType == typeof(Enumerable) ||
                 mc.Method.DeclaringType == typeof(Queryable) ||
                 mc.Method.DeclaringType == typeof(Updatable))
                 )
            {
                return false;
            }
            if (expression.NodeType == ExpressionType.Convert &&
                expression.Type == typeof(object))
                return true;
            return expression.NodeType != ExpressionType.Parameter &&
                   expression.NodeType != ExpressionType.Lambda;
        }

        /// <summary>
        /// 获取映射实体对象的主键对象。
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract object GetPrimaryKey(MappingEntity entity, object instance);


        /// <summary>
        /// 通过表达式获取映射实体对象的主键对象。
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="source">The source.</param>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public abstract Expression GetPrimaryKeyQuery(MappingEntity entity, Expression source, Expression[] keys);
        /// <summary>
        /// Gets the dependent entities.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract IEnumerable<EntityInfo> GetDependentEntities(MappingEntity entity, object instance);
        /// <summary>
        /// Gets the depending entities.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract IEnumerable<EntityInfo> GetDependingEntities(MappingEntity entity, object instance);
        /// <summary>
        /// Clones the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public abstract object CloneEntity(MappingEntity entity, object instance);
        /// <summary>
        /// Determines whether the specified entity is modified.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="original">The original.</param>
        /// <returns>
        /// 	<c>true</c> if the specified entity is modified; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsModified(MappingEntity entity, object instance, object original);

        /// <summary>
        /// Creates the mapper.
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <returns></returns>
        public abstract QueryMapper CreateMapper(QueryTranslator translator);
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class QueryMapper
    {
        /// <summary>
        /// Gets the mapping.
        /// </summary>
        /// <value>The mapping.</value>
        public abstract QueryMapping Mapping { get; }
        /// <summary>
        /// Gets the translator.
        /// </summary>
        /// <value>The translator.</value>
        public abstract QueryTranslator Translator { get; }

        /// <summary>
        /// Get a query expression that selects all entities from a table
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public abstract ProjectionExpression GetQueryExpression(MappingEntity entity);

        /// <summary>
        /// Gets an expression that constructs an entity instance relative to a root.
        /// The root is most often a TableExpression, but may be any other experssion such as
        /// a ConstantExpression.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract EntityExpression GetEntityExpression(Expression root, MappingEntity entity);

        /// <summary>
        /// Get an expression for a mapped property relative to a root expression. 
        /// The root is either a TableExpression or an expression defining an entity instance.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="entity"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public abstract Expression GetMemberExpression(Expression root, MappingEntity entity, MemberInfo member);

        /// <summary>
        /// Get an expression that represents the insert operation for the specified instance.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="instance">The instance to insert.</param>
        /// <param name="selector">A lambda expression that computes a return value from the operation.</param>
        /// <returns></returns>
        public abstract Expression GetInsertExpression(MappingEntity entity, Expression instance, LambdaExpression selector);

        /// <summary>
        /// Get an expression that represents the insert operation for the specified instance.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="instance">The instance to insert.</param>
        /// <param name="selector">A lambda expression that computes a return value from the operation.</param>
        /// <param name="indeedColumns">指定需要插入的列.</param>
        /// <returns></returns>
        public abstract Expression GetInsertExpression(MappingEntity entity, Expression instance, LambdaExpression selector, ColumnIndeedExpression indeedColumns);

        /// <summary>
        /// Get an expression that represents the update operation for the specified instance.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="instance"></param>
        /// <param name="updateCheck"></param>
        /// <param name="selector"></param>
        /// <param name="else"></param>
        /// <returns></returns>
        public abstract Expression GetUpdateExpression(MappingEntity entity, Expression instance, LambdaExpression updateCheck, LambdaExpression selector, Expression @else);

        /// <summary>
        /// Get an expression that represents the update operation for the specified instance.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="updateCheck">The update check.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="indeedColumns">The indeed columns.</param>
        /// <param name="else">The @else.</param>
        /// <returns></returns>
        public abstract Expression GetUpdateExpression(MappingEntity entity, Expression instance, LambdaExpression updateCheck, LambdaExpression selector, ColumnIndeedExpression indeedColumns, Expression @else);


        /// <summary>
        /// Get an expression that represents the insert-or-update operation for the specified instance.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="instance"></param>
        /// <param name="updateCheck"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public abstract Expression GetInsertOrUpdateExpression(MappingEntity entity, Expression instance, LambdaExpression updateCheck, LambdaExpression resultSelector);

        /// <summary>
        /// Get an expression that represents the delete operation for the specified instance.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="instance"></param>
        /// <param name="deleteCheck"></param>
        /// <returns></returns>
        public abstract Expression GetDeleteExpression(MappingEntity entity, Expression instance, LambdaExpression deleteCheck);

        /// <summary>
        /// Recreate the type projection with the additional members included
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fnIsIncluded"></param>
        /// <returns></returns>
        public abstract EntityExpression IncludeMembers(EntityExpression entity, Func<MemberInfo, bool> fnIsIncluded);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public abstract bool HasIncludedMembers(EntityExpression entity);

        /// <summary>
        /// Apply mapping to a sub query expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Expression ApplyMapping(Expression expression)
        {
            return QueryBinder.Bind(this, expression);
        }

        /// <summary>
        /// Apply mapping translations to this expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Expression Translate(Expression expression)
        {
            // convert references to LINQ operators into query specific nodes
            expression = QueryBinder.Bind(this, expression);

            // move aggregate computations so they occur in same select as group-by
            expression = AggregateRewriter.Rewrite(this.Translator.Linguist.Language, expression);

            // do reduction so duplicate association's are likely to be clumped together
            expression = UnusedColumnRemover.Remove(expression);
            expression = RedundantColumnRemover.Remove(expression);
            expression = RedundantSubqueryRemover.Remove(expression);
            expression = RedundantJoinRemover.Remove(expression);

            // convert references to association properties into correlated queries
            var bound = RelationshipBinder.Bind(this, expression);
            if (bound != expression)
            {
                expression = bound;
                // clean up after ourselves! (multiple references to same association property)
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            // rewrite comparision checks between entities and multi-valued constructs
            expression = ComparisonRewriter.Rewrite(this.Mapping, expression);

            return expression;
        }
    }



}
