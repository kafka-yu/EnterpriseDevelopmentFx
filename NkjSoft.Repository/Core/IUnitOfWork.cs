using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class;
        void ExecuteProcedure(string procedureCommand, params object[] sqlParams);
        void SaveChanges();
    }
}
