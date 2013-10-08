
using NkjSoft.Core.Data.Repositories.Account;
using NkjSoft.Core.Data.Repositories.Security;
using NkjSoft.Core.Models.Account;
using NkjSoft.Core.Models.Security;
using NkjSoft.Framework.Common;
using NkjSoft.ServiceContracts.Core;
using NkjSoft.ServiceContracts.Core.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace NkjSoft.ServiceContracts.Impl.Account
{
    /// <summary>
    ///     账户模块核心业务实现
    /// </summary>
    public abstract class UserAccountService : CoreServiceBase, IUserAccountService
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 用户信息数据访问对象
        /// </summary>
        [Import]
        protected IUsersRepository MemberRepository { get; set; }

        /// <summary>
        /// 获取或设置 用户扩展信息数据访问对象
        /// </summary>
        [Import]
        protected IProfilesRepository ProfilesRepository { get; set; }

        /// <summary>
        /// 获取或设置 登录记录信息数据访问对象
        /// </summary>
        [Import]
        protected ILoginLogRepository LoginLogRepository { get; set; }

        /// <summary>
        /// 获取或设置 角色信息业务访问对象
        /// </summary>
        [Import]
        protected IRoleRepository RoleRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<Users> Members
        {
            get { return MemberRepository.Entities; }
        }

        /// <summary>
        /// 获取 用户扩展信息查询数据集
        /// </summary>
        public IQueryable<Profiles> MemberExtends
        {
            get { return ProfilesRepository.Entities; }
        }

        /// <summary>
        /// 获取 登录记录信息查询数据集
        /// </summary>
        public IQueryable<LoginLog> LoginLogs
        {
            get { return LoginLogRepository.Entities; }
        }

        /// <summary>
        /// 获取 角色信息查询数据集
        /// </summary>
        public IQueryable<Roles> Roles
        {
            get { return RoleRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult Login(LoginInfo loginInfo)
        {
            PublicHelper.CheckArgument(loginInfo, "loginInfo");
            Users member = MemberRepository.Entities.SingleOrDefault(m => m.UserName == loginInfo.Account || m.Memberships
            .Email == loginInfo.Account);
            if (member == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定账号的用户不存在。");
            }
            if (member.Memberships.Password != loginInfo.Password)
            {
                return new OperationResult(OperationResultType.Warning, "登录密码不正确。");
            }
            LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            LoginLogRepository.Insert(loginLog);
            return new OperationResult(OperationResultType.Success, "登录成功。", member);
        }

        public List<Users> GetAllActionPermission()
        {
            return new List<Users>();
        }
    }
}
