using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NkjSoft.DAL;
using Common;
using System.ComponentModel.Composition;
using NkjSoft.IBLL;

namespace NkjSoft.BLL
{
    /// <summary>
    /// 菜单 
    /// </summary>

    [Export(typeof(ISysMenuBLL))]
    public class SysMenuBLL : IBLL.ISysMenuBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;
        /// <summary>
        /// 菜单的数据库访问对象
        /// </summary>
        SysMenuRepository repository = new SysMenuRepository();
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public SysMenuBLL()
        {
            db = new SysEntities();
        }
        /// <summary>
        /// 已有数据访问上下文的方法中调用
        /// </summary>
        /// <param name="entities">数据访问上下文</param>
        public SysMenuBLL(SysEntities entities)
        {
            db = entities;
        }
        /// <summary>
        /// 查询的数据
        /// </summary>
        /// <param name="id">额外的参数</param>
        /// <param name="page">页码</param>
        /// <param name="rows">每页显示的行数</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort">升序asc（默认）还是降序desc</param>
        /// <param name="search">查询条件</param>
        /// <param name="total">结果集的总数</param>
        /// <returns>结果集</returns>
        public List<SysMenu> GetByParam(string id, int page, int rows, string order, string sort, string search, ref int total)
        {


            IQueryable<SysMenu> queryData = repository.DaoChuData(db, order, sort, search);
            total = queryData.Count();
            if (total > 0)
            {
                if (page <= 1)
                {
                    queryData = queryData.Take(rows);
                }
                else
                {
                    queryData = queryData.Skip((page - 1) * rows).Take(rows);
                }

                foreach (var item in queryData)
                {
                    if (item.ParentId != null && item.SysMenu2 != null)
                    {
                        item.ParentIdOld = item.SysMenu2.Name.GetString();//                            
                    }

                    if (item.SysOperation != null)
                    {
                        item.SysOperationId = string.Empty;
                        foreach (var it in item.SysOperation)
                        {
                            item.SysOperationId += it.Name + ' ';
                        }
                    }

                }

            }
            return queryData.ToList();
        }

