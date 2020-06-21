using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Data
{
    public class ProductManagementDbContext : DbContext
    {
        public ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}
