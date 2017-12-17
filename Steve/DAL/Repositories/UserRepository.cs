using Steve.DAL.EF;
using Steve.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Steve.DAL.Entities;
using System.Collections;

namespace Steve.DAL.Repositories
{
    public class UserRepository<T> : IRepository<T> where T : class
    {

        private readonly SteveContext context;
        private readonly DbSet<T> entities;
        public UserRepository(SteveContext context)
        {
            this.context = context;
            this.entities = context.Set<T>();
        }

        public T GetById(object id)
        {
            return this.entities.Find(id);
        }

        public UserEntity GetByLogin(string login)
        {
            return this.context.Users.FirstOrDefault(u => u.Login == login);
        }

        public IList<UserEntity> GetAllUsers()
        {
            return this.context.Users.ToList();
        }
        public IList<EmailEntity> GetAllEmails()
        {
            return this.context.Emails.ToList();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.context.SaveChanges();
        }

        public void RemoveRange(IQueryable<T> entities)
        {
            foreach (var entity in entities.ToList())
            {
                this.context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.entities.Remove(entity);
        }

        public void DeleteById(object id)
        {
            T entityToDelete = this.entities.Find(id);

            if (entityToDelete == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.entities.Remove(entityToDelete);
        }

        public IQueryable<T> Set
        {
            get
            {
                return this.entities;
            }
        }

        public IQueryable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = this.entities;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = this.entities;
            return include.Aggregate(query, (current, inc) => current.Include(inc));
        }
    }
}


