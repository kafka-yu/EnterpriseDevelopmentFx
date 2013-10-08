using NkjSoft.Model.Common;
using NkjSoft.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NkjSoft.EFRepository.Repositories
{
    public class UserRepository : EFBasedRepository<Users>, IUserRepository
    {
         /// <summary>
         /// Initializes a new instance of the <see cref="EFBasedRepository{TEntity}"/> class.
         /// </summary>
         /// <param name="context">The context.</param>
         public UserRepository(DbContext context)
             :base(context)
        {
        }
    }
}
