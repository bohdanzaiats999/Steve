using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Steve.DAL.EF;
using Steve.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steve.DAL.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly SteveContext context;
        private bool disposed;
        private Dictionary<string, object> repositories;
        public UnitOfWork()
        {
            context = new SteveContext();
        }
        public UnitOfWork(SteveContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public UserRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(UserRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }
            return (UserRepository<T>)repositories[type];
        }
    }
}
