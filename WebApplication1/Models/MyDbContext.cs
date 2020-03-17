using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication2.Models
{
    public class MyDbContext :DbContext
    {
        public DbSet<UserAccount> UserAccount { get; set; }
    }
}