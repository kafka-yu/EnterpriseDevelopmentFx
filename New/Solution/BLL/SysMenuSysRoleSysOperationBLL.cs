using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NkjSoft.DAL;
using Common;
using System.ComponentModel.Composition;

namespace NkjSoft.BLL
{
    /// <summary>
    /// 角色菜单操作 
    /// </summary>
    [Export(typeof(IBLL.ISysMenuSysRoleSysOperationBLL))]
    public class SysMenuSysRoleSysOperationBLL : IBLL.ISysMenuSysRoleSysOperationBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;
        /// <summary>
        /// 角色菜单操作的数据库访问对象
        /// </summary>
        SysMenuSysRoleSysOperationRepository repository = new SysMenuSysRoleSysOperationRepository();
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public SysMenuSysRoleSysOperationBLL()
        {
            db = new SysEntities();
        }
        /// <summary>
        /// 已有数据访问上下文的方法中调用
        /// </summary>
        /// <param name="entities">数据访问上下文</param>
        public SysMenuSysRoleSysOperationBLL(SysEntities entities)
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
        public List<SysMenuSysRoleSysOperation> GetByParam(string id, int page, int rows, string order, string sort, string search, ref int total)
        {
            IQueryable<SysMenuSysRoleSysOperation> queryData = repository.DaoChuData(db, order, sort, search);
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
                    if (item.SysMenuId != null && item.SysMenu != null)
                    {
                        item.SysMenuIdOld = item.SysMenu.Name.GetString();//                            
                    }

                    if (item.SysRoleId != null && item.SysRole != null)
                    {
                        item.SysRoleIdOld = item.SysRole.Name.GetString();//                            
                    }

                    if (item.SysOperationId != null && item.SysOperation != null)
                    {
                        item.SysOperationIdOld = item.SysOperation.Name.GetString();//                            
                    }

                }

            }
            return queryData.ToList();
        }
        /// <summary>
        /// 创建一个角色菜单操作
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据库上下文</param>
        /// <param name="entity">一个角色菜单操作</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysMenuSysRoleSysOperation entity)
        {
            try
            {
                repository.Create(entity);
                return true;
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }
        /// <summary>
        ///  创建角色菜单操作集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">角色菜单操作集合</param>
        /// <returns></returns>
        public bool CreateCollection(ref ValidationErrors validationErrors, IQueryable<SysMenuSysRoleSysOperation> entitys)
        {
            try
            {
                if (entitys != null)
                {
                    int count = entitys.Count();
                    if (count == 1)
                    {
                        return this.Create(ref validationErrors, entitys.FirstOrDefault());
                    }
                    else if (count > 1)
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            repository.Create(db, entitys);
                            if (count == repository.Save(db))
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
        /// 删除一个角色菜单操作
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一角色菜单操作的主键</param>
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
        /// 删除角色菜单操作集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">角色菜单操作的集合</param>
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
        ///  创建角色菜单操作集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">角色菜单操作集合</param>
        /// <returns></returns>
        public bool EditCollection(ref ValidationErrors validationErrors, IQueryable<SysMenuSysRoleSysOperation> entitys)
        {
            try
            {
                if (entitys != null)
                {
                    int count = entitys.Count();
                    if (count == 1)
                    {
                        return this.Edit(ref validationErrors, entitys.FirstOrDefault());
                    }
                    else if (count > 1)
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            repository.Edit(db, entitys);
                            if (count == repository.Save(db))
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
        /// 编辑一个角色菜单操作
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个角色菜单操作</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors, SysMenuSysRoleSysOperation entity)
        {
            try
            {
                repository.Edit(db, entity);
                repository.Save(db);
                return true;
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }

        public List<SysMenuSysRoleSysOperation> GetAll()
        {
            return repository.GetAll(db).ToList();
        }

        /// <summary>
        /// 根据主键获取一个角色菜单操作
        /// </summary>
        /// <param name="id">角色菜单操作的主键</param>
        /// <returns>一个角色菜单操作</returns>
        public SysMenuSysRoleSysOperation GetById(string id)
        {
            return repository.GetById(db, id);
        }


        /// <summary>
        /// 根据SysMenuIdId，获取所有角色菜单操作数据
        /// </summary>
        /// <param name="id">外键的主键</param>
        /// <returns></returns>
        public List<SysMenuSysRoleSysOperation> GetByRefSysMenuId(string id)
        {
            return repository.GetByRefSysMenuId(db, id).ToList();
        }

        /// <summary>
        /// 根据SysRoleIdId，获取所有角色菜单操作数据
        /// </summary>
        /// <param name="id">外键的主键</param>
        /// <returns></returns>
        public IQueryable<SysMenuSysRoleSysOperation> GetByRefSysRoleId(string id)
        {
            return repository.GetByRefSysRoleId(db, id);
        }

        /// <summary>
        /// 根据SysOperationIdId，获取所有角色菜单操作数据
        /// </summary>
        /// <param name="id">外键的主键</param>
        /// <returns></returns>
        public List<SysMenuSysRoleSysOperation> GetByRefSysOperationId(string id)
        {
            return repository.GetByRefSysOperationId(db, id).ToList();
        }
        /// <summary>
        /// 根据SysMenuIdId，获取所有角色菜单操作数据
        /// </summary>
        /// <param name="id">外键的主键</param>
        /// <returns></returns>
        public List<SysOperation> GetByRefSysMenuIdAndSysRoleId(string id, List<string> sysRoleIds)
        {
            return repository.GetByRefSysMenuIdAndSysRoleId(db, id, sysRoleIds).ToList();
        }
        public void Dispose()
        {

        }
    }
}

