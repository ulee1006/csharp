  
using System;
using Microsoft.EntityFrameworkCore;

 
namespace cexam.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<List> Lists { get; set; }
        public DbSet<Join> Joins { get; set; }
    }
}