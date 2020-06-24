//using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Tratorfix.Models.Repository
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TratorfixContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Orçamento> Orçamentos { get; set; }
    }
}