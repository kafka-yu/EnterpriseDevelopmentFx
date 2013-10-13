using System;
namespace NkjSoft.IBLL
{
    public interface IAccountBLL
    {
        bool ChangePassword(string personName, string oldPassword, string newPassword);
        NkjSoft.DAL.SysPerson ValidateUser(string userName, string password);
    }
}
