using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FTDWebSite.Models
{
    public class FdtContext : DbContext
    {
        public FdtContext() : base("name=DefaultConnection") { }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}