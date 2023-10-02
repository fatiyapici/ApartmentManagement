using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApartmentManagement;

namespace ApartmentManagement.Data
{
    public class MvcInvoiceContext : DbContext
    {
        public MvcInvoiceContext (DbContextOptions<MvcInvoiceContext> options)
            : base(options)
        {
        }

        public DbSet<ApartmentManagement.Invoice> Invoice { get; set; } = default!;
    }
}
