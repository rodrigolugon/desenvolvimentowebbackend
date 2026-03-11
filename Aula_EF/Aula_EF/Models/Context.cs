using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Aula_EF.Models {
    public class Context : DbContext {
        public Context(DbContextOptions<Context> options)
            : base(options) {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Fabricante> Fabricantes { get; set; }

    }
}