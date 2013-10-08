using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NkjSoft.EFRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EFBasedRepository<TEntity> : NkjSoft.Repository.IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The context
        /// </summary>
         protected readonly DbContext Context;

         /// <summary>
         /// Initializes a new instance of the <see cref="EFBasedRepository{TEntity}"/> class.
         /// </summary>
         /// <param name="context">The context.</param>
         public EFBasedRepository(DbContext context)
        {
            Context = context;
        }

         /// <summary>
         /// Firsts the or default.
         /// </summary>
         /// <param name="expression">The expression.</param>
         /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return All().FirstOrDefault(expression);
        }

        /// <summary>
        /// Alls this instance.
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> All()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// Filters the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where<TEntity>(predicate).AsQueryable<TEntity>();
        }

        /// <summary>
        /// Filters the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="total">The total.</param>
        /// <param name="index">The index.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0,
                                               int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null
                                ? Context.Set<TEntity>().Where<TEntity>(filter).AsQueryable()
                                : Context.Set<TEntity>().AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// Creates the specified T object.
        /// </summary>
        /// <param name="TObject">The T object.</param>
        public virtual void Create(TEntity TObject)
        {
            Context.Set<TEntity>().Add(TObject);
        }

        /// <summary>
        /// Deletes the specified T object.
        /// </summary>
        /// <param name="TObject">The T object.</param>
        public virtual void Delete(TEntity TObject)
        {
            Context.Set<TEntity>().Remove(TObject);
        }

        /// <summary>
        /// Updates the specified T object.
        /// </summary>
        /// <param name="TObject">The T object.</param>
        public virtual void Update(TEntity TObject)
        {
            try
            {
                var entry = Context.Entry(TObject);
                Context.Set<TEntity>().Attach(TObject);
                entry.State = EntityState.Modified;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                Context.Set<TEntity>().Remove(obj);
            return Context.SaveChanges();
        }

        /// <summary>
        /// Determines whether [contains] [the specified predicate].
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified predicate]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Any(predicate);
        }

        /// <summary>
        /// Finds the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public virtual TEntity Find(params object[] keys)
        {
            return Context.Set<TEntity>().Find(keys);
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault<TEntity>(predicate);
        }
    }
}
