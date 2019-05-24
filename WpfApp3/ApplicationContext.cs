using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DatabaseConnection")
        {

        }
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<CounterStore> CounterStores { get; set; }
    }
}
