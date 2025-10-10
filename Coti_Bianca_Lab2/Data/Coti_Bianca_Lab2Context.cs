using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Coti_Bianca_Lab2.Models;
using System.Collections;

namespace Coti_Bianca_Lab2.Data
{
    public class Coti_Bianca_Lab2Context : DbContext
    {
        public Coti_Bianca_Lab2Context (DbContextOptions<Coti_Bianca_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Coti_Bianca_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Coti_Bianca_Lab2.Models.Publisher> Publisher { get; set; } = default!;
        public IEnumerable Authors { get; internal set; } = Array.Empty<object>();
        public DbSet<Coti_Bianca_Lab2.Models.Author> Author { get; set; } = default!;
    }
}