        /// <summary>
        /// 创建一个菜单
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据库上下文</param>
        /// <param name="entity">一个菜单</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysEntities db, SysMenu entity)
        {
            int count = 1;

            foreach (string item in entity.SysOperationId.GetIdSort())
            {
                SysOperation sys = new SysOperation { Id = item };
                db.SysOperation.Attach(sys);
                entity.SysOperation.Add(sys);
                count++;
            }

            repository.Create(db, entity);
            if (count == repository.Save(db))
            {
                //创建后重置菜单编码
                List<int> flags = new List<int>();//层级
                GetMenus2(null, flags);
                db.SaveChanges();
                return true;
            }
            else
            {
                validationErrors.Add("创建出错了");
            }
            return false;
        }
        /// <summary>
        /// 创建一个菜单
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个菜单</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysMenu entity)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (Create(ref validationErrors, db, entity))
                    {
                        transactionScope.Complete();
                        return true;
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }
        /// <summary>
        ///  创建菜单集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">菜单集合</param>
        /// <returns></returns>
        public bool CreateCollection(ref ValidationErrors validationErrors, IQueryable<SysMenu> entitys)
        {
            try
            {
                if (entitys != null)
                {
                    int flag = 0, count = entitys.Count();
                    if (count > 0)
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            foreach (var entity in entitys)
                            {
                                if (Create(ref validationErrors, db, entity))
                                {
                                    flag++;
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    return false;
                                }
                            }
                            if (count == flag)
                            {
                                transactionScope.Complete();
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }

        /// <summary>
        /// 删除一个菜单
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一个菜单的主键</param>
        /// <returns></returns>  
        public bool Delete(ref ValidationErrors validationErrors, string id)
        {
            try
            {
                return repository.Delete(id) == 1;
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }
        /// <summary>
        /// 删除菜单集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的菜单</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            try
            {
                if (deleteCollection != null)
                {

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        repository.Delete(db, deleteCollection);
                        if (deleteCollection.Length == repository.Save(db))
                        {
                            db.SaveChanges();
                            //要先提交事务，然后修改编码，否则IsLeaf字段得不到更新

                            //在1.2版本中修改 
                            //此方法由eastday(qq:76381028)提供  
                            //删除后重置菜单编码
                            List<int> flags = new List<int>();//层级
                            GetMenus2(null, flags);
                            db.SaveChanges();
                            
                             transactionScope.Complete();
                            return true;
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }
        /// <summary>
        ///  创建菜单集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">菜单集合</param>
        /// <returns></returns>
        public bool EditCollection(ref ValidationErrors validationErrors, IQueryable<SysMenu> entitys)
        {
            if (entitys != null)
            {
                try
                {
                    int flag = 0, count = entitys.Count();
                    if (count > 0)
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            foreach (var entity in entitys)
                            {
                                if (Edit(ref validationErrors, db, entity))
                                {
                                    flag++;
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    return false;
                                }
                            }
                            if (count == flag)
                            {
                                transactionScope.Complete();
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    validationErrors.Add(ex.Message);
                    ExceptionsHander.WriteExceptions(ex);
                }
            }
            return false;
        }
        /// <summary>
        /// 编辑一个菜单
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据上下文</param>
        /// <param name="entity">一个菜单</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, SysEntities db, SysMenu entity)
        {  /*                       
                           * 不操作 原有 现有
                           * 增加   原没 现有
                           * 删除   原有 现没
                           */
            if (entity == null)
            {
                return false;
            }
            int count = 1;
            SysMenu editEntity = repository.Edit(db, entity);

            List<string> addSysOperationId = new List<string>();
            List<string> deleteSysOperationId = new List<string>();
            DataOfDiffrent.GetDiffrent(entity.SysOperationId.GetIdSort(), entity.SysOperationIdOld.GetIdSort(), ref addSysOperationId, ref deleteSysOperationId);
            if (addSysOperationId != null && addSysOperationId.Count() > 0)
            {
                foreach (var item in addSysOperationId)
                {
                    SysOperation sys = new SysOperation { Id = item };
                    db.SysOperation.Attach(sys);
                    editEntity.SysOperation.Add(sys);
                    count++;
                }
            }
            if (deleteSysOperationId != null && deleteSysOperationId.Count() > 0)
            {
                List<SysOperation> listEntity = new List<SysOperation>();
                foreach (var item in deleteSysOperationId)
                {
                    SysOperation sys = new SysOperation { Id = item };
                    listEntity.Add(sys);
                    db.SysOperation.Attach(sys);
                }
                foreach (SysOperation item in listEntity)
                {
                    editEntity.SysOperation.Remove(item);//查询数据库
                    count++;
                }
            }

            if (count == repository.Save(db))
            {
                //修改后重置菜单编码
                List<int> flags = new List<int>();//层级
                GetMenus2(null, flags);
                db.SaveChanges();
                return true;
            }
            else
            {
                validationErrors.Add("编辑菜单出错了");
            }
            return false;
        }
        /// <summary>
        /// 编辑一个菜单
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个菜单</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, SysMenu entity)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (Edit(ref validationErrors, db, entity))
                    {
                        transactionScope.Complete();
                        return true;
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }
        public List<SysMenu> GetAll()
        {
            return repository.GetAll(db).ToList();
        }

        /// <summary>
        /// 获取自连接树形列表数据
        /// </summary>
        /// <returns>自定义的树形结构</returns>

        public IQueryable<SysMenu> GetAllMetadata(string id)
        {
            return db.SysMenu.Include("SysOperation").AsQueryable();

        }
        /// <summary>
        /// 获取自连接树形列表数据
        /// </summary>
        /// <returns>自定义的树形结构</returns>

        public IQueryable<SysMenu> GetAllMetadata()
        {
            return db.SysMenu.Include("SysOperation").AsQueryable();

        }

        /// <summary>
        /// 根据主键获取一个菜单
        /// </summary>
        /// <param name="id">菜单的主键</param>
        /// <returns>一个菜单</returns>
        public SysMenu GetById(string id)
        {
            return repository.GetById(db, id);
        }

        /// <summary>
        /// 获取在该表一条数据中，出现的所有外键实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>外键实体集合</returns>
        public List<SysOperation> GetRefSysOperation(string id)
        {
            return repository.GetRefSysOperation(db, id).ToList();
        }
        /// <summary>
        /// 获取在该表中出现的所有外键实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>外键实体集合</returns>
        public List<SysOperation> GetRefSysOperation()
        {
            return repository.GetRefSysOperation(db).ToList();
        }
        
        //递归更新
        protected void GetMenus2(string menuId, List<int> flags)
        {
            IQueryable<SysMenu> listTree;
            if (menuId == null)
            {
                listTree = from f in db.SysMenu
                           where (f.ParentId) == null
                           orderby f.Sort
                           select f;
            }
            else
            {
                listTree = from f in db.SysMenu
                           where menuId == (f.ParentId)
                           orderby f.Sort
                           select f;
            }


            if (listTree != null && listTree.Any())
            {
                flags.Add(1000);
                foreach (SysMenu item in listTree)
                {
                    //修改编码
                    item.Remark = string.Join("", flags);
                 
                    if (item.SysMenu1.Any())
                    {
                        item.IsLeaf = "叶子";
                        //非子节点，递归
                        GetMenus2(item.Id, flags);
                        flags.RemoveAt(flags.Count - 1);
                    }
                    else
                    {
                        item.IsLeaf = null;
                    }
                    //值+1
                    flags[flags.Count - 1]++;
                }
            }
        }

        public void Dispose()
        {

        }
    }
}

