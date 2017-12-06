using Steve.DAL.EF;
using Steve.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Steve.DAL.Entities;

namespace Steve.DAL.Repositories
{
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly SteveContext context;
        private readonly DbSet<T> entities;
        protected RepositoryBase(SteveContext context)
        {
            this.context = context;
            this.entities = context.Set<T>();
        }
    }
}
