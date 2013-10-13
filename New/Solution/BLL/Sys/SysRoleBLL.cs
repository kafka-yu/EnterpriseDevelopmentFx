using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NkjSoft.DAL;
using Common;

namespace NkjSoft.BLL
{
    /// <summary>
    /// 角色 
    /// </summary>
    public partial class SysRoleBLL : IBLL.ISysRoleBLL, IDisposable
    {
        public bool SaveCollection(ref ValidationErrors validationErrors, string[] ids, string id)
        {
            char split = '^';
            var data = (
                from f in ids
                where f.Contains(split)
                select f.Substring(0, f.IndexOf(split))
                        ).Union(
                from f in ids
                where !string.IsNullOrWhiteSpace(f) && !f.Contains(split)
                select f);

            using (TransactionScope transactionScope = new TransactionScope())
            {
                //利用编码机制，查询出所有的菜单
                var codes = db.SysMenu.Where(w => data.Contains(w.Id)).Select(s => s.Remark).Distinct();
                List<string> ls = new List<string>();
                foreach (var item in codes)
                {
                    for (int i = 0; i < item.Length / 4; i++)
                    {
                        //需要在项目中引用  System.Numerics.dll  
                        //在1.2版本中修改
                        string num = System.Numerics.BigInteger.Divide(System.Numerics.BigInteger.Parse(item), System.Numerics.BigInteger.Pow(10000, i)).ToString();

                        if (!ls.Contains(num))
                        {
                            ls.Add(num);
                        }
                    }
                }
                var SysMenusIds = from f in db.SysMenu
                                  where ls.Contains(f.Remark)
                                  select f.Id; //现在所有的菜单
                var d = db.SysMenuSysRoleSysOperation.Where(SysRoleId => SysRoleId.SysRoleId == id);
                foreach (var item in d)
                {
                    db.SysMenuSysRoleSysOperation.DeleteObject(item);
                }
                foreach (var item in SysMenusIds)
                {//插入菜单
                    db.SysMenuSysRoleSysOperation.AddObject(new SysMenuSysRoleSysOperation()
                    {
                        Id = Common.Result.GetNewId(),
                        SysRoleId = id,
                        SysMenuId = item
                    });

                }

                foreach (var item in ids)
                {//插入操作
                    if (item.Contains(split))
                        db.SysMenuSysRoleSysOperation.AddObject(new SysMenuSysRoleSysOperation()
                        {
                            Id = Common.Result.GetNewId(),
                            SysRoleId = id,
                            SysMenuId = item.Substring(0, item.IndexOf(split)),
                            SysOperationId = item.Substring(item.IndexOf(split) + 1)
                        });


                }

                db.SaveChanges();
                transactionScope.Complete();
                return true;

            }
        }
  
    }
}

