using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using Steve.DAL.Entities;

namespace Steve.DAL.EF
{
   public class SteveContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-VSNMGHA\\SQLEXPRESS;Initial Catalog=T;Integrated Security=True;Pooling=False");
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<EmailEntity> Emails { get; set; }




    }
}
