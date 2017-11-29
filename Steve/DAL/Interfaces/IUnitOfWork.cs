using Steve.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Steve.DAL.Interfaces
{
   public interface IUnitOfWork
    {
        UserRepository<T> Repository<T>() where T : class;
        void SaveChanges();
    }
}
