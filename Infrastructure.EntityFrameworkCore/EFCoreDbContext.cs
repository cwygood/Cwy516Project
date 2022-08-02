using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.EntityFrameworkCore
{
    public class EFCoreDbContext : DbContext
    {
        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options)
            : base(options)
        {

        }
        public DbSet<Menu_Info> Menu_Infos { get; set; }
        public DbSet<User_Info> User_infos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu_Info>().ToTable("menu_info");
            modelBuilder.Entity<User_Info>().ToTable("user_info");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            Console.WriteLine(DbLoggerCategory.Database.Name);
        }
    }
}
